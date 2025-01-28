using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using NBitcoin;
using Cryptography.ECDSA;

namespace FixMyCrypto {
    class PhraseToAddressXrp : PhraseToAddressBitAltcoin {

        static string base58orig = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        static string base58xrp =  "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz";

        public PhraseToAddressXrp(PhraseProducer phrases) : base(phrases, CoinType.XRP) {
        }
        public override CoinType GetCoinType() { return CoinType.XRP; }
        public override string[] GetDefaultPaths(string[] knownAddresses) {
            string[] p = { "m/44'/144'/{account}'/0/{index}" };
            return p;
        }
 
        protected override Address DeriveAddress(PathNode node, int index) {
            Cryptography.Key key = (Cryptography.Key)node.Keys[index];
            ExtKey sk = new ExtKey(new Key(key.data), key.cc);
 
            int version = 0;
            int version_size = version > 255 ? 2 : 1;
            byte[] tmp = new byte[20 + version_size + 4];

            if (version_size == 2) {
                tmp[0] = (byte)(version >> 8);
                tmp[1] = (byte)(version & 0xff);
            }
            else {
                tmp[0] = (byte)version;
            }

            byte[] pub = sk.PrivateKey.PubKey.ToBytes();
            if (pub.Length == 32) {
                //  Ed25519 public key
                byte[] edPub = new byte[33];
                edPub[0] = 0xed;
                Array.Copy(pub, 0, edPub, 1, 32);
                pub = edPub;
            }

            //  Double hash public key
            byte[] pub1 = Cryptography.SHA256Hash(pub);
            byte[] pub2 = Cryptography.RipeMD160Hash(pub1);

            Array.Copy(pub2, 0, tmp, version_size, 20);

            //  Checksum is first 4 bytes of double hash of hash
            byte[] cs_buf = Cryptography.SHA256Hash(tmp.Slice(0, 20 + version_size));
            cs_buf = Cryptography.SHA256Hash(cs_buf);
            Array.Copy(cs_buf, 0, tmp, 20 + version_size, 4);

            string address = Base58.Encode(tmp);

            //  Use modified Base58
            string xrp = "";
            for (int i = 0; i < address.Length; i++) {
                int ix = base58orig.IndexOf(address[i]);
                xrp += base58xrp[ix];
            }

            return new Address(xrp, node.GetPath());
        }

        public override void ValidateAddress(string address) {
            if (!address.StartsWith("r")) throw new Exception("XRP address should start with r");

            //  Reverse modified Base58
            string xrp = "";
            for (int i = 0; i < address.Length; i++) {
                int ix = base58xrp.IndexOf(address[i]);
                xrp += base58orig[ix];
            }

            //  Try decoding Base58
            byte[] pub = Base58.Decode(xrp);
        }
    }
}