namespace FixMyCrypto {
    class Address
    {
        public string address, path, passphrase;
        public Phrase phrase;

        public Address(string address, string path) {
            this.address = address;
            this.path = path;
        }
        public Address(string address, Phrase phrase, string passphrase, string path) { 
            this.address = address; 
            this.path = path; 
            this.phrase = phrase;
            this.passphrase = passphrase;
        }

        public override string ToString()
        {
            return this.address;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Address b = (Address)obj;

            return (this.address == b.address && this.path == b.path && this.phrase == b.phrase && this.passphrase == b.passphrase);
        }

        public override int GetHashCode()
        {
            string s = address + path + phrase + passphrase;

            return s.GetHashCode();
        }
    }
}