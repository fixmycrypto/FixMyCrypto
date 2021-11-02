using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixMyCrypto {
    class Wordlists {
        public static IList<string> OriginalWordlist = NBitcoin.Wordlist.English.GetWords();

        //  Array of word strings, indexed by word index
        public static string[] WordArray = null;

        //  Dictionary of word strings (including invalid words) and their indices
        public static Dictionary<string, short> Wordlist = null;

        //  Lists of word indices by their starting letter a-z
        public static IList<short>[] WordListByLetter = null;

        //  2d Array of word indices and their distances to other words
        public static double[][] WordDistances = null;

        //  Array (indcies) of List of word indices that are within the specified max edit distance
        public static List<short>[] WordsByMaxDistance = null;

        //  Arrays (by indices) of all word indices, sorted by thier distances
        public static short[][] SortedWords = null;
        private static List<short> GetWordsSortedByMaxDistance(short word, double maxDistance) {
            List<(short,double)> words = new List<(short,double)>();

            for (short i = 0; i < WordDistances[word].Length; i++) {
                if (word == i) continue;
                if (WordDistances[word][i] <= maxDistance) {
                    words.Add((i, WordDistances[word][i]));
                }
            }

            words.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            return words.Select(a => a.Item1).ToList();
        }
        public static short GetWordIndex(string word) {
            return Wordlist[word];
        }

        public static string GetWord(short index) {
           return WordArray[index];
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

        public static void Initialize(string[] phrase) {

            if (Wordlist == null) {
                Wordlist = new Dictionary<string, short>();
                for (short i = 0; i < OriginalWordlist.Count; i++) {
                    Wordlist[OriginalWordlist[i]] = i;
                }
            }

            if (WordArray == null) {
                WordArray = OriginalWordlist.ToArray();
            }

            if (phrase == null) return;

            Log.Debug($"Generating word tables...");

            int originalWordCount = OriginalWordlist.Count;
            int allWordCount = originalWordCount;
            List<string> invalidWords = new List<string>();
            foreach (string word in phrase) {
                if (!OriginalWordlist.Contains(word) && !invalidWords.Contains(word)) {
                    invalidWords.Add(word);
                }
            }
            allWordCount += invalidWords.Count;

            //  Create word lists per letter
            if (WordListByLetter == null) {
                WordListByLetter = new List<short>[26];
                for (char l = 'a'; l <= 'z'; l++) {
                    WordListByLetter[l - 'a'] = new List<short>();
                }
                foreach (string word in WordArray) {
                    WordListByLetter[word[0] - 'a'].Add(GetWordIndex(word));
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
            if (WordDistances == null) {
                WordDistances = new double[allWordCount][];

                Parallel.ForEach(Wordlist.Values, word => {
                    WordDistances[word] = new double[originalWordCount];

                    foreach (short w2 in Wordlist.Values) {
                        if (word == w2) {
                            WordDistances[word][w2] = 0;
                            continue;
                        }

                        WordDistances[word][w2] = KeyboardDistance.GetKeyboardWeightedDamerauLevenshteinDistance(WordArray[word], WordArray[w2]);
                    }
                });

                // Also need distances from invalid words to valid words
                foreach (string word in phrase) {
                    if (!Wordlist.ContainsKey(word)) {
                        short wordIndex = (short)Wordlist.Count;
                        Wordlist[word] = wordIndex;
                        Array.Resize(ref WordArray, WordArray.Length + 1);
                        WordArray[wordIndex] = word;

                        WordDistances[wordIndex] = new double[originalWordCount];

                        foreach (string w2 in OriginalWordlist) {
                            if (word == w2) {
                                WordDistances[wordIndex][Wordlist[w2]] = 0;
                                continue;
                            }
                            WordDistances[wordIndex][Wordlist[w2]] = KeyboardDistance.GetKeyboardWeightedDamerauLevenshteinDistance(word, w2);
                        }
                    }
                }
            }

            Rhymes rhymes = new Rhymes();

            //  Create list of closest words by max distance
            if (WordsByMaxDistance == null) {
                WordsByMaxDistance = new List<short>[allWordCount];

                Parallel.ForEach(Wordlist.Values, word => {
                // foreach (var word in wordlist.Values) {
                    WordsByMaxDistance[word] = GetWordsSortedByMaxDistance(word, Settings.WordDistance);

                    //  Add rhymes / sounds like from table

                    List<string> soundsLike = rhymes.GetWords(WordArray[word]);

                    foreach (string w in soundsLike) {
                        short ix = Wordlist[w];
                        if (!WordsByMaxDistance[word].Contains(ix)) WordsByMaxDistance[word].Add(ix);
                    }
                });
                // }

                //  debug
                
                int total = 0;
                for (int i = 0; i < WordsByMaxDistance.Length; i++) {
                    
                    List<short> l = WordsByMaxDistance[i];
                    if (l.Count == 0) continue;
                    string words = "";
                    foreach (short w2 in l) {
                        words += " " + WordArray[w2] + $"({WordDistances[i][w2]:F2})";
                        total++;
                    }
                    if (phrase.Contains(WordArray[i])) Log.Debug($"{WordArray[i]}:{words}");
                }
                Log.Debug($"Average # of similar words: {(double)total/WordsByMaxDistance.Length:F1}");
                
            }

            //  Create arrays of word indices sorted by their distances 
            if (SortedWords == null) {
                SortedWords = new short[WordDistances.Length][];

                Parallel.For(0, WordDistances.Length, wordIndex => {
                    SortedWords[wordIndex] = SortAndIndex<double>(WordDistances[wordIndex]);

                    //  Don't include the original word itself in the list
                    if (SortedWords[wordIndex][0] == wordIndex) SortedWords[wordIndex] = SortedWords[wordIndex].Slice(1);

                    try {
                        if (OriginalWordlist.Contains(WordArray[wordIndex])) {
                            //  for words in wordlist, sorted list should contain count - 1
                            if (SortedWords[wordIndex].Length != OriginalWordlist.Count - 1) throw new Exception($"{SortedWords[wordIndex].Length} != {OriginalWordlist.Count - 1}");
                        }
                        else {
                            //  for words not in wordlist, sorted list should contain count
                            if (SortedWords[wordIndex].Length != OriginalWordlist.Count) throw new Exception($"{SortedWords[wordIndex].Length} != {OriginalWordlist.Count}");
                        }
                    }
                    catch (Exception) {
                        //  debug
                        string words = "";
                        for (short j = 0; j < SortedWords[wordIndex].Length; j++) words += $" {WordArray[SortedWords[wordIndex][j]]}";
                        Log.Debug($"{WordArray[wordIndex]}:{words}");

                        throw;
                    }
                });
            }
        }
    }
}