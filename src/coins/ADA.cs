using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CardanoSharp.Wallet;
using CardanoSharp.Wallet.Extensions.Models;
using CardanoSharp.Wallet.Enums;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace FixMyCrypto {
    class PhraseToAddressCardano : PhraseToAddress {
        protected AddressService addressService;
        public PhraseToAddressCardano(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses) : base(phrases, addresses) {
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
        public override Object DeriveRootKey(Phrase phrase, string passphrase) {
            if (IsUsingOpenCL()) {
                return DeriveRootKey_BatchPhrases(new Phrase[] { phrase }, passphrase)[0];
            }

            byte[] entropy = phrase.Indices.ElevenToEight();
            var rootKey = Cryptography.Pbkdf2_HMAC512(passphrase, entropy, 4096, 96);
            rootKey = Cryptography.TweakBits(rootKey);
            return new CardanoSharp.Wallet.Models.Keys.PrivateKey(rootKey.Slice(0, 64), rootKey.Slice(64));
        }
        
        public override bool IsUsingOpenCL() {
            return (ocl != null);
        }
        
        public override int GetKeyLength() {
            return 96;
        }
        public override Object[] DeriveRootKey_BatchPhrases(Phrase[] phrases, string passphrase) {
            if (!IsUsingOpenCL()) {
                return base.DeriveRootKey_BatchPhrases(phrases, passphrase);
            }
            else {
                //  ADA reverses password/salt
                byte[][] salts = new byte[phrases.Length][];
                for (int i = 0; i < phrases.Length; i++) salts[i] = phrases[i].Indices.ElevenToEight();
                byte[] password = passphrase.ToUTF8Bytes();
                Seed[] seeds = ocl.Pbkdf2_Sha512_MultiSalt(phrases, new string[] { passphrase }, password, salts, 4096, 96);
                Object[] keys = new object[phrases.Length];
                Parallel.For(0, phrases.Length, i => {
                    if (Global.Done) return;
                    byte[] key = Cryptography.TweakBits(seeds[i].seed);
                    keys[i] = new CardanoSharp.Wallet.Models.Keys.PrivateKey(key.Slice(0, 64), key.Slice(64));
                });
                return keys;
            }
        }
        public override Object[] DeriveRootKey_BatchPassphrases(Phrase phrase, string[] passphrases) {
            if (!IsUsingOpenCL()) {
                return base.DeriveRootKey_BatchPassphrases(phrase, passphrases);
            }
            else {
                //  ADA reverses password/salt
                byte[] salt = phrase.Indices.ElevenToEight();
                byte[][] passwords = new byte[passphrases.Length][];
                for (int i = 0; i < passphrases.Length; i++) passwords[i] = passphrases[i].ToUTF8Bytes();
                Seed[] seeds = ocl.Pbkdf2_Sha512_MultiPassword(new Phrase[] { phrase }, passphrases, passwords, salt, 4096, 96);
                Object[] keys = new object[passphrases.Length];
                Parallel.For(0, passphrases.Length, i => {
                    if (Global.Done) return;
                    byte[] key = Cryptography.TweakBits(seeds[i].seed);
                    keys[i] = new CardanoSharp.Wallet.Models.Keys.PrivateKey(key.Slice(0, 64), key.Slice(64));
                });
                return keys;
            }
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

            byte[] hash = Cryptography.SHA256Hash(includeChecksum ? entropy.Slice(0, 32) : entropy);
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
        public PhraseToAddressCardanoLedger(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses) : base(phrases, addresses) {
        }
        public override CoinType GetCoinType() { return CoinType.ADALedger; }
        public override Object DeriveRootKey(Phrase phrase, string passphrase) {
            var key = Cryptography.Ledger_expand_seed_ed25519_bip32(phrase, passphrase);

            return new CardanoSharp.Wallet.Models.Keys.PrivateKey(key.data, key.cc);
        }
    }
    class PhraseToAddressCardanoTrezor : PhraseToAddressCardano {
        public PhraseToAddressCardanoTrezor(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses) : base(phrases, addresses) {
        }

        public override CoinType GetCoinType() { return CoinType.ADATrezor; }

        public override Object DeriveRootKey(Phrase phrase, string passphrase) {
            byte[] entropy = phrase.Indices.ElevenToEight(phrase.Length == 24);
            var rootKey = Cryptography.Pbkdf2_HMAC512(passphrase, entropy, 4096, 96);
            rootKey = Cryptography.TweakBits(rootKey);
            return new CardanoSharp.Wallet.Models.Keys.PrivateKey(rootKey.Slice(0, 64), rootKey.Slice(64));
        }
    }


}