using System;
using System.Security.Cryptography;

namespace FixMyCrypto {
    public class Phrase {
        private short[] ix;
        private int hash;

        public int Length { get { return ix.Length; } }
        public short[] Indices { get { return ix; } }
        public Phrase(short[] indices, int hash) { ix = indices; this.hash = hash; }
        public Phrase(string[] phrase) {
            ix = new short[phrase.Length];
            for (int i = 0; i < phrase.Length; i++) ix[i] = PhraseProducer.GetWordIndex(phrase[i]);
            (bool b, int hash) = VerifyChecksum(ix);
            this.hash = hash;
        }
        public Phrase(string phrase) : this(phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries)) { }

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
            for (int i = 0; i < ix.Length; i++) words[i] = PhraseProducer.GetWord(ix[i]);
            return words;
        }

        public string ToPhrase() {
            return String.Join(' ', this.ToWords());
        }

        public static string ToPhrase(short[] ix) {
            string[] words = new string[ix.Length];
            for (int i = 0; i < ix.Length; i++) words[i] = PhraseProducer.GetWord(ix[i]);
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

                break;

                default:

                throw new Exception("Phrase must be 12/15/18/24 words separated by spaces");
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
                if (!PhraseProducer.originalWordlist.Contains(word)) invalid++;
            }

            if (invalid > 4) throw new Exception("Phrase has too many missing or non-BIP39 words");
        }

        public static (bool, int) VerifyChecksum(short[] indices) {
            // Compute and check checksum
            int MS = indices.Length;
            int ENTCS = MS * 11;
            int CS = ENTCS % 32;
            int ENT = ENTCS - CS;

            var entropy = new byte[ENT / 8];

            int itemIndex = 0;
            int bitIndex = 0;
            // Number of bits in a word
            int toTake = 8;
            // Indexes are held in a UInt32 but they are only 11 bits
            int maxBits = 11;
            for (int i = 0; i < entropy.Length; i++)
            {
                if (bitIndex + toTake <= maxBits)
                {
                    // All 8 bits are in one item

                    // To take 8 bits (*) out of 00000000 00000000 00000xx* *******x:
                    // 1. Shift right to get rid of extra bits on right, then cast to byte to get rid of the rest
                    // >> maxBits - toTake - bitIndex
                    entropy[i] = (byte)(indices[itemIndex] >> (3 - bitIndex));
                }
                else
                {
                    // Only a part of 8 bits are in this item, the rest is in the next.
                    // Since items are only 32 bits there is no other possibility (8<32)

                    // To take 8 bits(*) out of [00000000 00000000 00000xxx xxxx****] [00000000 00000000 00000*** *xxxxxxx]:
                    // Take first item at itemIndex [00000000 00000000 00000xxx xxxx****]: 
                    //    * At most 7 bits and at least 1 bit should be taken
                    // 1. Shift left [00000000 00000000 0xxxxxxx ****0000] (<< 8 - (maxBits - bitIndex)) 8-max+bi
                    // 2. Zero the rest of the bits (& (00000000 00000000 00000000 11111111))

                    // Take next item at itemIndex+1 [00000000 00000000 00000*** *xxxxxxx]
                    // 3. Shift right [00000000 00000000 00000000 0000****]
                    // number of bits already taken = maxBits - bitIndex
                    // nuber of bits to take = toTake - (maxBits - bitIndex)
                    // Number of bits on the right to get rid of= maxBits - (toTake - (maxBits - bitIndex))
                    // 4. Add two values to each other using bitwise OR [****0000] | [0000****]
                    entropy[i] = (byte)(((indices[itemIndex] << (bitIndex - 3)) & 0xff) |
                                         (indices[itemIndex + 1] >> (14 - bitIndex)));
                }

                bitIndex += toTake;
                if (bitIndex >= maxBits)
                {
                    bitIndex -= maxBits;
                    itemIndex++;
                }
            }

            // Compute and compare checksum:
            // CS is at most 8 bits and it is the remaining bits from the loop above and it is only from last item
            // [00000000 00000000 00000xxx xxxx****]
            // We already know the number of bits here: CS
            // A simple & does the work
            uint mask = (1U << CS) - 1;
            byte expectedChecksum = (byte)(indices[itemIndex] & mask);

            // Checksum is the "first" CS bits of hash: [****xxxx]

            using SHA256 hash = SHA256.Create();
            byte[] hashOfEntropy = hash.ComputeHash(entropy);
            byte actualChecksum = (byte)(hashOfEntropy[0] >> (8 - CS));

            return ((expectedChecksum == actualChecksum), BitConverter.ToInt32(hashOfEntropy));
        }

    }
}