using System;
using System.Collections;
using System.Collections.Generic;

namespace FixMyCrypto {
    abstract class Passphrase {
        public abstract IEnumerable<string> Next();

        public abstract override string ToString();

    }

    class SimplePassphrase : Passphrase {
        private string passphrase;

        public SimplePassphrase(string passphrase) {
            this.passphrase = passphrase;
            if (this.passphrase == null) this.passphrase = "";
        }

        public override IEnumerable<string> Next() {
            yield return this.passphrase;
        }

        public override string ToString() {
            return this.passphrase;
        }
    }
    abstract class Part {
        /*
        public static bool IsStartDelimiter(string s) {
            switch (s[0]) {
                case '[':
                case '(':

                return true;
            }

            return false;
        }

        public static bool IsEndDelimiter(string s)
        {
            char c = s[s.Length - 1];
            if (c == '?') c = s[s.Length - 2];
            switch (c)
            {
                case ']':
                case ')':

                    return true;
            }

            return false;
        }
        */

        public abstract int Min {get;}

        public abstract int Max {get;}

        public abstract IEnumerable<string> Next();

        public static Part Create(string p) {
            Log.Debug($"Part.Create(\"{p}\")");
            // if (IsStartDelimiter(p) && IsEndDelimiter(p)) {
                return new VariablePart(p);
            // }
            // else {
            //     return new FixedPart(p);
            // }
        }

    }
    class FixedPart : Part {
        string val;
        public FixedPart(string s) {
            val = s;
        }
        public override int Min { get { return 1; }}

        public override int Max { get { return 1; }}

        public override IEnumerable<string> Next() {
            yield return val;
        }
    }
    class VariablePart : Part {
        int minCount, maxCount;
        List<string> values;
        public VariablePart(string set) {
            minCount = 1;
            maxCount = 1;
            values = new List<string>();
            if (set.EndsWith("?")) {
                minCount = 0;
                maxCount = 1;
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
            }
            else {
                values.Add(set);
            }

            foreach (string v in values) {
                Log.Debug($"part min:{Min} value: {v}");
            }
        }

        public override int Min { get { return minCount; } }

        public override int Max { get { return maxCount; } }

        public override IEnumerable<string> Next() {
            foreach (string val in this.values) {
                yield return val;
            }
        }
    }

    class ComplexPassphrase : Passphrase
    {
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

        public ComplexPassphrase(string passphrase) {
            parts = new List<Part>();
            string current = "";
            bool inBlock = false;
            char blockType = ' ';

            for (int i = 0; i < passphrase.Length; i++) {
                if (!inBlock && IsStartDelimiter(passphrase[i])) {
                    if (current.Length > 0) {
                        Part p = Part.Create(current);
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

                    Part p = Part.Create(current);
                    parts.Add(p);
                    current = "";

                    inBlock = false;
                }
                else {
                    current += passphrase[i];
                }
            }

            if (current.Length > 0) parts.Add(Part.Create(current));
        }

        private IEnumerable<string> Recurse(string prefix, List<Part> parts, int start = 0) {
            if (start >= parts.Count) {
                yield return prefix;
                yield break;
            }

            if (parts[start].Min == 0) {
                foreach (string r in Recurse(prefix, parts, start + 1)) {
                    yield return r;
                }
            }

            foreach (string p in parts[start].Next()) {
                foreach (string r in Recurse(prefix + p, parts, start + 1)) {
                    yield return r;
                }
            }
        }

        public override IEnumerable<string> Next() {
            foreach (string r in Recurse("", this.parts)) yield return r;
        }

        public override string ToString() {
            throw new NotSupportedException();
        }
    }

}