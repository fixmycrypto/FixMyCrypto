namespace FixMyCrypto {
    public class RootKey {
        public object key { get; }

        public Phrase phrase { get; }

        public string passphrase { get; }

        public RootKey(object key, Phrase phrase, string passphrase) {
            this.key = key;
            this.phrase = phrase;
            this.passphrase = passphrase;
        }

        public override string ToString()
        {
            return $"rootkey: {((byte[])key).ToHexString()}\nphrase: {phrase.ToPhrase()}\npassphrase: {passphrase}";
        }
    }
}