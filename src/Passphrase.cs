using System;
using System.Collections;

namespace FixMyCrypto {
    public abstract class Passphrase {
        public abstract IEnumerable Next();

        public abstract override string ToString();

        public static Passphrase Create(dynamic source) {
            if (source.Value != null && source.Value is string) {
                return new SimplePassphrase((string)source.Value);
            }

            throw new NotImplementedException();
        }
    }

    public class SimplePassphrase: Passphrase {
        private string passphrase;

        public SimplePassphrase(string passphrase) {
            this.passphrase = passphrase;
        }

        public override IEnumerable Next() {
            yield return this.passphrase;
        }

        public override string ToString()
        {
            return this.passphrase;
        }
    }
}