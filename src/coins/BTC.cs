using NBitcoin;
using NBitcoin.Altcoins;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FixMyCrypto {
    class PhraseToAddressBitAltcoin : PhraseToAddress {
        private Network network;
        private CoinType coinType;
        public PhraseToAddressBitAltcoin(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses, int threadNum, int threadMax, CoinType coin) : base(phrases, addresses, threadNum, threadMax) {
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
            return ExtKey.CreateFromSeed(b.DeriveSeed(passphrase));
        }
        protected override Object DeriveChildKey(Object parentKey, uint index) {
            ExtKey key = (ExtKey)parentKey;
            return key.Derive(index);
        }
        protected override Address DeriveAddress(PathNode node) {
            ExtKey pk = (ExtKey)node.Key;
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
}