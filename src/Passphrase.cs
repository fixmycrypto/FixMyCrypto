using System;
using System.Collections;
using System.Collections.Generic;

namespace FixMyCrypto {
     class Part {
        List<Part> values;
        string stringValue;

        private static void Permute(string[] parts, int start, List<string[]> list) {
            if (start >= parts.Length) {
                list.Add(parts);
                return;
            }

            for (int i = start; i < parts.Length; i++) {
                string[] p = (string[])parts.Clone();

                string tmp = p[start];
                p[start] = p[i];
                p[i] = tmp;

                Permute(p, start + 1, list);
            }
        }
        public Part(string set) {
            // Log.Debug($"Part(\"{set}\")");

            values = new List<Part>();
            stringValue = null;

            if (set.EndsWith("?")) {
                values.Add(new Part(""));
                set = set.Substring(0, set.Length - 1);
            }

            if (set.StartsWith("(") && set.EndsWith(")")) {
                set = set.Substring(1, set.Length - 2);

                if (set.Contains("&&")) {
                    Log.Debug($"&& set: {set}");
                    string[] andParts = set.Split("&&");

                    List<string[]> parts = new List<string[]>();
                    Permute(andParts, 0, parts);

                    foreach (string[] part in parts) {
                        string combined = String.Concat(part);
                        Log.Debug($"&& part: {combined}");
                        values.Add(new Part(combined));
                    }
                }
                else if (set.Contains("||")) {
                    Log.Debug($"|| set: {set}");
                    string[] orParts = set.Split("||");
                    foreach (string part in orParts) {
                        Log.Debug($"|| part: {part}");
                        values.Add(new Part(part));
                    }
                }
                else {
                    values.Add(new Part(set));
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

                    if (p.Length > 0) values.Add(new Part(p));

                    char a = set[dash - 1];
                    char z = set[dash + 1];

                    for (var i = a; i <= z; ++i)
                        values.Add(new Part($"{i}"));

                    start = dash + 2;
                }

                List<string> list = new List<string>();

                for (int i = start; i < set.Length; i++) list.Add($"{set[i]}");

                if (exclude) {
                    List<string> rValues = new List<string>();
                    for (char i = (char)0x20; i < 0x7f; i++) {
                        if (!list.Contains($"{i}")) rValues.Add($"{i}");
                    }
                    list = rValues;
                }

                foreach (string s in list) {
                    values.Add(new Part(s));
                }
            }
            else {
                this.stringValue = set;
            }

            // foreach (string v in values) {
            //     Log.Debug($"part value: {v}");
            // }
        }

        public IEnumerable<string> Enumerate() {
            if (this.stringValue != null) {
                yield return this.stringValue;
                yield break;
            }
            foreach (Part p in this.values) {
                foreach (string s in p.Enumerate()) {
                    yield return s;
                }
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
            int depth = 0;
            char blockType = ' ';

            for (int i = 0; i < passphrase.Length; i++) {
                if (depth == 0 && IsStartDelimiter(passphrase[i])) {
                    if (current.Length > 0) {
                        Part p = new Part(current);
                        parts.Add(p);
                        current = "";
                    }

                    current += passphrase[i];

                    blockType = passphrase[i];
                    depth++;
                }
                else if (depth == 1 && passphrase[i] == GetEndDelimiter(blockType)) {
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

                    depth--;
                    blockType = ' ';
                }
                else {
                    current += passphrase[i];
                    if (passphrase[i] == blockType) depth++;
                    if (passphrase[i] == GetEndDelimiter(blockType)) depth--;
                }
            }

            if (depth > 0) {
                throw new Exception("Invalid passphrase format (check for unescaped characters)");
            }

            if (current.Length > 0) parts.Add(new Part(current));
        }

        private IEnumerable<string> Recurse(string prefix, List<Part> parts, int start = 0) {
            if (start >= parts.Count) {
                yield return prefix;
                yield break;
            }

            foreach (string p in parts[start].Enumerate()) {
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