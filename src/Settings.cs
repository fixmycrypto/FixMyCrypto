using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace FixMyCrypto {
    class Settings {

        public static int[] indices, accounts;
        public static string phrase { get { return result.phrase; } }

        public static string passphrase { get { if (result.passphrase != null) return (string)result.passphrase.Value; else return null; } }

        public static string[] knownAddresses { get { return result.knownAddresses.ToObject<string[]>(); } }

        public static CoinType coinType { get { return GetCoinType(result.coin.Value); } }

        public static string adaApi {get { return result.adaApi; } }
        
        public static AdaApiType adaApiType { get { return (AdaApiType)Enum.Parse(typeof(AdaApiType), result.adaApiType.Value, true); } }

        public static string ethApi {get { return result.ethApi; } }

        public static EthApiType ethApiType { get { return (EthApiType)Enum.Parse(typeof(EthApiType), result.ethApiType.Value, true); } }

        public static string btcApi {get { return result.btcApi; } }

        public static BtcApiType btcApiType { get { return (BtcApiType)Enum.Parse(typeof(BtcApiType), result.btcApiType.Value, true); } }

        public static string[] paths {get { if (result.paths == null) return null; return result.paths.ToObject<string[]>(); } }

        public static string altcoinApi {get { return result.altcoinApi; } }

        public static AltcoinApiType altcoinApiType { get { return (AltcoinApiType)Enum.Parse(typeof(AltcoinApiType), result.altcoinApiType.Value, true); } }

        public static int threads {get { if (result.threads != null) return (int)result.threads.Value; else return Environment.ProcessorCount; } }

        public static double wordDistance {get { if (result.wordDistance != null) return (double)result.wordDistance.Value; else return 2.0; } }

        public static int difficulty {get { if (result.difficulty != null) return (int)result.difficulty.Value; else return 0; } }

        public static LogLevel logLevel {get { if (result.logLevel != null) return (LogLevel)Enum.Parse(typeof(LogLevel), result.logLevel.Value.ToString(), true); else return LogLevel.Info; } }

        public static string GetApiPath(CoinType coin) {
            switch (coin) {
                case CoinType.ADA:
                case CoinType.ADALedger:
                case CoinType.ADATrezor:
                return Settings.adaApi;

                case CoinType.BTC:
                return Settings.btcApi;

                case CoinType.ETH:
                return Settings.ethApi;

                case CoinType.DOGE:
                case CoinType.LTC:
                case CoinType.BCH:
                case CoinType.XRP:
                return Settings.altcoinApi;

                case CoinType.SOL:
                return null;

                default:
                throw new NotSupportedException();
            }
        }

        private static dynamic result;

        public static void LoadSettings(bool ignore = false) {
            string str = File.ReadAllText("settings.json");
            result = JsonConvert.DeserializeObject(str);

            if (ignore) {
                result.knownAddresses = null;
            }

            indices = ParseRanges((string)result.indices);
            accounts = ParseRanges((string)result.accounts);
        }

        public static CoinType GetCoinType(string str) {
            return (CoinType)Enum.Parse(typeof(CoinType), str.ToUpper(), true);
        }

        public static int[] ParseRanges(string s) {
            if (String.IsNullOrEmpty(s)) {
                int[] i = { 0 };
                return i;
            }

            List<int> l = new List<int>();

            foreach (string range in s.Split(',')) {
                if (range.Contains("-")) {
                    string[] subs = range.Split("-");
                    if (subs.Length == 2) {
                        int start, end;
                        if (int.TryParse(subs[0], out start) && int.TryParse(subs[1], out end)) {
                            for (int i = start; i <= end; i++) l.Add(i);
                        }
                    }
                }
                else {
                    int val;
                    if (int.TryParse(range, out val)) l.Add(val);
                }
            }

            return l.ToArray();
        }

        public static string GetRangeString(int[] l) {
            return String.Join(",", l);
        }
        public static string GetVersion() {
            Assembly assem = Assembly.GetExecutingAssembly();
            AssemblyName assemName = assem.GetName();
            Version ver = assemName.Version;
            string version = $"{assemName.Name} Version {ver.ToString()}";
            return version;
        }
    }
}
