using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CardanoSharp.Wallet.Extensions.Models;

namespace FixMyCrypto {
    class PhraseToAddressAlgorand : PhraseToAddress {

        public PhraseToAddressAlgorand(BlockingCollection<Work> phrases, BlockingCollection<Work> addresses) : base(phrases, addresses) {
        }

        public override CoinType GetCoinType() { return CoinType.ALGO; }

        public override string[] GetDefaultPaths(string[] knownAddresses) {
            string[] p =    { 
                                "m",                                //  My Algo
                                "m/44'/283'/{account}'/0/{index}"   //  Ledger
                            };
            return p;
        }
 
        //  https://github.com/algorand/js-algorand-sdk/blob/a80a1e6d683aef12bf72431c6842530b1bb26235/src/mnemonic/mnemonic.ts

        protected Cryptography.Key Restore25(Phrase phrase) {
            if (phrase.Length != 25) throw new NotSupportedException();

            short[] indices = phrase.Indices;

            byte[] entropy = indices.Slice(0, 24).ElevenToEightReverse();

            if (entropy.Length != 33 || entropy[entropy.Length - 1] != 0) throw new Exception("invalid ALGO entropy");

            //  25th word is the entire CS
            short expectedChecksum = indices[24];

            //  Use SHA512/256 instead of SHA256
            byte[] hash = Cryptography.SHA512_256Hash(entropy.Slice(0, entropy.Length - 1));

            //  use first 11 bits of hash, not 8
            short actualChecksum = (short)((hash[1] & 7) << 8 | hash[0]);

            if (expectedChecksum != actualChecksum) {
                throw new FormatException("Wrong checksum.");
            }

            return new Cryptography.Key(entropy.Slice(0, 32), null);
        }
        public override Object DeriveRootKey(Phrase phrase, string passphrase) {
            if (phrase.Length == 25) return this.Restore25(phrase);

            //  Ledger: https://github.com/algorand/ledger-app-algorand/blob/master/src/algo_keys.c
            return Cryptography.Ledger_expand_seed_ed25519_bip32(phrase, passphrase);

        }
        protected override Object DeriveChildKey(Object parentKey, uint index) {
            var k = (Cryptography.Key)parentKey;
            if (k == null || k.data.Length != 64) return null;  //  for non-Ledger

            //  Borrowing CardanoSharp's BIP32 Derive

            var key = new CardanoSharp.Wallet.Models.Keys.PrivateKey(k.data, k.cc);
            string path = PathNode.GetPath(index);
            var derived = key.Derive(path);
            return new Cryptography.Key(derived.Key, derived.Chaincode);
        }
        protected override Address DeriveAddress(PathNode node) {
            var key = (Cryptography.Key)node.Key;
            if (key == null) return null;

            byte[] pub = Cryptography.Ed25519PublicKeyFromSeed(key.data.Slice(0, 32));

            byte[] hash = Cryptography.SHA512_256Hash(pub);

            byte[] addr = new byte[36];
            Array.Copy(pub, addr, 32);
            Array.Copy(hash, 32 - 4, addr, 32, 4);

            string address = BytesToBase32(addr);
            return new Address(address, node.GetPath());
        }

        public override void ValidateAddress(string address) {
            if (address.Length != 58) throw new Exception("ALGO address length should be 58 chars");

            byte[] data = Base32ToBytes(address);

            byte[] pub = data.Slice(0, 32);
            byte[] cs = data.Slice(32, 4);

            byte[] hash = Cryptography.SHA512_256Hash(pub);

            for (int i = 0; i < 4; i++) if (cs[i] != hash[(32 - 4) + i]) throw new Exception("ALGO checksum invalid");
        }

        //  https://stackoverflow.com/a/42231034
        private static string BytesToBase32(byte[] bytes) {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            string output = "";
            for (int bitIndex = 0; bitIndex < bytes.Length * 8; bitIndex += 5) {
                int dualbyte = bytes[bitIndex / 8] << 8;
                if (bitIndex / 8 + 1 < bytes.Length)
                    dualbyte |= bytes[bitIndex / 8 + 1];
                dualbyte = 0x1f & (dualbyte >> (16 - bitIndex % 8 - 5));
                output += alphabet[dualbyte];
            }

            return output;
        }

        private static byte[] Base32ToBytes(string base32) {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            List<byte> output = new List<byte>();
            char[] bytes = base32.ToCharArray();
            for (int bitIndex = 0; bitIndex < base32.Length * 5; bitIndex += 8) {
                int dualbyte = alphabet.IndexOf(bytes[bitIndex / 5]) << 10;
                if (bitIndex / 5 + 1 < bytes.Length)
                    dualbyte |= alphabet.IndexOf(bytes[bitIndex / 5 + 1]) << 5;
                if (bitIndex / 5 + 2 < bytes.Length)
                    dualbyte |= alphabet.IndexOf(bytes[bitIndex / 5 + 2]);

                dualbyte = 0xff & (dualbyte >> (15 - bitIndex % 5 - 8));
                output.Add((byte)(dualbyte));
            }
            return output.ToArray();
        }
    }
}