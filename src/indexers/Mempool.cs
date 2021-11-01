using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FixMyCrypto {
    class LookupAddressBitcoin : LookupAddress {
        public LookupAddressBitcoin(ConcurrentQueue<Work> queue, int threadNum, int threadMax) : base(queue, threadNum, threadMax){

        }
        public override CoinType GetCoinType() { return CoinType.BTC; }
        public override async Task<List<string>> GetTransactions(string address) {
            List<string> tx = new List<string>();

            if (Settings.BtcApiType != BtcApiType.mempool) throw new Exception("not implemented");

            string query = Settings.BtcApi + $"/api/address/{address}/txs"; 
            // Log.Debug($"query: {query}");

            string response = "";

            try {
                response = await WebClient.client.GetStringAsync(query);
                // Log.Debug($"response: {response}");

                dynamic stuff = JsonConvert.DeserializeObject(response);
                // Log.Debug($"stuff: {stuff}");

                for (int i = 0; i < stuff.Count; i++) {
                    string txid = stuff[i].txid.Value;

                    tx.Add(txid);
                }
            }
            catch (Exception e) {
                Console.Error.WriteLine($"GetContents error: {e.Message}\nquery: \"{query}\"\nresponse: {response}");
                throw;
            }

            return tx;
        }
        public override async Task<LookupResult> GetContentsAsync(string address) {
            string query;
            if (Settings.BtcApiType == BtcApiType.blockcypher) {
                query = Settings.BtcApi + $"/v1/btc/main/addrs/{address}/balance";
            }
            else if (Settings.BtcApiType == BtcApiType.mempool) {
                query = Settings.BtcApi + $"/api/address/{address}";
            }
            else {
                throw new Exception("unsupported Bitcoin API type");
            }
           
            int txCount = 0;
            double coins = 0;
            string response = "";

            try {
                response = await WebClient.client.GetStringAsync(query);
                //Log.Debug($"response: {response}");

                dynamic stuff = JsonConvert.DeserializeObject(response);
                //Log.Debug($"reponse: {response}\nstuff: {stuff}");

                if (Settings.BtcApiType == BtcApiType.blockcypher) {
                    txCount = (int)stuff.n_tx.Value;
                    coins = (long)stuff.balance.Value / 100000000.0;
                }
                else if (Settings.BtcApiType == BtcApiType.mempool) {
                    txCount = (int)stuff.chain_stats.tx_count.Value;
                    coins = (long)stuff.chain_stats.funded_txo_sum.Value / 100000000.0;
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