using System;
using System.Collections.Concurrent;
using Bech32;

namespace FixMyCrypto {
    class PhraseToAddressCRO : PhraseToAddressBitAltcoin {
        public PhraseToAddressCRO(PhraseProducer phrases) : base(phrases, CoinType.CRO) {
        }
        public override CoinType GetCoinType() { return CoinType.CRO; }
        public override string[] GetDefaultPaths(string[] knownAddresses) {
            string[] p = { 
                            "m/44'/394'/{account}'/0/{index}"
                         };
            return p;
        }

        private string SkToAddress(Cryptography.Key sk) {
            byte[] pub = Cryptography.Secp256K_GetPublicKey(sk.data, true);

            byte[] h1 = Cryptography.SHA256Hash(pub);
            byte[] h2 = Cryptography.RipeMD160Hash(h1);

            string addr = Bech32Engine.Encode("cro", h2);

            return addr;
        }
 
        protected override Address DeriveAddress(PathNode node, int index) {
            string address = SkToAddress(node.Keys[index]);
            return new Address(address, node.GetPath());
        }

        public override void ValidateAddress(string address) {
            Bech32Engine.Decode(address, out var hrp, out var data);
            if (data == null) throw new Exception("invalid address");
            if (hrp != "cro") throw new Exception("incorrect CRO address format");
        }
    }
}