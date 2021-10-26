using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FixMyCrypto {
    class LookupAddressAltcoin : LookupAddress {
        private CoinType coinType;
        public LookupAddressAltcoin(ConcurrentQueue<Work> queue, int threadNum, int threadMax, CoinType coin) : base(queue, threadNum, threadMax) {
            this.coinType = coin;
        }
        public override CoinType GetCoinType() { return this.coinType; }
        public override async Task<List<string>> GetTransactions(string address) {
            List<string> tx = new List<string>();

            if (Settings.altcoinApiType != AltcoinApiType.bitcore) throw new Exception("not implemented");

            string query = Settings.altcoinApi + $"/api/{this.coinType}/mainnet/address/{address}/txs"; 
            // Log.Debug($"query: {query}");

            string response = "";

            try {
                response = await WebClient.client.GetStringAsync(query);
                // Log.Debug($"response: {response}");

                dynamic stuff = JsonConvert.DeserializeObject(response);
                // Log.Debug($"stuff: {stuff}");

                for (int i = 0; i < stuff.Count; i++) {
                    string txid = stuff[i].mintTxid.Value;
                    if (!String.IsNullOrEmpty(txid)) tx.Add(txid);
                    txid = stuff[i].spentTxid.Value;
                    if (!String.IsNullOrEmpty(txid)) tx.Add(txid);
                }
            }
            catch (Exception e) {
                Console.Error.WriteLine($"GetContents error: {e.Message}\nquery: \"{query}\"\nresponse: {response}");
                throw;
            }

            return tx;
        }
        public override async Task<LookupResult> GetContentsAsync(string address) {
            string query, query2 = null;
            if (Settings.altcoinApiType == AltcoinApiType.dogechain) {
                query = Settings.altcoinApi + $"/chain/Dogecoin/q/addressbalance/{address}";
            }
            else if (Settings.altcoinApiType == AltcoinApiType.bitcore) {
                query = Settings.altcoinApi + $"/api/{this.coinType}/mainnet/address/{address}/balance";

                query2 = Settings.altcoinApi + $"/api/{this.coinType}/mainnet/address/{address}/txs";
            }
            else {
                throw new Exception("unsupported Altcoin API type");
            }
           
            int txCount = 0;
            double coins = 0;
            string response = "", response2 = "";

            try {
                response = await WebClient.client.GetStringAsync(query);
                // Log.Debug($"response: {response}");

                if (!String.IsNullOrEmpty(query2)) response2 = await WebClient.client.GetStringAsync(query2);
                // Log.Debug($"response: {response2}");

                if (Settings.altcoinApiType == AltcoinApiType.dogechain) {
                    coins = Double.Parse(response);
                }
                else if (Settings.altcoinApiType == AltcoinApiType.bitcore) {
                    dynamic stuff = JsonConvert.DeserializeObject(response);
                    //Log.Debug($"response: {response}\nstuff: {stuff}");

                    coins = (long)stuff.balance.Value / 100000000.0;

                    dynamic stuff2 = JsonConvert.DeserializeObject(response2);
                    // Log.Debug($"response2: {response2}\nstuff2: {stuff2}");

                    txCount = stuff2.Count;
                }
            }
            catch (Exception e) {
                Console.Error.WriteLine($"GetContents error: {e.Message}\nquery: \"{query}\"\nresponse: {response}");
                throw;
            }
           
            return new LookupResult(txCount, coins);
        }
    }
}