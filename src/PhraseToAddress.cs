using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;

namespace FixMyCrypto {
    abstract class PhraseToAddress {
        protected BlockingCollection<Work> phraseQueue, addressQueue;
        protected int threadNum, threadMax;
        string[] defaultPaths;
        PathTree tree;
        Passphrase passphrases;

        private HMACSHA512 HMAC512;
        public static PhraseToAddress Create(CoinType coin, BlockingCollection<Work> phrases, BlockingCollection<Work> addresses, int threadNum, int threadMax) {
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

                case CoinType.DOT:

                return new PhraseToAddressPolkadot(phrases, addresses, threadNum, threadMax);

                case CoinType.DOTLedger:

                return new PhraseToAddressPolkadotLedger(phrases, addresses, threadNum, threadMax);

                default:

                throw new NotSupportedException();
            }
        }
        Stopwatch queueWaitTime = new Stopwatch();
        protected PhraseToAddress(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses, int threadNum, int threadMax) {
            this.phraseQueue = phrases;
            this.addressQueue = addresses;
            this.threadNum = threadNum;
            this.threadMax = threadMax;
            this.mutex = new object();

            this.passphrases = new Passphrase(Settings.Passphrase);
            this.HMAC512 = new HMACSHA512(ed25519_seed);
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
            node.Key = DeriveChildKey(node.Parent.Key, node.Value);

            foreach (PathNode child in node.Children) {
                DeriveChildKeys(child);
            }
        }

        private void DeriveAddresses(PathNode node, List<Address> addresses) {
            if (node.End) {
                var address = DeriveAddress(node);
                if (address != null) addresses.Add(address);
            }

            foreach (PathNode child in node.Children) {
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
                        defaultPaths = GetDefaultPaths(Settings.KnownAddresses);
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

            tree.Root.Key = DeriveMasterKey(phrase, passphrase);

            foreach (PathNode child in tree.Root.Children) {
                DeriveChildKeys(child);
            }

            List<Address> addresses = new List<Address>();

            DeriveAddresses(tree.Root, addresses);

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
            Global.Done = true;
            phraseQueue.CompleteAdding();
            addressQueue.CompleteAdding();
        }
        public void Consume() {
            int count = 0;
            Stopwatch stopWatch = new Stopwatch();
            Log.Debug("P2A" + threadNum + " start");

            while (!Global.Done) {

                //  Dequeue phrase
                Work w = null;

                queueWaitTime.Start();
                phraseQueue.TryTake(out w, System.Threading.Timeout.Infinite);
                queueWaitTime.Stop();

                if (w != null) {
 
                    foreach (string passphrase in this.passphrases.Enumerate()) {
                        //  Convert phrase to address

                        List<Address> addresses = null;
                        try {
                            stopWatch.Start();
                            if (Settings.KnownAddresses != null && Settings.KnownAddresses.Length > 0) {
                                //  Try to generate the known address
                                addresses = GetAddresses(w.phrase, passphrase, Settings.Paths, Settings.Accounts, Settings.Indices);
                                count++;
                            } 
                            else {
                                //  Generate address for account 0 index 0
                                addresses = GetAddresses(w.phrase, passphrase, 0, 0, Settings.Paths);
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

                            Work w2 = new Work(w.phrase, addresses);

                            //  Enqueue address

                            queueWaitTime.Start();
                            try {
                                addressQueue.Add(w2);
                            }
                            catch (InvalidOperationException) {
                                break;
                            }
                            finally {
                                queueWaitTime.Stop();
                            }
                        }
                    }
                }
            }

            if (count > 0) Log.Info("P2A" + threadNum + " done, count: " + count + " total time: " + stopWatch.ElapsedMilliseconds/1000 + $"s, time/req: {(count != 0 ? ((double)stopWatch.ElapsedMilliseconds/count) : 0):F2}ms/req, queue wait: " + queueWaitTime.ElapsedMilliseconds/1000 + "s");
            Finish();
        }

        protected static byte[] ed25519_seed = Encoding.ASCII.GetBytes("ed25519 seed");

        protected static byte[] TweakBits(byte[] data) {
            // * clear the lowest 3 bits
            // * clear the highest bit
            // * set the highest 2nd bit
            data[0]  &= 0b1111_1000;
            data[31] &= 0b0111_1111;
            data[31] |= 0b0100_0000;

            return data;         
        }
        protected byte[] HashRepeatedly(byte[] message) {
            HMAC512.Initialize();
            var iLiR = HMAC512.ComputeHash(message);
            if ((iLiR[31] & 0b0010_0000) != 0) {
                return HashRepeatedly(iLiR);
            }
            return iLiR;
        }

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