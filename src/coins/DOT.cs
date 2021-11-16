using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using CardanoSharp.Wallet.Extensions.Models;
using Cryptography.ECDSA;

namespace FixMyCrypto {

    class PhraseToAddressPolkadot : PhraseToAddress {
        public PhraseToAddressPolkadot(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) { 
        }

        public override CoinType GetCoinType() { return CoinType.DOT; }

        public override string[] GetDefaultPaths(string[] knownAddresses) {
            if (knownAddresses == null || knownAddresses.Length == 0)
                return new string[] { "m/0", "m/2", "m/42" };

            List<string> paths = new List<string>();

            foreach (string address in knownAddresses) {
                if (address.StartsWith("1")) paths.Add("m/0");          //  Polkadot

                else if (address.StartsWith("5")) paths.Add("m/42");    //  Generic substrate

                else paths.Add("m/2");                                  //  Kusama
            }

            return paths.ToArray();
        }
 
        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {
            //  https://github.com/paritytech/substrate-bip39/blob/c56994c06fe29693cfed445400ddc53bb12e472b/src/lib.rs#L45
            byte[] entropy = phrase.Indices.ElevenToEight();
            byte[] salt = Cryptography.PassphraseToSalt(passphrase);
            byte[] seed = Cryptography.Pbkdf2_HMAC512(entropy, salt, 2048, 64);

            return new Cryptography.Key(seed, null);
        }

        protected override Object DeriveChildKey(Object parentKey, uint index) {
            return parentKey;
        }

        protected virtual byte[] GetPublicKey(byte[] seed) {
            return Cryptography.Sr25519PublicKeyFromSeed(seed);
        }

        protected virtual byte GetPrefix(PathNode node) {
            return (byte)node.Value;
        }
        protected override Address DeriveAddress(PathNode node) {
            var key = (Cryptography.Key)node.Key;
            if (key == null) return null;

            byte[] pub = GetPublicKey(key.data);

            //  https://github.com/usetech-llc/polkadot_api_dotnet/blob/2b0021f8403525358d1f4721c9a64f838a74cd90/Polkadot/src/DataStructs/Metadata/Metadata.cs#L252

            var ssPrefixed1 = new byte[] { 0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45 };
            byte[] prefixed = new byte[32 + 8];
            Array.Copy(ssPrefixed1, 0, prefixed, 0, 7);
            prefixed[7] = GetPrefix(node);
            Array.Copy(pub, 0, prefixed, 8, 32);

            byte[] blake2b = Cryptography.Blake2bHash(prefixed);

            byte[] checksummed = new byte[35];
            checksummed[0] = GetPrefix(node);
            Array.Copy(pub, 0, checksummed, 1, 32);
            checksummed[33] = blake2b[0];
            checksummed[34] = blake2b[1];

            string address = Base58.Encode(checksummed);
            return new Address(address, node.GetPath());
        }

        public override void ValidateAddress(string address) {
            byte[] data = Base58.Decode(address);

            if (data.Length != 35) throw new Exception($"Incorrect DOT pk length: {data.Length}");

            byte[] prefixedPub = data.Slice(0, 33);
            var ssPrefixed1 = new byte[] { 0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45 };
            byte[] toHash = new byte[32 + 8];
            Array.Copy(ssPrefixed1, 0, toHash, 0, 7);
            Array.Copy(prefixedPub, 0, toHash, 7, 33);
            byte[] blake2b = Cryptography.Blake2bHash(toHash);

            if (data[33] != blake2b[0] || data[34] != blake2b[1]) throw new Exception("Incorrect DOT checksum");
        }
    }
    class PhraseToAddressPolkadotLedger : PhraseToAddressPolkadot {
        public PhraseToAddressPolkadotLedger(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) {
        }

        public override string[] GetDefaultPaths(string[] knownAddresses) {
            string[] p = { "m/44'/354'/{account}'/0'/{index}'" };
            return p;
        }
 
        public override CoinType GetCoinType() { return CoinType.DOTLedger; }

        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {

            //  Ledger: https://github.com/algorand/ledger-app-algorand/blob/master/src/algo_keys.c
            return Cryptography.Ledger_expand_seed_ed25519_bip32(phrase, passphrase);
        }

        protected override Object DeriveChildKey(Object parentKey, uint index) {
            var k = (Cryptography.Key)parentKey;

            //  Borrowing CardanoSharp's BIP32 Derive

            var key = new CardanoSharp.Wallet.Models.Keys.PrivateKey(k.data, k.cc);
            string path = PathNode.GetPath(index);
            var derived = key.Derive(path);
            return new Cryptography.Key(derived.Key, derived.Chaincode);
        }

        protected override byte GetPrefix(PathNode node) {
            return 0;
        }
        protected override byte[] GetPublicKey(byte[] seed) {
            //  Ed25519
            return Cryptography.Ed25519PublicKeyFromSeed(seed.Slice(0, 32));
        }
    }
}