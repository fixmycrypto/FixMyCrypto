using System;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace FixMyCrypto {
    class PhraseToAddressEth : PhraseToAddressBitAltcoin {
        public PhraseToAddressEth(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses) : base(phrases, addresses, CoinType.ETH) {
        }
        public override CoinType GetCoinType() { return CoinType.ETH; }
        private char GetChecksumDigit(byte h, char c) {
            if (h >= 8) {
                return Char.ToUpper(c);
            }
            else {
                return Char.ToLower(c);
            }
        }
        public string Checksum(string addr) {
            byte[] bytes = Encoding.ASCII.GetBytes(addr.ToLower());
            byte[] h = Cryptography.KeccakDigest(bytes);

            char[] ret = new char[42];
            ret[0] = '0';
            ret[1] = 'x';
            int q = 0;
            int p = 0;

            while (p < 40) {
                byte hi = (byte)(h[q] >> 4);
                byte lo = (byte)(h[q] & 0x0f);
                q++;

                ret[p + 2] = GetChecksumDigit(hi, addr[p]);
                p++;
                ret[p + 2] = GetChecksumDigit(lo, addr[p]);
                p++;
            }

            return new string(ret);
        }
        private string SkToAddress(Cryptography.Key sk) {
            byte[] pk = Cryptography.Secp256K_GetPublicKey(sk.data, false);

            byte[] hash = Cryptography.KeccakDigest(pk.Slice(1, 64));

            string l = hash.ToHexString(12, 20);

            string checksum = Checksum(l);

            return checksum;
        }
        public override string[] GetDefaultPaths(string[] knownAddresses) {
            string[] p = { "m/44'/60'/{account}'/0/{index}",    //  BIP44
                           "m/44'/60'/{account}'/{index}",      //  Coinomi, Old Ledger
                         };
            return p;
        }
 
        protected override Address DeriveAddress(PathNode node, int index) {
            string address = SkToAddress(node.Keys[index]);
            return new Address(address, node.GetPath());
        }

        public override void ValidateAddress(string address) {
            if (address.Length != 42) throw new Exception("ETH address length should be 42 chars");

            if (!address.StartsWith("0x")) throw new Exception("ETH address should start with 0x");

            string stripped = address.Substring(2);

            string checksum = Checksum(stripped);

            if (checksum != address) Log.Warning($"ETH address checksum is incorrect, should be: {checksum}");
        }
    }
}