using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FixMyCrypto {
    class LookupAddressCardano : LookupAddress {
        CoinType coinType;
        public LookupAddressCardano(ConcurrentQueue<Work> queue, int threadNum, int threadMax, CoinType coin) : base(queue, threadNum, threadMax) {
            this.coinType = coin;
        }
        public override CoinType GetCoinType() { return this.coinType; }
        public override async Task<List<string>> GetTransactions(string address) {
            List<string> txs = new List<string>();

            if (Settings.adaApiType != AdaApiType.graphql) throw new Exception("not implemented");

            string query = null, response = null;

            try {
                query = @"{""query"":""query getAddressTransactions($address: String!) {\n  transactions(\n    limit: 10\n    where: {\n      _or: [\n        { inputs: { address: { _eq: $address } } }\n        { outputs: { address: { _eq: $address } } }\n      ]\n    }\n  ) {\n    hash\n  }\n}\n"",""variables"":{""address"":""" + address + @"""}}";
                // Log.Debug(query);
                var data = new StringContent(query, Encoding.UTF8, "application/json");
                var reply = await WebClient.client.PostAsync(Settings.adaApi, data);
                response = reply.Content.ReadAsStringAsync().Result;
                // Log.Debug(response);

                dynamic stuff = JsonConvert.DeserializeObject(response);

                if (stuff.data.transactions != null) {
                    for (int i = 0; i < stuff.data.transactions.Count; i++) {
                        dynamic tx = stuff.data.transactions[i];

                        string hash = tx.hash.Value;

                        txs.Add(hash);
                    }
                }
            }
            catch (Exception e) {
                Console.Error.WriteLine($"GetTransactions error: {e.Message}\nquery: \"{query}\"\nresponse: {response}");
                throw;
            }

            return txs;
        }
        public override async Task<LookupResult> GetContentsAsync(string address) {
            int txCount = 0;
            double coins = 0;
            string response = "";
            string query = "";

            try {
                if (Settings.adaApiType == AdaApiType.rest) {
                    query = Settings.adaApi + $"/api/addresses/summary/{address}";
                    response = await WebClient.client.GetStringAsync(query);
                    dynamic stuff = JsonConvert.DeserializeObject(response);
                    //Log.Debug($"reponse: {response}\nstuff: {stuff}");

                    txCount = (int)stuff.Right.caTxNum;
                    coins = Int64.Parse(stuff.Right.caBalance.getCoin.Value)/1000000.0;
                } 
                else if (Settings.adaApiType == AdaApiType.graphql) {
                    query = $"{{ \"query\": \"{{ paymentAddresses (addresses: \\\"{address}\\\") {{ summary {{ assetBalances {{ quantity }}, utxosCount }} }} }}\" }}";
                    //Log.Debug($"query: {query}");
                    var data = new StringContent(query, Encoding.UTF8, "application/json");
                    var reply = await WebClient.client.PostAsync(Settings.adaApi, data);
                    response = reply.Content.ReadAsStringAsync().Result;
                    //Log.Debug($"response: {response}");

                    dynamic stuff = JsonConvert.DeserializeObject(response);
                    //Log.Debug($"stuff: {stuff}");

                    if (stuff.data.paymentAddresses != null && stuff.data.paymentAddresses.Count > 0) {
                        txCount = (int)stuff.data.paymentAddresses[0].summary.utxosCount;
                        if (stuff.data.paymentAddresses[0].summary.assetBalances.Count > 0) {
                            coins = Int64.Parse(stuff.data.paymentAddresses[0].summary.assetBalances[0].quantity.Value)/1000000.0;
                        }
                    }

                }
                //Log.Debug($"response: {response}");

            }
            catch (Exception e) {
                Console.Error.WriteLine($"GetContents error: {e.Message}\nquery: \"{query}\"\nresponse: {response}");
                throw;
            }
           
            return new LookupResult(txCount, coins);
        }
    }
}