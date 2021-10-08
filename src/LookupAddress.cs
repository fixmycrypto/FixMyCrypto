using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FixMyCrypto {
    abstract class LookupAddress {
        protected ConcurrentQueue<Work> queue;
        protected int threadNum, threadMax;
        Stopwatch queueWaitTime = new Stopwatch();
        public static LookupAddress Create(CoinType coin, ConcurrentQueue<Work> queue, int threadNum, int threadMax) {
            switch (coin) {
                case CoinType.ADA:
                case CoinType.ADALedger:
                case CoinType.ADATrezor:

                return new LookupAddressCardano(queue, threadNum, threadMax, coin);

                case CoinType.BTC:

                return new LookupAddressBitcoin(queue, threadNum, threadMax);

                case CoinType.ETH:

                return new LookupAddressEth(queue, threadNum, threadMax);

                case CoinType.DOGE:
                case CoinType.LTC:
                case CoinType.BCH:
                case CoinType.XRP:

                return new LookupAddressAltcoin(queue, threadNum, threadMax, coin);

                default:

                throw new NotSupportedException();
            }
        }
        protected LookupAddress(ConcurrentQueue<Work> queue, int threadNum, int threadMax) {
            this.queue = queue;
            this.threadNum = threadNum;
            this.threadMax = threadMax;
        }
        public class LookupResult {
            public int txCount;
            public double coins;

            public LookupResult(int txCount = 0, double coins = 0) { this.txCount = txCount; this.coins = coins; }

            public override string ToString()
            {
                return "{ txCount: " + txCount + ", coins: " + coins + " }";
            }
        };

        public abstract CoinType GetCoinType();
        public abstract Task<LookupResult> GetContentsAsync(string address);
        public LookupResult GetContents(string address) {
            for (int i = 0; i < WebClient.retryCount; i++)
            {
                if (i > 0) Console.Error.WriteLine($"retry {i+1}/{WebClient.retryCount} query: {address}");
                try {
                    return Task.Run<LookupResult>(async() => await GetContentsAsync(address)).Result;
                }
                catch (Exception) { Thread.Sleep(1000 * (int)Math.Pow(2, i)); }
            }

            throw new TaskCanceledException($"GetContentsAsync({address}) failed after {WebClient.retryCount} attempts");
        }
        public abstract Task<List<string>> GetTransactions(string address);
        public void Finish() {
            Global.done = true;
            lock(queue) { Monitor.PulseAll(queue); }
        }
        public void Consume() {
            Log.Debug("LA" + threadNum + " start");
            int count = 0;
            Stopwatch stopWatch = new Stopwatch();

            while (!Global.done) {

                Work w = null;

                lock(queue) {
                    queueWaitTime.Start();
                    while (queue.Count == 0) {
                        if (Global.done) break;
                        //Log.Debug("LA thread " + threadNum + " waiting for work");
                        Monitor.Wait(queue);
                    }
                    queueWaitTime.Stop();

                    if (queue.TryDequeue(out w)) {
                        //Log.Debug("LA thread " + threadNum + " got address: \"" + w + "\", queue size: " + queue.Count);
                        Monitor.Pulse(queue);
                    }
                }

                if (w != null) {
                    stopWatch.Start();
                    try {
                        foreach (Address address in w.addresses) {
                            //Log.Debug($"LA{threadNum} query phrase: \"{w.phrase}\" address: {address}");
                            LookupResult lookup = GetContents(address.address);
                            //Log.Debug($"LA{threadNum} phrase: \"{w.phrase}\" address: {w.address} result: {lookup}");
                            count++;

                            if (lookup.txCount > 0 || lookup.coins > 0) {
                                Finish();

                                FoundResult.DoFoundResult(this.GetCoinType(), w.phrase, Settings.passphrase, address);
                            }
                        }
                    }
                    catch (Exception e) {
                        Log.Error("Lookup error: " + e.Message);
                    }
                    finally {
                        stopWatch.Stop();
                    }
                }
            }
            if (count > 0) Log.Info("LA" + threadNum + " done, count: " + count + " total time: " + stopWatch.ElapsedMilliseconds/1000 + $"s, time/req: {(count != 0 ? ((double)stopWatch.ElapsedMilliseconds/count) : 0):F2}ms/req, queue wait: " + queueWaitTime.ElapsedMilliseconds/1000 + "s");
            Finish();
        }
    }
    class LookupAddressEth : LookupAddress {
        public LookupAddressEth(ConcurrentQueue<Work> queue, int threadNum, int threadMax) : base(queue, threadNum, threadMax) {

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
                if (Settings.ethApiType == EthApiType.blockcypher) {
                    query = Settings.ethApi + $"/v1/eth/main/addrs/{address}/balance";
                    response = await WebClient.client.GetStringAsync(query);
                    //Log.Debug($"response: {response}");

                    dynamic stuff = JsonConvert.DeserializeObject(response);
                    //Log.Debug($"reponse: {response}\nstuff: {stuff}");

                    txCount = (int)stuff.n_tx.Value;
                    coins = (long)stuff.balance.Value / 1e18;
                }
                else if (Settings.ethApiType == EthApiType.gethrpc) {
                    query = $"[{{\"jsonrpc\":\"2.0\",\"method\":\"eth_getBalance\",\"params\":[\"{address}\", \"latest\"],\"id\":1}}, {{\"jsonrpc\":\"2.0\",\"method\":\"eth_getTransactionCount\",\"params\":[\"{address}\", \"latest\"],\"id\":2}}]";
                    var data = new StringContent(query, Encoding.UTF8, "application/json");
                    var reply = await WebClient.client.PostAsync(Settings.ethApi, data);
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
    class LookupAddressBitcoin : LookupAddress {
        public LookupAddressBitcoin(ConcurrentQueue<Work> queue, int threadNum, int threadMax) : base(queue, threadNum, threadMax){

        }
        public override CoinType GetCoinType() { return CoinType.BTC; }
        public override async Task<List<string>> GetTransactions(string address) {
            List<string> tx = new List<string>();

            if (Settings.btcApiType != BtcApiType.mempool) throw new Exception("not implemented");

            string query = Settings.btcApi + $"/api/address/{address}/txs"; 
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
            if (Settings.btcApiType == BtcApiType.blockcypher) {
                query = Settings.btcApi + $"/v1/btc/main/addrs/{address}/balance";
            }
            else if (Settings.btcApiType == BtcApiType.mempool) {
                query = Settings.btcApi + $"/api/address/{address}";
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

                if (Settings.btcApiType == BtcApiType.blockcypher) {
                    txCount = (int)stuff.n_tx.Value;
                    coins = (long)stuff.balance.Value / 100000000.0;
                }
                else if (Settings.btcApiType == BtcApiType.mempool) {
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