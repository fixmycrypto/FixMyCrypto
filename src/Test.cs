using NBitcoin;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace FixMyCrypto {
    class Test {

        public static void TestKnownPhrase(string phrase, string passphrase, CoinType coin, string path, int expectTx, double expectCoins) {
            Log.Info($"TestKnownPhrase {coin} phrase: {phrase}");
            try {
                PhraseToAddress p2a = PhraseToAddress.Create(coin, null, null, 0, 0);
                List<Address> addresses = p2a.GetAddress(phrase, passphrase, 0, 0, path);
                LookupAddress la = LookupAddress.Create(coin, null, 0, 0);
                foreach (Address address in addresses) {
                    LookupAddress.LookupResult result = la.GetContents(address.address);
                    Log.Info($"{coin} known address {address}: {result}");
                    if (result.coins < expectCoins || result.txCount < expectTx) {
                        Log.Error($"results did not match expected: txCount: {expectTx} coins: {expectCoins}");
                        Environment.Exit(1);
                    }

                    if (result.txCount > 0) {
                        List<string> txs = Task<List<string>>.Run(async () => await la.GetTransactions(address.address)).Result;
                        foreach (string tx in txs) {
                            Log.Debug($"tx: {tx}");
                        }
                    }

                    Log.Debug();
                }
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                Environment.Exit(1);
            }
        }

        public static List<Address> TestDerivation(string phrase, string passphrase, CoinType coin, string path) {
            PhraseToAddress p2a = PhraseToAddress.Create(coin, null, null, 0, 0);

            int account, index;

            if (!String.IsNullOrEmpty(path)) TestTokenization(path);

            Path.GetAccountIndex(path, out account, out index);

            List<Address> addresses = p2a.GetAddress(phrase, passphrase, account, index, path);

            return addresses;
        }

        public static void TestTokenization(string path) {
            if (path.Contains("{account}") || path.Contains("{index}")) return;

            int account, index;

            Path.GetAccountIndex(path, out account, out index);

            string p = Path.Tokenize(path);

            string o = Path.Resolve(p, account, index);

            if (o != path) throw new Exception($"{o} != {path}");
        }
        public static void TestCardanoKeyVector(string name, string phrase, string passphrase, CoinType coin, string expectSK, string expectCC) {
            PhraseToAddress p = PhraseToAddress.Create(coin, null, null, 0, 0);
            Phrase ph = new Phrase(phrase);
            CardanoSharp.Wallet.Models.Keys.PrivateKey sk = (CardanoSharp.Wallet.Models.Keys.PrivateKey)p.DeriveMasterKey(ph, passphrase);
            string gotSK = sk.Key.ToHexString();
            string gotCC = sk.Chaincode.ToHexString();
            if (gotSK != expectSK) {
                Log.Debug($"{coin} test vector '{name}' failed SK:\nexpect: {expectSK}\ngot   : {gotSK}");
                throw new Exception("test failed");
            }
            if (gotCC != expectCC) {
                Log.Debug($"{coin} test vector '{name}' failed CC:\nexpect: {expectCC}\ngot   : {gotCC}");
                throw new Exception("test failed");
            }
        }
        public static void Run(int count, string opts) {

            //  Needed to init word lists
            PhraseProducer pp = new PhraseProducer(null, 0, 0, null);

            //  Test Phrase hashing
            HashSet<Phrase> testedPhrases = new HashSet<Phrase>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1e5; i++) {
                Mnemonic m = new Mnemonic(Wordlist.English, WordCount.Twelve);
                Phrase ph = new Phrase(m.Words);
                if (!testedPhrases.Add(ph)) throw new Exception("item already in set");
                // Log.Debug($"phrase: {p} hash: {ph.GetHashCode():x2}");
            }
            foreach (Phrase p2 in testedPhrases) {
                if (!testedPhrases.Contains(p2)) {
                    throw new Exception($"not found: {p2}");
                }
            }
            sw.Stop();
            Log.Debug($"phrase hash time: {sw.ElapsedMilliseconds}ms");

            //  Test keyboard distance
            /*
            for (char a = ' '; a <= '~'; a++) {
                for (char b = ' '; b <= '~'; b++) {
                    double distance = KeyboardDistance.GetKeyboardDistance(a, b);
                    Log.Debug($"{a} -> {b} = {distance:F2}");
                }
            }
            */

            //  Test vectors
            //  TODO: Test addresses
            //  TODO: Add Ledger/Trezor 12/15/18-word test vectors
            
            /*
            this probably isn't right - they used a weird path
            {
                //  Trezor from: https://github.com/trezor/trezor-firmware/pull/1388/files
                string name = "Trezor 1388";
                string trezorPhrase = "balance exotic ranch knife glory slow tape favorite yard gym awake ill exist useless parent aim pig stay effort into square gasp credit butter";
                string expectSK = "38eb2a79486e516cb6658700503a3e2c870c03e9d1aec731f780aa6fb7f7de4480d2c677638e5dbd4395cdec279bf2a42077f2797c9e887949d37cdb317fce6a";
                string expectCC = "9b226add79f90086ea18b260da633089fe121db758aa31284ad1affaf3c9bb68";
                TestCardanoKeyVector(name, trezorPhrase, "", CoinType.ADATrezor, expectSK, expectCC);
            }
            */
            {
                //  Ledger from: https://github.com/cardano-foundation/CIPs/blob/master/CIP-0003/Ledger.md
                string name = "No passphrase no iterations";
                string ledgerPhrase = "recall grace sport punch exhibit mad harbor stand obey short width stem awkward used stairs wool ugly trap season stove worth toward congress jaguar";
                string expectSK = "a08cf85b564ecf3b947d8d4321fb96d70ee7bb760877e371899b14e2ccf88658104b884682b57efd97decbb318a45c05a527b9cc5c2f64f7352935a049ceea60";
                string expectCC = "680d52308194ccef2a18e6812b452a5815fbd7f5babc083856919aaf668fe7e4";
                TestCardanoKeyVector(name, ledgerPhrase, "", CoinType.ADALedger, expectSK, expectCC);
            }
            /*
            //  This one doesn't work
            {
                //  Ledger from: https://github.com/cardano-foundation/CIPs/blob/master/CIP-0003/Ledger.md
                string name = "No passphrase with iterations";
                string ledgerPhrase = "correct cherry mammal bubble want mandate polar hazard crater better craft exotic choice fun tourist census gap lottery neglect address glow carry old business";
                string expectSK = "1091f9fd9d2febbb74f08798490d5a5727eacb9aa0316c9eeecf1ff2cb5d8e55bc21db1a20a1d2df9260b49090c35476d25ecefa391baf3231e56699974bdd46";
                string expectCC = "652f8e7dd4f2a66032ed48bfdffa4327d371432917ad13909af5c47d0d356beb";
                TestCardanoKeyVector(name, ledgerPhrase, "", CoinType.ADALedger, expectSK, expectCC);
            }
            */
            {
                //  Ledger from: https://github.com/cardano-foundation/CIPs/blob/master/CIP-0003/Ledger.md
                string name = "With passphrase";
                string passphrase = "foo";
                string ledgerPhrase = "abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon abandon art";
                string expectSK = "f053a1e752de5c26197b60f032a4809f08bb3e5d90484fe42024be31efcba7578d914d3ff992e21652fee6a4d99f6091006938fac2c0c0f9d2de0ba64b754e92";
                string expectCC = "a4f3723f23472077aa4cd4dd8a8a175dba07ea1852dad1cf268c61a2679c3890";
                TestCardanoKeyVector(name, ledgerPhrase, passphrase, CoinType.ADALedger, expectSK, expectCC);
            }

            dynamic secrets = null;

            try {
                string str = File.ReadAllText("secrets.json");
                secrets = JsonConvert.DeserializeObject(str);
            }
            catch (Exception e) {
                Log.Error($"Error reading secrets.json: ${e}");
                Environment.Exit(1);
            }

            //  Test address derivation (no blockchain)
            foreach (dynamic coin in secrets) {
                CoinType ct;

                try {
                    string name = (string)coin.Name;
                    if (name.Contains(",")) name = name.Substring(0, name.IndexOf(","));
                    ct = Settings.GetCoinType(name);
                }
                catch (Exception) {
                    continue;
                }

                dynamic secret = coin.Value;

                //  Validate phrase
                Phrase.Validate(secret.phrase.Value);

                //  Test address derivation
                if (secret.knownAddresses != null) {
                    foreach (dynamic known in secret.knownAddresses) {
                        string address = known.address.Value;

                        //  Validate address
                        PhraseToAddress.ValidateAddress(ct, address);

                        string path = null;
                        if (known.path != null) path = known.path.Value;
                        List<Address> addresses = TestDerivation(secret.phrase.Value, secret.passphrase.Value, ct, path);

                        bool found = false;
                        foreach (Address a in addresses) {
                            if (a.address == address && (path == null || a.path == path)) {
                                Log.Debug($"Derived {ct} address: {address} {path}");
                                found = true;
                            }
                        }

                        if (!found) {
                            Log.Error($"{ct} failed to derive: {address} {path}. Found:");
                            foreach (Address a in addresses) {
                                Log.Error($"{a.address} {a.path}");
                            }
                            Environment.Exit(1);
                        }
                    }
                }

                Log.Debug();
            }

            string[] coins = opts.Split(",");
            int numCoins = coins.Length;

            //  Test known phrases - requires blockchain API
            foreach (dynamic coin in secrets) {
                
                CoinType ct;
                try {
                    string name = (string)coin.Name;
                    if (name.Contains(",")) name = name.Substring(0, name.IndexOf(","));
                    ct = Settings.GetCoinType(name);
                    if (String.IsNullOrEmpty(Settings.GetApiPath(ct))) continue;
                }
                catch (Exception) {
                    continue;
                }

                dynamic secret = coin.Value;

                //  Validate phrase
                Phrase.Validate(secret.phrase.Value);

                TestKnownPhrase(secret.phrase.Value, secret.passphrase.Value, ct, secret.path != null ? secret.path.Value : null, (int)secret.txCount.Value, (double)secret.coins.Value);
            }
        }
    }
}