using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixMyCrypto {
    public class BenchmarkMnemonic {

        /*
        //  Slow!
        [Benchmark]
        public void MnemonicCreate_NBitcoin_100() {
            Parallel.For(0, 100, i => {
                NBitcoin.Mnemonic m = new NBitcoin.Mnemonic(NBitcoin.Wordlist.English, NBitcoin.WordCount.Twelve);
                string phrase = String.Join(' ', m.Words);
            });
        }
        */

        [Benchmark]
        public void MnemonicCreate_Native_100() {
            Wordlists.Initialize();
            Parallel.For(0, 100, i => {
                Phrase p = new Phrase(12);
                string phrase = p.ToPhrase();
            });
        }

        [Benchmark]
        public void MnemonicToAddress_Bitcoin_100() {
            Wordlists.Initialize();
            string[] paths = { "m/84'/0'/0'/0/0" };
            int[] accounts = { 0 };
            int[] indices = { 0 };
            Parallel.For(0, 100, i => {
                PhraseToAddress p2a = PhraseToAddress.Create(CoinType.BTC, null, null, 0, 0);
                Phrase phrase = new Phrase(12);
                List<Address> addresses = p2a.GetAddresses(phrase, "", paths, accounts, indices);
            });
        }

        [Benchmark]
        public void MnemonicToAddress_Eth_100() {
            Wordlists.Initialize();
            string[] paths = { "m/44'/60'/0'/0/0" };
            int[] accounts = { 0 };
            int[] indices = { 0 };
            Parallel.For(0, 100, i => {
                PhraseToAddress p2a = PhraseToAddress.Create(CoinType.ETH, null, null, 0, 0);
                Phrase phrase = new Phrase(12);
                List<Address> addresses = p2a.GetAddresses(phrase, "", paths, accounts, indices);
            });
        }

        [Benchmark]
        public void MnemonicToAddress_Cardano_100() {
            Wordlists.Initialize();
            string[] paths = { "m/1852'/1815'/0'/0/0" };
            int[] accounts = { 0 };
            int[] indices = { 0 };
            Parallel.For(0, 100, i => {
                PhraseToAddress p2a = PhraseToAddress.Create(CoinType.ADA, null, null, 0, 0);
                Phrase phrase = new Phrase(12);
                List<Address> addresses = p2a.GetAddresses(phrase, "", paths, accounts, indices);
            });
        }

        [Benchmark]
        public void MnemonicToAddress_Solana_100() {
            Wordlists.Initialize();
            string[] paths = { "m/44'/501'" };
            int[] accounts = { 0 };
            int[] indices = { 0 };
            Parallel.For(0, 100, i => {
                PhraseToAddress p2a = PhraseToAddress.Create(CoinType.SOL, null, null, 0, 0);
                Phrase phrase = new Phrase(12);
                List<Address> addresses = p2a.GetAddresses(phrase, "", paths, accounts, indices);
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