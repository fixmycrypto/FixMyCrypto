using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBitcoin;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace FixMyCrypto {
    class PhraseProducer {
        public ConcurrentQueue<Work> queue;
        int threadNum, threadMax, internalThreads;
        int valid = 0, invalid = 0, dupes = 0;
        string[] phrase;
        static IList<string> originalWordlist = Wordlist.English.GetWords();

        //  Array of word strings, indexed by word index
        public static string[] wordArray = null;

        //  Dictionary of word strings (including invalid words) and their indices
        public static Dictionary<string, short> wordlist = null;

        //  Lists of word indices by their starting letter a-z
        static IList<short>[] wordListByLetter = null;

        //  2d Array of word indices and their distances to other words
        static double[][] wordDistances = null;

        //  Array (indcies) of List of word indices that are within the specified max edit distance
        static List<short>[] wordsByMaxDistance = null;

        //  Arrays (by indices) of all word indices, sorted by thier distances
        static short[][] sortedWords = null;
        static Object mutex = new Object();
        enum SwapMode {
            SameLetter,
            Similar,
            AnyLetter
        }
        private ParallelOptions parallelOptions;
        Stopwatch queueWaitTime = new Stopwatch();

        private static List<short> GetWordsSortedByMaxDistance(short word, double maxDistance) {
            List<(short,double)> words = new List<(short,double)>();

            for (short i = 0; i < wordDistances[word].Length; i++) {
                if (word == i) continue;
                if (wordDistances[word][i] <= maxDistance) {
                    words.Add((i, wordDistances[word][i]));
                }
            }

            words.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            return words.Select(a => a.Item1).ToList();
        }
        public static short GetWordIndex(string word) {
            return wordlist[word];
        }

        public static string GetWord(short index) {
           return wordArray[index];
        }
        public PhraseProducer(ConcurrentQueue<Work> queue, int threadNum, int threadMax, string[] phrase) {
            this.queue = queue;
            this.threadNum = threadNum;
            this.threadMax = threadMax;
            this.phrase = phrase;
            this.internalThreads = Settings.threads / 4;

            lock(mutex) {

                if (wordlist == null) {
                    wordlist = new Dictionary<string, short>();
                    for (short i = 0; i < originalWordlist.Count; i++) {
                        wordlist[originalWordlist[i]] = i;
                    }
                }

                if (wordArray == null) {
                    wordArray = originalWordlist.ToArray();
                }

                if (this.phrase == null) return;

                Log.Debug($"PP{threadNum} Generating word tables...");

                int originalWordCount = originalWordlist.Count;
                int allWordCount = originalWordCount;
                List<string> invalidWords = new List<string>();
                foreach (string word in this.phrase) {
                    if (!originalWordlist.Contains(word) && !invalidWords.Contains(word)) {
                        invalidWords.Add(word);
                    }
                }
                allWordCount += invalidWords.Count;

                //  Create word lists per letter
                if (wordListByLetter == null) {
                    wordListByLetter = new List<short>[26];
                    for (char l = 'a'; l <= 'z'; l++) {
                        wordListByLetter[l - 'a'] = new List<short>();
                    }
                    foreach (string word in wordArray) {
                        wordListByLetter[word[0] - 'a'].Add(GetWordIndex(word));
                    }

                    //  Debug
                    /*
                    for (char letter = 'a'; letter <= 'z'; letter++) {
                        IList<short> words = wordListByLetter[letter - 'a'];
                        string w = "";
                        foreach (short word in words) {
                            w += $" {wordArray[word]}";
                        }
                        Log.Debug($"Letter {letter}:{w}");
                    }
                    */
                }

                //  Array of distances from each word index to each word index
                if (wordDistances == null) {
                    wordDistances = new double[allWordCount][];

                    Parallel.ForEach(wordlist.Values, word => {
                        wordDistances[word] = new double[originalWordCount];

                        foreach (short w2 in wordlist.Values) {
                            if (word == w2) {
                                wordDistances[word][w2] = 0;
                                continue;
                            }

                            wordDistances[word][w2] = KeyboardDistance.GetKeyboardWeightedDamerauLevenshteinDistance(wordArray[word], wordArray[w2]);
                        }
                    });

                    // Also need distances from invalid words to valid words
                    foreach (string word in this.phrase) {
                        if (!wordlist.ContainsKey(word)) {
                            short wordIndex = (short)wordlist.Count;
                            wordlist[word] = wordIndex;
                            Array.Resize(ref wordArray, wordArray.Length + 1);
                            wordArray[wordIndex] = word;

                            wordDistances[wordIndex] = new double[originalWordCount];

                            foreach (string w2 in originalWordlist) {
                                if (word == w2) {
                                    wordDistances[wordIndex][wordlist[w2]] = 0;
                                    continue;
                                }
                                wordDistances[wordIndex][wordlist[w2]] = KeyboardDistance.GetKeyboardWeightedDamerauLevenshteinDistance(word, w2);
                            }
                        }
                    }
                }

                //  Create list of closest words by max distance
                if (wordsByMaxDistance == null) {
                    wordsByMaxDistance = new List<short>[allWordCount];

                    Parallel.ForEach(wordlist.Values, word => {
                        wordsByMaxDistance[word] = GetWordsSortedByMaxDistance(word, Settings.wordDistance);
                    });

                    //  debug
                    
                    int total = 0;
                    for (int i = 0; i < wordsByMaxDistance.Length; i++) {
                        
                        List<short> l = wordsByMaxDistance[i];
                        if (l.Count == 0) continue;
                        string words = "";
                        foreach (short w2 in l) {
                            words += " " + wordArray[w2] + $"({wordDistances[i][w2]:F2})";
                            total++;
                        }
                        if (this.phrase.Contains(wordArray[i])) Log.Debug($"{wordArray[i]}:{words}");
                    }
                    Log.Debug($"Average # of similar words: {(double)total/wordsByMaxDistance.Length:F1}");
                    
                }

                //  Create arrays of word indices sorted by their distances 
                if (sortedWords == null) {
                    sortedWords = new short[wordDistances.Length][];

                    Parallel.For(0, wordDistances.Length, wordIndex => {
                        sortedWords[wordIndex] = SortAndIndex<double>(wordDistances[wordIndex]);

                        //  Don't include the original word itself in the list
                        if (sortedWords[wordIndex][0] == wordIndex) sortedWords[wordIndex] = sortedWords[wordIndex].Slice(1);

                        try {
                            if (originalWordlist.Contains(wordArray[wordIndex])) {
                                //  for words in wordlist, sorted list should contain count - 1
                                if (sortedWords[wordIndex].Length != originalWordlist.Count - 1) throw new Exception($"{sortedWords[wordIndex].Length} != {originalWordlist.Count - 1}");
                            }
                            else {
                                //  for words not in wordlist, sorted list should contain count
                                if (sortedWords[wordIndex].Length != originalWordlist.Count) throw new Exception($"{sortedWords[wordIndex].Length} != {originalWordlist.Count}");
                            }
                        }
                        catch (Exception) {
                            //  debug
                            string words = "";
                            for (short j = 0; j < sortedWords[wordIndex].Length; j++) words += $" {wordArray[sortedWords[wordIndex][j]]}";
                            Log.Debug($"{wordArray[wordIndex]}:{words}");

                            throw;
                        }
                    });
                }
            }
        }
        
        //  https://stackoverflow.com/a/46068115
        /// sort array 'rg', returning the original index positions
        static short[] SortAndIndex<T>(T[] rg)
        {
            short i; 
            int c = rg.Length;
            var keys = new short[c];
            if (c > 1)
            {
                for (i = 0; i < c; i++)
                    keys[i] = i;

                System.Array.Sort(rg, keys /*, ... */);
            }
            return keys;
        }
        public void Finish() {
            Global.done = true;
            lock(queue) { Monitor.PulseAll(queue); }
        }
        private static HashSet<Phrase> testedPhrases = new HashSet<Phrase>();
        private void TestPhrase(short[] phraseArray) {
            Phrase p = new Phrase(phraseArray);
            if (testedPhrases.Contains(p)) {
                dupes++;
                return;
            }
            lock (testedPhrases) {
                testedPhrases.Add(p);
            }

            //  Check if phrase has valid BIP39 checksum

            bool isValid = VerifyChecksum(phraseArray);
            
            if (isValid) {
                valid++;
                Work w = new Work(p, null);

                lock (queue) {
                    queueWaitTime.Start();
                    while (queue.Count > Settings.threads) {
                        if (Global.done) break;
                        //Log.Debug("PP thread " + threadNum + " waiting on full queue");
                        Monitor.Wait(queue);
                    }
                    queueWaitTime.Stop();

                    queue.Enqueue(w);
                    //Log.Debug("PP thread " + threadNum + " enqueued work: \"" + w + "\", queue size: " + queue.Count);
                    Monitor.Pulse(queue);
                }
            }
            else {
                invalid++;
            }
        }

        private void SwapTwo(short[] phrase) {
            // for (int i = 0; i < phrase.Length - 1; i++) {
            Parallel.For(0, phrase.Length, this.parallelOptions, i => {

                for (int j = i + 1; j < phrase.Length; j++) {
                    short[] p = phrase.Copy();

                    short temp = p[i];
                    p[i] = p[j];
                    p[j] = temp;

                    TestPhrase(p);
                    if (Global.done) return;
                }
            });
        }
        private void SwapThree(short[] phrase) {
            // for (int i = 0; i < phrase.Length - 1; i++) {
            Parallel.For(0, phrase.Length, this.parallelOptions, i => {
                for (int j = i + 1; j < phrase.Length; j++) {
                    
                    // Range range = GetRange(j + 1);

                    // for (int k = range.Start.Value; k < range.End.Value; k++) {
                    for (int k = j + 1; k < phrase.Length; k++) {
                        short[] p = phrase.Copy();

                        //  Swap ijk - jki
                        short temp = p[i];
                        p[i] = p[j];
                        p[j] = p[k];
                        p[k] = temp;

                        TestPhrase(p);
                        if (Global.done) return;

                        //  Swap ijk - kij
                        p = phrase.Copy();

                        temp = p[i];
                        p[i] = p[k];
                        p[j] = temp;
                        p[k] = p[j];

                        TestPhrase(p);
                        if (Global.done) return;                      
                    }
                }
            });
        }
        private void SwapFour(short[] phrase) {
            // for (int i = 0; i < phrase.Length - 1; i++) {
            Parallel.For(0, phrase.Length, this.parallelOptions, i => {
                for (int j = i + 1; j < phrase.Length; j++) {
                    for (int k = j + 1; k < phrase.Length; k++) {

                        // Range range = GetRange(k + 1);

                        // for (int l = range.Start.Value; l < range.End.Value; l++) {
                        for (int l = k + 1; l < phrase.Length; l++) {
                            short[] p = phrase.Copy();

                            //  Swap ijkl - jkli
                            short temp = p[i];
                            p[i] = p[j];
                            p[j] = p[k];
                            p[k] = p[l];
                            p[l] = temp;

                            TestPhrase(p);
                            if (Global.done) return;

                            //  Swap ijkl - klij
                            p = phrase.Copy();
                            temp = p[i];
                            p[i] = p[k];
                            p[l] = p[j];
                            p[j] = p[l];
                            p[k] = temp;

                            TestPhrase(p);
                            if (Global.done) return;

                            //  Swap ijkl - lijk
                            p = phrase.Copy();
                            temp = p[i];
                            p[i] = p[l];
                            p[l] = p[k];
                            p[k] = p[j];
                            p[j] = temp;

                            TestPhrase(p);
                            if (Global.done) return;
                        }             
                    }
                }
            });
        }
        private Range GetRange(int first) {
            double numPerThread = (phrase.Length - first)/(double)threadMax;
            int start = first + (int)Math.Ceiling(numPerThread * threadNum);
            int end = first + (int)Math.Ceiling(numPerThread * (threadNum+1));

            return new Range(start, end);           
        }
        private IList<short> GetReplacementWords(short word, SwapMode swapMode) {
            IList<short> words;

            switch (swapMode) {
                case SwapMode.Similar:
                words = wordsByMaxDistance[word];
                break;

                case SwapMode.AnyLetter:
                words = sortedWords[word];
                break;

                case SwapMode.SameLetter:
                default:
                char letter = wordArray[word][0];
                words = wordListByLetter[letter-'a'];
                break;
            }

            return words;
        }
        private void SwapTwoCOW(short[] phrase, SwapMode swapMode) {
            //  for (int i = 0; i < phrase.Length - 1; i++) {
            Parallel.For(0, phrase.Length, this.parallelOptions, i => {
                if (Global.done) return;

                if (i > 0) Log.Info($"PP{threadNum} S2C1W ({swapMode}) progress: {(100*i/phrase.Length)}%");
                
                // Range range = GetRange(i + 1);

                // for (int j = range.Start.Value; j < range.End.Value; j++) {
                for (int j = i + 1; j < phrase.Length; j++) {
                    short[] swapped = phrase.Copy();

                    short temp = swapped[i];
                    swapped[i] = swapped[j];
                    swapped[j] = temp;

                    /*  already tested by SwapTwo
                    //  Test swapped only
                    TestPhrase(swapped);
                    if (Global.done) return;
                    */

                    for (int k = 0; k < phrase.Length; k++) {
                        IList<short> words = GetReplacementWords(phrase[k], swapMode);

                        foreach (short word in words) {
                            //  COW on swapped
                            if (swapped[k] != word) {
                                short[] cow_swapped = swapped.Copy();
                                cow_swapped[k] = word;

                                TestPhrase(cow_swapped);
                                if (Global.done) return;
                            }
                        } 
                    }
                }
            });
        }

        private void ChangeWords(short[] phrase, int depth, SwapMode swapMode, List<int> skip = null) {
            if (depth == 0) return;

            int start = 0;
            int end = phrase.Length;

            if (skip == null) {
                //  outermost loop
                start = (int)Math.Ceiling((phrase.Length / (double)threadMax) * threadNum);
                end = (int)Math.Ceiling((phrase.Length / (double)threadMax) * (threadNum + 1));
            }

            Parallel.For(start, end, this.parallelOptions, i => {
                if (Global.done) return;
            // for (int i = start; i < end; i++) {
                if (skip != null && skip.Contains(i)) return;

                if (i > start && skip == null) Log.Info($"PP{threadNum} C{depth}W ({swapMode}) progress: {(100*(i-start)/(end-start))}%");

                IList<short> words = GetReplacementWords(phrase[i], swapMode);

                foreach (short word in words) {
                    if (word == phrase[i]) continue;

                    short[] cow = phrase.Copy();
                    cow[i] = word;

                    if (depth > 1) {
                        List<int> skip2 = new List<int>();
                        if (skip != null) skip2.AddRange(skip);
                        skip2.Add(i);

                        //  test recursive swaps
                        ChangeWords(cow, depth - 1, swapMode, skip2);
                    }
                    else {
                        //  test after current swap
                        TestPhrase(cow);
                        if (Global.done) return;
                    }
                }
            });
        }

        private void ChangeWordsSwap(short[] phrase, int depth, SwapMode swapMode, List<int> skip = null) {
            if (depth == 0) return;

            int start = 0;
            int end = phrase.Length;

            if (skip == null) {
                //  outermost loop
                start = (int)Math.Ceiling((phrase.Length / (double)threadMax) * threadNum);
                end = (int)Math.Ceiling((phrase.Length / (double)threadMax) * (threadNum + 1));
            }

            Parallel.For(start, end, this.parallelOptions, i => {
            // for (int i = start; i < end; i++) {
                if (Global.done) return;
                if (skip != null && skip.Contains(i)) return;

                if (i > start && skip == null) Log.Info($"PP{threadNum} C{depth}W+S ({swapMode}) progress: {(100*(i-start)/(end-start))}%");

                IList<short> words = GetReplacementWords(phrase[i], swapMode);

                foreach (short word in words) {
                    if (word == phrase[i]) continue;

                    short[] cow = phrase.Copy();
                    cow[i] = word;

                    if (depth > 1) {
                        List<int> skip2 = new List<int>();
                        if (skip != null) skip2.AddRange(skip);
                        skip2.Add(i);

                        //  test recursive swaps
                        ChangeWordsSwap(cow, depth - 1, swapMode, skip2);
                    }
                    else {
                        //  test after current swap
                        TestPhrase(cow);
                        if (Global.done) return;

                        //  plus 1 swap

                        for (int j = 0; j < cow.Length - 1; j++) {
                            for (int k = j + 1; k < cow.Length; k++) {
                                short[] cowswap = cow.Copy();

                                short temp = cowswap[j];
                                cowswap[j] = cowswap[k];
                                cowswap[k] = temp;

                                TestPhrase(cowswap);
                                if (Global.done) return;
                            }
                        }
                    }
                }
            });
        }

        private void SwapColumns(short[] phrase) {
            if (phrase.Length % 2 != 0) return;

            short[] p = new short[phrase.Length];

            //  Swap two columns i.e. 6 7 8 9 10 11 0 1 2 3 4 5

            for (int i = 0; i < phrase.Length / 2; i++) {
                p[i] = phrase[i + phrase.Length / 2];
            }

            for (int i = 0; i < phrase.Length / 2; i++) {
                p[i + phrase.Length / 2] = phrase[i];
            }

            TestPhrase(p);
            if (Global.done) return;

            // Swap 2 col x N/2 rows, i.e. 0 6 1 7 2 8 3 9 4 10 5 11

            p = new short[phrase.Length];

            for (int i = 0; i < phrase.Length / 2; i++) {
                p[i] = phrase[2 * i];

                p[i + phrase.Length / 2] = phrase[2 * i + 1];
            }

            TestPhrase(p);
            if (Global.done) return;
        }

        private void FixInvalid(short[] phrase, int depth, int maxDepth, bool runAlgorithms, SwapMode mode, int difficulty, int threads) {
            if (Global.done) return;

            if (depth == 0) {
                TestPhrase(phrase);
                if (Global.done) return;

                if (runAlgorithms) RunAlgorithms(phrase, difficulty, 1);
                
                return;
            }

            for (int i = 0; i < phrase.Length; i++) {
                if (Global.done) return;

                if (phrase[i] < originalWordlist.Count) continue;

                IList<short> words = GetReplacementWords(phrase[i], mode);

                // int attempt = 0;

                ParallelOptions po = new ParallelOptions();
                po.MaxDegreeOfParallelism = threads;

                // Log.Info($"PP{threadNum}: Replace invalid word #{depth} ({words.Count})");

                Parallel.ForEach(words, po, replacement => {
                // foreach (int replacement in words) {
                    // attempt++;

                    if (Global.done) return;

                    short[] fix = phrase.Copy();

                    // Log.Info($"PP{threadNum}: Replace invalid word {fix[i]} with {replacement} ({attempt}/{words.Count})");

                    fix[i] = replacement;

                    FixInvalid(fix, depth - 1, maxDepth, runAlgorithms, mode, difficulty, 1);
                });
            }
        }
        private void RunAlgorithms(short[] phrase, int difficulty, int threads) {
            // Log.Debug($"RunAlgorithms on phrase: {String.Join(' ', phrase)}");

            if (Global.done) return;

            parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = threads;

            Stopwatch sw2 = new Stopwatch();

            //  instant
            Log.Info($"PP{threadNum}: Swap columns");
            sw2.Start();
            SwapColumns(phrase);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Swap columns finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            //  <1s
            Log.Info($"PP{threadNum}: Swap any 2");
            sw2.Start();
            SwapTwo(phrase);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Swap any 2 finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            //  3s
            Log.Info($"PP{threadNum}: Change 1 word (same letter)");
            sw2.Restart();
            ChangeWords(phrase, 1, SwapMode.SameLetter);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Change 1 word (same letter) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            //  7s
            Log.Info($"PP{threadNum}: Swap any 3");
            sw2.Start();
            SwapThree(phrase);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Swap any 3 finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            //  40s
            Log.Info($"PP{threadNum}: Swap any 4");
            sw2.Start();
            SwapFour(phrase);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Swap any 4 finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            //  1 min
            Log.Info($"PP{threadNum}: Change 1 word (any letter)");
            sw2.Restart();
            ChangeWords(phrase, 1, SwapMode.AnyLetter);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Change 1 word (any letter) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            //  2 mins for 12
            Log.Info($"PP{threadNum}: Swap 2, Change 1 (distance = {Settings.wordDistance})");
            sw2.Restart();
            SwapTwoCOW(phrase, SwapMode.Similar);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Swap 2, Change 1 (distance = {Settings.wordDistance}) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            //  2-3mins for 12
            Log.Info($"PP{threadNum}: Change 2 words (distance = {Settings.wordDistance})");
            sw2.Restart();
            ChangeWords(phrase, 2, SwapMode.Similar);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Change 2 words (distance = {Settings.wordDistance}) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            if (difficulty < 1) return;

            //  Advanced modes
            //  ?
            Log.Info($"PP{threadNum}: Swap 2, Change 2 (distance = {Settings.wordDistance})");
            sw2.Restart();
            ChangeWordsSwap(phrase, 2, SwapMode.Similar);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Swap 2, Change 2 (distance = {Settings.wordDistance}) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            //  10 mins
            Log.Info($"PP{threadNum}: Swap 2, Change 1 (same letter)");
            sw2.Restart();
            SwapTwoCOW(phrase, SwapMode.SameLetter);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Swap 2, Change 1 (same letter) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            //  2hrs
            Log.Info($"PP{threadNum}: Change 2 words (same letter)");
            sw2.Restart();
            ChangeWords(phrase, 2, SwapMode.SameLetter);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Change 2 words (same letter) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            //  3hrs
            Log.Info($"PP{threadNum}: Swap 2, Change 1 (any letter)");
            sw2.Restart();
            SwapTwoCOW(phrase, SwapMode.AnyLetter);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Swap 2, Change 1 (any letter) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            //  3-4hrs for 10
            Log.Info($"PP{threadNum}: Change 3 words (distance = {Settings.wordDistance})");
            sw2.Restart();
            ChangeWords(phrase, 3, SwapMode.Similar);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Change 3 words (distance = {Settings.wordDistance}) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;
            
            if (difficulty < 2) return;

            //  SUPER ADVANCED MODE
            //  Long time?
            Log.Info($"PP{threadNum}: Change 4 words (distance = {Settings.wordDistance})");
            sw2.Restart();
            ChangeWords(phrase, 4, SwapMode.Similar);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Change 4 words (distance = {Settings.wordDistance}) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            if (difficulty < 3) return;

            //  60 days?
            Log.Info($"PP{threadNum}: Change 2 words (any letter)");
            sw2.Restart();
            ChangeWords(phrase, 2, SwapMode.AnyLetter);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Change 2 words (any letter) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;

            if (difficulty < 4) return;

            //  crazy time
            Log.Info($"PP{threadNum}: Change 3 words (any letter)");
            sw2.Restart();
            ChangeWords(phrase, 3, SwapMode.AnyLetter);
            sw2.Stop();
            Log.Info($"PP{threadNum}: Change 3 words (any letter) finished in {sw2.ElapsedMilliseconds/1000}s valid: {valid} invalid: {invalid} dupes: {dupes}");
            if (Global.done) return;
        }
    
        public void ProduceWork() {
            Log.Debug("PP" + threadNum + " start");

            int wrongWords = 0;
            foreach (string word in this.phrase) {
                if (!originalWordlist.Contains(word)) {
                    wrongWords++;
                    Log.Debug($"invalid word {wrongWords}: {word}");
                }
            }

            Phrase p = new Phrase(this.phrase);
            short[] phrase = p.Indices;

            Stopwatch sw1 = new Stopwatch();
            sw1.Start();

            if (wrongWords == 0) {
                TestPhrase(phrase);

                //  Run all algorithms with a bit extra difficulty

                if (!Global.done) RunAlgorithms(phrase, Settings.difficulty + 1, this.internalThreads);
            }
            else {
                //  Try fixing invalid words only without any swaps/changes, starting with similar words
                Log.Info($"PP{threadNum} replace {wrongWords} invalid words with similar words");
                FixInvalid(phrase, wrongWords, wrongWords, false, SwapMode.Similar, Settings.difficulty, this.internalThreads);

                //  Try fixing invalid words plus swaps/changes
                if (!Global.done) {
                    Log.Info($"PP{threadNum} replace {wrongWords} invalid words with similar words + swaps/changes");
                    FixInvalid(phrase, wrongWords, wrongWords, true, SwapMode.Similar, Settings.difficulty, this.internalThreads);
                }

                //  Try any substitute for invalid words (no swaps/changes) if practical
                if (!Global.done && wrongWords < 3) {
                    Log.Info($"PP{threadNum} replace {wrongWords} invalid words with any words");
                    FixInvalid(phrase, wrongWords, wrongWords, false, SwapMode.AnyLetter, Settings.difficulty, this.internalThreads);
                }

                //  Last ditch effort
                if (!Global.done) {
                    Log.Info($"PP{threadNum} replace {wrongWords} invalid words with any words + swaps/changes");
                    FixInvalid(phrase, wrongWords, wrongWords, true, SwapMode.AnyLetter, Settings.difficulty + 1, this.internalThreads);
                }
            }

            sw1.Stop();
            Log.Info("PP" + threadNum + " done, valid: " + valid + " invalid: " + invalid + $", dupes: {dupes}, total time: {sw1.ElapsedMilliseconds/1000.0:F2}s, time/req: {((valid + invalid != 0) ? ((double)sw1.ElapsedMilliseconds/(valid+invalid)) : 0):F3}ms/req, queue wait: " + queueWaitTime.ElapsedMilliseconds/1000 + "s");
            Finish();
        }

        public bool VerifyChecksum(short[] indices) {
            // Compute and check checksum
            int MS = indices.Length;
            int ENTCS = MS * 11;
            int CS = ENTCS % 32;
            int ENT = ENTCS - CS;

            var entropy = new byte[ENT / 8];

            int itemIndex = 0;
            int bitIndex = 0;
            // Number of bits in a word
            int toTake = 8;
            // Indexes are held in a UInt32 but they are only 11 bits
            int maxBits = 11;
            for (int i = 0; i < entropy.Length; i++)
            {
                if (bitIndex + toTake <= maxBits)
                {
                    // All 8 bits are in one item

                    // To take 8 bits (*) out of 00000000 00000000 00000xx* *******x:
                    // 1. Shift right to get rid of extra bits on right, then cast to byte to get rid of the rest
                    // >> maxBits - toTake - bitIndex
                    entropy[i] = (byte)(indices[itemIndex] >> (3 - bitIndex));
                }
                else
                {
                    // Only a part of 8 bits are in this item, the rest is in the next.
                    // Since items are only 32 bits there is no other possibility (8<32)

                    // To take 8 bits(*) out of [00000000 00000000 00000xxx xxxx****] [00000000 00000000 00000*** *xxxxxxx]:
                    // Take first item at itemIndex [00000000 00000000 00000xxx xxxx****]: 
                    //    * At most 7 bits and at least 1 bit should be taken
                    // 1. Shift left [00000000 00000000 0xxxxxxx ****0000] (<< 8 - (maxBits - bitIndex)) 8-max+bi
                    // 2. Zero the rest of the bits (& (00000000 00000000 00000000 11111111))

                    // Take next item at itemIndex+1 [00000000 00000000 00000*** *xxxxxxx]
                    // 3. Shift right [00000000 00000000 00000000 0000****]
                    // number of bits already taken = maxBits - bitIndex
                    // nuber of bits to take = toTake - (maxBits - bitIndex)
                    // Number of bits on the right to get rid of= maxBits - (toTake - (maxBits - bitIndex))
                    // 4. Add two values to each other using bitwise OR [****0000] | [0000****]
                    entropy[i] = (byte)(((indices[itemIndex] << (bitIndex - 3)) & 0xff) |
                                         (indices[itemIndex + 1] >> (14 - bitIndex)));
                }

                bitIndex += toTake;
                if (bitIndex >= maxBits)
                {
                    bitIndex -= maxBits;
                    itemIndex++;
                }
            }

            // Compute and compare checksum:
            // CS is at most 8 bits and it is the remaining bits from the loop above and it is only from last item
            // [00000000 00000000 00000xxx xxxx****]
            // We already know the number of bits here: CS
            // A simple & does the work
            uint mask = (1U << CS) - 1;
            byte expectedChecksum = (byte)(indices[itemIndex] & mask);

            // Checksum is the "first" CS bits of hash: [****xxxx]

            using SHA256 hash = SHA256.Create();
            byte[] hashOfEntropy = hash.ComputeHash(entropy);
            byte actualChecksum = (byte)(hashOfEntropy[0] >> (8 - CS));

            return (expectedChecksum == actualChecksum);
        }
    }
}