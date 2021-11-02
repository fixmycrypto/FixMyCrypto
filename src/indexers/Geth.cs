using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FixMyCrypto {
    class LookupAddressEth : LookupAddress {
        public LookupAddressEth(BlockingCollection<Work> queue, int threadNum, int threadMax) : base(queue, threadNum, threadMax) {

        }
        public override CoinType GetCoinType() { return CoinType.ETH; }
        public override Task<List<string>> GetTransactions(string address) {
            //  TODO - requires indexer

            List<string> tx = new List<string>();

            return Task<List<string>>.FromResult(tx);
        }
        public override async Task<LookupResult> GetContentsAsync(string address) {
            string query = "";
           
            int txCount = 0;
            double coins = 0;
            string response = "";

            try {
                if (Settings.EthApiType == EthApiType.blockcypher) {
                    query = Settings.EthApi + $"/v1/eth/main/addrs/{address}/balance";
                    response = await WebClient.client.GetStringAsync(query);
                    //Log.Debug($"response: {response}");

                    dynamic stuff = JsonConvert.DeserializeObject(response);
                    //Log.Debug($"reponse: {response}\nstuff: {stuff}");

                    txCount = (int)stuff.n_tx.Value;
                    coins = (long)stuff.balance.Value / 1e18;
                }
                else if (Settings.EthApiType == EthApiType.gethrpc) {
                    query = $"[{{\"jsonrpc\":\"2.0\",\"method\":\"eth_getBalance\",\"params\":[\"{address}\", \"latest\"],\"id\":1}}, {{\"jsonrpc\":\"2.0\",\"method\":\"eth_getTransactionCount\",\"params\":[\"{address}\", \"latest\"],\"id\":2}}]";
                    var data = new StringContent(query, Encoding.UTF8, "application/json");
                    var reply = await WebClient.client.PostAsync(Settings.EthApi, data);
                    response = reply.Content.ReadAsStringAsync().Result;
                    //Log.Debug($"response: {response}");

                    dynamic stuff = JsonConvert.DeserializeObject(response);
                    //Log.Debug($"stuff: {stuff}");

                    if (stuff != null && stuff.Count == 2) {
                        //  balance
                        string value = stuff[0].result.Value;
                        value = value.Replace("0x", "");
                        coins = (double)BigInteger.Parse(value, System.Globalization.NumberStyles.HexNumber) / 1e18;

                        //  tx count (only counts sent tx from this address)
                        value = stuff[1].result.Value;
                        value = value.Replace("0x", "");
                        txCount = Int32.Parse(value, System.Globalization.NumberStyles.HexNumber);
                    }
                }
                else {
                    throw new Exception("unsupported Ethereum API type");
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