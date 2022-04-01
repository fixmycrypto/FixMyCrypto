using NBitcoin;
using OpenCl.DotNetCore;
using OpenCl.DotNetCore.CommandQueues;
using OpenCl.DotNetCore.Contexts;
using OpenCl.DotNetCore.Devices;
using OpenCl.DotNetCore.Kernels;
using OpenCl.DotNetCore.Memory;
using OpenCl.DotNetCore.Platforms;
using OpenCl.DotNetCore.Programs;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;
using ConsoleTables;

namespace FixMyCrypto {

    class OpenCL {

        private Context context;
        private Program program_pbkdf2;

        private Program program_bip32derive;
        private CommandQueue commandQueue;
        private ConcurrentDictionary<string, Kernel> pbkdf2_kernels = new();

        private ConcurrentDictionary<string, Kernel> bip32_kernels = new();

        private bool program_pbkdf2_ready = false;

        private bool program_bip32derive_ready = false;
        private int usingDkLen;

        private int saltBufferSize;     //  max char length of a passphrase

        private int inBufferSize = 256 - 8; //  max char length of a phrase

        private int outBufferSize = 64;

        private int wordSize;

        private Device chosenDevice;

        private int maxPassphraseLength;
        private int platformId, deviceId;
        object mutex = new();
        int kernelsRunning = 0;

        public OpenCL(int platformId = 0, int deviceId = 0, int maxPassphraseLength = 32) {
            // LogOpenCLInfo();
            if (platformId < 0 || deviceId < 0) throw new ArgumentException();

            IEnumerable<Platform> platforms = Platform.GetPlatforms();
            chosenDevice = platforms.ToList()[platformId].GetDevices(DeviceType.All).ToList()[deviceId];
            // Log.Info($"Selected device ({platformId}, {deviceId}): {chosenDevice.Name} ({chosenDevice.Vendor})");
            context = Context.CreateContext(chosenDevice);            
            commandQueue = CommandQueue.CreateCommandQueue(context, chosenDevice);
            this.maxPassphraseLength = Math.Max(maxPassphraseLength, 32);   //  "mnemonic" + passphrase
            this.platformId = platformId;
            this.deviceId = deviceId;

            System.Timers.Timer passphraseLogger = new System.Timers.Timer(5 * 1000);
            passphraseLogger.Elapsed += (StringReader, args) => {
                Log.Debug($"kernelsRunning={kernelsRunning}");
            };
            passphraseLogger.Start();
        }

        public string GetDeviceInfo() {
            return $"Selected device ({platformId}, {deviceId}): {chosenDevice.Name} ({chosenDevice.Vendor})";
        }

        public static int GetPlatformCount() {
            return Platform.GetPlatforms().Count();
        }

        public static int GetDeviceCount(int platform) {
            return Platform.GetPlatforms().ToList()[platform].GetDevices(DeviceType.All).ToList().Count;
        }

        [System.Runtime.Versioning.SupportedOSPlatformGuard("windows")]
        private static string GetCpuName_Windows() {
            try {
#pragma warning disable CA1416
                var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0\");
                return key?.GetValue("ProcessorNameString").ToString() ?? "CPU";
#pragma warning restore CA1416
            }
            catch (Exception) {}
            return "CPU";
        }

        private static string GetCpuName() {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows)) {
                return GetCpuName_Windows();
            }
            return "CPU";
        }
        public static void BenchmarkDevices(int count = 4096) {
            Log.Info("Benchmarking devices...");

            ConsoleTable table = new ConsoleTable("Device", "phrases/ms", "passphrases/ms", "Secp256k1 keys/ms", "BIP32 paths/ms");
            for (int p = 0; p < GetPlatformCount(); p++) {
                Platform platform = Platform.GetPlatforms().ToList()[p];

                for (int d = 0; d < GetDeviceCount(p); d++) {
                    OpenCL ocl = new(p, d);
                    Dictionary<string, int> results = new();
                    ocl.Benchmark_Pbkdf2(count, results);
                    ocl.Benchmark_Bip32Derive(PathNode.Harden(0), count, results);
                    ocl.Benchmark_Bip32Derive(0, count, results);
                    ocl.Benchmark_Bip32DerivePath(null, count, results);

                    if (p == 0 && d == 0) {
                        table.AddRow(GetCpuName(), $"{results["cpuPhrases"]/1000.0:F1}", $"{results["cpuPassphrases"]/1000.0:F1}", $"{results["cpuSecDerives"]/1000.0:F1}", $"{results["cpuSecPaths"]/1000.0:F1}");
                    }

                    Device dev = platform.GetDevices(DeviceType.All).ToList()[d];

                    table.AddRow($"{dev.Name} ({p}:{d})", $"{results["oclPhrases"]/1000.0:F1}", $"{results["oclPassphrases"]/1000.0:F1}", $"{results["oclSecDerives"]/1000.0:F1}", $"{results["oclSecPaths"]/1000.0:F1}");
                }
            }
            Log.Info("Benchmark Results:");
            Log.Info(table.ToStringAlternative());
        }

        ~OpenCL() {
            program_pbkdf2?.Dispose();
            program_bip32derive?.Dispose();
            foreach (Kernel k in pbkdf2_kernels.Values) k?.Dispose();
            commandQueue?.Dispose();
            context?.Dispose();
            chosenDevice?.Dispose();
        }

        public void Stop() {
            commandQueue?.Dispose();
            commandQueue = null;
        }

        public enum Mode {
            opencl_brute,
            bip39_solver
        }

        public void Init_Sha512(int dklen = 64, Mode mode = Mode.bip39_solver) {
            if (program_pbkdf2_ready && dklen == usingDkLen) return;

            program_pbkdf2_ready = false;
            outBufferSize = (dklen > 64) ? 128 : 64;
            wordSize = 8;
            saltBufferSize = maxPassphraseLength + 8;   //  +8 for "mnemonic"
            if (saltBufferSize % wordSize != 0) saltBufferSize += wordSize - (saltBufferSize % wordSize);

            // Creates a program and then the kernel from it
            
            string code;
            if (mode == Mode.opencl_brute) {
                code = OpenCL_Bufferstructs.buffer_structs_template_cl + OpenCL_Sha512.hmac512_cl + OpenCL_Pbkdf2.pbkdf2_cl + OpenCL_Pbkdf2.pbkdf2_variants;
            }
            else {
                code = Bip39_Solver.common_cl + Bip39_Solver_Sha.sha2_cl + Bip39_Solver.int_to_address_cl;
            }
            
            code = code.Replace("<hashBlockSize_bits>", "1024");
            code = code.Replace("<hashDigestSize_bits>", "512");
            code = code.Replace("<inBufferSize_bytes>", $"{inBufferSize}");
            code = code.Replace("<outBufferSize_bytes>", $"{outBufferSize}");
            code = code.Replace("<saltBufferSize_bytes>", $"{saltBufferSize}");
            code = code.Replace("<word_size>", $"{wordSize}");
            code = code.Replace("<word_type>", wordSize == 8 ? "ulong" : "uint");

            Log.Info($"Compiling OpenCL PBKDF2 scripts (keylen={dklen})...");
            foreach (var k in pbkdf2_kernels.Values) k?.Dispose();
            pbkdf2_kernels.Clear();
            program_pbkdf2?.Dispose();
            program_pbkdf2 = context.CreateAndBuildProgramFromString(code, "-cl-std=CL2.0");
            // Log.Debug(code);
            Log.Info("OpenCL Compiled");
            program_pbkdf2_ready = true;
            usingDkLen = dklen;
        }

        public int GetBatchSize() {
            //  TODO: improve this
                        
            // return 0x40000 / outBufferSize;
            // return 48 * 1024;
            int batchSize = 256 * chosenDevice.MaximumComputeUnits;
            Log.Debug($"batchSize={batchSize}, out buffer size={batchSize*outBufferSize}");
            return batchSize;
        }

        public Seed[] Pbkdf2_Sha512_MultiPassword(Phrase[] phrases, string[] passphrases, byte[][] passwords, byte[] salt, bool final_hmac = true, int iters = 2048, int dklen = 64) {
            Init_Sha512(dklen);

            // Log.Debug($"password batch size={passwords.Length}");

            byte[] data = new byte[passwords.Length * (wordSize + inBufferSize)];
            using BinaryWriter w = new(new MemoryStream(data));
            for (int i = 0; i < passwords.Length; i++) {
                byte[] pb = passwords[i];
                if (pb.Length > inBufferSize) {
                        throw new Exception("phrase exceeds max length");
                }
                if (wordSize == 8) {
                    w.Write((ulong)pb.Length);
                }
                else {
                    w.Write((uint)pb.Length);
                }
                w.Write(pb);
                w.BaseStream.Position += inBufferSize - pb.Length;
            }
            w.Close();

            if (salt.Length > saltBufferSize) {
                throw new Exception($"passphrase length {salt.Length}, max length {saltBufferSize} set incorrectly");
            }
            byte[] saltData = new byte[wordSize + saltBufferSize];
            using BinaryWriter w2 = new(new MemoryStream(saltData));
            if (wordSize == 8) {
                w2.Write((ulong)salt.Length);
            }
            else {
                w2.Write((uint)salt.Length);
            }
            w2.Write(salt);
            w2.Close();

            // Log.Debug($"data: {data.ToHexString()}");
            // Log.Debug($"saltData: {saltData.ToHexString()}");

            byte[] result = RunKernel($"pbkdf2_{iters}_{dklen}{(final_hmac ? "_final_hmac" : "")}", data, saltData, passwords.Length);

            if (Global.Done) return null;

            // Console.WriteLine($"ocl: {result.ToHexString()}");

            Seed[] retval = new Seed[passwords.Length];
            using BinaryReader r = new BinaryReader(new MemoryStream(result));
            for (int i = 0; i < passwords.Length; i++) {
                byte[] seed = r.ReadBytes(dklen);
                if (outBufferSize > dklen) r.BaseStream.Position += (outBufferSize - dklen);

                /*
                //  debug
                #if DEBUG
                bool allZero = true;
                for (int j = 0; j < seed.Length; j++) {
                    if (seed[j] != 0) {
                        allZero = false;
                        break;
                    }
                }
                if (allZero) {
                    throw new Exception($"seed {i} is all zero");
                }
                #endif
                */

                // Console.WriteLine($"seed: {seed.ToHexString()}");
                if (phrases.Length == passwords.Length) {
                    retval[i] = new Seed(seed, phrases[i], passphrases[0]);
                }
                else {
                    retval[i] = new Seed(seed, phrases[0], passphrases[i]);
                }
            }
            return retval;
        }

        private byte[] RunKernel(string kernelName, byte[] data, byte[] saltData, int count) {
            
            Kernel kernel;
            if (pbkdf2_kernels.ContainsKey(kernelName)) {
                kernel = pbkdf2_kernels[kernelName];
            }
            else {
                try {
                    kernel = program_pbkdf2.CreateKernel(kernelName);
                    pbkdf2_kernels[kernelName] = kernel;
                }
                catch (Exception e) {
                    Log.Error(e.ToString() + $"\nkernel name: {kernelName}");
                    throw;
                }
            }
            
            if (Global.Done) return null;

            try {
                int outSize = outBufferSize * count;
                using MemoryBuffer inBuffer = context.CreateBuffer(MemoryFlag.ReadOnly | MemoryFlag.CopyHostPointer, data);
                using MemoryBuffer saltBuffer = context.CreateBuffer(MemoryFlag.ReadOnly | MemoryFlag.CopyHostPointer, saltData);
                using MemoryBuffer outBuffer = context.CreateBuffer<byte>(MemoryFlag.WriteOnly, outSize);

                lock (kernel) {
                    kernel.SetKernelArgument(0, inBuffer);
                    kernel.SetKernelArgument(1, saltBuffer);
                    kernel.SetKernelArgument(2, outBuffer);
                    commandQueue.EnqueueNDRangeKernel(kernel, 1, count);
                }
                Interlocked.Increment(ref kernelsRunning);
                byte[] r = commandQueue.EnqueueReadBuffer<byte>(outBuffer, outSize);
                Interlocked.Decrement(ref kernelsRunning);

                return r;
            }
            catch (Exception e) {
                if (Global.Done) return null;

                Log.Error(e.ToString() + $"\nkernel name: {kernelName}");
                throw;
            }
        }

        public Seed[] Pbkdf2_Sha512_MultiSalt(Phrase[] phrases, string[] passphrases, byte[] password, byte[][] salts, bool final_hmac = true, int iters = 2048, int dklen = 64) {
            Init_Sha512(dklen);
            // Log.Debug($"salt batch size={salts.Length}");

            byte[] data = new byte[wordSize + inBufferSize];
            using BinaryWriter w = new(new MemoryStream(data));
            if (password.Length > inBufferSize) {
                throw new Exception("phrase exceeds max length");
            }
            if (wordSize == 8) {
                w.Write((ulong)password.Length);
            }
            else {
                w.Write((uint)password.Length);
            }
            w.Write(password);
            w.Close();

            byte[] saltData = new byte[salts.Length * (wordSize + saltBufferSize)];
            using BinaryWriter w2 = new(new MemoryStream(saltData));
            for (int i = 0; i < salts.Length; i++) {
                if (salts[i].Length > saltBufferSize) {
                    throw new Exception($"passphrase length {salts[i].Length}, max length {saltBufferSize} set incorrectly");
                }
                if (wordSize == 8) {
                    w2.Write((ulong)salts[i].Length);
                }
                else {
                    w2.Write((uint)salts[i].Length);
                }
                w2.Write(salts[i]);
                w2.BaseStream.Position += saltBufferSize - salts[i].Length;
            }
            w2.Close();

            // Log.Debug($"data: {data.ToHexString()}");
            // Log.Debug($"saltData: {saltData.ToHexString()}");

            byte[] result = RunKernel($"pbkdf2_saltlist_{iters}_{dklen}{(final_hmac ? "_final_hmac" : "")}", data, saltData, salts.Length);

            if (Global.Done) return null;
            // Console.WriteLine($"ocl: {result.ToHexString()}");

            Seed[] retval = new Seed[salts.Length];
            using BinaryReader r = new BinaryReader(new MemoryStream(result));
            for (int i = 0; i < salts.Length; i++) {
                byte[] seed = r.ReadBytes(dklen);
                if (outBufferSize > dklen) r.BaseStream.Position += (outBufferSize - dklen);

                /*
                //  debug
                #if DEBUG
                bool allZero = true;
                for (int j = 0; j < seed.Length; j++) {
                    if (seed[j] != 0) {
                        allZero = false;
                        break;
                    }
                }
                if (allZero) {
                    throw new Exception($"seed {i} is all zero");
                }
                #endif
                */

                if (phrases.Length == salts.Length) {
                    retval[i] = new Seed(seed, phrases[i], passphrases[0]);
                }
                else {
                    retval[i] = new Seed(seed, phrases[0], passphrases[i]);
                }
            }
            return retval;
        }

        public Cryptography.Key[] Bip32DeriveFromRoot(byte[][] passwords, byte[][] salts, uint[] paths, int iters = 2048, int dklen = 64) {
            Init_Bip32Derive(dklen);
            // Log.Debug($"salt batch size={salts.Length}");

            int count = Math.Max(passwords.Length, salts.Length);

            byte[] data = new byte[passwords.Length * (wordSize + inBufferSize)];
            using BinaryWriter w = new(new MemoryStream(data));
            for (int i = 0; i < passwords.Length; i++) {
                byte[] pb = passwords[i];
                if (pb.Length > inBufferSize) {
                        throw new Exception("phrase exceeds max length");
                }
                if (wordSize == 8) {
                    w.Write((ulong)pb.Length);
                }
                else {
                    w.Write((uint)pb.Length);
                }
                w.Write(pb);
                w.BaseStream.Position += inBufferSize - pb.Length;
                }
            w.Close();

            byte[] saltData = new byte[salts.Length * (wordSize + saltBufferSize)];
            using BinaryWriter w2 = new(new MemoryStream(saltData));
            for (int i = 0; i < salts.Length; i++) {
                if (salts[i].Length > saltBufferSize) {
                    throw new Exception($"passphrase length {salts[i].Length}, max length {saltBufferSize} set incorrectly");
                }
                if (wordSize == 8) {
                    w2.Write((ulong)salts[i].Length);
                }
                else {
                    w2.Write((uint)salts[i].Length);
                }
                w2.Write(salts[i]);
                w2.BaseStream.Position += saltBufferSize - salts[i].Length;
            }
            w2.Close();

            byte[] pathData = new byte[wordSize + 8 * 4];
            using BinaryWriter w3 = new(new MemoryStream(pathData));
            w3.Write((uint)paths.Length);
            for (int i = 0; i < paths.Length; i++) {
                w3.Write((uint)paths[i]);
            }
            w3.Close();

            // Log.Debug($"data: {data.ToHexString()}");
            // Log.Debug($"saltData: {saltData.ToHexString()}");
            // Log.Debug($"pathData: {pathData.ToHexString()}");

            if (Global.Done) return null;
            // Console.WriteLine($"ocl: {result.ToHexString()}");

            string kernelName = "bip32_derive_path";

            try {
                
                Kernel kernel;
                if (bip32_kernels.ContainsKey(kernelName)) {
                    kernel = bip32_kernels[kernelName];
                }
                else {
                    kernel = program_bip32derive.CreateKernel(kernelName);
                    bip32_kernels[kernelName] = kernel;
                }
                
                if (Global.Done) return null;

                int outSize = dklen * count;
                int[] mode = { (passwords.Length > 1 ? 0 : 1) };
                using MemoryBuffer inBuffer = context.CreateBuffer(MemoryFlag.ReadOnly | MemoryFlag.CopyHostPointer, data);
                using MemoryBuffer saltBuffer = context.CreateBuffer(MemoryFlag.ReadOnly | MemoryFlag.CopyHostPointer, saltData);
                using MemoryBuffer pathBuffer = context.CreateBuffer(MemoryFlag.ReadOnly | MemoryFlag.CopyHostPointer, pathData);
                using MemoryBuffer modeBuffer = context.CreateBuffer(MemoryFlag.ReadOnly | MemoryFlag.CopyHostPointer, mode);
                using MemoryBuffer outBuffer = context.CreateBuffer<byte>(MemoryFlag.WriteOnly, outSize);

                lock (kernel) {
                    kernel.SetKernelArgument(0, inBuffer);
                    kernel.SetKernelArgument(1, saltBuffer);
                    kernel.SetKernelArgument(2, pathBuffer);
                    kernel.SetKernelArgument(3, modeBuffer);
                    kernel.SetKernelArgument(4, outBuffer);
                    commandQueue.EnqueueNDRangeKernel(kernel, 1, count);
                }
                Interlocked.Increment(ref kernelsRunning);
                byte[] result = commandQueue.EnqueueReadBuffer<byte>(outBuffer, outSize);
                Interlocked.Decrement(ref kernelsRunning);

                if (Global.Done) return null;

                Cryptography.Key[] ret = new Cryptography.Key[count];
                using BinaryReader r = new BinaryReader(new MemoryStream(result));
                for (int i = 0; i < count; i++) {
                    ret[i] = new Cryptography.Key(r.ReadBytes(32), r.ReadBytes(32));
                }
                return ret;
            }
            catch (Exception e) {
                if (Global.Done) return null;

                Log.Error(e.ToString());
                throw;
            }
        }

        public void Init_Bip32Derive(int dklen = 64) {
            if (program_bip32derive_ready) return;

            program_bip32derive_ready = false;
            outBufferSize = (dklen > 64) ? 128 : 64;
            wordSize = 8;
            saltBufferSize = maxPassphraseLength + 8;   //  +8 for "mnemonic"
            if (saltBufferSize % wordSize != 0) saltBufferSize += wordSize - (saltBufferSize % wordSize);

            string code = Bip39_Solver.common_cl + Bip39_Solver_Secp256k1.secp256k1_common_cl + Bip39_Solver_Secp256k1.secp256k1_scalar_cl + Bip39_Solver_Secp256k1.secp256k1_field_cl + Bip39_Solver_Secp256k1.secp256k1_group_cl + Bip39_Solver_Secp256k1.secp256k1_preq_cl + Bip39_Solver_Secp256k1.secp256k1_cl + Bip39_Solver_Sha.sha2_cl + Bip39_Solver.address_cl;

            code = code.Replace("<hashBlockSize_bits>", "1024");
            code = code.Replace("<hashDigestSize_bits>", "512");
            code = code.Replace("<inBufferSize_bytes>", $"{inBufferSize}");
            code = code.Replace("<outBufferSize_bytes>", $"{outBufferSize}");
            code = code.Replace("<saltBufferSize_bytes>", $"{saltBufferSize}");
            code = code.Replace("<word_size>", $"{wordSize}");
            code = code.Replace("<word_type>", wordSize == 8 ? "ulong" : "uint");

            Log.Info("Compiling OpenCL BIP32 Derive scripts...");
            foreach (var k in bip32_kernels.Values) k?.Dispose();
            bip32_kernels.Clear();
            program_bip32derive?.Dispose();
            program_bip32derive = context.CreateAndBuildProgramFromString(code, "-cl-std=CL2.0");
            Log.Info("OpenCL Compiled");
            program_bip32derive_ready = true;
        }

        public Cryptography.Key[] Bip32_Derive(Cryptography.Key[] keys, uint path, int keyLen = 32, int ccLen = 32) {
            Init_Bip32Derive();

            int length = keyLen + ccLen;
            int count = keys.Length;
            byte[] data = new byte[count * length];
            using BinaryWriter w = new(new MemoryStream(data));
            for (int i = 0; i < count; i++) {
                w.Write(keys[i].data);
                w.Write(keys[i].cc);
            }
            w.Close();

            string kernelName;

            if (PathNode.IsHardened(path)) {
                kernelName = "bip32_derive_hardened";
            }
            else {
                kernelName = "bip32_derive_normal";
            }

            uint[] paths = new uint[] { path };

            try {
                
                Kernel kernel;
                if (bip32_kernels.ContainsKey(kernelName)) {
                    kernel = bip32_kernels[kernelName];
                }
                else {
                    kernel = program_bip32derive.CreateKernel(kernelName);
                    bip32_kernels[kernelName] = kernel;
                }
                
                if (Global.Done) return null;

                int outSize = length * count;
                using MemoryBuffer inBuffer = context.CreateBuffer(MemoryFlag.ReadOnly | MemoryFlag.CopyHostPointer, data);
                using MemoryBuffer pathBuffer = context.CreateBuffer(MemoryFlag.ReadOnly | MemoryFlag.CopyHostPointer, paths);
                using MemoryBuffer outBuffer = context.CreateBuffer<byte>(MemoryFlag.WriteOnly, outSize);

                lock (kernel) {
                    kernel.SetKernelArgument(0, inBuffer);
                    kernel.SetKernelArgument(1, outBuffer);
                    kernel.SetKernelArgument(2, pathBuffer);
                    commandQueue.EnqueueNDRangeKernel(kernel, 1, count);
                }
                Interlocked.Increment(ref kernelsRunning);
                byte[] result = commandQueue.EnqueueReadBuffer<byte>(outBuffer, outSize);
                Interlocked.Decrement(ref kernelsRunning);

                if (Global.Done) return null;

                Cryptography.Key[] ret = new Cryptography.Key[count];
                using BinaryReader r = new BinaryReader(new MemoryStream(result));
                for (int i = 0; i < count; i++) {
                    ret[i] = new Cryptography.Key(r.ReadBytes(keyLen), r.ReadBytes(ccLen));

                    /*
                    //  debug
                    #if DEBUG
                    bool allZero = true;
                    for (int j = 0; j < 32; j++) {
                        if (ret[i].data[j] != 0) {
                            allZero = false;
                            break;
                        }
                    }
                    if (allZero) {
                        throw new Exception($"key {i} is all zero");
                    }
                    #endif
                    */
                }
                return ret;
            }
            catch (Exception e) {
                if (Global.Done) return null;

                Log.Error(e.ToString());
                throw;
            }
        }

        public void Benchmark_Bip32Derive(uint path, int count = 4096, Dictionary<string, int> results = null)
        {
            Log.Debug($"Benchmark_Bip32Derive({PathNode.GetPath(path)})");
            Cryptography.Key[] src = new Cryptography.Key[count];
            ExtKey[] ex = new ExtKey[count];
            ExtKey[] child = new ExtKey[count];
            Parallel.For(0, count, i => {
                Phrase p = new Phrase();
                byte[] seed = Cryptography.Pbkdf2_HMAC512(p.ToPhrase(), Cryptography.PassphraseToSalt(""), 2048, 64);
                ex[i] = ExtKey.CreateFromSeed(seed);

                src[i] = new(ex[i].PrivateKey.ToBytes(), ex[i].ChainCode);
            });

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Parallel.For(0, count, i => {
                child[i] = ex[i].Derive(path);
            });
            sw.Stop();
            long cpu = sw.ElapsedMilliseconds;

            Cryptography.Key[] keys;
            
            Init_Bip32Derive();
            sw.Restart();
            keys = Bip32_Derive(src, path);
            sw.Stop();
            long ocl = sw.ElapsedMilliseconds;

            int badkey = 0, badcc = 0;
            for (int i = 0; i < count; i++) {
                if (keys[i].data.ToHexString() != child[i].PrivateKey.ToBytes().ToHexString()) {
                    badkey++;
                    // Log.Error($"SK{i} OCL {keys[i].data.ToHexString()} != ExtKey {child[i].PrivateKey.ToBytes().ToHexString()}");
                }
                if (keys[i].cc.ToHexString() != child[i].ChainCode.ToHexString()) {
                    badcc++;
                    // Log.Error($"CC{i} OCL {keys[i].cc.ToHexString()} != ExtKey {child[i].ChainCode.ToHexString()}");
                }
            }
            Log.Debug($"Benchmark_Bip32Derive({path}) P:{platformId} D:{deviceId} cputime={cpu} ocltime={ocl} badkey={badkey} badcc={badcc}");
            if (badkey > 0 || badcc > 0) throw new Exception("bad results in Benchmark_Bip32Derive()");
            Log.Debug($"CPU derives/s: {count/(cpu/1000.0):F0}");
            Log.Debug($"P:{platformId} D:{deviceId} derives/s: {count/(ocl/1000.0):F0}");
            if (results != null) {
                results["cpuSecDerives"] = (int)(count/(cpu/1000.0));
                results["oclSecDerives"] = (int)(count/(ocl/1000.0));
            }
        }

        public void Benchmark_Bip32DerivePath(string path = null, int count = 4096, Dictionary<string, int> results = null)
        {
            Log.Debug($"Benchmark_Bip32DerivePath()");
            if (path == null) path = "m/44'/0'/0'/0/0";
            Phrase phrase = new Phrase(12);
            string[] passphrases = new string[count];
            for (int i = 0; i < count; i++) {
                passphrases[i] = $"passphrase{i}";
            }
            PhraseToAddress p2a = PhraseToAddress.Create(CoinType.BTC, null, null);
            Stopwatch sw = new Stopwatch();

            p2a.SetOpenCL(null);
            List<Address>[] addressesC = new List<Address>[Settings.Threads];
            sw.Start();
            Parallel.For(0, addressesC.Length, i => {
                addressesC[i] = p2a.GetAddressesList(phrase, passphrases, new string[] { path }, new int[] { 0 }, new int[] { 0 });
            });
            sw.Stop();
            long cpu = sw.ElapsedMilliseconds;

            p2a.SetOpenCL(this);
            Init_Bip32Derive();
            List<Address>[] addressesO = new List<Address>[addressesC.Length];
            sw.Restart();
            Parallel.For(0, addressesC.Length, i => {
                addressesO[i] = p2a.GetAddressesList(phrase, passphrases, new string[] { path }, new int[] { 0 }, new int[] { 0 });
            });
            sw.Stop();
            long ocl = sw.ElapsedMilliseconds;

            int badaddr = 0;
            count = 0;
            for (int i = 0; i < addressesC.Length; i++) {
                for (int j = 0; j < addressesC[i].Count; j++) {
                    count++;
                    if (!addressesC[i][j].Equals(addressesO[i][j])) {
                        badaddr++;
                        Log.Debug($"cpu={addressesC[i][j]} != ocl={addressesO[i][j]}");
                    }
                }
            }
            Log.Debug($"Benchmark_Bip32DerivePath({path}) P:{platformId} D:{deviceId} cputime={cpu} ocltime={ocl} badaddr={badaddr}");
            if (badaddr > 0) throw new Exception("bad results in Benchmark_Bip32DerivePath()");
            Log.Debug($"CPU path/s: {count/(cpu/1000.0):F0}");
            Log.Debug($"P:{platformId} D:{deviceId} path/s: {count/(ocl/1000.0):F0}");
            if (results != null) {
                results["cpuSecPaths"] = (int)(count/(cpu/1000.0));
                results["oclSecPaths"] = (int)(count/(ocl/1000.0));
            }
        }

        public void Benchmark_Pbkdf2(int tcount = 4096, Dictionary<string, int> results = null)
        {
            Wordlists.Initialize();

            {
                Log.Debug($"Benchmark OpenCL phrases...");
                Init_Sha512();
                Phrase[] ph = new Phrase[tcount];
                byte[][] passwords = new byte[tcount][];
                for (int i = 0; i < tcount; i++) ph[i] = new Phrase(12);
                for (int i = 0; i < tcount; i++) passwords[i] = ph[i].ToPhrase().ToUTF8Bytes();
                string pp = "";
                byte[] salt = Cryptography.PassphraseToSalt(pp);
                Stopwatch oclsw = new Stopwatch();
                oclsw.Start();
                Seed[] oclseeds = Pbkdf2_Sha512_MultiPassword(ph, new string[] { pp }, passwords, salt, false);
                oclsw.Stop();
                long ocltime = oclsw.ElapsedMilliseconds;
                int badcount = 0;
                Seed[] cpuseeds = new Seed[tcount];
                oclsw.Restart();
                Parallel.For(0, tcount, i => 
                {
                    cpuseeds[i] = new Seed(Cryptography.Pbkdf2_HMAC512(ph[i].ToPhrase(), salt, 2048, 64), ph[i], pp);
                });
                oclsw.Stop();
                long cputime = oclsw.ElapsedMilliseconds;

                for (int i = 0; i < tcount; i++)
                {
                    if (cpuseeds[i].seed.ToHexString() != oclseeds[i].seed.ToHexString())
                    {
                        // Console.WriteLine($"phrase {i}: {ph[i].ToPhrase()}\ncpu: {cpuseeds[i].seed.ToHexString()}\nocl: {((byte[])(oclseeds[i].seed)).ToHexString()}");
                        badcount++;
                    }
                }
                Log.Debug($"Benchmark_Pbkdf2() phrases P:{platformId} D:{deviceId} cputime={cputime} ocltime={ocltime} badcount={badcount}");
                if (badcount > 0) throw new SystemException("failed Benchmark_Pbkdf2() phrases");
                Log.Debug($"CPU phrases/s: {tcount/(cputime/1000.0):F0}");
                Log.Debug($"P:{platformId} D:{deviceId} phrases/s: {tcount/(ocltime/1000.0):F0}");
                if (results != null) {
                    results["cpuPhrases"] = (int)(tcount/(cputime/1000.0));
                    results["oclPhrases"] = (int)(tcount/(ocltime/1000.0));
                }
            }

            {
                Log.Debug($"Benchmark OpenCL passphrases...");
                Init_Sha512();
                Phrase ph = new Phrase(12);
                string phrase = ph.ToPhrase();
                byte[] password = phrase.ToUTF8Bytes();
                string[] pp = new string[tcount];
                for (int i = 0; i < tcount; i++) {
                    pp[i] = $"{i}";
                }
                byte[][] salts = new byte[tcount][];
                for (int i = 0; i < tcount; i++) salts[i] = Cryptography.PassphraseToSalt(pp[i]);
                Stopwatch oclsw = new Stopwatch();
                oclsw.Start();
                Seed[] oclseeds = Pbkdf2_Sha512_MultiSalt(new Phrase[] { ph }, pp, password, salts, false);
                oclsw.Stop();
                long ocltime = oclsw.ElapsedMilliseconds;
                int badcount = 0;
                Seed[] cpuseeds = new Seed[tcount];
                oclsw.Restart();
                Parallel.For(0, tcount, i => 
                {
                    byte[] salt = Cryptography.PassphraseToSalt(pp[i]);
                    cpuseeds[i] = new Seed(Cryptography.Pbkdf2_HMAC512(phrase, salt, 2048, 64), ph, pp[i]);
                });
                oclsw.Stop();
                long cputime = oclsw.ElapsedMilliseconds;

                for (int i = 0; i < tcount; i++)
                {
                    if (cpuseeds[i].seed.ToHexString() != oclseeds[i].seed.ToHexString())
                    {
                        // Console.WriteLine($"pp {i}: {pp[i]}\ncpu: {cpuseeds[i].seed.ToHexString()}\nocl: {((byte[])(oclseeds[i].seed)).ToHexString()}");
                        badcount++;
                    }
                }
                Log.Debug($"Benchmark_Pbkdf2() P:{platformId} D:{deviceId} passphrases cputime={cputime} ocltime={ocltime} badcount={badcount}");
                if (badcount > 0) throw new SystemException("failed Benchmark_Pbkdf2() passphrases");
                Log.Debug($"CPU passphrases/s: {tcount/(cputime/1000.0):F0}");
                Log.Debug($"P:{platformId} D:{deviceId} passphrases/s: {tcount/(ocltime/1000.0):F0}");
                if (results != null) {
                    results["cpuPassphrases"] = (int)(tcount/(cputime/1000.0));
                    results["oclPassphrases"] = (int)(tcount/(ocltime/1000.0));
                }
            }

        }

        public static void LogOpenCLInfo() {
            IEnumerable<Platform> platforms = Platform.GetPlatforms();
            ConsoleTable consoleTable = new ConsoleTable("Platform", "Device", "Global Memory", "Local Memory", "CUs");
            int p = 0;
            foreach (Platform platform in platforms)
            {
                int d = 0;
                foreach (Device device in platform.GetDevices(DeviceType.All))
                {
                    if (!device.IsAvailable) continue;

                    consoleTable.AddRow(
                        p + ": " + platform.Name,
                        // $"{platform.Version.MajorVersion}.{platform.Version.MinorVersion}",
                        d + ": " + device.Name,
                        // device.DriverVersion,
                        // $"{device.AddressBits} Bit",
                        $"{Math.Round(device.GlobalMemorySize / 1024.0f / 1024.0f / 1024.0f, 2)} GiB",
                        $"{Math.Round(device.LocalMemorySize / 1024.0f, 2)} KiB",
                        // $"{device.MaximumClockFrequency} MHz",
                        device.MaximumComputeUnits
                        // device.IsAvailable ? "✔" : "✖"
                        );

                        d++;
                }
                p++;
            }
            Log.Info("OpenCL Supported Platforms & Devices:");
            Log.Info(consoleTable.ToStringAlternative());
        }
    }
}