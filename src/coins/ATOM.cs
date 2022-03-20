using System;
using System.Collections.Concurrent;
using NBitcoin;
using Bech32;

namespace FixMyCrypto {
    class PhraseToAddressAtom : PhraseToAddressBitAltcoin {
        public PhraseToAddressAtom(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses) : base(phrases, addresses, CoinType.ATOM) {
        }
        public override CoinType GetCoinType() { return CoinType.ATOM; }
        public override string[] GetDefaultPaths(string[] knownAddresses) {
            string[] p = { 
                            "m/44'/118'/{account}'/0/{index}"
                         };
            return p;
        }

        private string SkToAddress(ExtKey sk) {
            byte[] pub = sk.GetPublicKey().ToBytes();

            byte[] h1 = Cryptography.SHA256Hash(pub);
            byte[] h2 = Cryptography.RipeMD160Hash(h1);

            string addr = Bech32Engine.Encode("cosmos", h2);

            return addr;
        }
 
        protected override Address DeriveAddress(PathNode node, int index) {
            Cryptography.Key key = node.Keys[index];
            ExtKey sk = new ExtKey(new Key(key.data), key.cc);

            string address = SkToAddress(sk);
            return new Address(address, node.GetPath());
        }

        public override void ValidateAddress(string address) {
            Bech32Engine.Decode(address, out var hrp, out var data);
            if (data == null) throw new Exception("invalid address");
        }
    }
}