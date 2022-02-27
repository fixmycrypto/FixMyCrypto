
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using ConsoleTables;

namespace FixMyCrypto {

    public class OpenCL {

        private Context context;
        private Program program;

        private bool programReady = false;

        private int saltBufferSize;     //  max char length of a passphrase

        private int inBufferSize = 256; //  max char length of a phrase

        private int pwdBufferSize;

        private int outBufferSize;

        private int wordSize;

        private Device chosenDevice;


        // private CommandQueue commandQueue;

        public OpenCL(int platformId = 0, int deviceId = 0, int maxPassphraseLength = 32) {
            LogOpenCLInfo();
            if (platformId < 0 || deviceId < 0) throw new ArgumentException();

            IEnumerable<Platform> platforms = Platform.GetPlatforms();
            chosenDevice = platforms.ToList()[platformId].GetDevices(DeviceType.All).ToList()[deviceId];
            Log.Info($"Selected device ({platformId}, {deviceId}): {chosenDevice.Name} ({chosenDevice.Vendor})");
            context = Context.CreateContext(chosenDevice);            

            saltBufferSize = 8 + maxPassphraseLength;   //  "mnemonic" + passphrase
        }

        public void Init(int dklen = 64) {
            outBufferSize = dklen;
            wordSize = (dklen > 32) ? 8 : 4;
            pwdBufferSize = inBufferSize;

            // Creates a program and then the kernel from it
            string code = buffer_structs_template_cl + hmac512_cl + pbkdf2_cl + pbkdf2_variants;
            code = code.Replace("<hashBlockSize_bits>", "1024");
            code = code.Replace("<hashDigestSize_bits>", "512");
            code = code.Replace("<inBufferSize_bytes>", $"{inBufferSize}");
            code = code.Replace("<outBufferSize_bytes>", $"{outBufferSize}");
            code = code.Replace("<saltBufferSize_bytes>", $"{saltBufferSize}");
            code = code.Replace("<pwdBufferSize_bytes>", $"{pwdBufferSize}");
            code = code.Replace("<ctBufferSize_bytes>", $"{saltBufferSize}");
            code = code.Replace("<word_size>", $"{wordSize}");

            Log.Info("Compiling OpenCL scripts...");
            program = context.CreateAndBuildProgramFromString(code);
            Log.Info("OpenCL Compiled");
            programReady = true;
        }

        public int GetBatchSize() {
            //  TODO
            return 16384;
        }

        public Seed[] Pbkdf2_Sha512_MultiPassword(Phrase[] phrases, string[] passphrases, byte[][] passwords, byte[] salt, int iters = 2048, int dklen = 64) {
            if (!programReady) Init(dklen);

            Log.Debug($"password batch size={passwords.Length}");

            byte[] data = new byte[passwords.Length * (wordSize + inBufferSize)];
            BinaryWriter w = new(new MemoryStream(data));
            for (int i = 0; i < passwords.Length; i++) {
                byte[] pb = passwords[i];
                if (pb.Length > inBufferSize) {
                        throw new Exception("phrase exceeds max length");
                }
                w.Write((ulong)pb.Length);
                w.Write(pb);
                w.Write(new byte[inBufferSize - pb.Length]);
            }
            w.Close();

            if (salt.Length > saltBufferSize) {
                throw new Exception("passphrase max length set incorrectly");
            }
            byte[] saltData = new byte[wordSize + saltBufferSize];
            BinaryWriter w2 = new(new MemoryStream(saltData));
            w2.Write((ulong)salt.Length);
            w2.Write(salt);
            w2.Write(new byte[saltBufferSize - salt.Length]);
            w2.Close();

            // Log.Debug($"data: {data.ToHexString()}");
            // Log.Debug($"saltData: {saltData.ToHexString()}");

            byte[] result = RunKernel($"pbkdf2_{iters}_{dklen}", data, saltData, passwords.Length);

            // Console.WriteLine($"ocl: {result.ToHexString()}");

            Seed[] retval = new Seed[passwords.Length];
            BinaryReader r = new BinaryReader(new MemoryStream(result));
            for (int i = 0; i < passwords.Length; i++) {
                byte[] seed = r.ReadBytes(outBufferSize);
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
            using Kernel kernel = program.CreateKernel(kernelName);
            byte[] result = null;

            try {
                MemoryBuffer inBuffer = context.CreateBuffer(MemoryFlag.ReadOnly | MemoryFlag.CopyHostPointer, data);
                MemoryBuffer saltBuffer = context.CreateBuffer(MemoryFlag.ReadOnly | MemoryFlag.CopyHostPointer, saltData);
                int outSize = outBufferSize * count;
                MemoryBuffer outBuffer = context.CreateBuffer<byte>(MemoryFlag.WriteOnly, outSize);

                using (CommandQueue commandQueue = CommandQueue.CreateCommandQueue(context, chosenDevice)) {
                    kernel.SetKernelArgument(0, inBuffer);
                    kernel.SetKernelArgument(1, saltBuffer);
                    kernel.SetKernelArgument(2, outBuffer);
                    commandQueue.EnqueueNDRangeKernel(kernel, 1, count);
                    result = commandQueue.EnqueueReadBuffer<byte>(outBuffer, outSize);
                }

                inBuffer.Dispose();
                saltBuffer.Dispose();
                outBuffer.Dispose();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                throw;
            }

            return result;
        }

        public Seed[] Pbkdf2_Sha512_MultiSalt(Phrase[] phrases, string[] passphrases, byte[] password, byte[][] salts, int iters = 2048, int dklen = 64) {
            if (!programReady) Init(dklen);
            Log.Debug($"salt batch size={salts.Length}");

            byte[] data = new byte[wordSize + pwdBufferSize];
            BinaryWriter w = new(new MemoryStream(data));
            if (password.Length > pwdBufferSize) {
                    throw new Exception("phrase exceeds max length");
            }
            w.Write((ulong)password.Length);
            w.Write(password);
            w.Write(new byte[pwdBufferSize - password.Length]);
            w.Close();

            byte[] saltData = new byte[salts.Length * (wordSize + inBufferSize)];
            BinaryWriter w2 = new(new MemoryStream(saltData));
            for (int i = 0; i < salts.Length; i++) {
                if (salts[i].Length > inBufferSize) {
                    throw new Exception("passphrase max length set incorrectly");
                }
                w2.Write((ulong)salts[i].Length);
                w2.Write(salts[i]);
                w2.Write(new byte[inBufferSize - salts[i].Length]);
            }
            w2.Close();

            // Log.Debug($"data: {data.ToHexString()}");
            // Log.Debug($"saltData: {saltData.ToHexString()}");

            byte[] result = RunKernel($"pbkdf2_saltlist_{iters}_{dklen}", data, saltData, salts.Length);

            // Console.WriteLine($"ocl: {result.ToHexString()}");

            Seed[] retval = new Seed[salts.Length];
            BinaryReader r = new BinaryReader(new MemoryStream(result));
            for (int i = 0; i < salts.Length; i++) {
                byte[] seed = r.ReadBytes(outBufferSize);
                if (phrases.Length == salts.Length) {
                    retval[i] = new Seed(seed, phrases[i], passphrases[0]);
                }
                else {
                    retval[i] = new Seed(seed, phrases[0], passphrases[i]);
                }
            }
            return retval;
        }

        public void Benchmark(int tcount = 20480)
        {
            Wordlists.Initialize();

            {
                Log.Debug("Benchmark OpenCL phrases...");
                Phrase[] ph = new Phrase[tcount];
                byte[][] passwords = new byte[tcount][];
                for (int i = 0; i < tcount; i++) ph[i] = new Phrase();
                for (int i = 0; i < tcount; i++) passwords[i] = ph[i].ToPhrase().ToUTF8Bytes();
                string pp = "";
                byte[] salt = Cryptography.PassphraseToSalt(pp);
                Stopwatch oclsw = new Stopwatch();
                oclsw.Start();
                Seed[] oclseeds = Pbkdf2_Sha512_MultiPassword(ph, new string[] { pp }, passwords, salt);
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
                Log.Debug($"badcount={badcount} ocltime={ocltime} cputime={cputime}");
            }

            {
                Log.Debug("Benchmark OpenCL passphrases...");
                Phrase ph = new Phrase();
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
                Seed[] oclseeds = Pbkdf2_Sha512_MultiSalt(new Phrase[] { ph }, pp, password, salts);
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
                Log.Debug($"badcount={badcount} ocltime={ocltime} cputime={cputime}");
            }

        }

        public static void LogOpenCLInfo() {
            IEnumerable<Platform> platforms = Platform.GetPlatforms();
            ConsoleTable consoleTable = new ConsoleTable("Platform", "OpenCL Version", "Vendor", "Device", "Driver Version", "Bits", "Memory", "Clock Speed", "Available");
            foreach (Platform platform in platforms)
            {
                foreach (Device device in platform.GetDevices(DeviceType.All))
                {
                    consoleTable.AddRow(
                        platform.Name,
                        $"{platform.Version.MajorVersion}.{platform.Version.MinorVersion}",
                        platform.Vendor,
                        device.Name,
                        device.DriverVersion,
                        $"{device.AddressBits} Bit",
                        $"{Math.Round(device.GlobalMemorySize / 1024.0f / 1024.0f / 1024.0f, 2)} GiB",
                        $"{device.MaximumClockFrequency} MHz",
                        device.IsAvailable ? "✔" : "✖");
                }
            }
            Log.Info("OpenCL Supported Platforms & Devices:");
            Log.Info(consoleTable.ToStringAlternative());
        }
        
        //  https://github.com/bkerler/opencl_brute
        private static string pbkdf2_cl = @"
/*
    pbkdf2 and HMAC implementation
    requires implementation of PRF (pseudo-random function),
      probably using HMAC and an implementation of hash_main
*/
/*
    REQ: outBuf.buffer must have space for ceil(dkLen / PRF_output_bytes) * PRF_output_bytes
    REQ: PRF implementation MUST allow that output may be the salt (m in hmac)
    inBuffer / pwdBuffer / the like are not const to allow for padding
*/

// Determine (statically) the actual required buffer size
// Correct for both 64 & 32 bit
//   Just allowing for MD padding: 2 words for length, 1 for the 1-pad = 3 words
#define sizeForHash(reqSize) (ceilDiv((reqSize) + 2 + 1, hashBlockSize) * hashBlockSize)

#if wordSize == 4
    __constant const unsigned int opad = 0x5c5c5c5c;
    __constant const unsigned int ipad = 0x36363636;
#elif wordSize == 8
    __constant const unsigned long opad = 0x5c5c5c5c5c5c5c5c;
    __constant const unsigned long ipad = 0x3636363636363636;
#endif

__constant const word xoredPad = opad ^ ipad;

// Slightly ugly: large enough for hmac_main usage, and tight for pbkdf2
#define m_buffer_size (saltBufferSize + 1)

static void hmac(__global word *K, const word K_len_bytes,
    const word *m, const word m_len_bytes, word *output)
{
    // REQ: If K_len_bytes isn't divisible by 4/8, final word should be clean (0s to the end)
    // REQ: s digestSize is a multiple of 4/8 bytes

    /* Declare the space for input to the last hash function:
         Compute and write K_ ^ opad to the first block of this. This will be the only place that we store K_ */

    #define size_2 sizeForHash(hashBlockSize + hashDigestSize)
    word input_2[size_2] = {0};
    #undef size_2

    word end;
    if (K_len_bytes <= hashBlockSize_bytes)
    {
        end = ceilDiv(K_len_bytes, wordSize);
        // XOR with opad and slightly pad with zeros..
        for (int j = 0; j < end; j++){
            input_2[j] = K[j] ^ opad;
        }
    } else {
        end = hashDigestSize;
        // Hash K to get K'. XOR with opad..
        hash_glbl_to_priv(K, K_len_bytes, input_2);
        for (int j = 0; j < hashDigestSize; j++){
            input_2[j] ^= opad;
        }
    }
    // And if short, pad with 0s to the BLOCKsize, completing xor with opad
    for (int j = end; j < hashBlockSize; j++){
        input_2[j] = opad;
    }

    // Copy K' ^ ipad into the first block.
    // Be careful: hash needs a whole block after the end. ceilDiv from buffer_structs
    #define size_1 sizeForHash(hashBlockSize + m_buffer_size)

    // K' ^ ipad into the first block
    word input_1[size_1] = {0};
    word temp_output[size_1] = {0};
    #undef size_1
    for (int j = 0; j < hashBlockSize; j++){
        input_1[j] = input_2[j]^xoredPad;
    }

    // Slightly inefficient copying m in..
    word m_len_word = ceilDiv(m_len_bytes, wordSize);
    for (int j = 0; j < m_len_word; j++){
        input_1[hashBlockSize + j] = m[j];
    }

    // Hash input1 into the second half of input2
    word leng = hashBlockSize_bytes + m_len_bytes;
    hash_private(input_1, leng, input_2 + hashBlockSize);

    // Hash input2 into output!
    hash_private(input_2, hashBlockSize_bytes + hashDigestSize_bytes, temp_output);
    for (int j = 0; j < hashBlockSize; j++){
        output[j] = temp_output[j];
    }
}

#undef sizeForHash


// PRF
#define PRF_output_size hashDigestSize
#define PRF_output_bytes (PRF_output_size * wordSize)
// Our PRF is the hmac using the hash. Commas remove need for bracketing
#define PRF(pwd, pwdLen_bytes, salt, saltLen_bytes, output) \
    hmac(pwd, pwdLen_bytes, salt, saltLen_bytes, output)


static void F(__global word *pwd, const word pwdLen_bytes,
    word *salt, const word saltLen_bytes,
    const unsigned int iters, unsigned int callI,
    __global word *output)
{
    // ASSUMPTION: salt array has wordSize bytes more room
    // Note salt is not const, so we can efficiently tweak the end of it

    // Add the integer to the end of the salt
    // NOTE! Always adding callI as just a u32
    word overhang = saltLen_bytes % wordSize;
    overhang *= 8; // convert to bits
    word saltLastI = saltLen_bytes / wordSize;

    // ! Crucial line: BE, moved as if it's a u32 but still within the word
    word be_callI = SWAP((word)callI) >> (8*(wordSize-4));
    if (overhang>0)
    {
        salt[saltLastI] |= be_callI << overhang;
        salt[saltLastI+1] = be_callI >> ((8*wordSize)-overhang);
    }
    else
    {
        salt[saltLastI]=be_callI;
    }

    // Make initial call, copy into output
    // This copy is avoidable, but only with __global / __private macro stuff
    word u[PRF_output_size] = {0};
    // +4 is correct even for 64 bit
    PRF(pwd, pwdLen_bytes, salt, saltLen_bytes + 4, u);
    for (unsigned int j = 0; j < PRF_output_size; j++){
        output[j] = u[j];
    }

    #define xor(x,acc)                                  \
    /* xors PRF output x onto acc*/                     \
    {                                                   \
        for (int k = 0; k < PRF_output_size; k++){     \
            acc[k] ^= x[k];                             \
        }                                               \
    }

    // Perform all the iterations, reading salt from- AND writing to- u.
    for (unsigned int j = 1; j < iters; j++){
        PRF(pwd, pwdLen_bytes, u, PRF_output_bytes, u);
        xor(u,output);
    }
}

__kernel void pbkdf2(__global inbuf *inbuffer, __global const saltbuf *saltbuffer, __global outbuf *outbuffer,
    __private unsigned int iters, __private unsigned int dkLen_bytes)
{

    unsigned int idx = get_global_id(0);
    word pwdLen_bytes = inbuffer[idx].length;
    __global word *pwdBuffer = inbuffer[idx].buffer;
    __global word *currOutBuffer = outbuffer[idx].buffer;

    // Copy salt so that we can write our integer into the last 4 bytes
    word saltLen_bytes = saltbuffer[0].length;
    int saltLen = ceilDiv(saltLen_bytes, wordSize);
    word personal_salt[saltBufferSize+2] = {0};

    for (int j = 0; j < saltLen; j++){
        personal_salt[j] = saltbuffer[0].buffer[j];
    }

    // Determine the number of calls to F that we need to make
    unsigned int nBlocks = ceilDiv(dkLen_bytes, PRF_output_bytes);
    for (unsigned int j = 1; j <= nBlocks; j++)
    {
        F(pwdBuffer, pwdLen_bytes, personal_salt, saltbuffer[0].length, iters, j, currOutBuffer);
        currOutBuffer += PRF_output_size;
    }
}


// Exposing HMAC in the same way. Useful for testing atleast.
__kernel void hmac_main(__global inbuf *inbuffer, __global const saltbuf *saltbuffer, __global outbuf *outbuffer)
{
    unsigned int idx = get_global_id(0);
    word pwdLen_bytes = inbuffer[idx].length;
    __global word *pwdBuffer = inbuffer[idx].buffer;

    // Copy salt just to cheer the compiler up
    int saltLen_bytes = (int)saltbuffer[0].length;
    int saltLen = ceilDiv(saltLen_bytes, wordSize);
    word personal_salt[saltBufferSize] = {0};

    for (int j = 0; j < saltLen; j++){
        personal_salt[j] = saltbuffer[0].buffer[j];
    }

    // Call hmac, with local
    word out[hashDigestSize];
    
    hmac(pwdBuffer, pwdLen_bytes, personal_salt, saltLen_bytes, out);

    for (int j = 0; j < hashDigestSize; j++){
        outbuffer[idx].buffer[j] = out[j];
    }
}

// A modified version of the pbkdf2 kernel that allows you to use these kernels in a situation where you have a password
// and are attempting to brute-force the salt. (So this kernel takes a single password and an array of salts
//
// Originally created for BTCRecover by Stephen Rothery, available at https://github.com/3rdIteration/btcrecover
//    MIT License

__kernel void pbkdf2_saltlist(__global const pwdbuf *pwdbuffer_arg, __global inbuf *inbuffer, __global outbuf *outbuffer,
    __private unsigned int iters, __private unsigned int dkLen_bytes)
{

	unsigned int idx = get_global_id(0);
    word pwdLen_bytes = pwdbuffer_arg[0].length;
    __global word *pwdBuffer = pwdbuffer_arg[0].buffer;
    __global word *currOutBuffer = outbuffer[idx].buffer;

    // Copy salt so that we can write our integer into the last 4 bytes
    word saltLen_bytes = inbuffer[idx].length;
    int saltLen = ceilDiv(saltLen_bytes, wordSize);
    word personal_salt[saltBufferSize+2] = {0};


    for (int j = 0; j < saltLen; j++){
        personal_salt[j] = inbuffer[idx].buffer[j];
    }

    // Determine the number of calls to F that we need to make
    unsigned int nBlocks = ceilDiv(dkLen_bytes, PRF_output_bytes);
    for (unsigned int j = 1; j <= nBlocks; j++)
    {
        F(pwdBuffer, pwdLen_bytes, personal_salt, saltLen_bytes, iters, j, currOutBuffer);
        currOutBuffer += PRF_output_size;
    }
}
";

static string pbkdf2_variants = @"
__kernel void pbkdf2_2048_64(__global inbuf *inbuffer, __global const saltbuf *saltbuffer, __global outbuf *outbuffer)
{
    pbkdf2(inbuffer, saltbuffer, outbuffer, 2048, 64);
}

__kernel void pbkdf2_saltlist_2048_64(__global const pwdbuf *pwdbuffer_arg, __global inbuf *inbuffer, __global outbuf *outbuffer)
{
    pbkdf2_saltlist(pwdbuffer_arg, inbuffer, outbuffer, 2048, 64);
}

__kernel void pbkdf2_4096_96(__global inbuf *inbuffer, __global const saltbuf *saltbuffer, __global outbuf *outbuffer)
{
    pbkdf2(inbuffer, saltbuffer, outbuffer, 4096, 96);
}

__kernel void pbkdf2_saltlist_4096_96(__global const pwdbuf *pwdbuffer_arg, __global inbuf *inbuffer, __global outbuf *outbuffer)
{
    pbkdf2_saltlist(pwdbuffer_arg, inbuffer, outbuffer, 4096, 96);
}
";

private static string hmac512_cl = @"
/*
    Original copyright (sha256):
    OpenCL Optimized kernel
    (c) B. Kerler 2018
    MIT License

    Adapted for SHA512 by C.B .. apparently quite a while ago
    The moral of the story is always use UL on unsigned longs!
*/



// bitselect is ""if c then b else a"" for each bit
// so equivalent to (c & b) | ((~c) & a)
#define choose(x,y,z)   (bitselect(z,y,x))
// Cleverly determines majority vote, conditioning on x=z
#define bit_maj(x,y,z)   (bitselect (x, y, ((x) ^ (z))))

// Hopefully rotate works for long too?




// ==============================================================================
// =========  S0,S1,s0,s1  ======================================================


#define S0(x) (rotr64(x,28ul) ^ rotr64(x,34ul) ^ rotr64(x,39ul))
#define S1(x) (rotr64(x,14ul) ^ rotr64(x,18ul) ^ rotr64(x,41ul))

#define little_s0(x) (rotr64(x,1ul) ^ rotr64(x,8ul) ^ ((x) >> 7ul))
#define little_s1(x) (rotr64(x,19ul) ^ rotr64(x,61ul) ^ ((x) >> 6ul))


// ==============================================================================
// =========  MD-pads the input, taken from md5.cl  =============================
// Adapted for unsigned longs
// Note that the padding is still in a distinct unsigned long to the appended length.


// 'highBit' macro is (i+1) bytes, all 0 but the last which is 0x80
//  where we are thinking Little-endian thoughts.
// Don't forget to call constants longs!!
#define highBit(i) (0x1UL << (8*i + 7))
#define fBytes(i) (0xFFFFFFFFFFFFFFFFUL >> (8 * (8-i)))
__constant unsigned long padLong[8] = {
    highBit(0), highBit(1), highBit(2), highBit(3),
    highBit(4), highBit(5), highBit(6), highBit(7)
};
__constant unsigned long maskLong[8] = {
    0, fBytes(1), fBytes(2), fBytes(3),     // strange behaviour for fBytes(0)
    fBytes(4), fBytes(5), fBytes(6), fBytes(7)
};

#define bs_long hashBlockSize_long64
#define def_md_pad_128(funcName, tag)               \
/* The standard padding, INPLACE,
    add a 1 bit, then little-endian original length mod 2^128 (not 64) at the end of a block
    RETURN number of blocks */                  \
static int funcName(tag unsigned long *msg, const long msgLen_bytes)      \
{                                                                       \
    /* Appends the 1 bit to the end, and 0s to the end of the byte */   \
    const unsigned int padLongIndex = ((unsigned int)msgLen_bytes) / 8;                \
    const unsigned int overhang = (((unsigned int)msgLen_bytes) - padLongIndex*8);     \
    /* Don't assume that there are zeros here! */                       \
    msg[padLongIndex] &= maskLong[overhang];                              \
    msg[padLongIndex] |= padLong[overhang];                               \
                                                                        \
    /* Previous code was horrible
        Now we zero until we reach a multiple of the block size,
        Skipping TWO longs to ensure there is room for the length */     \
    msg[padLongIndex + 1] = 0;                                          \
    msg[padLongIndex + 2] = 0;                                          \
    unsigned int i = 0;                                                 \
    for (i = padLongIndex + 3; i % bs_long != 0; i++)                   \
    {                                                                   \
        msg[i] = 0;                                                     \
    }                                                                   \
                                                                        \
    /* Determine the total number of blocks */                          \
    int nBlocks = i / bs_long;                                          \
    /* Add the bit length to the end, 128-bit, big endian? (source wikipedia)
       Seemingly this does require SWAPing, so perhaps it's little-endian? */           \
    msg[i-2] = 0;   /* For clarity */                                   \
    msg[i-1] = SWAP(msgLen_bytes*8);                                    \
                                                                        \
    return nBlocks;                                                     \
};                                                                      

// Define it with the various tags to cheer OpenCL up
def_md_pad_128(md_pad__global, __global)
def_md_pad_128(md_pad__private, __private)

#undef bs_long
#undef def_md_pad_128
#undef highBit
#undef fBytes




// ==============================================================================

__constant unsigned long k_sha256[80] =
{
    0x428a2f98d728ae22UL, 0x7137449123ef65cdUL, 0xb5c0fbcfec4d3b2fUL, 0xe9b5dba58189dbbcUL, 0x3956c25bf348b538UL, 
    0x59f111f1b605d019UL, 0x923f82a4af194f9bUL, 0xab1c5ed5da6d8118UL, 0xd807aa98a3030242UL, 0x12835b0145706fbeUL, 
    0x243185be4ee4b28cUL, 0x550c7dc3d5ffb4e2UL, 0x72be5d74f27b896fUL, 0x80deb1fe3b1696b1UL, 0x9bdc06a725c71235UL, 
    0xc19bf174cf692694UL, 0xe49b69c19ef14ad2UL, 0xefbe4786384f25e3UL, 0x0fc19dc68b8cd5b5UL, 0x240ca1cc77ac9c65UL, 
    0x2de92c6f592b0275UL, 0x4a7484aa6ea6e483UL, 0x5cb0a9dcbd41fbd4UL, 0x76f988da831153b5UL, 0x983e5152ee66dfabUL, 
    0xa831c66d2db43210UL, 0xb00327c898fb213fUL, 0xbf597fc7beef0ee4UL, 0xc6e00bf33da88fc2UL, 0xd5a79147930aa725UL, 
    0x06ca6351e003826fUL, 0x142929670a0e6e70UL, 0x27b70a8546d22ffcUL, 0x2e1b21385c26c926UL, 0x4d2c6dfc5ac42aedUL, 
    0x53380d139d95b3dfUL, 0x650a73548baf63deUL, 0x766a0abb3c77b2a8UL, 0x81c2c92e47edaee6UL, 0x92722c851482353bUL, 
    0xa2bfe8a14cf10364UL, 0xa81a664bbc423001UL, 0xc24b8b70d0f89791UL, 0xc76c51a30654be30UL, 0xd192e819d6ef5218UL, 
    0xd69906245565a910UL, 0xf40e35855771202aUL, 0x106aa07032bbd1b8UL, 0x19a4c116b8d2d0c8UL, 0x1e376c085141ab53UL, 
    0x2748774cdf8eeb99UL, 0x34b0bcb5e19b48a8UL, 0x391c0cb3c5c95a63UL, 0x4ed8aa4ae3418acbUL, 0x5b9cca4f7763e373UL, 
    0x682e6ff3d6b2b8a3UL, 0x748f82ee5defb2fcUL, 0x78a5636f43172f60UL, 0x84c87814a1f0ab72UL, 0x8cc702081a6439ecUL, 
    0x90befffa23631e28UL, 0xa4506cebde82bde9UL, 0xbef9a3f7b2c67915UL, 0xc67178f2e372532bUL, 0xca273eceea26619cUL, 
    0xd186b8c721c0c207UL, 0xeada7dd6cde0eb1eUL, 0xf57d4f7fee6ed178UL, 0x06f067aa72176fbaUL, 0x0a637dc5a2c898a6UL, 
    0x113f9804bef90daeUL, 0x1b710b35131c471bUL, 0x28db77f523047d84UL, 0x32caab7b40c72493UL, 0x3c9ebe0a15c9bebcUL, 
    0x431d67c49c100d4cUL, 0x4cc5d4becb3e42b6UL, 0x597f299cfc657e2aUL, 0x5fcb6fab3ad6faecUL, 0x6c44198c4a475817UL   
};


#define SHA512_STEP(a,b,c,d,e,f,g,h,x,K)  \
/**/                \
{                   \
  h += K + S1(e) + choose(e,f,g) + x; /* h = temp1 */   \
  d += h;           \
  h += S0(a) + bit_maj(a,b,c);  \
}


static void printAll(unsigned long a, unsigned long b, unsigned long c, unsigned long d,
                unsigned long e, unsigned long f, unsigned long g, unsigned long h)
{
    printf(""a = %lX\n"", a);
    printf(""b = %lX\n"", b);
    printf(""c = %lX\n"", c);
    printf(""d = %lX\n"", d);
    printf(""e = %lX\n"", e);
    printf(""f = %lX\n"", f);
    printf(""g = %lX\n"", g);
    printf(""h = %lX\n\n"", h);
}

#define ROUND_STEP(i) \
/**/                  \
{                     \
    SHA512_STEP(a, b, c, d, e, f, g, h, W[i + 0], k_sha256[i +  0]); \
    SHA512_STEP(h, a, b, c, d, e, f, g, W[i + 1], k_sha256[i +  1]); \
    SHA512_STEP(g, h, a, b, c, d, e, f, W[i + 2], k_sha256[i +  2]); \
    SHA512_STEP(f, g, h, a, b, c, d, e, W[i + 3], k_sha256[i +  3]); \
    SHA512_STEP(e, f, g, h, a, b, c, d, W[i + 4], k_sha256[i +  4]); \
    SHA512_STEP(d, e, f, g, h, a, b, c, W[i + 5], k_sha256[i +  5]); \
    SHA512_STEP(c, d, e, f, g, h, a, b, W[i + 6], k_sha256[i +  6]); \
    SHA512_STEP(b, c, d, e, f, g, h, a, W[i + 7], k_sha256[i +  7]); \
    SHA512_STEP(a, b, c, d, e, f, g, h, W[i + 8], k_sha256[i +  8]); \
    SHA512_STEP(h, a, b, c, d, e, f, g, W[i + 9], k_sha256[i +  9]); \
    SHA512_STEP(g, h, a, b, c, d, e, f, W[i + 10], k_sha256[i + 10]); \
    SHA512_STEP(f, g, h, a, b, c, d, e, W[i + 11], k_sha256[i + 11]); \
    SHA512_STEP(e, f, g, h, a, b, c, d, W[i + 12], k_sha256[i + 12]); \
    SHA512_STEP(d, e, f, g, h, a, b, c, W[i + 13], k_sha256[i + 13]); \
    SHA512_STEP(c, d, e, f, g, h, a, b, W[i + 14], k_sha256[i + 14]); \
    SHA512_STEP(b, c, d, e, f, g, h, a, W[i + 15], k_sha256[i + 15]); \
}


#define def_hash(funcName, inputTag, hashTag, mdPadFunc, printFromLongFunc)   \
/* The main hashing function */     \
static void funcName(inputTag unsigned long *input, const unsigned int length, hashTag unsigned long* hash)    \
{                                   \
    /* Do the padding - we weren't previously for some reason */            \
    const unsigned int nBlocks = mdPadFunc(input, (const unsigned long) length);      \
    /*if (length == 8){   \
        printf(""Padded input: "");   \
        printFromLongFunc(input, hashBlockSize_bytes, true); \
    }*/   \
                                    \
    unsigned long W[0x50]={0};      \
    /* state which is repeatedly processed & added to */    \
    unsigned long State[8]={0};    \
    State[0] = 0x6a09e667f3bcc908UL;	\
    State[1] = 0xbb67ae8584caa73bUL;	\
    State[2] = 0x3c6ef372fe94f82bUL;	\
    State[3] = 0xa54ff53a5f1d36f1UL;	\
    State[4] = 0x510e527fade682d1UL;	\
    State[5] = 0x9b05688c2b3e6c1fUL;	\
    State[6] = 0x1f83d9abfb41bd6bUL;	\
    State[7] = 0x5be0cd19137e2179UL;	\
                                    \
    unsigned long a,b,c,d,e,f,g,h;  \
                                \
    /* loop for each block */   \
    for (int block_i = 0; block_i < nBlocks; block_i++)     \
    {                                           \
        /* No need to (re-)initialise W.
            Note that the input pointer is updated */    \
        W[0] = SWAP(input[0]);	\
        W[1] = SWAP(input[1]);	\
        W[2] = SWAP(input[2]);	\
        W[3] = SWAP(input[3]);	\
        W[4] = SWAP(input[4]);	\
        W[5] = SWAP(input[5]);	\
        W[6] = SWAP(input[6]);	\
        W[7] = SWAP(input[7]);	\
        W[8] = SWAP(input[8]);	\
        W[9] = SWAP(input[9]);	\
        W[10] = SWAP(input[10]);	\
        W[11] = SWAP(input[11]);	\
        W[12] = SWAP(input[12]);	\
        W[13] = SWAP(input[13]);	\
        W[14] = SWAP(input[14]);	\
        W[15] = SWAP(input[15]);	\
                            \
        for (int i = 16; i < 80; i++)   \
        {                   \
            W[i] = W[i-16] + little_s0(W[i-15]) + W[i-7] + little_s1(W[i-2]);   \
        }               \
                        \
        a = State[0];   \
        b = State[1];   \
        c = State[2];   \
        d = State[3];   \
        e = State[4];   \
        f = State[5];   \
        g = State[6];   \
        h = State[7];   \
                        \
        /* Note loop is only 5 */  \
        for (int i = 0; i < 80; i += 16)    \
        {                   \
            ROUND_STEP(i)   \
        }                   \
                        \
        State[0] += a;  \
        State[1] += b;  \
        State[2] += c;  \
        State[3] += d;  \
        State[4] += e;  \
        State[5] += f;  \
        State[6] += g;  \
        State[7] += h;  \
                        \
        input += hashBlockSize_long64;   \
    }                   \
                        \
    hash[0]=SWAP(State[0]);   \
    hash[1]=SWAP(State[1]);   \
    hash[2]=SWAP(State[2]);   \
    hash[3]=SWAP(State[3]);   \
    hash[4]=SWAP(State[4]);   \
    hash[5]=SWAP(State[5]);   \
    hash[6]=SWAP(State[6]);   \
    hash[7]=SWAP(State[7]);   \
    return;             \
}

def_hash(hash_global, __global, __global, md_pad__global, printFromLong_glbl_n)
def_hash(hash_private, __private, __private, md_pad__private, printFromLong_n)
def_hash(hash_glbl_to_priv, __global, __private, md_pad__global, printFromLong_glbl_n)
def_hash(hash_priv_to_glbl, __private, __global, md_pad__private, printFromLong_n)

#undef bit_maj
#undef choose
#undef S0
#undef S1
#undef little_s0
#undef little_s1

__kernel void hash_main(__global inbuf * inbuffer, __global outbuf * outbuffer)
{
    unsigned int idx = get_global_id(0);
    hash_global(inbuffer[idx].buffer, inbuffer[idx].length, outbuffer[idx].buffer);
}
";

private static string buffer_structs_template_cl = @"
/*
    In- and out- buffer structures (of int32), with variable sizes, for hashing.
    These allow indexing just using just get_global_id(0)
    Variables tagged with <..> are replaced, so we can specify just enough room for the data.
    These are:
        - hashBlockSize_bits   : The hash's block size in Bits
        - inMaxNumBlocks      : per hash operation
        - hashDigestSize_bits   : The hash's digest size in Bits

    Originally adapted from Bjorn Kerler's sha256.cl
    MIT License
*/
#define DEBUG 1

// All macros left defined for usage in the program
#define ceilDiv(n,d) (((n) + (d) - 1) / (d))

// All important now, defining whether we're working with unsigned ints or longs
#define wordSize <word_size>

// Practical sizes of buffers, in words.
#define inBufferSize ceilDiv(<inBufferSize_bytes>, wordSize)
#define outBufferSize ceilDiv(<outBufferSize_bytes>, wordSize)
#define pwdBufferSize ceilDiv(<pwdBufferSize_bytes>, wordSize)
#define saltBufferSize ceilDiv(<saltBufferSize_bytes>, wordSize)
#define ctBufferSize ceilDiv(<ctBufferSize_bytes>, wordSize)

// 
#define hashBlockSize_bytes ceilDiv(<hashBlockSize_bits>, 8) /* Needs to be a multiple of 4, or 8 when we work with unsigned longs */
#define hashDigestSize_bytes ceilDiv(<hashDigestSize_bits>, 8)

// just Size always implies _word
#define hashBlockSize ceilDiv(hashBlockSize_bytes, wordSize)
#define hashDigestSize ceilDiv(hashDigestSize_bytes, wordSize)


// Ultimately hoping to faze out the Size_int32/long64,
//   in favour of just size (_word implied)
#if wordSize == 4
    #define hashBlockSize_int32 hashBlockSize
    #define hashDigestSize_int32 hashDigestSize
    #define word unsigned int
        
    unsigned int SWAP (unsigned int val)
    {
        return (rotate(((val) & 0x00FF00FF), 24U) | rotate(((val) & 0xFF00FF00), 8U));
    }

#elif wordSize == 8
    // Initially for use in SHA-512
    #define hashBlockSize_long64 hashBlockSize
    #define hashDigestSize_long64 hashDigestSize
    #define word unsigned long
    #define rotl64(a,n) (rotate ((a), (n)))
    #define rotr64(a,n) (rotate ((a), (64ul-n)))
    
    unsigned long SWAP (const unsigned long val)
    {
        // ab cd ef gh -> gh ef cd ab using the 32 bit trick
        unsigned long tmp = (rotr64(val & 0x0000FFFF0000FFFFUL, 16UL) | rotl64(val & 0xFFFF0000FFFF0000UL, 16UL));
        
        // Then see this as g- e- c- a- and -h -f -d -b to swap within the pairs,
        // gh ef cd ab -> hg fe dc ba
        return (rotr64(tmp & 0xFF00FF00FF00FF00UL, 8UL) | rotl64(tmp & 0x00FF00FF00FF00FFUL, 8UL));
    }
#endif



// ====  Define the structs with the right word size  =====
//  Helpful & more cohesive to have the lengths of structures as words too,
//   (rather than unsigned int for both)
typedef struct {
    word length; // in bytes
    word buffer[inBufferSize];
} inbuf;

typedef struct {
    word buffer[outBufferSize];
} outbuf;

// Salt buffer, used by pbkdf2 & pbe
typedef struct {
    word length; // in bytes
    word buffer[saltBufferSize];
} saltbuf;

// Password buffer, used by pbkdf2 & pbe
typedef struct {
    word length; // in bytes
    word buffer[pwdBufferSize];
} pwdbuf;

// ciphertext buffer, used in pbe.
// no code relating to this in the opencl.py core, dealt with in signal_pbe_mac.cl as it's a special case
typedef struct {
    word length; // in bytes
    word buffer[ctBufferSize];
} ctbuf;




// ========== Debugging function ============

#ifdef DEBUG
#if DEBUG

    #define def_printFromWord(tag, funcName, end)               \
    /* For printing the string of bytes stored in an array of words.
    Option to print hex. */    \
    static void funcName(tag const word *arr, const unsigned int len_bytes, const bool hex)\
    {                                           \
        for (int j = 0; j < len_bytes; j++){    \
            word v = arr[j / wordSize];                 \
            word r = (j % wordSize) * 8;                \
            /* Prints little endian, since that's what we use */   \
            v = (v >> r) & 0xFF;                \
            if (hex) {                          \
                printf(""%02x"", v);              \
            } else {                            \
                printf(""%c"", (char)v);          \
            }                                   \
        }                                       \
        printf(end);                            \
    }

    def_printFromWord(__private, printFromWord, """")
    def_printFromWord(__global, printFromWord_glbl, """")
    def_printFromWord(__private, printFromWord_n, ""\n"")
    def_printFromWord(__global, printFromWord_glbl_n, ""\n"")

#endif
#endif
";

    }
}