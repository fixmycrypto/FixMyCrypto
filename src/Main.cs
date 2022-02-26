using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace FixMyCrypto {
    class FixMyCrypto {
        static bool interactive = true;

        public static void PauseAndExit(int code) {
            if (interactive) {
                Log.All("Press 'E' to exit...");
                try {
                    while (Console.ReadKey(true).Key != ConsoleKey.E) { }
                }
                catch (Exception) { }
            }

            Environment.Exit(code);
        }
        static void Main(string[] args) {
            Log.All(Settings.GetVersion());

            bool test = args.Length > 0 && args[0] == "-t";

            bool benchmark = args.Length > 0 && args[0] == "-b";

            interactive = !(args.Length > 0 && args[0] == "-ni");

            try {
                Settings.LoadSettings(test);
            }
            catch (Exception e) {
                Console.Error.WriteLine($"Error loading settings.json: {e.Message}");
                PauseAndExit(1);
            }

            // OpenCL ocl = new OpenCL();
            // ocl.Benchmark(20480);
 
            WebClient.client.Timeout = new System.TimeSpan(0, 0, 60);

            if (test) {
                int count = 100;
                if (args.Length > 1) Int32.TryParse(args[1], out count);
                string tests = "btc,eth";
                if (args.Length > 2) tests = args[2];
                Console.WriteLine("Running tests...");
                try {
                    Test.Run(count, tests);
                }
                catch (Exception e) {
                    Log.Error($"tests failed with exception {e}");
                    PauseAndExit(1);
                }
                PauseAndExit(0);
            }

            if (benchmark) {
                Benchmark.RunBenchmarks();
                Environment.Exit(0);
            }
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //  Ensure we have write permissions
            try {
                if (File.Exists("results.json")) {
                    Log.Error("results.json file already exists on disk. If you want to start a new run, please rename or delete this file first.");
                    PauseAndExit(1);
                }

                StreamWriter writer = File.CreateText("results.json");
                writer.WriteLine();
                writer.Close();
                File.Delete("results.json");
            }
            catch (Exception) {
                Log.Error("Please ensure the current user has read & write permissions for the current working directory.");
                PauseAndExit(1);
            }

            //  Validate phrase
            try {
                Phrase.Validate(Settings.Phrase);
            }
            catch (Exception e) {
                Log.Error($"Invalid phrase: {e}");
                PauseAndExit(1);
            }

            //  Initialize word lists
            string[] phraseArray = Settings.Phrase.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Wordlists.Initialize(phraseArray);

            Checkpoint checkpoint = new Checkpoint();
            bool resumeFromCheckpoint = false;

            //  Check for checkpoint file
            try {
                if (File.Exists("checkpoint.json")) {
                    resumeFromCheckpoint = checkpoint.RestoreCheckpoint();
                }
            }
            catch (Exception) {
                PauseAndExit(1);
            }

            BlockingCollection<Work> phraseQueue = new BlockingCollection<Work>(Settings.Threads * 2);
            BlockingCollection<Work> addressQueue = new BlockingCollection<Work>(Settings.Threads * 2);

            int phraseProducerCount = 1,
                phraseToAddressCount = 1,
                addressLookupCount = Math.Max(Settings.Threads - 2, 1);

            string api = Settings.GetApiPath(Settings.CoinType);
            if (!String.IsNullOrEmpty(api)) {
                Log.All($"API Server: {api}");
            }
            else {
                phraseToAddressCount = 1;
                addressLookupCount = 0;
            }

            Log.Info($"thread count {Settings.Threads} PP={phraseProducerCount} P2A={phraseToAddressCount} LA={addressLookupCount}");
            Log.All($"Coin type: {Settings.CoinType}");
            Log.All($"Phrase to test: \"{Settings.Phrase}\"");

            int maxPassphraseLength = 0;
            long passphraseCount = 0;
            MultiPassphrase p = null;

            if (Settings.Passphrases != null) {
                foreach (string passphrase in Settings.Passphrases) {
                    Log.All($"passphrase: \"{passphrase}\"");
                }

                Log.All("Enumerating passphrase...");
                p = new MultiPassphrase(Settings.Passphrases);
                (passphraseCount, maxPassphraseLength) = p.GetCountAndMaxLength();
                Log.All($"passphrase permutations: {passphraseCount}, max length: {maxPassphraseLength}");

                if (!String.IsNullOrEmpty(Settings.TopologyFile)) {
                    p.WriteTopologyFile(Settings.TopologyFile);
                    Log.All($"Passphrase topology written to: {Settings.TopologyFile}");
                }
            }
            
            if (Settings.Paths != null) {
                foreach (string path in Settings.Paths) {
                    Log.All($"path: {path}");
                }
            }
            Log.All($"Accounts: {Settings.GetRangeString(Settings.Accounts)}");
            Log.All($"Indices: {Settings.GetRangeString(Settings.Indices)}");

            //  Log the path tree
            PhraseToAddress p2at = PhraseToAddress.Create(Settings.CoinType, null, null);
            PathTree tree = p2at.CreateTree(Settings.Paths, Settings.Accounts, Settings.Indices);
            Log.All($"Derivation path tree:\n{tree.ToString()}");

            if (Settings.KnownAddresses != null) {
                foreach (string knownAddress in Settings.KnownAddresses) {
                    Log.All($"knownAddress: {knownAddress}");
                }
            }
            
            Log.All($"difficulty: {Settings.Difficulty}, wordDistance: {Settings.WordDistance}");

            //  OpenCL
            OpenCL ocl = null;
            if (Settings.OpenCLPlatform >= 0 && Settings.OpenCLDevice >= 0) {
                ocl = new OpenCL(Settings.OpenCLPlatform, Settings.OpenCLDevice, maxPassphraseLength);
            }

            System.Timers.Timer timer = new System.Timers.Timer(30 * 1000);
            timer.Elapsed += (StringReader, args) => { 
                if (phraseQueue.Count > 0 || addressQueue.Count > 0) Log.Debug($"Queue status: phrases {phraseQueue.Count} addresses {addressQueue.Count}");
            };
            timer.Start();

            LookupAddress[] la = new LookupAddress[addressLookupCount];
            List<Thread> laThreads = new List<Thread>();
            for (int i = 0; i < addressLookupCount; i++) {
                la[i] = LookupAddress.Create(Settings.CoinType, addressQueue, i, addressLookupCount);

                Thread thread = new Thread (la[i].Consume);
                thread.Name = "LA" + i;
                laThreads.Add(thread);
                thread.Start(); 
            }

            PhraseToAddress[] p2a = new PhraseToAddress[phraseToAddressCount];
            List<Thread> p2aThreads = new List<Thread>();
            for (int i = 0; i < phraseToAddressCount; i++) {
                p2a[i] = PhraseToAddress.Create(Settings.CoinType, phraseQueue, addressQueue);

                if (i == 0) checkpoint.SetPhraseToAddress(p2a[i]);

                if (p != null) p2a[i].SetPassphrase(p, passphraseCount);

                p2a[i].SetOpenCL(ocl);

                if (i == 0 && Settings.KnownAddresses != null) {
                    //  Validate addresses
                    foreach (string address in Settings.KnownAddresses) {
                        try {
                            p2a[i].ValidateAddress(address);
                        }
                        catch (Exception e) {
                            Log.Error($"Invalid known address: {address}: {e}. Please check the known addresses.");
                            PauseAndExit(1);
                        }
                    }
                }

                Thread thread = new Thread (p2a[i].Consume);
                thread.Name = "P2A" + i;
                p2aThreads.Add(thread);
                thread.Start(); 
             }

            PhraseProducer[] phrasers = new PhraseProducer[phraseProducerCount];
            List<Thread> phraseThreads = new List<Thread>();
            for (int i = 0; i < phraseProducerCount; i++) {
                phrasers[i] = new PhraseProducer(phraseQueue, phraseArray);

                if (i == 0) checkpoint.SetPhraseProducer(phrasers[i]);

                Thread thread = new Thread (phrasers[i].ProduceWork);
                thread.Name = "PP" + i;
                phraseThreads.Add(thread);
                thread.Start(); 
            }

            if (!resumeFromCheckpoint) checkpoint.Start();

            for (int i = 0; i < phraseProducerCount; i++) {
                phraseThreads[i].Join();
            }

            for (int i = 0; i < phraseToAddressCount; i++) {
                p2aThreads[i].Join();
            }
 
            for (int i = 0; i < addressLookupCount; i++) {
                laThreads[i].Join();
            }

            checkpoint.Stop();
            timer.Stop();
            stopWatch.Stop();

            Log.Info("Program Finished in " + stopWatch.ElapsedMilliseconds/1000 + "s");

            if (!Global.Found) {
                Log.All("\n\nRecovery Failed. :( Contact help@fixmycrypto.com for further assistance.");
            }

            //  Delete checkpoint file on clean exit
            if (File.Exists("checkpoint.json")) {
                File.Delete("checkpoint.json");
            }

            PauseAndExit(0);
        }
    }
}