using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FixMyCrypto {
    abstract class PhraseToAddress {
        protected BlockingCollection<Work> phraseQueue, addressQueue;

        public static PhraseToAddress Create(CoinType coin, BlockingCollection<Work> phrases, BlockingCollection<Work> addresses) {
            switch (coin) {
                case CoinType.ADA:

                return new PhraseToAddressCardano(phrases, addresses);

                case CoinType.ADALedger:

                return new PhraseToAddressCardanoLedger(phrases, addresses);

                case CoinType.ADATrezor:

                return new PhraseToAddressCardanoTrezor(phrases, addresses);

                case CoinType.ETH:

                return new PhraseToAddressEth(phrases, addresses);

                case CoinType.BTC:
                case CoinType.DOGE:
                case CoinType.LTC:
                case CoinType.BCH:
                case CoinType.XRP:

                return new PhraseToAddressBitAltcoin(phrases, addresses, coin);

                case CoinType.SOL:

                return new PhraseToAddressSolana(phrases, addresses);

                case CoinType.ALGO:

                return new PhraseToAddressAlgorand(phrases, addresses);

                case CoinType.DOT:

                return new PhraseToAddressPolkadot(phrases, addresses);

                case CoinType.DOTLedger:

                return new PhraseToAddressPolkadotLedger(phrases, addresses);

                default:

                throw new NotSupportedException();
            }
        }
        Stopwatch queueWaitTime = new Stopwatch();
        Stopwatch stopWatch = new Stopwatch();
        int count = 0;

        protected PhraseToAddress(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses) {
            this.phraseQueue = phrases;
            this.addressQueue = addresses;
        }
        private MasterKey[] DeriveMasterKeys(Phrase phrase, string[] passphrases) {
            MasterKey[] keys = new MasterKey[passphrases.Length];
            int i = 0;
            foreach (string passphrase in passphrases) {
                keys[i++] = new MasterKey(DeriveMasterKey(phrase, passphrase), phrase, passphrase);
            }
            return keys;
        }
        private class MasterKey {
            public object key { get; }

            public Phrase phrase { get; }

            public string passphrase { get; }

            public MasterKey(object key, Phrase phrase, string passphrase) {
                this.key = key;
                this.phrase = phrase;
                this.passphrase = passphrase;
            }
        }
        public abstract Object DeriveMasterKey(Phrase phrase, string passphrase);
        protected abstract Object DeriveChildKey(Object parentKey, uint index);
        protected abstract Address DeriveAddress(PathNode node);
        public abstract void ValidateAddress(string address);

        public static void ValidateAddress(CoinType coin, string address) {
            PhraseToAddress p2a = PhraseToAddress.Create(coin, null, null);
            p2a.ValidateAddress(address);
        }

        protected virtual string GetStakePath() { return null; }
        private void DeriveChildKeys(PathNode node) {
            node.Key = DeriveChildKey(node.Parent.Key, node.Value);

            foreach (PathNode child in node.Children) {
                DeriveChildKeys(child);
            }
        }

        private void DeriveAddresses(PathNode node, Phrase phrase, string passphrase, List<Address> addresses) {
            if (node.End) {
                var address = DeriveAddress(node);
                if (address != null) {
                    address.phrase = phrase;
                    address.passphrase = passphrase;
                    addresses.Add(address);
                }
            }

            foreach (PathNode child in node.Children) {
                DeriveAddresses(child, phrase, passphrase, addresses);
            }
        }
        private List<Address> GetAddressesList(Phrase phrase, string[] passphrases, int account, int index, string[] paths) {
            int[] accounts = { account };
            int[] indices = { index };
            return GetAddressesList(phrase, passphrases, paths, accounts, indices);
        }

        public PathTree CreateTree(string[] paths, int[] accounts, int[] indices) {
            PathTree tree = new PathTree();

            if (paths == null || paths.Length == 0 || (paths.Length == 1 && String.IsNullOrEmpty(paths[0]))) {
                paths = GetDefaultPaths(Settings.KnownAddresses);
            }

            if (accounts == null || accounts.Length == 0) accounts = Settings.Accounts;
            if (indices == null || indices.Length == 0) indices = Settings.Indices;

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

            return tree;
        }

        public List<Address> GetAddressesList(Phrase phrase, string[] passphrases, string[] paths, int[] accounts, int[] indices) {

            //  Create path tree
            PathTree tree = CreateTree(paths, accounts, indices);

            //  Derive descendent keys

            List<Address> addresses = new();

            // MasterKey[] masterKeys = DeriveMasterKeys(phrase, passphrases);
            // foreach (MasterKey key in masterKeys) {

            foreach (string passphrase in passphrases) {
                if (Global.Done) break;

                // tree.Root.Key = key.key;
                tree.Root.Key = DeriveMasterKey(phrase, passphrase);

                foreach (PathNode child in tree.Root.Children) {
                    DeriveChildKeys(child);
                }

                List<Address> addrs = new();

                DeriveAddresses(tree.Root, phrase, passphrase, addrs);

                bool found = false;

                foreach (Address address in addrs) {

                    if (Settings.KnownAddresses != null && Settings.KnownAddresses.Length > 0) {
                        foreach (string known in Settings.KnownAddresses) {
                            if (known.Equals(address.address, StringComparison.OrdinalIgnoreCase)) {
                                found = true;
                                break;
                            }
                        }
                    }
                }

                addresses.AddRange(addrs);

                if (found) break;
            }

            return addresses;
        }

        public delegate void ProduceAddress(List<Address> addresses);

        public int GetAddressesBatchPassphrases(Phrase phrase, string[] passphrases, PathTree tree, ProduceAddress Produce) {

            int count = 0;

            //  Derive path keys

            Parallel.ForEach (passphrases, passphrase => {
                if (Global.Done) return;

                PathTree t = new PathTree(tree);

                t.Root.Key = DeriveMasterKey(phrase, passphrase);

                foreach (PathNode child in t.Root.Children) {
                    DeriveChildKeys(child);
                }

                List<Address> addrs = new();

                DeriveAddresses(t.Root, phrase, passphrase, addrs);

                if (Produce != null) Produce(addrs);

                count += addrs.Count;
            });

            return count;
        }

        public int GetAddressesBatchPhrases(Phrase[] phrases, string passphrase, PathTree tree, ProduceAddress Produce = null) {

            int count = 0;

            //  Derive path keys

            Parallel.ForEach (phrases, phrase => {
                if (Global.Done) return;

                PathTree t = new PathTree(tree);

                t.Root.Key = DeriveMasterKey(phrase, passphrase);

                foreach (PathNode child in t.Root.Children) {
                    DeriveChildKeys(child);
                }

                List<Address> addrs = new();

                DeriveAddresses(t.Root, phrase, passphrase, addrs);

                if (Produce != null) Produce(addrs);

                count += addrs.Count;
            });

            return count;
        }
        public abstract string[] GetDefaultPaths(string[] knownAddresses);
        public List<Address> GetAddressList(string phrase, string passphrase, int account, int index, string path = null) {
            string[] paths = { path };
            Phrase p = new Phrase(phrase);
            string[] passphrases = { passphrase };
            return GetAddressesList(p, passphrases, account, index, paths);
        }
        public abstract CoinType GetCoinType();
        public void Finish() {
            Global.Done = true;
            phraseQueue.CompleteAdding();
            addressQueue.CompleteAdding();
            if (count > 0) Log.Info("P2A done, count: " + count + " total time: " + stopWatch.ElapsedMilliseconds/1000 + $"s, time/req: {(count != 0 ? ((double)stopWatch.ElapsedMilliseconds/count) : 0):F2}ms/req, queue wait: " + queueWaitTime.ElapsedMilliseconds/1000 + "s");
            count = 0;
        }
        public void Consume() {
            Log.Debug("P2A start");

            PathTree tree = CreateTree(Settings.Paths, Settings.Accounts, Settings.Indices);

            //  Generate passphrase list
            List<string> list = new List<string>();
            Passphrase p = new Passphrase(Settings.Passphrase, Settings.FuzzDepth);
            foreach (string pass in p) list.Add(pass);
            string[] passphrases = list.ToArray();
            // Log.Debug($"P2A{threadNum} passphrases[].Length={passphrases.Length}");
            int batchSize = 1024;
            Queue<Phrase> phraseBatch = new(batchSize);

            stopWatch.Start();

            while (!Global.Done) {

                //  Dequeue phrase
                Work w = null;

                queueWaitTime.Start();
                phraseQueue.TryTake(out w, 10);
                queueWaitTime.Stop();

                if (w != null) {
 
                    if (Global.Done) break;
                    
                    //  Convert phrase to address

                    try {
                        stopWatch.Start();
                        if (passphrases.Length > 1) {
                            count += GetAddressesBatchPassphrases(w.phrase, passphrases, tree, Produce);
                        }
                        else {
                            phraseBatch.Enqueue(w.phrase);
                            if (phraseBatch.Count >= batchSize) {
                                Phrase[] phrases = phraseBatch.ToArray();
                                phraseBatch.Clear();
                                count += GetAddressesBatchPhrases(phrases, passphrases[0], tree, Produce);
                            }
                        }
                    }
                    catch (Exception e) {
                        Log.Error("P2A error: " + e.Message);
                    }
                    finally {
                        stopWatch.Stop();
                    }
                }
                else if (phraseBatch.Count > 0) {
                    //  Run a partial batch

                    Phrase[] phrases = phraseBatch.ToArray();
                    phraseBatch.Clear();
                    count += GetAddressesBatchPhrases(phrases, passphrases[0], tree, Produce);
                }
            }

            if (phraseBatch.Count > 0) {
                //  End of phrase generation; finish any remaining in queue

                Phrase[] phrases = phraseBatch.ToArray();
                phraseBatch.Clear();
                count += GetAddressesBatchPhrases(phrases, passphrases[0], tree, Produce);
            }

            Finish();
            stopWatch.Stop();
        }

        public void Produce(List<Address> addresses) {
            if (Settings.KnownAddresses != null && Settings.KnownAddresses.Length > 0) {
                //  See if we generated the known address
                foreach (Address address in addresses) {
                    foreach (string knownAddress in Settings.KnownAddresses) {
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

                Work w2 = new Work(null, addresses);

                //  Enqueue address

                queueWaitTime.Start();
                try {
                    addressQueue.Add(w2);
                }
                catch (InvalidOperationException) {
                    return;
                }
                finally {
                    queueWaitTime.Stop();
                }
            }                    
        }
    }
}