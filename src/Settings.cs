using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace FixMyCrypto {
    class Settings {

        public static int[] Indices = { 0 }, Accounts = { 0 };
        public static string Phrase { get { return result.phrase; } }

        public static string[] Passphrases { 
            
            get {
                if (result == null) {
                    return null;
                }
                else if (result.passphrase != null) 
                {
                    string[] passphrases = null;

                    try {
                        passphrases = result.passphrase.ToObject<string[]>(); 
                    }
                    catch (Exception) { }

                    if (passphrases == null) 
                        passphrases = new string[] { result.passphrase.Value };

                    return passphrases;
                }
                else 
                    return null; 
            } 
        }

        public static string[] KnownAddresses { get { return result?.knownAddresses?.ToObject<string[]>(); } }

        public static CoinType CoinType { get { return GetCoinType(result.coin.Value); } }

        public static string AdaApi {get { return result.adaApi; } }
        
        public static AdaApiType AdaApiType { get { return (AdaApiType)Enum.Parse(typeof(AdaApiType), result.adaApiType.Value, true); } }

        public static string EthApi {get { return result.ethApi; } }

        public static EthApiType EthApiType { get { return (EthApiType)Enum.Parse(typeof(EthApiType), result.ethApiType.Value, true); } }

        public static string BtcApi {get { return result.btcApi; } }

        public static BtcApiType BtcApiType { get { return (BtcApiType)Enum.Parse(typeof(BtcApiType), result.btcApiType.Value, true); } }

        public static string[] Paths {get { if (result != null && result.paths == null) return null; return result.paths.ToObject<string[]>(); } }

        public static string AltcoinApi {get { return result.altcoinApi; } }

        public static AltcoinApiType AltcoinApiType { get { return (AltcoinApiType)Enum.Parse(typeof(AltcoinApiType), result.altcoinApiType.Value, true); } }

        public static int Threads {get { if (result != null && result.threads != null) return (int)result.threads.Value; else return Environment.ProcessorCount; } }

        public static double WordDistance {get { if (result != null && result.wordDistance != null) return (double)result.wordDistance.Value; else return 2.0; } }

        public static int Difficulty {get { if (result != null && result.difficulty != null) return (int)result.difficulty.Value; else return 0; } }

        public static LogLevel logLevel {get { if (result != null && result.logLevel != null) return (LogLevel)Enum.Parse(typeof(LogLevel), result.logLevel.Value.ToString(), true); else return LogLevel.Info; } }

        public static string TopologyFile { get { if (result != null) return result.topologyFile; else return null; } }

        public static bool NoETA { get { if (result?.noETA != null) return result.noETA; return false; }}

        private static int platform = -1;
        public static int OpenCLPlatform {get { return platform; } }
        private static int[] devices = null;
        public static int[] OpenCLDevices {get { return devices; } }

        private static bool ignoreResults = false;
        public static bool IgnoreResults {get { return ignoreResults; } }

        public static string GetApiPath(CoinType coin) {
            switch (coin) {
                case CoinType.ADA:
                case CoinType.ADALedger:
                case CoinType.ADATrezor:
                return Settings.AdaApi;

                case CoinType.BTC:
                return Settings.BtcApi;

                case CoinType.ETH:
                return Settings.EthApi;

                case CoinType.DOGE:
                case CoinType.LTC:
                case CoinType.BCH:
                case CoinType.XRP:
                return Settings.AltcoinApi;

                case CoinType.SOL:
                case CoinType.ALGO:
                case CoinType.DOT:
                case CoinType.DOTLedger:
                case CoinType.ATOM:
                case CoinType.CRO:
                return null;

                default:
                throw new NotSupportedException();
            }
        }

        private static dynamic result;

        public static void LoadSettings(bool ignore = false, string[] args = null) {
            string str = File.ReadAllText("settings.json");
            result = JsonConvert.DeserializeObject(str);

            if (ignore) {
                result.knownAddresses = null;
            }

            Indices = ParseRanges((string)result.indices);
            Accounts = ParseRanges((string)result.accounts);
            if (result.platform != null) platform = (int)result.platform.Value;
            if (result.devices != null) devices = result.devices.ToObject<int[]>();

            try {
                for (int i = 0; i < args.Length; i++) {
                    switch (args[i]) {
                        case "-platform":
                        case "-p":

                        Int32.TryParse(args[i + 1], out platform);
                        i++;

                        break;

                        case "-device":
                        case "-devices":
                        case "-d":

                        devices = ParseRanges(args[i + 1]);
                        i++;

                        break;

                        case "-ignoreResults":

                        ignoreResults = true;

                        break;
                    }
                }
            }
            catch (Exception e) {
                throw new ArgumentException(e.Message);
            }
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
