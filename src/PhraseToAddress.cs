using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Text;

namespace FixMyCrypto {
    abstract class PhraseToAddress {
        protected ConcurrentQueue<Work> phraseQueue, addressQueue;
        protected int threadNum, threadMax;
        string[] defaultPaths;
        PathTree tree;
        Passphrase passphrases;
        public static PhraseToAddress Create(CoinType coin, ConcurrentQueue<Work> phrases, ConcurrentQueue<Work> addresses, int threadNum, int threadMax) {
            switch (coin) {
                case CoinType.ADA:

                return new PhraseToAddressCardano(phrases, addresses, threadNum, threadMax);

                case CoinType.ADALedger:

                return new PhraseToAddressCardanoLedger(phrases, addresses, threadNum, threadMax);

                case CoinType.ADATrezor:

                return new PhraseToAddressCardanoTrezor(phrases, addresses, threadNum, threadMax);

                case CoinType.ETH:

                return new PhraseToAddressEth(phrases, addresses, threadNum, threadMax);

                case CoinType.BTC:
                case CoinType.DOGE:
                case CoinType.LTC:
                case CoinType.BCH:
                case CoinType.XRP:

                return new PhraseToAddressBitAltcoin(phrases, addresses, threadNum, threadMax, coin);

                case CoinType.SOL:

                return new PhraseToAddressSolana(phrases, addresses, threadNum, threadMax);

                case CoinType.ALGO:

                return new PhraseToAddressAlgorand(phrases, addresses, threadNum, threadMax);

                default:

                throw new NotSupportedException();
            }
        }
        Stopwatch queueWaitTime = new Stopwatch();
        protected PhraseToAddress(ConcurrentQueue<Work> phrases, ConcurrentQueue<Work> addresses, int threadNum, int threadMax) {
            this.phraseQueue = phrases;
            this.addressQueue = addresses;
            this.threadNum = threadNum;
            this.threadMax = threadMax;
            this.mutex = new object();

            this.passphrases = new Passphrase(Settings.passphrase);
        }
        public abstract Object DeriveMasterKey(Phrase phrase, string passphrase);
        protected abstract Object DeriveChildKey(Object parentKey, uint index);
        protected abstract Address DeriveAddress(PathNode node);
        public abstract void ValidateAddress(string address);

        public static void ValidateAddress(CoinType coin, string address) {
            PhraseToAddress p2a = PhraseToAddress.Create(coin, null, null, 0, 0);
            p2a.ValidateAddress(address);
        }

        private Object mutex;

        protected virtual string GetStakePath() { return null; }
        private void DeriveChildKeys(PathNode node) {
            node.key = DeriveChildKey(node.parent.key, node.value);

            foreach (PathNode child in node.children) {
                DeriveChildKeys(child);
            }
        }

        private void DeriveAddresses(PathNode node, List<Address> addresses) {
            if (node.end) {
                addresses.Add(DeriveAddress(node));
            }

            foreach (PathNode child in node.children) {
                DeriveAddresses(child, addresses);
            }
        }
        private List<Address> GetAddresses(Phrase phrase, string passphrase, int account, int index, string[] paths) {
            int[] accounts = { account };
            int[] indices = { index };
            return GetAddresses(phrase, passphrase, paths, accounts, indices);
        }
        public List<Address> GetAddresses(Phrase phrase, string passphrase, string[] paths, int[] accounts, int[] indices) {
            //  Create default path list if needed
            if (paths == null || paths.Length == 0 || (paths.Length == 1 && String.IsNullOrEmpty(paths[0]))) {
                if (defaultPaths == null) {
                    lock (mutex) {
                        defaultPaths = GetDefaultPaths(Settings.knownAddresses);
                    }
                }

                paths = defaultPaths;
            }

            //  Create path tree if needed
            if (tree == null) {
                lock (mutex) {
                    tree = new PathTree();

                    foreach (string path in paths) {
                        string pa = path;
                        if (!pa.Contains("{account}") && !pa.Contains("{index}")) {
                            pa = Path.Tokenize(path);
                        }

                        foreach (int account in accounts) {
                            string pat = pa.Replace("{account}", $"{account}");

                            foreach (int index in indices) {
                                string p = pat.Replace("{index}", $"{index}");
                                tree.AddPath(p);
                            }

                            switch (this.GetCoinType()) {
                                case CoinType.ADA:
                                case CoinType.ADALedger:
                                case CoinType.ADATrezor:

                                //  Derive stake key
                                string stakePath = GetStakePath().Replace("{account}", $"{account}");
                                tree.AddPath(stakePath, false);
                                break;
                            }
                        }
                    }

                    if (this.threadNum == 0) {
                        Log.Info($"{this.GetCoinType()} path derivation tree:");
                        Log.Info(tree.ToString());
                    }
                }
            }

            //  Derive descendent keys

            tree.root.key = DeriveMasterKey(phrase, passphrase);

            foreach (PathNode child in tree.root.children) {
                DeriveChildKeys(child);
            }

            List<Address> addresses = new List<Address>();

            DeriveAddresses(tree.root, addresses);

            foreach (Address address in addresses) {
                address.phrase = phrase;
                address.passphrase = passphrase;
            }

            return addresses;
        }
        public abstract string[] GetDefaultPaths(string[] knownAddresses);
        public List<Address> GetAddress(string phrase, string passphrase, int account, int index, string path = null) {
            string[] paths = { path };
            Phrase p = new Phrase(phrase);
            return GetAddresses(p, passphrase, account, index, paths);
        }
        public abstract CoinType GetCoinType();
        public void Finish() {
            Global.done = true;
            lock(phraseQueue) { Monitor.PulseAll(phraseQueue); }
            lock(addressQueue) { Monitor.PulseAll(addressQueue); }
        }
        public void Consume() {
            int count = 0;
            Stopwatch stopWatch = new Stopwatch();
            Log.Debug("P2A" + threadNum + " start");

            while (!Global.done) {

                Work w = null;

                //  Dequeue phrase

                lock(phraseQueue) {
                    queueWaitTime.Start();
                    while (phraseQueue.Count == 0) {
                        if (Global.done) break;
                        //Log.Debug("P2A thread " + threadNum + " waiting for work");
                        Monitor.Wait(phraseQueue);
                    }
                    queueWaitTime.Stop();

                    if (phraseQueue.TryDequeue(out w)) {
                        //Log.Debug("P2A thread " + threadNum + " got phrase: \"" + w + "\", queue size: " + phraseQueue.Count);
                        Monitor.Pulse(phraseQueue);
                    }
                }

                if (w != null) {
 
                    foreach (string passphrase in this.passphrases.Enumerate()) {
                        //  Convert phrase to address

                        List<Address> addresses = null;
                        try {
                            stopWatch.Start();
                            if (Settings.knownAddresses != null && Settings.knownAddresses.Length > 0) {
                                //  Try to generate the known address
                                addresses = GetAddresses(w.phrase, passphrase, Settings.paths, Settings.accounts, Settings.indices);
                                count++;
                            } 
                            else {
                                //  Generate address for account 0 index 0
                                addresses = GetAddresses(w.phrase, passphrase, 0, 0, Settings.paths);
                                count++;
                            }
                        }
                        catch (Exception e) {
                            Log.Error("P2A error: " + e.Message);
                        }
                        finally {
                            stopWatch.Stop();
                        }

                        if (addresses == null) continue;

                        if (Settings.knownAddresses != null && Settings.knownAddresses.Length > 0) {
                            //  See if we generated the known address
                            foreach (Address address in addresses) {
                                foreach (string knownAddress in Settings.knownAddresses) {
                                    if (address.address.Equals(knownAddress, StringComparison.OrdinalIgnoreCase)) {
                                        //  Found known address
                                        Finish();

                                        FoundResult.DoFoundResult(this.GetCoinType(), address);
                                    }
                                }
                            }
                        }
                        else {
                            //  Need to search blockchain for address

                            Work w2 = new Work(w.phrase, addresses);

                            //  Enqueue address

                            lock (addressQueue) {
                                queueWaitTime.Start();
                                while (addressQueue.Count > Settings.threads) {
                                    if (Global.done) break;
                                    //Log.Debug("P2A thread " + threadNum + " waiting on full address queue");
                                    Monitor.Wait(addressQueue);
                                }
                                queueWaitTime.Stop();

                                addressQueue.Enqueue(w2);
                                //Log.Debug("P2A thread " + threadNum + " enqueued address: \"" + w2 + "\", queue size: " + addressQueue.Count);
                                Monitor.Pulse(addressQueue);
                            }
                        }
                    }
                }
            }

            if (count > 0) Log.Info("P2A" + threadNum + " done, count: " + count + " total time: " + stopWatch.ElapsedMilliseconds/1000 + $"s, time/req: {(count != 0 ? ((double)stopWatch.ElapsedMilliseconds/count) : 0):F2}ms/req, queue wait: " + queueWaitTime.ElapsedMilliseconds/1000 + "s");
            Finish();
        }

        protected static byte[] ed25519_seed = Encoding.ASCII.GetBytes("ed25519 seed");

        public class Key {
            public byte[] data;
            public byte[] cc;
            public Key(byte[] data, byte[] cc) {
                this.data = data;
                this.cc = cc;
            }
        }

    }
}