using System;
using System.Collections.Concurrent;
using CardanoSharp.Wallet.Extensions.Models;
using Cryptography.ECDSA;

namespace FixMyCrypto {

    class PhraseToAddressPolkadot : PhraseToAddress {
        public PhraseToAddressPolkadot(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) { 
        }

        public override CoinType GetCoinType() { return CoinType.DOT; }

        public override string[] GetDefaultPaths(string[] knownAddresses) {
            string[] p = { };
            return p;
        }
 
        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {
            throw new NotSupportedException();
        }
        protected override Object DeriveChildKey(Object parentKey, uint index) {
            var k = (Key)parentKey;

            //  Borrowing CardanoSharp's BIP32 Derive

            var key = new CardanoSharp.Wallet.Models.Keys.PrivateKey(k.data, k.cc);
            string path = PathNode.GetPath(index);
            var derived = key.Derive(path);
            return new Key(derived.Key, derived.Chaincode);
        }
        protected override Address DeriveAddress(PathNode node) {
            Key key = (Key)node.Key;
            if (key == null) return null;

            byte[] pub = Chaos.NaCl.Ed25519.PublicKeyFromSeed(key.data.Slice(0, 32));

            //  https://github.com/usetech-llc/polkadot_api_dotnet/blob/2b0021f8403525358d1f4721c9a64f838a74cd90/Polkadot/src/DataStructs/Metadata/Metadata.cs#L252

            var ssPrefixed1 = new byte[] { 0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45 };
            byte[] prefixed = new byte[32 + 8];
            Array.Copy(ssPrefixed1, 0, prefixed, 0, 7);
            prefixed[7] = 0;
            Array.Copy(pub, 0, prefixed, 8, 32);

            byte[] blake2b = Blake2Fast.Blake2b.ComputeHash(prefixed);

            byte[] checksummed = new byte[35];
            checksummed[0] = 0;
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
            byte[] blake2b = Blake2Fast.Blake2b.ComputeHash(toHash);

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
 
        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {

            //  Ledger: https://github.com/algorand/ledger-app-algorand/blob/master/src/algo_keys.c
            return Ledger_expand_seed_ed25519_bip32(phrase, passphrase);
        }
    }
}