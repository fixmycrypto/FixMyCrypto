using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Threading.Tasks;

namespace FixMyCrypto {
    public class BenchmarkMnemonic {

        /*
        [Benchmark]
        public void MnemonicToEntropy_100_NBitcoin() {
            string phrase = "siren bottom inform vehicle else donkey dirt task cook tide general when";
            Parallel.For(0, 100, i => {
                var m = new NBitcoin.Mnemonic(phrase);
                var ix = m.Indices;
                bool valid = m.IsValidChecksum;
            });
        }
        */

        [Benchmark]
        public void MnemonicToEntropy_100_Native() {
            Wordlists.Initialize();
            string phrase = "siren bottom inform vehicle else donkey dirt task cook tide general when";
            Parallel.For(0, 100, i => {
                var m = new Phrase(phrase);
                var ix = m.Indices;
                bool valid = m.IsValid;
            });
        }
    }

    public class BenchmarkCrypto {

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
                byte[] salt = Cryptography.PassphraseToSalt("ThePassphrase!");
                var seed = Cryptography.Pbkdf2_HMAC512(password, salt, 2048, 64);
            });
        }
    }

    public class BenchmarkPassphrase {

        [Benchmark]
        public void PassphraseGuess() {
            // string passphrase = "((C||c)orrect&&(H||h)orse&&(B||b)attery&&(S||s)taple)[1-9]?[0-9][^a-zA-Z0-9]";
            string passphrase = "(The||the)(P||p)assphrase[0-9]?[!@#$%^&*()]?";
            Passphrase p = new Passphrase(passphrase);
            int count = 0;
            Parallel.ForEach(p.Enumerate(), r => {
                byte[] salt = Cryptography.PassphraseToSalt(r);
                var seed = Cryptography.Pbkdf2_HMAC512("siren bottom inform vehicle else donkey dirt task cook tide general when", salt, 2048, 64);
                count++;
            }); 
        }

        [Benchmark]
        public void PassphraseFuzz() {
            string passphrase = "{{ThePassphrase!}}";
            // Passphrase p = new Passphrase(passphrase, fuzzDepth: 2);
            Passphrase p = new Passphrase(passphrase, fuzzDepth: 1);
            int count = 0;
            Parallel.ForEach(p.Enumerate(), r => {
                byte[] salt = Cryptography.PassphraseToSalt(r);
                var seed = Cryptography.Pbkdf2_HMAC512("siren bottom inform vehicle else donkey dirt task cook tide general when", salt, 2048, 64);
                count++;
            }); 
        }
    }

    class Benchmark {
        public static void RunBenchmarks() {
           BenchmarkRunner.Run<BenchmarkMnemonic>();
           BenchmarkRunner.Run<BenchmarkCrypto>();
           BenchmarkRunner.Run<BenchmarkPassphrase>();
        }
    }
}