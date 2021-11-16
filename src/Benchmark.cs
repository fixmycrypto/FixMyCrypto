using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Threading.Tasks;
using System.Text;

namespace FixMyCrypto {
    public class Benchmark {

        [Benchmark]
        public void Sha256_1m() {
            byte[] data = new byte[32];
            Parallel.For(0, 1000000, i => {
                data = Cryptography.SHA256Hash(data);
            });
        }

        [Benchmark]
        public void Sha512_1m() {
            byte[] data = new byte[32];
            Parallel.For(0, 1000000, i => {
                data = Cryptography.SHA256Hash(data);
            });
        }

        [Benchmark]
        public void Pbkdf2_Sha512_1k() {
            Parallel.For(0, 1000, i => {
                string password = "siren bottom inform vehicle else donkey dirt task cook tide general when";
                byte[] salt = Encoding.UTF8.GetBytes("mnemonic" + "ThePassphrase");
                var seed = Cryptography.Pbkdf2_HMAC512(password, salt, 2048, 64);
            });
        }

        [Benchmark]
        public void PassphraseGuess() {
            string passphrase = "((C||c)orrect&&(H||h)orse&&(B||b)attery&&(S||s)taple)[1-9]?[0-9][^a-zA-Z0-9]";
            Passphrase p = new Passphrase(passphrase);
            int count = 0;
            foreach (string r in p.Enumerate()) count++;
        }

        [Benchmark]
        public void PassphraseFuzz() {
            string passphrase = "{{ThePassphrase!}}";
            Passphrase p = new Passphrase(passphrase, fuzzDepth: 2);
            int count = 0;
            foreach (string r in p.Enumerate()) count++;
        }

        public static BenchmarkDotNet.Reports.Summary RunBenchmarks() {
            return BenchmarkRunner.Run<Benchmark>();
        }
    }
}