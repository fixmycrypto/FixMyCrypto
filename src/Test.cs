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
            if (expectTx == 0 && expectCoins == 0) return;
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

        public static void FailValidateAddress(CoinType ct, string address) {
            try {
                PhraseToAddress.ValidateAddress(ct, address);
            }
            catch (Exception) {
                return;
            }

            throw new Exception($"validation should have failed: {ct} {address}");
        }
        public static void FailValidatePhrase(string phrase) {
            try {
                Phrase.Validate(phrase);
            }
            catch (Exception) {
                return;
            }

            throw new Exception($"validation should have failed: {phrase}");
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
        public static void TestPassphrase(string pattern, string expect, int expectCount = -1, int depth = 1) {
            string[] s = { expect };
            TestPassphrase(pattern, s, expectCount, depth);
        }

        public static void TestPassphrase(string pattern, string[] expect, int expectCount = -1, int depth = 1) {
            // Log.Debug($"Test Passphrase: {pattern}");

            Passphrase ph = new Passphrase(pattern, depth);
            int measuredCount = ph.GetCount();
            if (expectCount > -1 && measuredCount != expectCount) throw new Exception($"Passphrase pattern: \"{pattern}\" GetCount() returned {measuredCount} permutations; expected {expectCount}");
            bool[] found = new bool[expect.Length];
            int count = 0;
            foreach (string pass in ph.Enumerate()) {
                // Log.Debug($"generated: {pass}");
                int i = Array.IndexOf(expect, pass);
                if (i > -1) found[i] = true;
                count++;
            }

            if (count != measuredCount) throw new Exception($"Passphrase pattern: \"{pattern}\" GetCount() returned {measuredCount} permutations; counted {count}");

            if (expectCount > -1 && count != expectCount) throw new Exception($"Passphrase pattern: \"{pattern}\" generated {count} permutations; expected {expectCount}");

            for (int i = 0; i < found.Length; i++) if (!found[i]) throw new Exception($"Passphrase pattern: \"{pattern}\" failed to generate: \"{expect[i]}\"");

            if (expectCount == -1) Log.Debug($"Passphrase pattern: \"{pattern}\" {(depth == 1 ? "" : $"(depth = {depth}) ")}generated {count} permutations");
        }

        public static void FailPassphrase(string pattern, string expect) {
            try {
                TestPassphrase(pattern, expect);
            }
            catch (Exception) {
                return;
            }

            throw new Exception($"TestPassphrase({pattern}, {expect}) didn't throw an exception");
        }

        public static void BenchmarkPassphrase(string pattern) {
            Log.Debug($"Benchmark passphrase: {pattern}");
            Passphrase p = new Passphrase(pattern);
            int count = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (string pass in p.Enumerate()) {
                // Log.Debug(pass);
                count++;
            }
            sw.Stop();
            Log.Debug($"Generated {count} results in {sw.ElapsedMilliseconds}ms ({1000.0 * (double)sw.ElapsedMilliseconds/count:F4}us/attempt)");
        }
        public static void TestRandomPassphrase() {
            int length = Random.Shared.Next(0, 12);
            string expect = "";
            string pattern = "";
            for (int i = 0; i < length; i++) {
                char c = (char)Random.Shared.Next(0x20, 0x7f);
                expect += c;

                switch (c) {
                    case '(':
                    case ')':

                    pattern += $"[{c}]";

                    break;

                    case '[':
                    case ']':

                    pattern += $"({c})";

                    break;

                    case '?':

                    if (pattern.Length > 0 && (pattern[pattern.Length - 1] == ')' || pattern[pattern.Length - 1] == ']')) {
                        pattern += "[?]";
                    }
                    else {
                        pattern += "?";
                    }

                    break;

                    case '^':

                    if (pattern.Length > 0 && pattern[pattern.Length - 1] == '[') {
                        pattern += "^^";
                    }
                    else {
                        pattern += "^";
                    }

                    break;

                    default:

                    pattern += c;

                    break;
                }
            }
            if (pattern.StartsWith("{{") && pattern.EndsWith("}}")) {
                pattern = "[{]" + pattern.Substring(1);
            }
            TestPassphrase(pattern, expect, 1);
        }

        public static void TestPhraseChecksum(string phrase) {
            (bool valid, int hash) = Phrase.VerifyChecksum(new Phrase(phrase).Indices);
            if (!valid) throw new Exception($"invalid checksum for: {phrase}");
        }

        public static void FailPhraseChecksum(string phrase) {
            try {
                TestPhraseChecksum(phrase);
            }
            catch (Exception) {
                return;
            }

            throw new Exception($"checksum should've failed: {phrase}");
        }

        public static void Run(int count, string opts) {
            //  Needed to init word lists
            Wordlists.Initialize(null);

            //  Passphrase topology
            // Passphrase ph = new Passphrase("(The||the)(P||p)assphrase[0-9]?[!@#$%^&*()]?");
            Passphrase ph = new Passphrase("((C||c)orrect&&(H||h)orse&&(B||b)attery&&(S||s)taple)[1-9]?[0-9][^a-zA-Z0-9]");
            ph.WriteTopologyFile("graph.dot");

            //  Test passphrase expansion
            TestPassphrase("Passphrase!", "Passphrase!", 1);
            TestPassphrase("(H||h)ello", "hello", 2);
            TestPassphrase("(H||h)ello[0-9]", "Hello7", 20);
            TestPassphrase("Hello[$%^]?", new string[] { "Hello", "Hello$" }, 4);
            TestPassphrase("(H||h)ello[0-24-5]", "hello4", 10);
            TestPassphrase("[(] [)] ([) (])", "( ) [ ]", 1);
            TestPassphrase("(something or ||)nothing", new string[] { "something or nothing", "nothing" }, 2);
            TestPassphrase("(something or )?nothing", new string[] { "something or nothing", "nothing" }, 2);
            TestPassphrase("Hello( Dolly||)[!@#$%^&*()]?", new string[] { "Hello Dolly!", "Hello", "Hello*" }, 22);
            TestPassphrase("(Big||Bunny)(Big||Bunny)", new string[] { "BigBunny", "BunnyBig", "BunnyBunny" }, 4);
            TestPassphrase("[a-zA-Z]", "Q", 52);
            TestPassphrase("[a-zA-Z][0-9]", "B4", 520);
            TestPassphrase("[1-9]?[0-9]", new string[] { "0", "9", "42" }, 100);
            TestPassphrase("(Correct||Horse||Battery)?Staple", new string[] { "Staple", "CorrectStaple", "HorseStaple", "BatteryStaple" }, 4);
            TestPassphrase("(Correct&&Horse)", new string[] { "CorrectHorse", "HorseCorrect" }, 2);
            TestPassphrase("(1&&2&&3)", new string[] { "123", "132", "213", "231", "312", "321" }, Utils.Factorial(3));
            TestPassphrase("((Correct||correct)&&(Horse||horse))", new string[] { "CorrectHorse", "Correcthorse", "horseCorrect" }, 8);
            TestPassphrase("((C||c)orrect&&(H||h)orse)", new string[] { "correcthorse", "HorseCorrect", "horsecorrect" }, 8);
            TestPassphrase("(H||h)ello(D||d)olly[!@#$%^&*][0-9][0-9]?", new string[] { "hellodolly!1", "hellodolly$42", "Hellodolly*69" }, 320 * 11);
            TestPassphrase("[^a-zA-Z0-9]", "~");
            TestPassphrase("[^^]", "^", 1);
            TestPassphrase("[^^$]", new string[] { "^", "$" }, 2);
            TestPassphrase("(T||t)?he", new string[] { "The", "the", "he" }, 3);
            TestPassphrase("((a&&b)||c)", new string[] { "ab", "ba", "c" }, 3);
            TestPassphrase("((a||b)&&c)", new string[] { "ac", "ca", "bc", "cb" }, 4);
            TestPassphrase("(The||the)(P||p)assphrase[0-9]?[!@#$%^&*()]?", "ThePassphrase!", 484);
            TestPassphrase("((C||c)orrect&&(H||h)orse&&(B||b)attery&&(S||s)taple)[1-9]?[0-9][^a-zA-Z0-9]", new string[] { "CorrectHorseBatteryStaple1!", "horseStaplebatteryCorrect42?", "batterystaplecorrectHorse99@" }, 16 * Utils.Factorial(4) * 10 * 10 * 33);
            TestPassphrase("(a&&b)?c", new string[] { "abc", "bac", "c" }, 3);
            //  fuzzing
            TestPassphrase("{{Foo92!}}", new string[] { "Foo93!", "Foo92", "Foo9!", "Food92!", "Foo92!a" });
            TestPassphrase("{{Food92?}}", new string[] { "Foo92?" });
            TestPassphrase("{{ThePassphrase!}}", new string[] { "ThePasshprase!", "thePassphrase!" });
            TestPassphrase("{{CorrectHorseBatteryStable42!}}", new string[] { "CorrectHorseBatteryStaple42!" });
            //  fuzz depth 2
            TestPassphrase("{{Foo92!}}", new string[] { "Foo93!", "Foo92", "Foo9!", "Food92!", "Foo92!a", "Foo83!", "Fo92", "food92!", "Foo92!ab" }, depth: 2);
            TestPassphrase("{{ThePassphrase!}}", new string[] { "ThePasshprase!", "thePassphrase!", "ThePasshprase1" }, depth: 2);

            //  random passphrase testing
            Parallel.For(0, 1000000, i => TestRandomPassphrase());

            //  should fail
            FailPassphrase("(stuff", "stuff");
            FailPassphrase("((stuff)", "(stuff");
            FailPassphrase("(Big||Bunny)(Big||Bunny)", "");
            FailPassphrase("[1-9]?[0-9]", "00");
            FailPassphrase("[1-9]?[0-9]", "01");
            FailPassphrase("[^a-zA-Z0-9]", "a");
            FailPassphrase("((a&&b)||c)", "ac");
            FailPassphrase("((a&&b)||c)", "bc");
            FailPassphrase("((a||b)&&c)", "ab");

            //  Benchmark
            //  976683582 results in 303864ms (0.3111us/attempt)
            // BenchmarkPassphrase("[a-zA-Z0-9]?[a-zA-Z0-9]?[a-zA-Z0-9]?[a-zA-Z0-9]?[a-zA-Z0-9]?");

            //  test phrase checksums
            TestPhraseChecksum("fantasy curious recycle slot tilt forward call jar fashion concert around symbol");
            TestPhraseChecksum("script degree trigger excite acid neither nut safe warrior burden lift bone hand squeeze try");
            TestPhraseChecksum("moon garage horse outer when glow task loan crane roof note lonely culture eight sunset business unknown emotion");
            TestPhraseChecksum("chronic snap between hand attack purity say airport expect depend below sunset useless winner old edit permit concert dwarf cake virus split first resource");
            TestPhraseChecksum("multiply saddle magic timber churn void lake wide reflect ball slender apple actress myself stock print mango notice slot emotion joke wage trend above wall");

            //  fail phrase checksums
            FailPhraseChecksum("fantasy curious recycle abandon tilt forward call jar fashion concert around symbol");
            FailPhraseChecksum("script degree trigger excite abandon neither nut safe warrior burden lift bone hand squeeze try");
            FailPhraseChecksum("moon garage horse outer when glow task abandon crane roof note lonely culture eight sunset business unknown emotion");
            FailPhraseChecksum("chronic snap between abandon attack purity say airport expect depend below sunset useless winner old edit permit concert dwarf cake virus split first resource");
            FailPhraseChecksum("multiply saddle magic timber churn void abandon wide reflect ball slender apple actress myself stock print mango notice slot emotion joke wage trend above wall");

            //  test phrase validation (with unknown/missing/bad lengths/etc) - NOT checksums
            Phrase.Validate("siren bottom inform vehicle else donkey dirt task cook tide general when");
            Phrase.Validate("siren bottom inform vehicle * donkey task cook tide general when ?");
            
            FailValidatePhrase("siren bottom inform vehicle else donkey dirt task cook tide general");
            FailValidatePhrase("siren bottom inform vehicle else donkey dirt task cook tide general when when");
            FailValidatePhrase("siren bottom inform vehicle ? donkey dirt task cook tide general when");
            FailValidatePhrase("nope nope nope nope nope donkey dirt task cook tide general when");

            //  TODO: benchmark phrase checksums

            //  Test address validation
            //  Should pass
            PhraseToAddress.ValidateAddress(CoinType.BTC, "14NPVhtZo8c5vxuZwTGGYxJPd8HbtqEJpu");
            PhraseToAddress.ValidateAddress(CoinType.BTC, "39JhELZ5E9HaGKBnnQhfWqTZLF4vUfKKsh");
            PhraseToAddress.ValidateAddress(CoinType.BTC, "bc1qmr9cmy3p3q695mkn5rnmz27csvnvr2ja05wa4p");
            PhraseToAddress.ValidateAddress(CoinType.ETH, "0x67cFdcFF9d22ED77F612043547f44980e6793859");
            PhraseToAddress.ValidateAddress(CoinType.LTC, "LcvWUyyTThg39acPTn5Ek2eC2kcBiz37Lk");
            PhraseToAddress.ValidateAddress(CoinType.LTC, "MFn6qthNdwqYyFGdq6TzEtev5rTupUjSVh");
            PhraseToAddress.ValidateAddress(CoinType.LTC, "ltc1q55zt0pq7dy9p9n7va9fyqhxldnp7ajcyhv84tx");
            PhraseToAddress.ValidateAddress(CoinType.DOGE, "D6NBFwSBkhYt88B1NCpE6Ecawym7m5w6i6");
            PhraseToAddress.ValidateAddress(CoinType.BCH, "1PVKjS1SueNyqLHXbWSP6Tb7YNo541tShZ");
            PhraseToAddress.ValidateAddress(CoinType.BCH, "1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa");
            PhraseToAddress.ValidateAddress(CoinType.BCH, "qp3wjpa3tjlj042z2wv7hahsldgwhwy0rq9sywjpyy");
            PhraseToAddress.ValidateAddress(CoinType.SOL, "uqYc2vewvfag8m6Ys6WWYekXf2BzKwWyLxBh1mftMPF");
            PhraseToAddress.ValidateAddress(CoinType.ALGO, "VCMJKWOY5P5P7SKMZFFOCEROPJCZOTIJMNIYNUCKH7LRO45JMJP6UYBIJA");
            PhraseToAddress.ValidateAddress(CoinType.ALGO, "CA73GRGZZPMVE57DVFDPEBTHUHN3RT76RZGH4MGBZFJGAAL3ODN2WVDN7Q");
            PhraseToAddress.ValidateAddress(CoinType.DOT, "12G1bZ4Ki2H37o4iZHTADd46aZSAyG93SpBZSubzrM2sYHNX");

            //  Should fail
            FailValidateAddress(CoinType.BTC, "14NPVhtZo8c5vxuZwTOGYxJPd8HbtqEJpu");
            FailValidateAddress(CoinType.BTC, "bc1qmr9cmy3p3q695mkn5rnmz27csvnvr2ja05wa4b");
            FailValidateAddress(CoinType.ETH, "0x67cFdcFF9d22ED77F612043547f44980e679385");
            FailValidateAddress(CoinType.LTC, "btc1q55zt0pq7dy9p9n7va9fyqhxldnp7ajcyhv84tx");
            FailValidateAddress(CoinType.DOGE, "D6NBFwSBkhYt88B1NCpE6Ecawym7m5w6i6o");
            FailValidateAddress(CoinType.SOL, "uqYc2vewvfag8m6Ys6WWYekXf2BzKwWyLxBh1mftMPFu");
            FailValidateAddress(CoinType.ALGO, "CA73GRGZZPMVE57DVFDPEBTHUHN3RT76RZGH4MGBZFJGAAL3ODN2WVDN7");
            FailValidateAddress(CoinType.ALGO, "CA73GRGZZPMVE57DVFDPEBTHUHM3RT76RZGH4MGBZFJGAAL3ODN2WVDN7Q");
            FailValidateAddress(CoinType.DOT, "12G1bZ4Ki2H37o4iZHTADd46aZSAyG93SpBZSubzrM2sYHND");

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
                                Log.Debug($"Derived {ct} address: {address} ({a.path})");
                                found = true;
                            }
                        }

                        if (!found) {
                            Log.Error($"{ct} failed to derive:\n\t{address} ({path}).\nFound:\n");
                            foreach (Address a in addresses) {
                                Log.Error($"\t{a.address} {a.path}");
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

                if (secret.txCount == null || secret.coins == null) continue;

                TestKnownPhrase(secret.phrase.Value, secret.passphrase.Value, ct, secret.path != null ? secret.path.Value : null, (int)secret.txCount.Value, (double)secret.coins.Value);
            }
        }
    }
}