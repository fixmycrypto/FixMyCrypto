using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
            Global.Done = true;
            lock(queue) { Monitor.PulseAll(queue); }
        }
        public void Consume() {
            Log.Debug("LA" + threadNum + " start");
            int count = 0;
            Stopwatch stopWatch = new Stopwatch();

            while (!Global.Done) {

                Work w = null;

                lock(queue) {
                    queueWaitTime.Start();
                    while (queue.Count == 0) {
                        if (Global.Done) break;
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

                                FoundResult.DoFoundResult(this.GetCoinType(), address);
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
}