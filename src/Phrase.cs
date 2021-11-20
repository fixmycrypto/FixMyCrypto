using System;
using System.Security.Cryptography;

namespace FixMyCrypto {
    public class Phrase {
        private short[] ix;
        private int hash;
        private bool valid;

        public int Length { get { return ix.Length; } }
        public short[] Indices { get { return ix; } }
        public bool IsValid { get { return valid; } }
        public Phrase(short[] indices, int hash) { ix = indices; this.hash = hash; valid = true; }
        public Phrase(string[] phrase) {
            ix = new short[phrase.Length];
            for (int i = 0; i < phrase.Length; i++) ix[i] = Wordlists.GetWordIndex(phrase[i]);
            (valid, hash) = VerifyChecksum(ix);
        }
        public Phrase(string phrase) : this(phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries)) { }

        //  Create a randomly initialized phrase
        public Phrase(int length = 24) {
            int entropyLength = (int)(length * 11 / 8.0 + 0.5);

            byte[] entropy = new byte[entropyLength];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(entropy, 0, entropy.Length - 1);

            byte[] hash = Cryptography.SHA256Hash(entropy.Slice(0, entropy.Length - 1));

            entropy[entropy.Length - 1] = hash[0];

            this.ix = EightToEleven(entropy);
            this.hash = BitConverter.ToInt32(hash);
            this.valid = true;
        }

        public static short[] EightToEleven(byte[] src) {
            /*
            string bits = "";
            for (int i = 0; i < src.Length; i++) bits += Convert.ToString(src[i], 2).PadLeft(8, '0') + ' ';
            Log.Debug($"entropy:\n{bits}");
            */

            short[] b = new short[(src.Length * 8) / 11];
            int ix = 0;
            int acc = 0;
            int accBits = 0;

            for (int i = 0; i < src.Length; i++) {
                acc <<= 8;
                acc |= src[i];
                accBits += 8;
                while (accBits >= 11) {
                    int val = (acc >> (accBits - 11));
                    b[ix++] = (short)(val & 0xffff);
                    acc &= (1 << (accBits - 11)) - 1;
                    accBits -= 11;
                }
            }

            /*
            bits = "";
            for (int i = 0; i < b.Length; i++) bits += Convert.ToString(b[i], 2).PadLeft(11, '0') + ' ';
            Log.Debug($"output:\n{bits}");
            */

            return b;
        }

        public override bool Equals(object obj) {
            Phrase p = (Phrase)obj;
            if (p == null) return false;

            if (p.Length != ix.Length) return false;

            for (int i = 0; i < p.Length; i++) if (p.ix[i] != ix[i]) return false;

            return true;
        }

        public override int GetHashCode() {
            return hash;
        }

        public Phrase Clone() {
            short[] i = ix.Copy();
            return new Phrase(i, hash);
        }

        public string[] ToWords() {
            string[] words = new string[ix.Length];
            for (int i = 0; i < ix.Length; i++) words[i] = Wordlists.GetWord(ix[i]);
            return words;
        }

        public string ToPhrase() {
            return String.Join(' ', this.ToWords());
        }

        public static string ToPhrase(short[] ix) {
            string[] words = new string[ix.Length];
            for (int i = 0; i < ix.Length; i++) words[i] = Wordlists.GetWord(ix[i]);
            return String.Join(' ', words);
        }

        public short this[int i] {
            get { return ix[i]; }
            set { ix[i] = value; }
        }

        public static void Validate(string phrase) {
            string[] split = phrase.Split(' ');

            switch (split.Length) {
                case 12:
                case 15:
                case 18:
                case 24:
                case 25:    //  ALGO

                break;

                default:

                throw new Exception("Phrase must be 12/15/18/24/25 words separated by spaces");
            }

            if (phrase.Contains("?")) {
                int i = split.Length - 1;
                for (; i >= 0; i--) {
                    if (split[i] != "?") break;
                }

                for (; i >= 0; i--) {
                    if (split[i] == "?") throw new Exception("All missing words (?) must be at the end of the phrase");
                }
            }

            int invalid = 0;

            foreach (string word in split) {
                if (!Wordlists.OriginalWordlist.Contains(word)) invalid++;
            }

            if (invalid > 4) throw new Exception("Phrase has too many missing or non-BIP39 words");
        }

        public static (bool, int) VerifyChecksum(short[] indices) {
            if (indices.Length == 25) {
                //  ALGO

                byte[] entropy = indices.Slice(0, 24).ElevenToEightReverse();

                if (entropy.Length != 33 || entropy[entropy.Length - 1] != 0) return ((false, 0));

                //  25th word is the entire CS
                short expectedChecksum = indices[24];

                //  Use SHA512/256 instead of SHA256
                byte[] hash = Cryptography.SHA512_256Hash(entropy.Slice(0, entropy.Length - 1));

                //  use first 11 bits of hash, not 8
                short actualChecksum = (short)((hash[1] & 7) << 8 | hash[0]);

                return ((expectedChecksum == actualChecksum), BitConverter.ToInt32(hash));
            }
            else {
                byte[] entropy = indices.ElevenToEight();
                int CS = (indices.Length * 11) % 32;

                uint mask = (1U << CS) - 1;
                byte expectedChecksum = (byte)(indices[indices.Length - 1] & mask);

                // Checksum is the "first" CS bits of hash: [****xxxx]

                byte[] hash = Cryptography.SHA256Hash(entropy);
                byte actualChecksum = (byte)(hash[0] >> (8 - CS));

                return ((expectedChecksum == actualChecksum), BitConverter.ToInt32(hash));
            }
        }

    }
}