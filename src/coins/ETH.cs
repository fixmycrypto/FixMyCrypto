using System;
using System.Text;
using System.Collections.Concurrent;
using Org.BouncyCastle.Crypto.Digests;
using Cryptography.ECDSA;
using NBitcoin;

namespace FixMyCrypto {
    class PhraseToAddressEth : PhraseToAddress {
        public PhraseToAddressEth(ConcurrentQueue<Work> phrases, ConcurrentQueue<Work> addresses, int threadNum, int threadMax) : base(phrases, addresses, threadNum, threadMax) {
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
            var digest = new KeccakDigest(256);
            byte[] bytes = Encoding.ASCII.GetBytes(addr.ToLower());
            digest.BlockUpdate(bytes, 0, bytes.Length);
            byte[] h = new byte[digest.GetByteLength()];
            digest.DoFinal(h, 0);

            char[] ret = new char[42];
            ret[0] = '0';
            ret[1] = 'x';
            int q = 0;
            int p = 0;

            while (p < 40) {
                byte upper = (byte)(h[q] >> 4);
                byte lower = (byte)(h[q] & 0x0f);
                q++;

                ret[p + 2] = GetChecksumDigit(upper, addr[p]);
                p++;
                ret[p + 2] = GetChecksumDigit(lower, addr[p]);
                p++;
            }

            return new string(ret);
        }
        private string PkToAddress(ExtKey pk) {
            byte[] pkeyBytes = pk.PrivateKey.PubKey.ToBytes();
            byte[] converted = Secp256K1Manager.PublicKeyDecompress(pkeyBytes);

            var digest = new KeccakDigest(256);
            digest.BlockUpdate(converted,1,64);
            byte[] hash = new byte[digest.GetByteLength()];
            digest.DoFinal(hash, 0);

            string l = hash.ToHexString(12, 20);

            string checksum = Checksum(l);

            return checksum;
        }
        public override string[] GetDefaultPaths(string[] knownAddresses) {
            string[] p = { "m/44'/60'/{account}'/0/{index}" };
            return p;
        }
 
        public override Object DeriveMasterKey(Phrase phrase, string passphrase) {
            //  TODO avoid string conversion
            string p = phrase.ToPhrase();
            Mnemonic b = new Mnemonic(p);
            return new ExtKey(b.DeriveSeed(passphrase));
        }
        protected override Object DeriveChildKey(Object parentKey, uint index) {
            ExtKey key = (ExtKey)parentKey;
            return key.Derive(index);
        }
        protected override Address DeriveAddress(PathNode node) {
            ExtKey pk = (ExtKey)node.Key;
            string address = PkToAddress(pk);
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