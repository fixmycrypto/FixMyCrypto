using System;

namespace FixMyCrypto {
    public class Seed {
        public byte[] seed { get; }

        public Phrase phrase { get; }

        public string passphrase { get; }

        public Seed(ReadOnlySpan<byte> key, Phrase phrase, string passphrase) {
            this.seed = key.ToArray();
            this.phrase = phrase;
            this.passphrase = passphrase;
        }

        public override string ToString()
        {
            return $"seed: {seed.ToHexString()}\nphrase: {phrase.ToPhrase()}\npassphrase: {passphrase}";
        }
    }
}