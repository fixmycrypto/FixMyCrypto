using System;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NBitcoin;

namespace FixMyCrypto {
    class PhraseToAddressEth : PhraseToAddress {
        public PhraseToAddressEth(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses) : base(phrases, addresses) {
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
        private string SkToAddress(ExtKey sk) {
            //  TODO: Get decompressed pubkey directly?
            byte[] pkeyBytes = sk.PrivateKey.PubKey.ToBytes();
            byte[] converted = Cryptography.Secp256KPublicKeyDecompress(pkeyBytes);

            byte[] hash = Cryptography.KeccakDigest(converted.Slice(1, 64));

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
 
        public override Object DeriveRootKey(Phrase phrase, string passphrase) {
            if (IsUsingOpenCL()) {
                return DeriveRootKey_BatchPhrases(new Phrase[] { phrase }, passphrase)[0];
            }

            string p = phrase.ToPhrase();
            byte[] salt = Cryptography.PassphraseToSalt(passphrase);
            byte[] seed = Cryptography.Pbkdf2_HMAC512(p, salt, 2048, 64);
            return ExtKey.CreateFromSeed(seed);
        }
        public override bool IsUsingOpenCL() {
            return (ocl != null);
        }
        public override Object[] DeriveRootKey_BatchPhrases(Phrase[] phrases, string passphrase) {
            if (ocl == null) {
                return base.DeriveRootKey_BatchPhrases(phrases, passphrase);
            }
            else {
                byte[][] passwords = new byte[phrases.Length][];
                for (int i = 0; i < phrases.Length; i++) passwords[i] = phrases[i].ToPhrase().ToUTF8Bytes();
                byte[] salt = Cryptography.PassphraseToSalt(passphrase);
                Seed[] seeds = ocl.Pbkdf2_Sha512_MultiPassword(phrases, new string[] { passphrase }, passwords, salt);
                Object[] keys = new object[phrases.Length];
                Parallel.For(0, phrases.Length, i => {
                    if (Global.Done) return;
                    keys[i] = ExtKey.CreateFromSeed(seeds[i].seed);
                });
                return keys;
            }
        }
        public override Object[] DeriveRootKey_BatchPassphrases(Phrase phrase, string[] passphrases) {
            if (ocl == null) {
                return base.DeriveRootKey_BatchPassphrases(phrase, passphrases);
            }
            else {
                byte[] password = phrase.ToPhrase().ToUTF8Bytes();
                byte[][] salts = new byte[passphrases.Length][];
                for (int i = 0; i < passphrases.Length; i++) salts[i] = Cryptography.PassphraseToSalt(passphrases[i]);
                Seed[] seeds = ocl.Pbkdf2_Sha512_MultiSalt(new Phrase[] { phrase }, passphrases, password, salts);
                Object[] keys = new object[passphrases.Length];
                Parallel.For(0, passphrases.Length, i => {
                    if (Global.Done) return;
                    keys[i] = ExtKey.CreateFromSeed(seeds[i].seed);
                });
                return keys;
            }
        }
        protected override Object DeriveChildKey(Object parentKey, uint index) {
            ExtKey key = (ExtKey)parentKey;
            return key.Derive(index);
        }
        protected override Address DeriveAddress(PathNode node) {
            ExtKey sk = (ExtKey)node.Key;
            string address = SkToAddress(sk);
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