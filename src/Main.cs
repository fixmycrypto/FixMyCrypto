using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace FixMyCrypto {
    class FixMyCrypto {
        static bool interactive = true;
        static void PauseAndExit(int code) {
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

            interactive = !(args.Length > 0 && args[0] == "-ni");

            try {
                Settings.LoadSettings(test);
            }
            catch (Exception e) {
                Console.Error.WriteLine($"Error loading settings.json: {e.Message}");
                PauseAndExit(1);
            }

            WebClient.client.Timeout = new System.TimeSpan(0, 0, 30);

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
                Phrase.Validate(Settings.phrase);
            }
            catch (Exception e) {
                Log.Error($"Invalid phrase: {e}");
                PauseAndExit(1);
            }

            ConcurrentQueue<Work> phraseQueue = new ConcurrentQueue<Work>();
            ConcurrentQueue<Work> addressQueue = new ConcurrentQueue<Work>();
            Stopwatch stopWatch = new Stopwatch();

            int phraseProducerCount = 1,
                phraseToAddressCount = Settings.threads / 2,
                addressLookupCount = Settings.threads / 2;

            Log.Info($"thread count {Settings.threads} PP={phraseProducerCount} P2A={phraseToAddressCount} LA={addressLookupCount}");
            Log.All($"Coin type: {Settings.coinType}");
            Log.All($"Phrase to test: \"{Settings.phrase}\"");
            if (!String.IsNullOrEmpty(Settings.passphrase)) Log.All($"passphrase: \"{Settings.passphrase}\"");
            if (Settings.paths != null) {
                foreach (string path in Settings.paths) {
                    Log.All($"path: {path}");
                }
            }
            Log.All($"Accounts: {Settings.GetRangeString(Settings.accounts)}");
            Log.All($"Indices: {Settings.GetRangeString(Settings.indices)}");
            if (Settings.knownAddresses != null) {
                foreach (string knownAddress in Settings.knownAddresses) {
                    //  TODO: validate addresses
                    Log.All($"knownAddress: {knownAddress}");
                }
            }
            Log.All($"difficulty: {Settings.difficulty}, wordDistance: {Settings.wordDistance}");
            Log.All($"API Server: {Settings.GetApiPath(Settings.coinType)}");

            System.Timers.Timer timer = new System.Timers.Timer(30 * 1000);
            timer.Elapsed += (StringReader, args) => { 
                if (phraseQueue.Count > 0 || addressQueue.Count > 0) Log.Debug($"Queue status: phrases {phraseQueue.Count} addresses {addressQueue.Count}");
            };
            timer.Start();

            stopWatch.Start();

            LookupAddress[] la = new LookupAddress[addressLookupCount];
            List<Thread> laThreads = new List<Thread>();
            for (int i = 0; i < addressLookupCount; i++) {
                la[i] = LookupAddress.Create(Settings.coinType, addressQueue, i, addressLookupCount);

                Thread thread = new Thread (la[i].Consume);
                thread.Name = "LA" + i;
                laThreads.Add(thread);
                thread.Start(); 
            }

            PhraseToAddress[] p2a = new PhraseToAddress[phraseToAddressCount];
            List<Thread> p2aThreads = new List<Thread>();
            for (int i = 0; i < phraseToAddressCount; i++) {
                p2a[i] = PhraseToAddress.Create(Settings.coinType, phraseQueue, addressQueue, i, phraseToAddressCount);

                if (i == 0 && Settings.knownAddresses != null) {
                    //  Validate addresses
                    foreach (string address in Settings.knownAddresses) {
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
                phrasers[i] = new PhraseProducer(phraseQueue, i, phraseProducerCount, Settings.phrase.Split(" ", StringSplitOptions.RemoveEmptyEntries));

                Thread thread = new Thread (phrasers[i].ProduceWork);
                thread.Name = "PP" + i;
                phraseThreads.Add(thread);
                thread.Start(); 
            }

            for (int i = 0; i < phraseProducerCount; i++) {
                phraseThreads[i].Join();
            }

            for (int i = 0; i < phraseToAddressCount; i++) {
                p2aThreads[i].Join();
            }
 
            for (int i = 0; i < addressLookupCount; i++) {
                laThreads[i].Join();
            }

            timer.Stop();
            stopWatch.Stop();

            Log.Info("Program Finished in " + stopWatch.ElapsedMilliseconds/1000 + "s");

            if (!Global.found) {
                Log.All("\n\nRecovery Failed. :( Contact help@fixmycrypto.com for further assistance.");
            }

            PauseAndExit(0);
        }
    }
}