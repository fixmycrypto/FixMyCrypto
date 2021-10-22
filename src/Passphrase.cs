using System;
using System.Collections;
using System.Collections.Generic;

namespace FixMyCrypto {

    class Part {
        List<Part> orValues;
        List<Part> andValues;
        List<Part> orderedValues;
        string stringValue;

        private static bool IsStartDelimiter(char c) {
            switch (c) {
                case '[':
                case '(':
                return true;

                default:
                return false;
            }
        }
        private static char GetEndDelimiter(char c) {
            switch (c) {
                case '[':
                return ']';

                case '(':
                return ')';

                default:
                return ' ';
            }
        }

        private bool IsOperator(char c) {
            switch (c) {
                case '&':
                case '|':

                return true;

                default:

                return false;
            }
        }

        private char GetOuterOperator(string set) {
            int depth = 0;
            char blockType = ' ';
            for (int i = 0; i < set.Length - 1; i++) {
                if (depth == 0 && IsStartDelimiter(set[i]) && set.Contains(GetEndDelimiter(set[i]))) {
                    depth++;
                    blockType = set[i];
                }
                else if (depth == 1 && set[i] == GetEndDelimiter(blockType)) {
                    depth--;
                    blockType = ' ';
                }
                else if (depth == 0 && IsOperator(set[i]) && set[i+1] == set[i]) {
                    //  found top level operator

                    return set[i];
                }
                else {
                    if (set[i] == blockType) depth++;
                    if (set[i] == GetEndDelimiter(blockType)) depth--;
                }
            }

            return ' ';
        }
        private void CreateBooleanSet(string set) {
            // Log.Debug($"bool set: {set}");

            char op = GetOuterOperator(set);

            if (op == '&') {
                // Log.Debug($"&& set: {set}");
                string[] andParts = set.Split("&&");

                foreach (string part in andParts) {
                    // Log.Debug($"&& part: {part}");
                    andValues.Add(new Part(part));
                }
            }
            else if (op == '|') {
                // Log.Debug($"|| set: {set}");
                string[] orParts = set.Split("||");
                foreach (string part in orParts) {
                    // Log.Debug($"|| part: {part}");
                    orValues.Add(new Part(part));
                }
            }
            else {
                orValues.Add(new Part(set));
            }
        }

        private void CreateOptionSet(string set) {
            bool exclude = false;

            if (set.StartsWith("^")) {
                set = set.Substring(1);
                if (set.Length > 0 && set[0] != '^') {
                    exclude = true;
                }
            }
            
            int start = 0;

            if (set.Length == 0) return;

            List<string> values = new List<string>();
            
            while (start < set.Length)
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

            foreach (string s in values) {
                orValues.Add(new Part(s));
            }
        }


        private bool IsSet(string set, char start, char end) {
            if (!set.StartsWith(start) || !set.EndsWith(end)) return false;

            int depth = 0;
            for (int i = 1; i < set.Length - 1; i++) {
                if (set[i] == start) depth++;
                if (set[i] == end) depth--;

                if (depth < 0) return false;
            }

            return (depth == 0);
        }

        private bool IsBooleanSet(string set) {
            return IsSet(set, '(', ')');
        }
        private bool IsOptionSet(string set) {
            return IsSet(set, '[', ']');
        }

        public Part(string set) {
            // Log.Debug($"Part: {set}");

            orValues = new List<Part>();
            andValues = new List<Part>();
            orderedValues = new List<Part>();
            stringValue = null;

            if ((set.StartsWith("(") && set.EndsWith(")?")) || (set.StartsWith("[") && set.EndsWith("]?"))) {
                orValues.Add(new Part(""));
                set = set.Substring(0, set.Length - 1);
            }

            if (IsBooleanSet(set)) {
                set = set.Substring(1, set.Length - 2);

                CreateBooleanSet(set);
            }
            else if (IsOptionSet(set)) {
                set = set.Substring(1, set.Length - 2);

                CreateOptionSet(set);
            }
            else if (((set.Contains("(") && set.Contains(")")) || (set.Contains("[") && set.Contains("]")))) {
                string current = "";
                int depth = 0;
                char blockType = ' ';

                for (int i = 0; i < set.Length; i++) {
                    if (depth == 0 && IsStartDelimiter(set[i])) {
                        if (current.Length > 0) {
                            Part p = new Part(current);
                            orderedValues.Add(p);
                            current = "";
                        }

                        current += set[i];

                        blockType = set[i];
                        depth++;
                    }
                    else if (depth == 1 && set[i] == GetEndDelimiter(blockType)) {
                        current += set[i];

                        if (i + 1 < set.Length) {
                            switch (set[i+1]) {
                                case '?':

                                current += set[i+1];
                                i += 1;
                                break;
                            }
                        }

                        Part p = new Part(current);
                        orderedValues.Add(p);
                        current = "";

                        depth--;
                        blockType = ' ';
                    }
                    else {
                        current += set[i];
                        if (set[i] == blockType) depth++;
                        if (set[i] == GetEndDelimiter(blockType)) depth--;
                    }
                }

                if (depth > 0) {
                    throw new Exception("Invalid passphrase format (check for unescaped characters)");
                }

                if (current.Length > 0) orderedValues.Add(new Part(current));
            }
            else {
                this.stringValue = set;
            }
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
        private IEnumerable<string> Permute(List<Part> parts, int start = 0) {
            if (start >= parts.Count) {
                foreach (string r in Recurse("", parts)) {
                    // Log.Debug($"Recurse returned: {r}");
                    yield return r;
                }
            }

            for (int i = start; i < parts.Count; i++) {
                List<Part> p = new List<Part>();
                foreach (Part part in parts) {
                    p.Add(part);
                }

                Part tmp = p[start];
                p[start] = p[i];
                p[i] = tmp;

                foreach (string s in Permute(p, start + 1)) {
                    // Log.Debug($"Permute returned: {s}");
                    yield return s;
                }
            }

        }

        public IEnumerable<string> Enumerate() {
            if (this.stringValue != null) {
                yield return this.stringValue;
                yield break;
            }
            if (this.orderedValues.Count > 0) {
                foreach (string r in Recurse("", this.orderedValues)) yield return r;
            }
            else if (this.andValues.Count > 0) {
                foreach (string p in Permute(this.andValues)) {
                    yield return p;
                }
            }
            else {
                foreach (Part p in this.orValues) {
                    foreach (string s in p.Enumerate()) {
                        yield return s;
                    }
                }
            }
        }
    }

    class Passphrase {
        Part root;

        public Passphrase(string passphrase) {
            root = new Part(passphrase);
        }

        public IEnumerable<string> Enumerate() {
            foreach (string r in root.Enumerate()) yield return r;
        }

        public override string ToString() {
            throw new NotSupportedException();
        }
    }

}