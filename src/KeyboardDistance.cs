using System;
using System.Collections.Concurrent;

namespace FixMyCrypto {

    public static class KeyboardDistance {

        private static double insertCost = 1.0;
        private static double deleteCost = 1.0;
        private static double transposeCost = 0.75;
        private static double substitutionCostBase = 0.6;
        private static double keyboardDistanceCostMultiplier = 0.16;
        private static double vowelSubstitutionCostMultiplier = 0.33;

        //  Make mistakes towards the beginning of the word more expensive
        private static double costFalloff = 0.82;

        //  Shift, Row, string index = col
        //  ü = No key
        private static string[,] qwertyMap = { {
            "`1234567890-=",

            "üqwertyuiop[]\\",

            "üasdfghjkl;'ü",

            "üzxcvbnm,./üü",

            "üüü      üüüü"
        }, {
            "~!@#$%^&*()_+",

            "üQWERTYUIOP{}|",

            "üASDFGHJKL:\"üü",

            "üZXCVBNM<>?üü",

            "üüü      üüüü"
        } };
        private static ConcurrentDictionary<(char, char), double> characterCache = new ConcurrentDictionary<(char, char), double>();
        private static ConcurrentDictionary<(string, string), double> distanceCache = new ConcurrentDictionary<(string, string), double>();

        private static double GetDistance((int, int, int) a, (int, int, int) b) {
            return Math.Sqrt(Math.Pow(a.Item1 - b.Item1, 2) + Math.Pow(a.Item2 - b.Item2, 2) + Math.Pow(a.Item3 - b.Item3, 2));
        }
        public static (int s, int x, int y) GetKeyboardLocation(char c) {
            for (int shift = 0; shift <= 1; shift++) {
                for (int row = 0; row < qwertyMap.GetLength(1); row++) {
                    for (int col = 0; col < qwertyMap[shift,row].Length; col++) {
                        if (qwertyMap[shift,row][col] == c) {
                            return (shift, row, col);
                        }
                    }
                }
            }
            return (-1, -1, -1);
        }
        private static string vowels = "aeiouy";
        private static bool IsVowel(char c) {
            return vowels.Contains(Char.ToLower(c));
        }
        public static double GetCharacterDistance(char c1, char c2) {
            if (c1 == c2) return 0;
            if (characterCache.ContainsKey((c1, c2))) return characterCache[(c1, c2)];
            if (characterCache.ContainsKey((c2, c1))) return characterCache[(c2, c1)];

            var l1 = GetKeyboardLocation(c1);
            var l2 = GetKeyboardLocation(c2);

            if (l1 == (-1, -1, -1) || l2 == (-1, -1, -1)) return -1;

            Double distance = GetDistance(l1, l2);

            //  consider vowel swaps more likely despite distance

            if (IsVowel(c1) && IsVowel(c2)) distance *= vowelSubstitutionCostMultiplier;

            characterCache[(c1, c2)] = distance;

            return distance;
        }
        public static double GetKeyboardWeightedDamerauLevenshteinDistance(string s, string t) {
            if (s == t) return 0;

            if (distanceCache.ContainsKey((s,t))) return distanceCache[(s,t)];
            if (distanceCache.ContainsKey((t,s))) return distanceCache[(t,s)];

            var bounds = new { Height = s.Length + 1, Width = t.Length + 1 };

            double[,] matrix = new double[bounds.Height, bounds.Width];

            for (int height = 0; height < bounds.Height; height++) { matrix[height, 0] = height * insertCost; };
            for (int width = 0; width < bounds.Width; width++) { matrix[0, width] = width * deleteCost; };

            for (int height = 1; height < bounds.Height; height++) {
                for (int width = 1; width < bounds.Width; width++)
                {
                    double falloff = Math.Pow(costFalloff, Math.Max(height - 1, width - 1));

                    double cost = (s[height - 1] == t[width - 1]) ? 0 : 1.0 * falloff;

                    double insertion = matrix[height, width - 1] + insertCost;
                    double deletion = matrix[height - 1, width] + deleteCost;

                    double cd = GetCharacterDistance(s[height - 1], t[width - 1]) * keyboardDistanceCostMultiplier;

                    double substitution = matrix[height - 1, width - 1] + cost * (substitutionCostBase + cd);

                    double distance = Math.Min(insertion, Math.Min(deletion, substitution));

                    //  transpositions
                    if (height > 1 && width > 1 && s[height - 1] == t[width - 2] && s[height - 2] == t[width - 1]) {
                        distance = Math.Min(distance, matrix[height - 2, width - 2] + transposeCost);
                    }

                    matrix[height, width] = distance;
                }
            }

            double d = matrix[bounds.Height - 1, bounds.Width - 1];

            distanceCache[(s,t)] = d;

            return d;
        }
    }
}