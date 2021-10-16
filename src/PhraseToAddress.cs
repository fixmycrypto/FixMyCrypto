using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NBitcoin;
using NBitcoin.Altcoins;
using Cryptography.ECDSA;
using Org.BouncyCastle.Crypto.Digests;
using CardanoSharp.Wallet;
using CardanoSharp.Wallet.Extensions.Models;
using CardanoSharp.Wallet.Enums;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace FixMyCrypto {
    abstract class PhraseToAddress {
        protected ConcurrentQueue<Work> phraseQueue, addressQueue;
        protected int threadNum, threadMax;
        string[] defaultPaths;
        PathTree tree;
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
                lock (mutex) {
                    if (defaultPaths == null) {
                        defaultPaths = GetDefaultPaths(Settings.knownAddresses);
                    }
                }

                paths = defaultPaths;
            }

            //  Create path tree if needed
            lock (mutex) {
                if (tree == null) {
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
                    Passphrase p = Passphrase.Create(Settings.passphrase);

                    foreach (string passphrase in p.Next()) {
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
    }
   class PhraseToAddressEth : PhraseToAddress {
        public PhraseToAddressEth(ConcurrentQueue<Work> phrases, ConcurrentQueue<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) {
        }
        public override CoinType GetCoinType() { return CoinType.ETH; }
        private char GetChecksumDigit(byte h, char c) {
            if (h >= 8) {
                return Char.ToUpper(c);
            }
            else {
                return Char.ToLower(c);
            }
        }
        public string Checksum(string addr) {
            var digest = new KeccakDigest(256);
            byte[] bytes = Encoding.ASCII.GetBytes(addr.ToLower());
            digest.BlockUpdate(bytes, 0, bytes.Length);
            byte[] h = new byte[digest.GetByteLength()];
            digest.DoFinal(h, 0);

            char[] ret = new char[42];
            ret[0] = '0';
            ret[1] = 'x';
            int q = 0;
            int p = 0;

            while (p < 40) {
                byte upper = (byte)(h[q] >> 4);
                byte lower = (byte)(h[q] & 0x0f);
                q++;

                ret[p + 2] = GetChecksumDigit(upper, addr[p]);
                p++;
                ret[p + 2] = GetChecksumDigit(lower, addr[p]);
                p++;
            }

            return new string(ret);
        }
        private string PkToAddress(ExtKey pk) {
            byte[] pkeyBytes = pk.PrivateKey.PubKey.ToBytes();
            byte[] converted = Secp256K1Manager.PublicKeyDecompress(pkeyBytes);

            var digest = new KeccakDigest(256);
            digest.BlockUpdate(converted,1,64);
            byte[] hash = new byte[digest.GetByteLength()];
            digest.DoFinal(hash, 0);

            string l = hash.ToHexString(12, 20);

            string checksum = Checksum(l);

            return checksum;
        }
        public override string[] GetDefaultPaths(string[] knownAddresses) {
            string[] p = { "m/44'/60'/{account}'/0/{index}" };
            return p;
        }
 
        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {
            //  TODO avoid string conversion
            string p = phrase.ToPhrase();
            Mnemonic b = new Mnemonic(p);
            return new ExtKey(b.DeriveSeed(passphrase));
        }
        protected override Object DeriveChildKey(Object parentKey, uint index) {
            ExtKey key = (ExtKey)parentKey;
            return key.Derive(index);
        }
        protected override Address DeriveAddress(PathNode node) {
            ExtKey pk = (ExtKey)node.key;
            string address = PkToAddress(pk);
            return new Address(address, node.GetPath());
        }

        public override void ValidateAddress(string address) {
            if (address.Length != 42) throw new Exception("ETH address length should be 42 chars");

            if (!address.StartsWith("0x")) throw new Exception("ETH address should start with 0x");

            string stripped = address.Substring(2);

            string checksum = Checksum(stripped);

            if (checksum != address) Log.Warning($"ETH address checksum is incorrect, should be: {checksum}");
        }
    }
    class PhraseToAddressBitAltcoin : PhraseToAddress {
        private Network network;
        private CoinType coinType;
        public PhraseToAddressBitAltcoin(ConcurrentQueue<Work> phrases, ConcurrentQueue<Work> addresses, int threadNum, int threadMax, CoinType coin) : base(phrases, addresses, threadNum, threadMax) {
            this.coinType = coin;

            switch (this.coinType) {
                case CoinType.BTC:

                this.network = Network.Main;

                break;

                case CoinType.DOGE:

                this.network = Dogecoin.Instance.Mainnet;

                break;

                case CoinType.LTC:

                this.network = Litecoin.Instance.Mainnet;

                break;

                case CoinType.BCH:

                this.network = Network.Main;    //  TODO

                break;

                case CoinType.XRP:

                this.network = Network.Main;    //  TODO

                break;

                default:

                throw new NotSupportedException();
            }
        }
        public override string[] GetDefaultPaths(string[] knownAddresses) {
            List<string> paths = new List<string>();

            switch (this.coinType) {
                case CoinType.BTC:

                if (knownAddresses != null && knownAddresses.Length > 0) {
                    foreach (string address in knownAddresses) {
                        //  Guess path from known address format

                        if (address.StartsWith("bc1")) {
                            //  BIP84
                            paths.Add("m/84'/0'/{account}'/0/{index}");
                        }
                        else if (address.StartsWith("3")) {
                            //  BIP49
                            paths.Add("m/49'/0'/{account}'/0/{index}");
                        }
                        else {
                            //  BIP32 MultiBit HD
                            paths.Add("m/0'/0/{index}");
                            //  BIP32 Bitcoin Core
                            paths.Add("m/0'/0'/{index}'");
                            //  BIP32 blockchain.info/Coinomi/Ledger
                            //this.defaultPaths.Add("m/44'/0'/{account}'/{index}");
                            //  BIP44
                            paths.Add("m/44'/0'/{account}'/0/{index}");
                        }
                    }
                }
                else {
                    //  Unknown path

                    //  BIP32 MultiBit HD
                    paths.Add("m/0'/0/{index}");
                    //  BIP32 Bitcoin Core
                    paths.Add("m/0'/0'/{index}'");
                    //  BIP32 blockchain.info/Coinomi/Ledger
                    //this.defaultPaths.Add("m/44'/0'/{account}'/{index}");
                    //  BIP44
                    paths.Add("m/44'/0'/{account}'/0/{index}");
                    //  BIP49
                    paths.Add("m/49'/0'/{account}'/0/{index}");
                    //  BIP84
                    paths.Add("m/84'/0'/{account}'/0/{index}");
                }

                break;

                case CoinType.DOGE:

                paths.Add("m/44'/3'/{account}'/0/{index}");

                break;

                case CoinType.LTC:

                if (knownAddresses != null && knownAddresses.Length > 0) {
                    foreach (string address in knownAddresses) {
                        if (address.StartsWith("M")) {
                            paths.Add("m/49'/2'/{account}'/0/{index}");
                        }
                        else if (address.StartsWith("ltc1")) {
                            paths.Add("m/84'/2'/{account}'/0/{index}");
                        }
                        else {
                            paths.Add("m/44'/2'/{account}'/0/{index}");
                        }
                    }
                }
                else {
                    paths.Add("m/44'/2'/{account}'/0/{index}");
                    paths.Add("m/49'/2'/{account}'/0/{index}");
                    paths.Add("m/84'/2'/{account}'/0/{index}");
                }

                break;

                case CoinType.BCH:

                //  Pre-Fork
                paths.Add("m/44'/0'/{account}'/0/{index}");

                //  Post-Fork
                paths.Add("m/44'/145'/{account}'/0/{index}");

                break;

                case CoinType.XRP:

                paths.Add("m/44'/144'/{account}'/0/{index}");

                break;

                default:

                throw new NotSupportedException();
            }

            return paths.ToArray();
        }
        public override CoinType GetCoinType() { return this.coinType; }

        private ScriptPubKeyType GetKeyType(string path) {
            ScriptPubKeyType keyType = ScriptPubKeyType.Legacy;
            if (path.StartsWith("m/49")) keyType = ScriptPubKeyType.SegwitP2SH;
            if (path.StartsWith("m/84")) keyType = ScriptPubKeyType.Segwit;
            return keyType;
        }
        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {
            //  TODO avoid string conversion
            string p = phrase.ToPhrase();
            Mnemonic b = new Mnemonic(p);
            return new ExtKey(b.DeriveSeed(passphrase));
        }
        protected override Object DeriveChildKey(Object parentKey, uint index) {
            ExtKey key = (ExtKey)parentKey;
            return key.Derive(index);
        }
        protected override Address DeriveAddress(PathNode node) {
            ExtKey pk = (ExtKey)node.key;
            string path = node.GetPath();
            string address = pk.GetPublicKey().GetAddress(GetKeyType(path), this.network).ToString();
            return new Address(address, path);
        }

        public override void ValidateAddress(string address) {
            switch (this.coinType) {
                case CoinType.BTC:

                if (address.StartsWith("1"))
                {
                    var addr = new BitcoinPubKeyAddress(address, Network.Main);
                } 
                else if(address.StartsWith("3"))
                {
                    var addr = new BitcoinScriptAddress(address, Network.Main);
                }
                else if (address.StartsWith("bc1q"))
                {
                    var addr = new BitcoinWitPubKeyAddress(address, Network.Main);
                }
                else
                {
                    throw new Exception("Invalid address");
                }

                break;

                case CoinType.BCH:

                //  TODO: Not working

                // var bch = BitcoinAddress.Create(address, NBitcoin.Altcoins.BCash.Instance.Mainnet);

                /*
                if (address.StartsWith("1")) {
                    var bch = new BitcoinPubKeyAddress(address, NBitcoin.Altcoins.BCash.Instance.Mainnet);
                }
                else if (address.StartsWith("q")) {
                    var bch = new BitcoinPubKeyAddress(address, NBitcoin.Altcoins.BCash.Instance.Mainnet);
                }
                else {
                    throw new Exception("Invalid address");
                }
                */

                break;

                case CoinType.DOGE:

                if (!address.StartsWith("D")) throw new Exception("Invalid address");

                var doge = new BitcoinPubKeyAddress(address, NBitcoin.Altcoins.Dogecoin.Instance.Mainnet);

                break;

                case CoinType.LTC:

                if (address.StartsWith("L"))
                {
                    var addr = new BitcoinPubKeyAddress(address, NBitcoin.Altcoins.Litecoin.Instance.Mainnet);
                }
                else if(address.StartsWith("M"))
                {
                    var addr = new BitcoinScriptAddress(address, NBitcoin.Altcoins.Litecoin.Instance.Mainnet);
                }
                else if (address.StartsWith("ltc1q"))
                {
                    var addr = new BitcoinWitPubKeyAddress(address, NBitcoin.Altcoins.Litecoin.Instance.Mainnet);
                }
                else
                {
                    throw new Exception("Invalid address");
                }

                break;

                //  TODO: other coins

                default:

                break;
            }
        }
    }

    class PhraseToAddressCardano : PhraseToAddress {
        protected AddressService addressService;
        public PhraseToAddressCardano(ConcurrentQueue<Work> phrases, ConcurrentQueue<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) {
            this.addressService = new AddressService();
        }
        public override CoinType GetCoinType() { return CoinType.ADA; }
        public override string[] GetDefaultPaths(string[] knownAddresses) {
            string[] p = { "m/1852'/1815'/{account}'/0/{index}" };
            return p;
        }
        protected override string GetStakePath() {
            return "m/1852'/1815'/{account}'/2/0";
        }
        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {
            var m = this.Restore(phrase);
            var masterKey = m.GetRootKey(passphrase);

            return masterKey;
        }
        protected override Object DeriveChildKey(Object parentKey, uint index) {
            var key = (CardanoSharp.Wallet.Models.Keys.PrivateKey)parentKey;
            string path = PathNode.GetPath(index);
            return key.Derive(path);
        }
        protected virtual CardanoSharp.Wallet.Models.Keys.PublicKey GetPublicKey(Object pk) {
            var key = (CardanoSharp.Wallet.Models.Keys.PrivateKey)pk;
            return key.GetPublicKey(false);
        }
        protected override Address DeriveAddress(PathNode node) {
            var pub = GetPublicKey(node.key);

            //  Stake key is path with last 2 sections replaced by /2/0

            PathNode stakeNode = node.parent.parent.GetChild(2U).GetChild(0U);

            var stakePub = GetPublicKey(stakeNode.key);

            var baseAddr = this.addressService.GetAddress(
                pub, 
                stakePub, 
                CardanoSharp.Wallet.Enums.NetworkType.Mainnet, 
                AddressType.Base);

            return new Address(baseAddr.ToString(), node.GetPath());
        }
        protected static byte[] TweakBits(byte[] data) {
            // * clear the lowest 3 bits
            // * clear the highest bit
            // * set the highest 2nd bit
            data[0]  &= 0b1111_1000;
            data[31] &= 0b0111_1111;
            data[31] |= 0b0100_0000;

            return data;         
        }

        public CardanoSharp.Wallet.Models.Keys.Mnemonic Restore(Phrase phrase)
        {
            short[] indices = phrase.Indices;
            // Compute and check checksum
            int MS = indices.Length;
            int ENTCS = MS * 11;
            int CS = ENTCS % 32;
            int ENT = ENTCS - CS;

            var entropy = new byte[ENT / 8];

            int itemIndex = 0;
            int bitIndex = 0;
            // Number of bits in a word
            int toTake = 8;
            // Indexes are held in a UInt32 but they are only 11 bits
            int maxBits = 11;
            for (int i = 0; i < entropy.Length; i++)
            {
                if (bitIndex + toTake <= maxBits)
                {
                    // All 8 bits are in one item

                    // To take 8 bits (*) out of 00000000 00000000 00000xx* *******x:
                    // 1. Shift right to get rid of extra bits on right, then cast to byte to get rid of the rest
                    // >> maxBits - toTake - bitIndex
                    entropy[i] = (byte)(indices[itemIndex] >> (3 - bitIndex));
                }
                else
                {
                    // Only a part of 8 bits are in this item, the rest is in the next.
                    // Since items are only 32 bits there is no other possibility (8<32)

                    // To take 8 bits(*) out of [00000000 00000000 00000xxx xxxx****] [00000000 00000000 00000*** *xxxxxxx]:
                    // Take first item at itemIndex [00000000 00000000 00000xxx xxxx****]: 
                    //    * At most 7 bits and at least 1 bit should be taken
                    // 1. Shift left [00000000 00000000 0xxxxxxx ****0000] (<< 8 - (maxBits - bitIndex)) 8-max+bi
                    // 2. Zero the rest of the bits (& (00000000 00000000 00000000 11111111))

                    // Take next item at itemIndex+1 [00000000 00000000 00000*** *xxxxxxx]
                    // 3. Shift right [00000000 00000000 00000000 0000****]
                    // number of bits already taken = maxBits - bitIndex
                    // nuber of bits to take = toTake - (maxBits - bitIndex)
                    // Number of bits on the right to get rid of= maxBits - (toTake - (maxBits - bitIndex))
                    // 4. Add two values to each other using bitwise OR [****0000] | [0000****]
                    entropy[i] = (byte)(((indices[itemIndex] << (bitIndex - 3)) & 0xff) |
                                         (indices[itemIndex + 1] >> (14 - bitIndex)));
                }

                bitIndex += toTake;
                if (bitIndex >= maxBits)
                {
                    bitIndex -= maxBits;
                    itemIndex++;
                }
            }

            // Compute and compare checksum:
            // CS is at most 8 bits and it is the remaining bits from the loop above and it is only from last item
            // [00000000 00000000 00000xxx xxxx****]
            // We already know the number of bits here: CS
            // A simple & does the work
            uint mask = (1U << CS) - 1;
            byte expectedChecksum = (byte)(indices[itemIndex] & mask);

            // Checksum is the "first" CS bits of hash: [****xxxx]

            using SHA256 hash = SHA256.Create();
            byte[] hashOfEntropy = hash.ComputeHash(entropy);
            byte actualChecksum = (byte)(hashOfEntropy[0] >> (8 - CS));

            if (expectedChecksum != actualChecksum)
            {
                Array.Clear(indices, 0, indices.Length);
                indices = null;

                throw new FormatException("Wrong checksum.");
            }

            return new CardanoSharp.Wallet.Models.Keys.Mnemonic(phrase.ToPhrase(), entropy);
        }

        public override void ValidateAddress(string address) {
            if (!address.StartsWith("addr1q")) throw new Exception("ADA address must start with addr1q");

            if (address.Length != 103) throw new Exception("ADA address incorrect length");

            //  TODO: validate characters
        }

    }

    class PhraseToAddressCardanoLedger : PhraseToAddressCardano {
        private HMACSHA512 HMAC512;
        private HMACSHA256 HMAC256;
        public PhraseToAddressCardanoLedger(ConcurrentQueue<Work> phrases, ConcurrentQueue<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) {
            HMAC512 = new HMACSHA512(ed25519_seed);
            HMAC256 = new HMACSHA256(ed25519_seed);
        }
        public override CoinType GetCoinType() { return CoinType.ADALedger; }
        protected byte[] HashRepeatedly(byte[] message) {
            HMAC512.Initialize();
            var iLiR = HMAC512.ComputeHash(message);
            if ((iLiR[31] & 0b0010_0000) != 0) {
                return HashRepeatedly(iLiR);
            }
            return iLiR;
        }

        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {
            //  https://github.com/LedgerHQ/speculos/blob/c0311aef48412e40741a55f113939469da78e8e5/src/bolos/os_bip32.c#L123

            byte[] salt = Encoding.UTF8.GetBytes("mnemonic" + passphrase);
            string password = phrase.ToPhrase();
            var seed = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, 2048, 64);

            byte[] message = new byte[seed.Length + 1];
            message[0] = 1;
            System.Buffer.BlockCopy(seed, 0, message, 1, seed.Length);

            //  Chain code

            HMAC256.Initialize();
            var cc = HMAC256.ComputeHash(message);

            var iLiR = HashRepeatedly(seed);
            iLiR = TweakBits(iLiR);

            return new CardanoSharp.Wallet.Models.Keys.PrivateKey(iLiR, cc);
        }
    }
    class PhraseToAddressCardanoTrezor : PhraseToAddressCardano {
        public PhraseToAddressCardanoTrezor(ConcurrentQueue<Work> phrases, ConcurrentQueue<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) {
        }

        public override CoinType GetCoinType() { return CoinType.ADATrezor; }

        //  Returns the seed entropy including the checksum bits
        //  See: https://github.com/trezor/trezor-firmware/issues/1387
        public static CardanoSharp.Wallet.Models.Keys.Mnemonic RestoreWithChecksum(Phrase phrase)
        {
            short[] indices = phrase.Indices;

            // Compute and check checksum
            int MS = indices.Length;
            int ENTCS = MS * 11;
            int CS = ENTCS % 32;
            int ENT = ENTCS;   //  changed;

            var entropy = new byte[(int)Math.Ceiling(ENT / 8.0)];   //  changed

            int itemIndex = 0;
            int bitIndex = 0;
            // Number of bits in a word
            int toTake = 8;
            // Indexes are held in a UInt32 but they are only 11 bits
            int maxBits = 11;
            for (int i = 0; i < entropy.Length; i++)
            {
                if (bitIndex + toTake <= maxBits)
                {
                    // All 8 bits are in one item

                    // To take 8 bits (*) out of 00000000 00000000 00000xx* *******x:
                    // 1. Shift right to get rid of extra bits on right, then cast to byte to get rid of the rest
                    // >> maxBits - toTake - bitIndex
                    entropy[i] = (byte)(indices[itemIndex] >> (3 - bitIndex));
                }
                else
                {
                    // Only a part of 8 bits are in this item, the rest is in the next.
                    // Since items are only 32 bits there is no other possibility (8<32)

                    // To take 8 bits(*) out of [00000000 00000000 00000xxx xxxx****] [00000000 00000000 00000*** *xxxxxxx]:
                    // Take first item at itemIndex [00000000 00000000 00000xxx xxxx****]: 
                    //    * At most 7 bits and at least 1 bit should be taken
                    // 1. Shift left [00000000 00000000 0xxxxxxx ****0000] (<< 8 - (maxBits - bitIndex)) 8-max+bi
                    // 2. Zero the rest of the bits (& (00000000 00000000 00000000 11111111))

                    // Take next item at itemIndex+1 [00000000 00000000 00000*** *xxxxxxx]
                    // 3. Shift right [00000000 00000000 00000000 0000****]
                    // number of bits already taken = maxBits - bitIndex
                    // nuber of bits to take = toTake - (maxBits - bitIndex)
                    // Number of bits on the right to get rid of= maxBits - (toTake - (maxBits - bitIndex))
                    // 4. Add two values to each other using bitwise OR [****0000] | [0000****]
                    entropy[i] = (byte)(((indices[itemIndex] << (bitIndex - 3)) & 0xff) |
                                         ((itemIndex + 1 >= indices.Length ? 0 : indices[itemIndex + 1]) >> (14 - bitIndex)));
                }

                bitIndex += toTake;
                if (bitIndex >= maxBits)
                {
                    bitIndex -= maxBits;
                    itemIndex++;
                }
            }

            return new CardanoSharp.Wallet.Models.Keys.Mnemonic(phrase.ToPhrase(), entropy);
        }
        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {
            CardanoSharp.Wallet.Models.Keys.Mnemonic l;

            //  For 24 words, Trezor includes the checksum in the entropy

            if (phrase.Length == 24) {
                l = RestoreWithChecksum(phrase);
            }
            else {
                l = Restore(phrase);
            }

            var rootKey = KeyDerivation.Pbkdf2(passphrase, l.Entropy, KeyDerivationPrf.HMACSHA512, 4096, 96);
            rootKey = TweakBits(rootKey);

            return new CardanoSharp.Wallet.Models.Keys.PrivateKey(rootKey.Slice(0, 64), rootKey.Slice(64));
        }
    }

    //  TODO: Not working
    class PhraseToAddressSolana : PhraseToAddress {
        private HMACSHA512 HMAC512;
        public enum KeyType {
            Public,
            Private
        }
        class Key {
            public byte[] data;
            public byte[] cc;
            public Key(byte[] data, byte[] cc) {
                this.data = data;
                this.cc = cc;
            }
        }
        public PhraseToAddressSolana(ConcurrentQueue<Work> phrases, ConcurrentQueue<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) {
            this.HMAC512 = new HMACSHA512(ed25519_seed);
        }
        public override CoinType GetCoinType() { return CoinType.SOL; }
        public override string[] GetDefaultPaths(string[] knownAddresses) {
            string[] p = { 
                "m/44'/501'/{account}'",
                "m/44'/501'/{account}'/{index}'",
                // not sure how to implement non-hardened derivation
                // "m/501'/{account}'/0/{index}" 
                };
            return p;
        }
        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {
            //  https://github.com/LedgerHQ/speculos/blob/c0311aef48412e40741a55f113939469da78e8e5/src/bolos/os_bip32.c#L112

            byte[] salt = Encoding.UTF8.GetBytes("mnemonic" + passphrase);
            string password = phrase.ToPhrase();
            var seed = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, 2048, 64);

            //  expand_seed_slip10

            HMAC512.Initialize();
            var hash = HMAC512.ComputeHash(seed);

            var iL = hash.Slice(0, 32);
            var iR = hash.Slice(32);

            return new Key(iL, iR);
        }
        protected override Object DeriveChildKey(Object parentKey, uint index) {
            //  https://github.com/LedgerHQ/speculos/blob/c0311aef48412e40741a55f113939469da78e8e5/src/bolos/os_bip32.c#L342
            //  https://github.com/alepop/ed25519-hd-key/blob/d8c0491bc39e197c86816973e80faab54b9cbc26/src/index.ts#L33

            if (!PathNode.IsHardened(index)) {
                throw new NotSupportedException("SOL non-hardened path not supported");
            }

            Key x = (Key)parentKey;

            byte[] tmp = new byte[37];
            tmp[0] = 0;
            System.Buffer.BlockCopy(x.data, 0, tmp, 1, 32);

            tmp[33] = (byte)((index >> 24) & 0xff);
            tmp[34] = (byte)((index >> 16) & 0xff);
            tmp[35] = (byte)((index >> 8) & 0xff);
            tmp[36] = (byte)(index & 0xff);

            HMACSHA512 hmac = new HMACSHA512(x.cc);
            byte[] I = hmac.ComputeHash(tmp);

            return new Key(I.Slice(0, 32), I.Slice(32));
        }

        protected override Address DeriveAddress(PathNode node) {
            Key key = (Key)node.key;

            byte[] pub = Chaos.NaCl.Ed25519.PublicKeyFromSeed(key.data);

            string address = Base58.Encode(pub);
            return new Address(address, node.GetPath());
        }

        public override void ValidateAddress(string address) {
            var tmp = Base58.Decode(address);
            if (tmp.Length != 32) throw new Exception("invalid SOL address length");
        }

    }
}