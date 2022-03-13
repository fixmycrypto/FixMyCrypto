using NBitcoin;
using NBitcoin.Altcoins;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixMyCrypto {
    class PhraseToAddressBitAltcoin : PhraseToAddress {
        private Network network;
        private CoinType coinType;
        public PhraseToAddressBitAltcoin(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses, CoinType coin) : base(phrases, addresses) {
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

                this.network = Network.Main;

                break;

                case CoinType.ETH:

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

                        if (address.StartsWith("bc1q")) {
                            //  BIP84
                            paths.Add("m/84'/0'/{account}'/0/{index}");
                        }
                        else if (address.StartsWith("bc1p") || address.StartsWith("tb1")) {
                            //  BIP86
                            paths.Add("m/86'/0'/{account}'/0/{index}");
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
                            paths.Add("m/44'/0'/{account}'/{index}");
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
                    paths.Add("m/44'/0'/{account}'/{index}");
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
            if (path.StartsWith("m/86")) keyType = ScriptPubKeyType.TaprootBIP86;
            return keyType;
        }
        public override Cryptography.Key DeriveRootKey(Phrase phrase, string passphrase) {
            if (IsUsingOpenCL() && !ocl.IsBusy) {
                return DeriveRootKey_BatchPhrases(new Phrase[] { phrase }, passphrase)[0];
            }

            string p = phrase.ToPhrase();
            byte[] salt = Cryptography.PassphraseToSalt(passphrase);
            byte[] seed = Cryptography.Pbkdf2_HMAC512(p, salt, 2048, 64);
            byte[] key = Cryptography.HMAC512_Bitcoin(seed);
            return new Cryptography.Key(key.Slice(0, 32), key.Slice(32));
        }
        public override bool IsUsingOpenCL() {
            return (ocl != null);
        }
        public override Cryptography.Key[] DeriveRootKey_BatchPhrases(Phrase[] phrases, string passphrase) {
            if (!IsUsingOpenCL() || ocl.IsBusy) {
                return base.DeriveRootKey_BatchPhrases(phrases, passphrase);
            }

            byte[][] passwords = new byte[phrases.Length][];
            for (int i = 0; i < phrases.Length; i++) passwords[i] = phrases[i].ToPhrase().ToUTF8Bytes();
            byte[] salt = Cryptography.PassphraseToSalt(passphrase);
            Seed[] seeds = ocl.Pbkdf2_Sha512_MultiPassword(phrases, new string[] { passphrase }, passwords, salt);
            Cryptography.Key[] keys = new Cryptography.Key[phrases.Length];
            Parallel.For(0, phrases.Length, i => {
                if (Global.Done) return;
                byte[] key = Cryptography.HMAC512_Bitcoin(seeds[i].seed);
                keys[i] = new Cryptography.Key(key.Slice(0, 32), key.Slice(32));
            });
            return keys;
        }
        public override Cryptography.Key[] DeriveRootKey_BatchPassphrases(Phrase phrase, string[] passphrases) {
            if (!IsUsingOpenCL() || ocl.IsBusy) {
                return base.DeriveRootKey_BatchPassphrases(phrase, passphrases);
            }

            byte[] password = phrase.ToPhrase().ToUTF8Bytes();
            byte[][] salts = new byte[passphrases.Length][];
            for (int i = 0; i < passphrases.Length; i++) salts[i] = Cryptography.PassphraseToSalt(passphrases[i]);
            Seed[] seeds = ocl.Pbkdf2_Sha512_MultiSalt(new Phrase[] { phrase }, passphrases, password, salts);
            Cryptography.Key[] keys = new Cryptography.Key[passphrases.Length];
            Parallel.For(0, passphrases.Length, i => {
                if (Global.Done) return;
                byte[] key = Cryptography.HMAC512_Bitcoin(seeds[i].seed);
                keys[i] = new Cryptography.Key(key.Slice(0, 32), key.Slice(32));
            });
            return keys;
        }
        protected override Cryptography.Key DeriveChildKey(Cryptography.Key parentKey, uint index) {
            if (IsUsingOpenCL() && !ocl.IsBusy) {
                return DeriveChildKey_Batch(new Cryptography.Key[] { parentKey }, index)[0];
            }

            return parentKey.Derive_Bip32(index);
        }
        protected override Cryptography.Key[] DeriveChildKey_Batch(Cryptography.Key[] parents, uint index) {
            if (!IsUsingOpenCL() || ocl.IsBusy) {
                return base.DeriveChildKey_Batch(parents, index);
            }

            return ocl.Bip32_Derive(parents, index);
        }

        protected override Address DeriveAddress(PathNode node, int index) {
            Cryptography.Key key = node.Keys[index];
            ExtKey sk = new ExtKey(new Key(key.data), key.cc);

            string path = node.GetPath();
            string address = sk.GetPublicKey().GetAddress(GetKeyType(path), this.network).ToString();
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
                else if (address.StartsWith("bc1p") || address.StartsWith("tb1"))
                {
                    var addr = TaprootAddress.Create(address, Network.Main);
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
}