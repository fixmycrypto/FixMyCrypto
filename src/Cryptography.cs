using System;
using System.Text;
using System.Security.Cryptography;
using Cryptography.ECDSA;

namespace FixMyCrypto {
    class Cryptography {

        public static byte[] SHA256Hash(ReadOnlySpan<byte> data) {
            return SHA256.HashData(data);
        }

        public static byte[] SHA512Hash(ReadOnlySpan<byte> data) {
            return SHA512.HashData(data);
        }

        //  SHA512/256
        public static byte[] SHA512_256Hash(byte[] data) {
            byte[] hash = new byte[32];
            var h = new Org.BouncyCastle.Crypto.Digests.Sha512tDigest(256);
            h.BlockUpdate(data, 0, 32);
            h.DoFinal(hash, 0);
            return hash;
        }

        public static byte[] Blake2bHash(ReadOnlySpan<byte> data) {
            return Blake2Fast.Blake2b.ComputeHash(data);
        }

        public static byte[] RipeMD160Hash(byte[] data) {
            return Ripemd160Manager.GetHash(data);
        }

        public static byte[] KeccakDigest(byte[] data) {
            var digest = new Org.BouncyCastle.Crypto.Digests.KeccakDigest(256);
            digest.BlockUpdate(data, 0, data.Length);
            byte[] h = new byte[digest.GetByteLength()];
            digest.DoFinal(h, 0);
            return h;
        }

        public static byte[] PassphraseToSalt(string passphrase) {
            return Encoding.UTF8.GetBytes("mnemonic" + passphrase);
        }

        public static byte[] Pbkdf2_HMAC512(byte[] password, byte[] salt, int iterations, int length) {
            using var rfc = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512);
            return rfc.GetBytes(length);
        }

        public static byte[] Pbkdf2_HMAC512(string password, byte[] salt, int iterations, int length) {
            return Pbkdf2_HMAC512(password.ToUTF8Bytes(), salt, iterations, length);
        }

        public class Key {
            public byte[] data { get; }
            public byte[] cc { get; }
            public Key(byte[] data, byte[] cc) {
                this.data = data;
                this.cc = cc;
            }
        }

        protected static byte[] ed25519_seed = Encoding.ASCII.GetBytes("ed25519 seed");

        public static byte[] HMAC512_Ed25519(byte[] data) {
            using HMACSHA512 HMAC512 = new HMACSHA512(ed25519_seed);
            return HMAC512.ComputeHash(data);
        }

        public static byte[] HMAC256_Edd25519(byte[] data) {
            using HMACSHA256 HMAC256 = new HMACSHA256(ed25519_seed);
            return HMAC256.ComputeHash(data);
        }

        public static byte[] TweakBits(byte[] data) {
            // * clear the lowest 3 bits
            // * clear the highest bit
            // * set the highest 2nd bit
            data[0]  &= 248;
            data[31] &= 31;
            data[31] |= 64;

            return data;         
        }

        public static byte[] HashRepeatedly(byte[] message) {
            var iLiR = HMAC512_Ed25519(message);
            if ((iLiR[31] & 0b0010_0000) != 0) {
                return HashRepeatedly(iLiR);
            }
            return iLiR;
        }

        public static Key Ledger_expand_seed_ed25519_bip32(Phrase phrase, string passphrase) {
            //  https://github.com/LedgerHQ/speculos/blob/c0311aef48412e40741a55f113939469da78e8e5/src/bolos/os_bip32.c#L123
            //  expand_seed_ed25519_bip32(...)
            
            string password = phrase.ToPhrase();
            byte[] salt = PassphraseToSalt(passphrase);
            var seed = Pbkdf2_HMAC512(password, salt, 2048, 64);

            byte[] message = new byte[seed.Length + 1];
            message[0] = 1;
            Array.Copy(seed, 0, message, 1, seed.Length);

            //  Chain code

            var cc = HMAC256_Edd25519(message);

            var iLiR = HashRepeatedly(seed);
            iLiR = TweakBits(iLiR);

            return new Key(iLiR, cc);            
        }

        public static byte[] Ed25519PublicKeyFromSeed(byte[] seed) {
            return Chaos.NaCl.Ed25519.PublicKeyFromSeed(seed);
        }

        public static byte[] Sr25519PublicKeyFromSeed(byte[] seed) {
            //  SR25519
            string seedHex = seed.ToHexString();
            var keypair = sr25519_dotnet.lib.SR25519.GenerateKeypairFromSeed(seedHex);
            return keypair.Public;
        }

        public static byte[] Secp256KPublicKeyDecompress(byte[] pub) {
            return Secp256K1Manager.PublicKeyDecompress(pub);
        }
    }
}