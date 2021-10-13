using System;

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
            (bool b, int hash) = PhraseProducer.VerifyChecksum(ix);
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

            int invalid = 0;

            foreach (string word in split) {
                if (!PhraseProducer.originalWordlist.Contains(word)) invalid++;
            }

            if (invalid > 5) throw new Exception("Phrase has too many non-BIP39 words");
        }
    }
}