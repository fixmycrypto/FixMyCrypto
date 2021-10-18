using System;
using System.Collections;
using System.Collections.Generic;

namespace FixMyCrypto {
     class Part {
        List<string> values;
        public Part(string set) {
            // Log.Debug($"Part(\"{set}\")");

            values = new List<string>();

            if (set.EndsWith("?")) {
                values.Add("");
                set = set.Substring(0, set.Length - 1);
            }

            if (set.StartsWith("(") && set.EndsWith(")")) {
                set = set.Substring(1, set.Length - 2);

                string[] parts = set.Split('|');

                foreach (string part in parts) {
                    values.Add(part);
                }
            }
            else if (set.StartsWith("[") && set.EndsWith("]")) {
                set = set.Substring(1, set.Length - 2);
                bool exclude = false;

                if (set.StartsWith("^")) {
                    set = set.Substring(1);
                    if (set.Length > 0 && set[0] != '^') {
                        exclude = true;
                    }
                }
                
                int start = 0;

                if (set.Length == 0) return;
                
                while (start < set.Length - 1)
                {
                    int dash = set.IndexOf('-', start + 1);

                    if (dash <= 0 || dash >= set.Length - 1)
                        break;

                    string p = set.Substring(start, dash - start - 1);

                    if (p.Length > 0) values.Add(p);

                    char a = set[dash - 1];
                    char z = set[dash + 1];

                    for (var i = a; i <= z; ++i)
                        values.Add($"{i}");

                    start = dash + 2;
                }

                for (int i = start; i < set.Length; i++) values.Add($"{set[i]}");

                if (exclude) {
                    List<string> rValues = new List<string>();
                    for (char i = (char)0x20; i < 0x7f; i++) {
                        if (!values.Contains($"{i}")) rValues.Add($"{i}");
                    }
                    values = rValues;
                }
            }
            else {
                values.Add(set);
            }

            // foreach (string v in values) {
            //     Log.Debug($"part value: {v}");
            // }
        }

        public IEnumerable<string> Next() {
            foreach (string val in this.values) {
                yield return val;
            }
        }
    }

    class Passphrase {
        public bool IsStartDelimiter(char c) {
            switch (c) {
                case '[':
                case '(':
                return true;

                default:
                return false;
            }
        }
        public char GetEndDelimiter(char c) {
            switch (c) {
                case '[':
                return ']';

                case '(':
                return ')';

                default:
                return ' ';
            }
        }

        List<Part> parts;

        public Passphrase(string passphrase) {
            parts = new List<Part>();
            string current = "";
            bool inBlock = false;
            char blockType = ' ';

            for (int i = 0; i < passphrase.Length; i++) {
                if (!inBlock && IsStartDelimiter(passphrase[i])) {
                    if (current.Length > 0) {
                        Part p = new Part(current);
                        parts.Add(p);
                        current = "";
                    }

                    current += passphrase[i];

                    blockType = passphrase[i];
                    inBlock = true;

                    if (i + 1 < passphrase.Length && passphrase[i+1] == GetEndDelimiter(passphrase[i])) {
                        current += passphrase[i+1];
                        i += 1;
                    }
                }
                else if (inBlock && passphrase[i] == GetEndDelimiter(blockType)) {
                    current += passphrase[i];

                    if (i + 1 < passphrase.Length) {
                        switch (passphrase[i+1]) {
                            case '?':

                            current += passphrase[i+1];
                            i += 1;
                            break;
                        }
                    }

                    Part p = new Part(current);
                    parts.Add(p);
                    current = "";

                    inBlock = false;
                }
                else {
                    current += passphrase[i];
                }
            }

            if (inBlock) {
                throw new Exception("Invalid passphrase format (check for unescaped characters)");
            }

            if (current.Length > 0) parts.Add(new Part(current));
        }

        private IEnumerable<string> Recurse(string prefix, List<Part> parts, int start = 0) {
            if (start >= parts.Count) {
                yield return prefix;
                yield break;
            }

            foreach (string p in parts[start].Next()) {
                foreach (string r in Recurse(prefix + p, parts, start + 1)) {
                    yield return r;
                }
            }
        }

        public IEnumerable<string> Enumerate() {
            foreach (string r in Recurse("", this.parts)) yield return r;
        }

        public override string ToString() {
            throw new NotSupportedException();
        }
    }

}