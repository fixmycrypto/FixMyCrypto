using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
namespace FixMyCrypto {

    class Part : IEnumerable<string> {

        enum OpType {
            Ordered,
            Or,
            And
        }

        OpType opType;
        Part[] parts;
        string stringValue;
        int minCount, maxCount;
        bool fuzz;

        //  for graphviz only
        string range;

        private static bool IsStartDelimiter(char c) {
            switch (c) {
                case '[':
                case '(':
                case '{':
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

                case '{':
                return '}';

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
        private List<Part> CreateBooleanSet(string set) {
            // Log.Debug($"bool set: {set}");

            List<Part> parts = new List<Part>();

            char op = GetOuterOperator(set);

            if (op == '&') {
                this.opType = OpType.And;
                // Log.Debug($"&& set: {set}");
                string[] andParts = set.Split("&&");

                foreach (string part in andParts) {
                    // Log.Debug($"&& part: {part}");
                    parts.Add(new Part(part));
                }
            }
            else if (op == '|') {
                this.opType = OpType.Or;
                // Log.Debug($"|| set: {set}");
                string[] orParts = set.Split("||");
                foreach (string part in orParts) {
                    // Log.Debug($"|| part: {part}");
                    parts.Add(new Part(part));
                }
            }
            else {
                this.opType = OpType.Ordered;
                parts.Add(new Part(set));
            }

            return parts;
        }

        private bool IsPrintableCharacter(char c) {
            switch (Char.GetUnicodeCategory(c)) {
                case System.Globalization.UnicodeCategory.Control:
                case System.Globalization.UnicodeCategory.OtherNotAssigned:
                return false;

                default:
                return true;
            }
        }

        private List<Part> CreateOptionSet(string set) {
            bool exclude = false;
            this.opType = OpType.Or;
            this.range = set;

            if (set.StartsWith("^")) {
                set = set.Substring(1);
                if (set.Length > 0 && set[0] != '^') {
                    exclude = true;
                }
            }
            
            int start = 0;

            if (set.Length == 0) return new List<Part>();

            List<string> items = new List<string>();
            
            while (start < set.Length)
            {
                int dash = set.IndexOf('-', start + 1);

                if (dash <= 0 || dash >= set.Length - 1)
                    break;

                string p = set.Substring(start, dash - start - 1);

                if (p.Length > 0) items.Add(p);

                char a = set[dash - 1];
                char z = set[dash + 1];

                for (var i = a; i <= z; ++i)
                    items.Add($"{i}");

                start = dash + 2;
            }

            for (int i = start; i < set.Length; i++) {
                if (set[i] == '\\' && i + 1 < set.Length) {
                    i++;
                    if (set[i] == 'c') {
                        for (char c = (char)0x20; c < 0x7f; c++) { items.Add($"{c}"); }
                    }
                    else if (set[i] == 'd') {
                        for (char c = '0'; c <= '9'; c++) { items.Add($"{c}"); }
                    }
                    else if (set[i] == 'u') {
                        items.EnsureCapacity(1200000);
                        //  U+0001 - U+D7FF
                        for (int c = 0x20; c < 0xd800; c++) { 
                            if (IsPrintableCharacter((char)c)) items.Add($"{(char)c}"); 
                        }
                        //  U+E000 - U+FFFF
                        for (int c = 0xe000; c <= 0xffff; c++) { 
                            if (IsPrintableCharacter((char)c)) items.Add($"{(char)c}"); 
                        }
                        // Surrogate pairs U+D800-U+DBFF + U+DC00-U+DFFF
                        for (int c = 0xd800; c <= 0xdbff; c++) { 
                            for (int d = 0xdc00; d <= 0xdfff; d++) {
                                items.Add($"{(char)c}{(char)d}");
                            }
                        }
                    }
                    else if (set[i] == '\\') {
                        items.Add($"{set[i]}");
                    }
                    else {
                        throw new Exception($"Unsupported character class \\{set[i]}");
                    }
                }
                else {
                    items.Add($"{set[i]}");
                }
            }

            if (exclude) {
                List<string> rValues = new List<string>();
                for (char i = (char)0x20; i < 0x7f; i++) {
                    if (!items.Contains($"{i}")) rValues.Add($"{i}");
                }
                items = rValues;
            }

            List<Part> parts = new List<Part>();

            foreach (string s in items) {
                parts.Add(new Part(s));
            }

            return parts;
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

        private bool IsFuzzSet(string set) {
            return IsSet(set, '{', '}');
        }

        public Part(string set, int minCount = 1, int maxCount = 1) {
            // Log.Debug($"Part: {set}");
            this.minCount = minCount;
            this.maxCount = maxCount;

            List<Part> values = new List<Part>();
            stringValue = null;

            if (IsBooleanSet(set)) {
                set = set.Substring(1, set.Length - 2);

                values.AddRange(CreateBooleanSet(set));
            }
            else if (IsOptionSet(set)) {
                set = set.Substring(1, set.Length - 2);

                values.AddRange(CreateOptionSet(set));
            }
            else if (IsFuzzSet(set)) {
                set = set.Substring(1, set.Length - 2);

                values.Add(new Part(set));
                this.fuzz = true;
            }
            else if (((set.Contains("(") && set.Contains(")")) || (set.Contains("[") && set.Contains("]")) || (set.Contains("{") && set.Contains("}")))) {
                string current = "";
                int depth = 0;
                char blockType = ' ';
                this.opType = OpType.Ordered;

                for (int i = 0; i < set.Length; i++) {
                    if (depth == 0 && IsStartDelimiter(set[i])) {
                        if (current.Length > 0) {
                            Part p = new Part(current);
                            values.Add(p);
                            current = "";
                        }

                        current += set[i];

                        blockType = set[i];
                        depth++;
                    }
                    else if (depth == 1 && set[i] == GetEndDelimiter(blockType)) {
                        current += set[i];
                        string repeat = "";
                        int minRep = 1, maxRep = 1;

                        if (i + 1 < set.Length) {
                            switch (set[i+1]) {
                                case '?':

                                minRep = 0;
                                i += 1;
                                break;

                                case '<':
                                i += 2;
                                while (i < set.Length && set[i] != '>') {
                                    repeat += set[i];
                                    i += 1;
                                }
                                if (i >= set.Length || set[i] != '>') throw new Exception($"invalid repetition after expression {set}");
                                if (repeat.Contains("-")) {
                                    string[] parts = repeat.Split("-");
                                    if (!int.TryParse(parts[0], out minRep)) minRep = 1;
                                    if (!int.TryParse(parts[1], out maxRep)) maxRep = 1;
                                }
                                else {
                                    if (!int.TryParse(repeat, out minRep)) minRep = 1;
                                    maxRep = minRep;
                                }
                                repeat = "";
                                break;
                            }
                        }

                        Part p = new Part(current, minRep, maxRep);
                        values.Add(p);
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
                    throw new Exception($"Invalid passphrase format (check for unescaped characters): {set}");
                }

                if (current.Length > 0) values.Add(new Part(current));
            }
            else {
                this.stringValue = set;
            }

            this.parts = values.ToArray();
        }

        private IEnumerable<string> Recurse(string prefix, Part[] parts, int start = 0) {
             if (start >= parts.Length) {
                yield return prefix;
                yield break;
            }

            foreach (string p in parts[start]) {
                foreach (string r in Recurse(prefix + p, parts, start + 1)) {
                    yield return r;
                }
            }
        }
        private IEnumerable<string> Permute(Part[] parts, int start = 0) {
            if (start >= parts.Length) {
                foreach (string r in Recurse("", parts)) {
                    // Log.Debug($"Recurse returned: {r}");
                    yield return r;
                }
            }

            for (int i = start; i < parts.Length; i++) {
                Part[] p = (Part[])parts.Clone();

                Part tmp = p[start];
                p[start] = p[i];
                p[i] = tmp;

                foreach (string s in Permute(p, start + 1)) {
                    // Log.Debug($"Permute returned: {s}");
                    yield return s;
                }
            }
        }

        private IEnumerator<string> EnumerateWithCount(int count) {
            if (count == 0) {
                yield return "";
            }
            else if (count == 1) {
                var r = EnumerateOnce();
                while (r.MoveNext()) {
                    yield return r.Current;
                }
            }
            else {
                var s = EnumerateOnce();
                while (s.MoveNext()) {
                    var r = EnumerateWithCount(count - 1);
                    while (r.MoveNext()) {
                        yield return s.Current + r.Current;
                    }
                }
            }
        }

        private IEnumerator<string> EnumerateOnce() {
            if (this.fuzz) {
                foreach (string r in parts[0]) {

                    //  CAPS / lower case

                    if (r.ToUpper() != r) yield return r.ToUpper();

                    if (r.ToLower() != r) yield return r.ToLower();

                    //  Deletions
                    for (int i = 0; i < r.Length; i++) {
                        yield return r.Substring(0, i) + r.Substring(i + 1);
                    }

                    //  Substitutions
                    for (int i = 0; i < r.Length; i++) {
                        for (byte c = 0x20; c < 0x7f; c++) {
                            if (r[i] == (char)c) continue;

                            yield return r.Substring(0, i) + (char)c + r.Substring(i + 1);
                        }
                    }

                    //  Insertions
                    for (int i = 0; i <= r.Length; i++) {
                        for (byte c = 0x20; c < 0x7f; c++) {
                            yield return r.Substring(0, i) + (char)c + r.Substring(i);
                        }
                    }

                    //  Transpositions
                    for (int i = 0; i < r.Length - 1; i++) {
                        for (int j = i + 1; j < r.Length; j++) {
                            char[] c = r.ToCharArray();

                            char tmp = c[i];
                            c[i] = c[j];
                            c[j] = tmp;

                            yield return new string(c);
                        }
                    }
                }

                yield break;
            }

            if (this.stringValue != null) {
                yield return this.stringValue;
                yield break;
            }

            if (this.opType == OpType.Ordered) {
                foreach (string r in Recurse("", this.parts)) yield return r;
            }
            else if (this.opType == OpType.And) {
                foreach (string p in Permute(this.parts)) {
                    yield return p;
                }
            }
            else {
                //  OR
                foreach (Part p in this.parts) {
                    foreach (string s in p) {
                        yield return s;
                    }
                }
            }
        }

        public IEnumerator<string> GetEnumerator() {
            for (int i = minCount; i <= maxCount; i++) {
                var r = EnumerateWithCount(i);
                while (r.MoveNext()) {
                    yield return r.Current;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override string ToString() {
            string label = this.stringValue;

            if (label == null) {
                if (this.opType == OpType.Ordered) {
                    label = "";
                    foreach (Part p in this.parts) {
                        label += p.ToString();
                    }
                }
                else if (this.range != null) {
                    label = $"[{this.range}]";
                }
                else if (this.opType == OpType.Or) {
                    label = "(" + String.Join("||", (object[])this.parts) + ")";
                }
                else if (this.opType == OpType.And) {
                    label = "(" + String.Join("&&", (object[])this.parts) + ")";
                }

                if (fuzz) label = "{" + label + "}";

                if (minCount != 1 || maxCount != 1) {
                    if (minCount == maxCount) {
                        label += $"<{minCount}>";
                    }
                    else {
                        label += $"<{minCount}-{maxCount}>";
                    }
                }
            }

            return label;
        }

        private string EscapeString(string s) {
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("<", "&lt;").Replace("|", "\\|");
        }

        public string GetTopology(bool root = false, string parentLabel = "") {
            string label = this.ToString();
            int id = (parentLabel + label).GetHashCode();
            string nodes;

            if (opType == OpType.Ordered && parts.Length > 1) {
                string recordLabel = "";
                int record = 0;
                foreach (Part p in parts) {
                    if (recordLabel.Length > 0) recordLabel += "|";
                    recordLabel += $"<f{record}>{EscapeString(p.ToString())}";
                    record++;
                }
                //  add record node
                nodes = $"\t\"{id:X8}\" [label=\"{recordLabel}\"{(root ? " root=\"true\"" : "")} ordering=\"out\" shape=record]\n";

                for (int i = 0; i < parts.Length; i++) {
                    Part p = parts[i];
                    //  skip single string parts, since they are contained in the record label
                    if (p.stringValue != null) continue;

                    nodes += p.GetTopology(parentLabel: label);
                    string child = p.ToString();
                    int childId =  (label + child).GetHashCode();
                    string target = p.opType == OpType.Ordered && p.parts.Length > 1 ? ":f0" : "";

                    //  add parent link to child
                    nodes += $"\t\"{id:X8}\":f{i} -> \"{childId:X8}{target}\":n\n";
                }
            }
            else {
                string readableLabel = label;
                if (parts.Length > 1 && range == null) {
                    readableLabel = "";
                    foreach (Part p in parts) {
                        if (readableLabel.Length > 0) readableLabel += opType == OpType.And ? " AND " : " OR ";
                        readableLabel += "\"" + p.ToString() + "\"";
                    }
                }
                string shape = root ? "square" : "oval";
                if (opType == OpType.Or && parts.Length > 1) shape = "invhouse";
                if (opType == OpType.And) shape = "box style=rounded";
                nodes = $"\t\"{id:X8}\" [label=\"{EscapeString(readableLabel)}\"{(root ? " root=\"true\"" : "")}{(opType == OpType.Ordered && parts.Length > 1 ? " ordering=\"out\"" : "")} shape={shape}]\n";

                foreach (Part p in this.parts) {
                    nodes += p.GetTopology(parentLabel: label);
                    string child = p.ToString();
                    int childId =  (label + child).GetHashCode();
                    string target = p.opType == OpType.Ordered && p.parts.Length > 1 ? ":f0" : "";
                    nodes += $"\t\"{id:X8}\"\t->\t\"{childId:X8}\"{target}:n\n";
                }

                if (this.minCount == 0) {
                    Part p = new Part("");
                    string child = p.ToString();
                    int childId =  (label + child).GetHashCode();
                    nodes += p.GetTopology(parentLabel: label);
                    nodes += $"\t\"{id:X8}\"\t->\t\"{childId:X8}\":n\n";
                }
            }

            return nodes;
        }
    }

    class Passphrase : IEnumerable<string> {
        Part root;

        public Passphrase(string passphrase) {
            if (passphrase == null) return;

            root = new Part(passphrase);
        }

        public IEnumerator<string> GetEnumerator() {
            if (root != null) {
                foreach (string r in root) yield return r;
            }
            else {
                yield return "";
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override string ToString() {
            throw new NotSupportedException();
        }

        public long GetCount() {
            long count = 0;
            foreach (string r in this) count++;
            return count;
        }   

        public void GetCountAndMaxLength(ref long count, ref int maxLength) {
            foreach (string r in this) {
                count++;
                maxLength = Math.Max(r.Length, maxLength);
            }
        }

        public void WriteTopologyFile(string path) {
            string topology = "digraph G {\n\toverlap = false\n\tconcentrate = false\n" + GetTopology() + "}\n";

            System.IO.File.WriteAllText(path, topology);
        }

        public string GetTopology() {

            //  https://graphviz.org/doc/info/lang.html

            return root.GetTopology(true);
        }
    }

    class MultiPassphrase : IEnumerable<string> {

        List<Passphrase> passphrases;

        public MultiPassphrase(string[] src) {
            passphrases = new();
            if (src == null) return;
            foreach (string p in src) {
                passphrases.Add(new Passphrase(p));
            }
        }

        public long GetCount() {
            long count = 0;
            foreach (Passphrase p in passphrases) {
                count += p.GetCount();
            }
            return count;
        }

        public void LoadFromFile(string file) {
            if (file == null) return;
            
            if (file.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)) {
                using var zip = ZipFile.OpenRead(file);
                Log.Info($"Extracting passphrases from {file}");
                foreach (var entry in zip.Entries) {
                    if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)) {
                        using var stream = entry.Open();
                        using var reader = new StreamReader(stream);
                        List<string> lines = new();
                        while (!reader.EndOfStream) {
                            lines.Add(reader.ReadLine());
                        }
                        LoadStrings(lines, entry.FullName);
                    }
                }
            }
            else {
                LoadStrings(File.ReadAllLines(file), file);
            }
        }

        private void LoadStrings(IEnumerable<string> lines, string file) {
            long count = 0;
            foreach (var line in lines) {
                if (line.Length <= 0 || line.Length > 255) continue;
                var p = EscapeString(line);
                passphrases.Add(new Passphrase(p));
                count++;
            }
            Log.Info($"Read {count:n0} passphrases from {file}");
        }

        private string EscapeString(string s) {
            StringBuilder sb = new();
            for (int i = 0; i < s.Length; i++) {
                switch (s[i]) {
                    case '(':
                    case '{':
                    case '<':
                    case '?':
                    sb.AppendFormat("[{0}]", s[i]);
                    break;

                    case '[':
                    sb.AppendFormat("({0})", s[i]);
                    break;

                    default:
                    sb.Append(s[i]);
                    break;
                }
            }

            return sb.ToString();
        }

        public (long, int) GetCountAndMaxLength() {
            long count = 0;
            int maxLength = 0;
            System.Timers.Timer t = new(10 * 1000);
            t.Elapsed += (StringReader, args) => {
                Log.Info($"Enumerating passphrases: {count:n0} and counting...");
            };
            t.Start();
            foreach (Passphrase p in passphrases) {
                p.GetCountAndMaxLength(ref count, ref maxLength);
            }
            t.Stop();
            return (count, maxLength);
        }

        public IEnumerator<string> GetEnumerator() {
            foreach (Passphrase p in passphrases) {
                foreach (string s in p) {
                    yield return s;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void WriteTopologyFile(string path) {
            string topology = "digraph G {\n\toverlap = false\n\tconcentrate = false\n" + GetTopology() + "}\n";

            System.IO.File.WriteAllText(path, topology);
        }

        private string GetTopology() {
            string topology = "";

            foreach (Passphrase p in passphrases) {
                topology += p.GetTopology();
            }

            return topology;
        }

        public override string ToString() {
            throw new NotSupportedException();
        }
    }
}