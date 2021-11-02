using System;
using System.Text;
using System.Collections.Concurrent;
using CardanoSharp.Wallet;
using CardanoSharp.Wallet.Extensions.Models;
using CardanoSharp.Wallet.Enums;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace FixMyCrypto {
    class PhraseToAddressCardano : PhraseToAddress {
        protected AddressService addressService;
        public PhraseToAddressCardano(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) {
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
            var pub = GetPublicKey(node.Key);

            //  Stake key is path with last 2 sections replaced by /2/0

            PathNode stakeNode = node.Parent.Parent.GetChild(2U).GetChild(0U);

            var stakePub = GetPublicKey(stakeNode.Key);

            var baseAddr = this.addressService.GetAddress(
                pub, 
                stakePub, 
                CardanoSharp.Wallet.Enums.NetworkType.Mainnet, 
                AddressType.Base);

            return new Address(baseAddr.ToString(), node.GetPath());
        }
        public CardanoSharp.Wallet.Models.Keys.Mnemonic Restore(Phrase phrase, bool includeChecksum = false) {
            byte[] entropy = phrase.Indices.ElevenToEight(includeChecksum);

            int CS = (phrase.Length * 11) % 32;
            uint mask = (1U << CS) - 1;
            byte expectedChecksum = (byte)(phrase.Indices[phrase.Length - 1] & mask);

            // Checksum is the "first" CS bits of hash: [****xxxx]

            using SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(includeChecksum ? entropy.Slice(0, 32) : entropy);
            byte actualChecksum = (byte)(hash[0] >> (8 - CS));

            if (expectedChecksum != actualChecksum) {
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
        private HMACSHA256 HMAC256;
        public PhraseToAddressCardanoLedger(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) {
            HMAC256 = new HMACSHA256(ed25519_seed);
        }
        public override CoinType GetCoinType() { return CoinType.ADALedger; }
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
        public PhraseToAddressCardanoTrezor(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) {
        }

        public override CoinType GetCoinType() { return CoinType.ADATrezor; }

        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {
            CardanoSharp.Wallet.Models.Keys.Mnemonic l;

            //  For 24 words, Trezor includes the checksum in the entropy

            if (phrase.Length == 24) {
                //  See: https://github.com/trezor/trezor-firmware/issues/1387
                l = Restore(phrase, true);
            }
            else {
                l = Restore(phrase);
            }

            var rootKey = KeyDerivation.Pbkdf2(passphrase, l.Entropy, KeyDerivationPrf.HMACSHA512, 4096, 96);
            rootKey = TweakBits(rootKey);

            return new CardanoSharp.Wallet.Models.Keys.PrivateKey(rootKey.Slice(0, 64), rootKey.Slice(64));
        }
    }


}