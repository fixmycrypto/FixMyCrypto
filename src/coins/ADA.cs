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
        public PhraseToAddressCardano(PhraseProducer phrases) : base(phrases) {
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
        public override Cryptography.Key DeriveRootKey(Phrase phrase, string passphrase) {
            if (IsUsingOpenCL()) {
                return DeriveRootKey_BatchPassphrases(phrase, new string[] { passphrase })[0];
            }

            byte[] entropy = phrase.Indices.ElevenToEight();
            var rootKey = Cryptography.Pbkdf2_HMAC512(passphrase, entropy, 4096, 96);
            rootKey = Cryptography.TweakBits(rootKey);
            return new Cryptography.Key(rootKey.Slice(0, 64), rootKey.Slice(64));
        }
        
        public override bool IsUsingOpenCL() {
            return (ocl != null);
        }
        
        public override int GetKeyLength() {
            return 96;
        }
        public override Cryptography.Key[] DeriveRootKey_BatchPhrases(Phrase[] phrases, string passphrase) {
            if (!IsUsingOpenCL()) {
                return base.DeriveRootKey_BatchPhrases(phrases, passphrase);
            }
            else {
                //  ADA reverses password/salt
                byte[][] salts = new byte[phrases.Length][];
                for (int i = 0; i < phrases.Length; i++) salts[i] = phrases[i].Indices.ElevenToEight();
                byte[] password = passphrase.ToUTF8Bytes();
                Seed[] seeds = ocl.Pbkdf2_Sha512_MultiSalt(phrases, new string[] { passphrase }, password, salts, false, 4096, 96);
                Cryptography.Key[] keys = new Cryptography.Key[phrases.Length];
                // Parallel.For(0, phrases.Length, i => {
                for (int i = 0; i < phrases.Length; i++) {
                    if (Global.Done) break;
                    byte[] key = Cryptography.TweakBits(seeds[i].seed);
                    keys[i] = new Cryptography.Key(key.Slice(0, 64), key.Slice(64));
                }
                return keys;
            }
        }
        public override Cryptography.Key[] DeriveRootKey_BatchPassphrases(Phrase phrase, string[] passphrases) {
            if (!IsUsingOpenCL()) {
                return base.DeriveRootKey_BatchPassphrases(phrase, passphrases);
            }
            else {
                //  ADA reverses password/salt
                byte[] salt = phrase.Indices.ElevenToEight();
                byte[][] passwords = new byte[passphrases.Length][];
                for (int i = 0; i < passphrases.Length; i++) passwords[i] = passphrases[i].ToUTF8Bytes();
                Seed[] seeds = ocl.Pbkdf2_Sha512_MultiPassword(new Phrase[] { phrase }, passphrases, passwords, salt, false, 4096, 96);
                Cryptography.Key[] keys = new Cryptography.Key[passphrases.Length];
                // Parallel.For(0, passphrases.Length, i => {
                for (int i = 0; i < passphrases.Length; i++) {
                    if (Global.Done) break;
                    byte[] key = Cryptography.TweakBits(seeds[i].seed);
                    keys[i] = new Cryptography.Key(key.Slice(0, 64), key.Slice(64));
                }
                return keys;
            }
        }
        
        protected override Cryptography.Key DeriveChildKey(Cryptography.Key parentKey, uint index) {
            var key = new CardanoSharp.Wallet.Models.Keys.PrivateKey(parentKey.data, parentKey.cc);
            string path = PathNode.GetPath(index);
            var child = key.Derive(path);
            return new Cryptography.Key(child.Key, child.Chaincode);
        }
        protected virtual CardanoSharp.Wallet.Models.Keys.PublicKey GetPublicKey(Cryptography.Key sk) {
            var key = new CardanoSharp.Wallet.Models.Keys.PrivateKey(sk.data, sk.cc);
            return key.GetPublicKey(false);
        }
        protected override Address DeriveAddress(PathNode node, int index) {
            var pub = GetPublicKey(node.Keys[index]);

            if (node.Parent.Value == 0) {
                //  Stake key is path with last 2 sections replaced by /2/0

                PathNode stakeNode = node.Parent.Parent.GetChild(2U).GetChild(0U);

                var stakePub = GetPublicKey(stakeNode.Keys[index]);

                var baseAddr = this.addressService.GetAddress(
                    pub, 
                    stakePub, 
                    CardanoSharp.Wallet.Enums.NetworkType.Mainnet, 
                    AddressType.Base);

                return new Address(baseAddr.ToString(), node.GetPath());
            }
            else if (node.Parent.Value == 2) {
                //  Stake address

                var stakeAddr = this.addressService.GetAddress(
                    pub,
                    pub,
                    CardanoSharp.Wallet.Enums.NetworkType.Mainnet, 
                    AddressType.Reward);
                
                return new Address(stakeAddr.ToString(), node.GetPath());
            }
            else {
                //  Shouldn't be here
                return null;
            }
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
            if (address.StartsWith("addr1")) {
                if (address.Length != 103) throw new Exception("ADA address incorrect length");
            }
            else if (address.StartsWith("stake1")) {
                if (address.Length != 59) throw new Exception("ADA address incorrect length");
            }
            else {
                 throw new Exception("ADA address must start with addr1 or stake1");
            }

            //  TODO: validate characters
        }

    }

    class PhraseToAddressCardanoLedger : PhraseToAddressCardano {
        public PhraseToAddressCardanoLedger(PhraseProducer phrases) : base(phrases) {
        }
        public override CoinType GetCoinType() { return CoinType.ADALedger; }
        public override Cryptography.Key DeriveRootKey(Phrase phrase, string passphrase) {
            var key = Cryptography.Ledger_expand_seed_ed25519_bip32(phrase, passphrase);

            return new Cryptography.Key(key.data, key.cc);
        }

        public override Cryptography.Key[] DeriveRootKey_BatchPhrases(Phrase[] phrases, string passphrase) {
            //  No OpenCL yet
            Cryptography.Key[] keys = new Cryptography.Key[phrases.Length];
            // Parallel.For(0, phrases.Length, i => {
            for (int i = 0; i < phrases.Length; i++) {
                if (Global.Done) break;
                keys[i] = DeriveRootKey(phrases[i], passphrase);
            }
            return keys;
        }

        public override Cryptography.Key[] DeriveRootKey_BatchPassphrases(Phrase phrase, string[] passphrases) {
            //  No OpenCL yet
            Cryptography.Key[] keys = new Cryptography.Key[passphrases.Length];
            // Parallel.For(0, passphrases.Length, i => {
            for (int i = 0; i < passphrases.Length; i++) {
                if (Global.Done) break;
                keys[i] = DeriveRootKey(phrase, passphrases[i]);
            }
            return keys;
        }
    }
    class PhraseToAddressCardanoTrezor : PhraseToAddressCardano {
        public PhraseToAddressCardanoTrezor(PhraseProducer phrases) : base(phrases) {
        }

        public override CoinType GetCoinType() { return CoinType.ADATrezor; }

        public override Cryptography.Key DeriveRootKey(Phrase phrase, string passphrase) {
            byte[] entropy = phrase.Indices.ElevenToEight(phrase.Length == 24);
            var rootKey = Cryptography.Pbkdf2_HMAC512(passphrase, entropy, 4096, 96);
            rootKey = Cryptography.TweakBits(rootKey);
            return new Cryptography.Key(rootKey.Slice(0, 64), rootKey.Slice(64));
        }

        public override Cryptography.Key[] DeriveRootKey_BatchPhrases(Phrase[] phrases, string passphrase) {
            //  No OpenCL yet
            Cryptography.Key[] keys = new Cryptography.Key[phrases.Length];
            // Parallel.For(0, phrases.Length, i => {
            for (int i = 0; i < phrases.Length; i++) {
                if (Global.Done) break;
                keys[i] = DeriveRootKey(phrases[i], passphrase);
            }
            return keys;
        }

        public override Cryptography.Key[] DeriveRootKey_BatchPassphrases(Phrase phrase, string[] passphrases) {
            //  No OpenCL yet
            Cryptography.Key[] keys = new Cryptography.Key[passphrases.Length];
            // Parallel.For(0, passphrases.Length, i => {
            for (int i = 0; i < passphrases.Length; i++) {
                if (Global.Done) break;
                keys[i] = DeriveRootKey(phrase, passphrases[i]);
            }
            return keys;
        }
    }
}