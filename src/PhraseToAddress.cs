using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FixMyCrypto {
    abstract class PhraseToAddress {

        PhraseProducer phraseProducer;

        public static PhraseToAddress Create(CoinType coin, PhraseProducer phrases) {
            switch (coin) {
                case CoinType.ADA:

                return new PhraseToAddressCardano(phrases);

                case CoinType.ADALedger:

                return new PhraseToAddressCardanoLedger(phrases);

                case CoinType.ADATrezor:

                return new PhraseToAddressCardanoTrezor(phrases);

                case CoinType.ETH:

                return new PhraseToAddressEth(phrases);

                case CoinType.BTC:
                case CoinType.DOGE:
                case CoinType.LTC:
                case CoinType.BCH:

                return new PhraseToAddressBitAltcoin(phrases, coin);

                case CoinType.SOL:

                return new PhraseToAddressSolana(phrases);

                case CoinType.ALGO:

                return new PhraseToAddressAlgorand(phrases);

                case CoinType.DOT:

                return new PhraseToAddressPolkadot(phrases);

                case CoinType.DOTLedger:

                return new PhraseToAddressPolkadotLedger(phrases);

                case CoinType.XRP:

                return new PhraseToAddressXrp(phrases);

                case CoinType.ATOM:

                return new PhraseToAddressAtom(phrases);

                case CoinType.CRO:

                return new PhraseToAddressCRO(phrases);

                default:

                throw new NotSupportedException();
            }
        }
        Stopwatch queueWaitTime = new Stopwatch();
        Stopwatch stopWatch = new Stopwatch();
        long count = 0;
        string lastPassphrase = null;
        Phrase lastPhrase = null;
        Checkpoint checkpoint = null;
        long lastPassphraseNum = 0, passphraseCount = 0, passphraseStart = 0, lastPhraseNum = 0, phraseStart = 0;
        long totalPhrasesTested = 0, totalPassphrasesTested = 0;
        object mutex = new();
        protected OpenCL ocl = null;
        protected MultiPassphrase mp = null;
        protected long mpCount = 0;

        protected int maxPassphraseLength = 0;
        public virtual bool IsUsingOpenCL() {
            return false;
        }

        public virtual int GetKeyLength() {
            return 64;
        }

        private abstract class Batch {
            public PathTree tree;

            public ProduceAddress produceAddress;

            public long id;

            public long count;

            public abstract Phrase[] GetPhrases();

            public abstract string[] GetPassphrases();

            public Batch(long id, PathTree tree, ProduceAddress produceAddress) {
                this.tree = tree;
                this.produceAddress = produceAddress;
                this.id = id;
                this.count = 0;
            }
        }
        private class PhraseBatch : Batch {
            public Phrase[] phrases;
            public string passphrase;

            public PhraseBatch(long id, Phrase[] phrases, string passphrase, PathTree tree, ProduceAddress produceAddress) : base(id, tree, produceAddress) {
                this.phrases = phrases;
                this.passphrase = passphrase;
            }

            public override Phrase[] GetPhrases() {
                return phrases;
            }

            public override string[] GetPassphrases() {
                return new string[] { passphrase };
            }
        }
        private class PassphraseBatch : Batch {
            public Phrase phrase;
            public string[] passphrases;
            public long passphraseSequenceNum;
            public PassphraseBatch(long id, Phrase phrase, string[] passphrases, long passphraseSequenceNum, PathTree tree, ProduceAddress produceAddress) : base(id, tree, produceAddress) {
                this.phrase = phrase;
                this.passphrases = passphrases;
                this.passphraseSequenceNum = passphraseSequenceNum;
            }
            public override Phrase[] GetPhrases() {
                return new Phrase[] { phrase };
            }

            public override string[] GetPassphrases() {
                return passphrases;
            }

        }
        private BlockingCollection<Batch> batchKeysQueue = new BlockingCollection<Batch>(Settings.Threads);
        private BlockingCollection<Batch> batchAddressesQueue = new BlockingCollection<Batch>(Settings.Threads);

        private ConcurrentDictionary<long, Batch> batchFinished = new();
        private bool allBatchesFinished = false;
        private long nextBatchId = 0;
        private long lastBatchFinished = 0;
        protected PhraseToAddress(PhraseProducer phraseProducer) {
            this.phraseProducer = phraseProducer;
        }
        public void SetOpenCL(OpenCL ocl) {
            this.ocl = ocl;
        }
        public void SetPassphrase(MultiPassphrase mp, long mpCount, int maxPassphraseLength) {
            this.mp = mp;
            this.mpCount = mpCount;
            this.maxPassphraseLength = maxPassphraseLength;
        }
        public void SetCheckpoint(Checkpoint c) {
            checkpoint = c;
        }
        public (Phrase, string, long, long, int) GetLastTested() {
            lock(mutex) {
                return (this.lastPhrase, 
                        this.lastPassphrase, 
                        this.lastPassphraseNum, 
                        this.passphraseCount, 
                        this.maxPassphraseLength);
            }
        }

        public abstract Cryptography.Key DeriveRootKey(Phrase phrase, string passphrase);

        public virtual Cryptography.Key[] DeriveRootKey_BatchPhrases(Phrase[] phrases, string passphrase)
        {
            Cryptography.Key[] keys = new Cryptography.Key[phrases.Length];
            Parallel.For(0, phrases.Length, GetParallelOptions(), i => {
            // for (int i = 0; i < phrases.Length; i++) {
                // if (Global.Done) break;
                if (Global.Done) return;
                keys[i] = DeriveRootKey(phrases[i], passphrase);
            // }
            });
            return keys;
        }

        public virtual Cryptography.Key[] DeriveRootKey_BatchPassphrases(Phrase phrase, string[] passphrases)
        {
            Cryptography.Key[] keys = new Cryptography.Key[passphrases.Length];
            Parallel.For(0, passphrases.Length, GetParallelOptions(), i => {
            // for (int i = 0; i < passphrases.Length; i++) {
                // if (Global.Done) break;
                if (Global.Done) return;
                keys[i] = DeriveRootKey(phrase, passphrases[i]);
            // }
            });
            return keys;
        }
        protected abstract Cryptography.Key DeriveChildKey(Cryptography.Key parentKey, uint index);

        protected virtual Cryptography.Key[] DeriveChildKey_Batch(Cryptography.Key[] parents, uint index) {
            Cryptography.Key[] keys = new Cryptography.Key[parents.Length];
            Parallel.For(0, parents.Length, GetParallelOptions(), i => {
            // for (int i = 0; i < parents.Length; i++) {
                // if (Global.Done) break;
                if (Global.Done) return;
                keys[i] = DeriveChildKey(parents[i], index);
            // }
            });
            return keys;
        }
        private Cryptography.Key[] DerivePath_Batch(Cryptography.Key[] parents, uint[] path) {
            Cryptography.Key[] keys = parents;
            foreach (uint index in path) {
                if (Global.Done) break;
                keys = DeriveChildKey_Batch(keys, index);
            }
            return keys;
        }
        protected virtual void DeriveRootLongestPath(Phrase[] phrases, string[] passphrases, PathNode node) {
            Cryptography.Key[] keys;
            if (phrases.Length == 1) {
                keys = DeriveRootKey_BatchPassphrases(phrases[0], passphrases);
            }
            else {
                keys = DeriveRootKey_BatchPhrases(phrases, passphrases[0]);
            }

            node.Keys = DerivePath_Batch(keys, node.GetPathValues().Slice(1));
        }
        protected abstract Address DeriveAddress(PathNode node, int keyIndex);
        public abstract void ValidateAddress(string address);

        public static void ValidateAddress(CoinType coin, string address) {
            PhraseToAddress p2a = PhraseToAddress.Create(coin, null);
            p2a.ValidateAddress(address);
        }

        protected virtual string GetStakePath() { return null; }

        private void DeriveChildKeys_Batch(PathNode node) {
            if (Global.Done) return;

            node.Keys = DeriveChildKey_Batch(node.Parent.Keys, node.Value);

            foreach (PathNode child in node.Children) {
                DeriveChildKeys_Batch(child);
            }
        }

        private void DeriveAddressesBatch(PathNode node, int index, Phrase phrase, string passphrase, List<Address> addresses) {
            if (Global.Done) return;

            if (node.End) {
                var address = DeriveAddress(node, index);
                if (address != null) {
                    address.phrase = phrase;
                    address.passphrase = passphrase;
                    addresses.Add(address);
                }
            }

            foreach (PathNode child in node.Children) {
                DeriveAddressesBatch(child, index, phrase, passphrase, addresses);
            }
        }
        private List<Address> GetAddressesList(Phrase phrase, string[] passphrases, int account, int index, string[] paths) {
            int[] accounts = { account };
            int[] indices = { index };
            return GetAddressesList(phrase, passphrases, paths, accounts, indices);
        }

        public PathTree CreateTree(string[] paths, int[] accounts, int[] indices) {
            PathTree tree = new PathTree();

            if (paths == null || paths.Length == 0 || (paths.Length == 1 && String.IsNullOrEmpty(paths[0]))) {
                paths = GetDefaultPaths(Settings.KnownAddresses);
            }

            if (accounts == null || accounts.Length == 0) accounts = Settings.Accounts;
            if (indices == null || indices.Length == 0) indices = Settings.Indices;

            foreach (string path in paths) {
                string pa = path;
                if (!pa.Contains("{account}") && !pa.Contains("{index}")) {
                    pa = Path.Tokenize(path);
                }

                foreach (int account in accounts) {
                    string pat = pa.Replace("{account}", $"{account}");

                    foreach (int index in indices) {
                        string p = pat.Replace("{index}", $"{index}");
                        tree.AddPath(p);
                    }

                    switch (this.GetCoinType()) {
                        case CoinType.ADA:
                        case CoinType.ADALedger:
                        case CoinType.ADATrezor:

                        //  Derive stake key
                        string stakePath = GetStakePath().Replace("{account}", $"{account}");
                        tree.AddPath(stakePath, path.EndsWith("/2/0"));
                        break;
                    }
                }
            }

            return tree;
        }

        public List<Address> GetAddressesList(Phrase phrase, string[] passphrases, string[] paths, int[] accounts, int[] indices) {

            //  Create path tree
            PathTree tree = CreateTree(paths, accounts, indices);

            PathNode path = tree.GetLongestPathFromRoot();
            // Log.Debug($"longest root path: {path.GetPath()} - {String.Join(',', path.GetPathValues())}");

            //  Derive the longest straight branch from root in one go
            DeriveRootLongestPath(new Phrase[] { phrase }, passphrases, path);
            //  Derive all sub-paths from there
            foreach (PathNode child in path.Children) {
                DeriveChildKeys_Batch(child);
            }

            //  Derive descendent keys

            List<Address> addresses = new();

            for (int i = 0; i < passphrases.Length; i++) {
                if (Global.Done) break;

                List<Address> addrs = new();

                DeriveAddressesBatch(tree.Root, i, phrase, passphrases[i], addrs);

                bool found = false;

                foreach (Address address in addrs) {

                    if (Settings.KnownAddresses != null && Settings.KnownAddresses.Length > 0) {
                        foreach (string known in Settings.KnownAddresses) {
                            if (known.Equals(address.address, StringComparison.OrdinalIgnoreCase)) {
                                found = true;
                                break;
                            }
                        }
                    }
                }

                addresses.AddRange(addrs);

                if (found) break;
            }

            return addresses;
        }

        public delegate void ProduceAddress(List<Address> addresses);

        public void ProcessBatchKeys(object thread) {
            int threadNum = (int)thread;
            while (!Global.Done && !batchKeysQueue.IsCompleted) {
                Batch batch = null;

                try {
                    batch = batchKeysQueue.Take();
                }
                catch (InvalidOperationException) {
                    break;
                }

                if (batch == null) continue;

                //  make a deep copy of the original tree
                batch.tree = new PathTree(batch.tree);
                //  Derive the longest straight branch from root in one go
                PathNode path = batch.tree.GetLongestPathFromRoot();
                DeriveRootLongestPath(batch.GetPhrases(), batch.GetPassphrases(), path);

                //  Derive all sub-paths from there
                foreach (PathNode child in path.Children) {
                    DeriveChildKeys_Batch(child);
                }

                try {
                    batchAddressesQueue.Add(batch);
                }
                catch (InvalidOperationException) {
                    break;
                }

                // Log.Debug($"{Thread.CurrentThread.Name}, ProcessBatchKeys, {batchStart}, {Global.sw.ElapsedMilliseconds}");
            }

            if (Global.Done && IsUsingOpenCL()) ocl.Stop();
        }

        public void ProcessBatchAddresses(object thread)
        {
            int threadNum = (int)thread;

            while (!Global.Done && !batchAddressesQueue.IsCompleted) {
                Batch batch = null;

                try {
                    batch = batchAddressesQueue.Take();
                }
                catch (InvalidOperationException) {
                    break;
                }

                if (batch == null) continue;

                long batchStart = Global.sw.ElapsedMilliseconds;
                if (batch is PhraseBatch) {
                    PhraseBatch pb = batch as PhraseBatch;
                    // Log.Debug($"PhraseBatch {pb.phrases.Length}");

                    Parallel.For(0, pb.phrases.Length, GetParallelOptions(), i => {
                    // for (int i = 0; i < pb.phrases.Length; i++) {
                        // if (Global.Done) break;
                        if (Global.Done) return;

                        List<Address> addrs = new();

                        DeriveAddressesBatch(batch.tree.Root, i, pb.phrases[i], pb.passphrase, addrs);

                        if (pb.produceAddress != null) pb.produceAddress(addrs);

                        lock (pb) {
                            pb.count += addrs.Count;
                        }
                    // }
                    });
                }
                else if (batch is PassphraseBatch) {
                    PassphraseBatch pb = batch as PassphraseBatch;
                    // Log.Debug($"PassphraseBatch {pb.passphrases.Length}");

                    Parallel.For(0, pb.passphrases.Length, GetParallelOptions(), i => {
                    // for (int i = 0; i < pb.passphrases.Length; i++) {
                        // if (Global.Done) break;
                        if (Global.Done) return;

                        List<Address> addrs = new();

                        DeriveAddressesBatch(batch.tree.Root, i, pb.phrase, pb.passphrases[i], addrs);

                        if (pb.produceAddress != null) pb.produceAddress(addrs);

                        lock (pb) {
                            pb.count += addrs.Count;
                        }
                    // }
                    });
                }
                // Log.Debug($"{Thread.CurrentThread.Name}, ProcessBatchAddresses, {batchStart}, {Global.sw.ElapsedMilliseconds}");

                // Log.Debug($"{Thread.CurrentThread.Name}, batch {batch.id}, {batchStart}, {Global.sw.ElapsedMilliseconds}");
                
                batchFinished.TryAdd(batch.id, batch);
            }
        }

        public void RetireBatch() {
            //  Update finished results for checkpoint, ensuring batches are retired in sequence

            while (!Global.Done && !allBatchesFinished) {
                
                batchFinished.TryRemove(lastBatchFinished, out Batch b);

                if (b == null) {
                    Thread.Sleep(100);
                    continue;
                }

                lock (mutex) {
                    count += b.count;

                    if (b is PhraseBatch) {
                        PhraseBatch pb = (PhraseBatch)b;
                        lastPhrase = pb.phrases[pb.phrases.Length - 1];
                        lastPhraseNum = lastPhrase.SequenceNum;
                        lastPassphrase = pb.passphrase;

                        //  for progress log
                        totalPhrasesTested += pb.phrases.Length;
                    }
                    else {
                        PassphraseBatch pb = (PassphraseBatch)b;

                        if (pb.passphraseSequenceNum == 0) {
                            passphraseLogTotal = 0;
                            if (lastPhrase != null) totalPhrasesTested += 1;
                        }

                        lastPhrase = pb.phrase;
                        lastPhraseNum = lastPhrase.SequenceNum;
                        lastPassphrase = pb.passphrases[pb.passphrases.Length - 1];
                        lastPassphraseNum = pb.passphraseSequenceNum + pb.passphrases.Length;

                        //  for progress log
                        totalPassphrasesTested += pb.passphrases.Length;

                        // Log.Debug($"Retire batch {b.id} with lassPassphrase=\"{lastPassphrase}\" lastPassphraseNum={lastPassphraseNum}");
                    }

                    lastBatchFinished++;
                }
            }
        }

        //  Phase 1 threads (derive path keys), Phase 2 threads (produce addresses)
        //  Hand tuned values
        protected virtual (int, int) GetTaskCount() { 
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                return (Settings.Threads, Settings.Threads);
            }
            else {
                return (
                    Math.Min(IsUsingOpenCL() ? 1 + 2 * ocl.GetDevicesInUse() : 3, Settings.Threads), 
                    Math.Min(2, Settings.Threads)
                ); 
            }
        }

        protected virtual ParallelOptions GetParallelOptions() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                //  For some reason, Parallel.For performs poorly on Mac
                return new ParallelOptions { MaxDegreeOfParallelism = 1 };
            }
            else {
                return new ParallelOptions();
            }
        }

        public void GetAddressesBatchPassphrases(Phrase phrase, string[] passphrases, long passphraseSequenceNum, PathTree tree, ProduceAddress Produce) {

            if (Global.Done) return;

            //  Enqueue batch

            PassphraseBatch pb = new PassphraseBatch(nextBatchId++, phrase, passphrases, passphraseSequenceNum, tree, Produce);
            
            try {
                batchKeysQueue.Add(pb);
            }
            catch (InvalidOperationException) {
                return;
            }
         }

        public void GetAddressesBatchPhrases(Phrase[] phrases, string passphrase, PathTree tree, ProduceAddress Produce = null) {

            if (Global.Done) return;

            PhraseBatch pb = new PhraseBatch(nextBatchId++, phrases, passphrase, tree, Produce);
            
            try {
                batchKeysQueue.Add(pb);
            }
            catch (InvalidOperationException) {
                return;
            }
        }
        public abstract string[] GetDefaultPaths(string[] knownAddresses);
        public List<Address> GetAddressList(string phrase, string passphrase, int account, int index, string path = null) {
            string[] paths = { path };
            Phrase p = new Phrase(phrase);
            string[] passphrases = { passphrase };
            return GetAddressesList(p, passphrases, account, index, paths);
        }
        public abstract CoinType GetCoinType();
        public void Finish() {
            batchKeysQueue.CompleteAdding();
        }
        private long phraseLogTotal = 0, passphraseLogTotal = 0;

        private int GetPhraseMultiplier() {
            return 1 << (Settings.Phrases[0].Split(' ').Length / 3);
        }
        public void ProgressLog() {
            if (stopWatch.ElapsedMilliseconds == 0) return;
            long done = totalPhrasesTested*GetPhraseMultiplier();
            long phrasesToGo = checkpoint.GetPhraseTotal() - done - phraseStart;
            long currentPhraseNum = lastPhrase != null ? lastPhrase.SequenceNum : 0;
            TimeSpan eta = TimeSpan.MaxValue;

            if (passphraseCount > 1) {
                if (totalPassphrasesTested > passphraseLogTotal) {
                    long ppps = 1000*totalPassphrasesTested/stopWatch.ElapsedMilliseconds;
                    if (ppps != 0) {
                        if (phrasesToGo > 1) {
                            try
                            {
                                eta = TimeSpan.FromSeconds(((phrasesToGo * passphraseCount) - totalPassphrasesTested) / ppps);
                            }
                            catch (Exception)
                            {
                            }
                            Log.Info($"Phrase {currentPhraseNum:n0} / {checkpoint.GetPhraseTotal():n0} ({100.0*currentPhraseNum/checkpoint.GetPhraseTotal():F2}%), Passphrases tested {lastPassphraseNum:n0} / {passphraseCount:n0} ({100.0*lastPassphraseNum/passphraseCount:F2}%), passphrases/s: {ppps:n0}, ETA: {eta}");
                        }
                        else {
                            try
                            {
                                eta = TimeSpan.FromSeconds((passphraseCount - lastPassphraseNum) / ppps);
                            }
                            catch (Exception)
                            {
                            }
                            Log.Info($"Passphrases tested {lastPassphraseNum:n0} / {passphraseCount:n0} ({100.0*lastPassphraseNum/passphraseCount:F2}%), passphrases/s: {ppps:n0}, ETA: {eta}");
                        }
                    }
                    passphraseLogTotal = totalPassphrasesTested;
                }
            }
            else {
                if (totalPhrasesTested > phraseLogTotal) {
                    long pps = 1000*done/stopWatch.ElapsedMilliseconds;
                    if (pps != 0) {
                        if (checkpoint.GetPhraseTotal() > 0) {
                            try
                            {
                                eta = TimeSpan.FromSeconds(phrasesToGo / pps);
                            }
                            catch (Exception)
                            {
                            }
                            Log.Info($"Phrases Tested total: {currentPhraseNum:n0} / {checkpoint.GetPhraseTotal():n0} ({100.0*(done+phraseStart)/checkpoint.GetPhraseTotal():F2}%), phrases/s: {pps:n0}, ETA: {eta}");
                        }
                        else {
                            Log.Info($"Phrases Tested total: {currentPhraseNum:n0} / ???, phrases/s: {pps:n0}");
                        }
                    }
                    phraseLogTotal = totalPhrasesTested;
                }
            }
        }
        public void Consume() {
            Log.Debug("P2A start");

            PathTree tree = CreateTree(Settings.Paths, Settings.Accounts, Settings.Indices);
            PathNode end = tree.GetLongestPathFromRoot();
            Log.Debug($"Longest root path: {end.GetPath()}");

            MultiPassphrase p;
            string passphrase = null;
            phraseStart = checkpoint.GetCheckpointPhrase()?.SequenceNum ?? 0;

            //  Generate passphrase list
            if (this.mp != null && this.mpCount > 0) {
                p = this.mp;
                passphraseCount = this.mpCount;
            }
            else {
                p = new MultiPassphrase(Settings.Passphrases);
                passphraseCount = p.GetCount();
            }

            if (passphraseCount == 1) {
                foreach (string s in p) {
                    passphrase = s;
                    break;
                }
            }

            int batchSize = IsUsingOpenCL() ? ocl.GetBatchSize() : 256;
            Queue<Phrase> phraseBatch = new(batchSize);
            Queue<string> passphraseBatch = new(batchSize);

            //  Start batch threads
            Thread[] t = new Thread[GetTaskCount().Item1];
            for (int i = 0; i < t.Length; i++) {
                t[i] = new Thread(new ParameterizedThreadStart(ProcessBatchKeys));
                t[i].Name = $"ProcessBatchKeys_{i}";
                t[i].Start(i);
            }
            Thread[] t2 = new Thread[GetTaskCount().Item2];
            for (int i = 0; i < t2.Length; i++) {
                t2[i] = new Thread(new ParameterizedThreadStart(ProcessBatchAddresses));
                t2[i].Name = $"ProcessBatchAddresses_{i}";
                t2[i].Start(i);
            }
            Thread retire = new Thread(RetireBatch);
            retire.Name = "RetireBatch";
            retire.Start();

            System.Timers.Timer progressLogger = new System.Timers.Timer(15 * 1000);
            progressLogger.Elapsed += (StringReader, args) => {
                ProgressLog();
            };
            progressLogger.Start();

            var phraseEnumerator = phraseProducer.ProduceWork();

            foreach (var workItem in phraseEnumerator) {

                if (Global.Done) break;

                if (workItem == null) continue;
 
                //  Convert phrase to address

                try {
                    if (!stopWatch.IsRunning) stopWatch.Start();

                    if (passphraseCount > 1) {
                        long passphraseNum = 0;
                        passphraseStart = 0;
                        IEnumerator<string> e = p.GetEnumerator();
                        while (e.MoveNext() && !Global.Done) {
                            string current = e.Current;

                            //  If checkpoint is set, skip passphrases until we reach the checkpoint
                            (string checkpointPassphrase, long num) = checkpoint.GetCheckpointPassphrase();
                            if (checkpointPassphrase != null) {
                                passphraseNum++;
                                if (num == passphraseNum) {
                                    if (checkpointPassphrase == current) {
                                        Log.Info($"Resuming from last checkpoint passphrase: \"{current}\" ({passphraseNum})");
                                        passphraseStart = passphraseNum;
                                        checkpoint.ClearPassphrase();
                                        checkpoint.Start();
                                        stopWatch.Restart();
                                        continue;
                                    }
                                    else {
                                        Log.Error($"Passphrase restore error\nexpect: \"{checkpointPassphrase}\"\ncurrent: \"{current}\"");
                                        FixMyCrypto.PauseAndExit(1);
                                    }
                                }
                                else {
                                    continue;
                                }
                            }

                            passphraseBatch.Enqueue(current);
                            if (passphraseBatch.Count >= batchSize) {
                                string[] passphrases = passphraseBatch.ToArray();
                                passphraseBatch.Clear();
                                GetAddressesBatchPassphrases(workItem, passphrases, passphraseNum, tree, Produce);
                                passphraseNum += passphrases.Length;
                            }
                        }

                        //  Finish remaining in batch
                        if (passphraseBatch.Count > 0 && !Global.Done) {
                            string[] passphrases = passphraseBatch.ToArray();
                            passphraseBatch.Clear();
                            GetAddressesBatchPassphrases(workItem, passphrases, passphraseNum, tree, Produce);
                            passphraseNum += passphrases.Length;
                        }
                    }
                    else {
                        phraseBatch.Enqueue(workItem);
                        if (phraseBatch.Count >= batchSize) {
                            Phrase[] phrases = phraseBatch.ToArray();
                            phraseBatch.Clear();
                            GetAddressesBatchPhrases(phrases, passphrase, tree, Produce);
                        }
                    }
                }
                catch (Exception ex) {
                    Log.Error("P2A error: " + ex.Message);
                    FixMyCrypto.PauseAndExit(1);
                }
             }

            //  End of phrase generation; finish any remaining phrases in queue

            if (phraseBatch.Count > 0) {
                Phrase[] phrases = phraseBatch.ToArray();
                phraseBatch.Clear();
                GetAddressesBatchPhrases(phrases, passphrase, tree, Produce);
            }

            //  finish key generation
            Finish();
            foreach (Thread th in t) th.Join();
            batchAddressesQueue.CompleteAdding();
            foreach (Thread th in t2) th.Join();

            //  finish retiring batches
            while (!Global.Done && batchFinished.Count > 0) {
                Thread.Sleep(100);
            }
            allBatchesFinished = true;
            retire.Join();

            progressLogger.Stop();
            stopWatch.Stop();

            if (count > 0) Log.Info($"P2A done, count: {count:n0} total time: {stopWatch.ElapsedMilliseconds/1000}s, keys/s: {1000*count/stopWatch.ElapsedMilliseconds}, queue wait: {queueWaitTime.ElapsedMilliseconds/1000}s");
        }

        public void Produce(List<Address> addresses) {
            //  See if we generated the known address
            foreach (Address address in addresses) {
                foreach (string knownAddress in Settings.KnownAddresses) {
                    if (address.address.Equals(knownAddress, StringComparison.OrdinalIgnoreCase)) {
                        //  Found known address
                        Finish();

                        FoundResult.DoFoundResult(this.GetCoinType(), address);
                    }
                }
            }
        }
    }
}