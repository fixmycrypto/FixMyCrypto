using System;
using System.Collections.Generic;

namespace FixMyCrypto {
    public class Path {
        public static string Resolve(string path, int account, int index) {
            string p = path;

            if (p == "m") {
                return p;
            }

            if (p.Contains("{account}")) {
                p = p.Replace("{account}", "" + account);
            }
            else {
                string accountPath = p.Substring(0, p.LastIndexOf("'"));
                accountPath = accountPath.Substring(0, accountPath.LastIndexOf("/") + 1);
                p = accountPath + account + p.Substring(p.LastIndexOf("'"));
            }

            if (p.Contains("{index}")) {
                p = p.Replace("{index}", "" + index);
            }
            else if (!path.StartsWith("m/44'/501'")) {
                p = p.Substring(0, p.LastIndexOf("/") + 1) + index;
                if (path.EndsWith("'")) p += "'";
            }

            return p;
        }
        public static string ResolveAccount(string path, int account) {
            if (path.Contains("{account}")) {
                return path.Replace("{account}", "" + account);
            }
            else {
                throw new NotSupportedException();
            }
        }
        public static string ResolveIndex(string path, int index) {
            if (path.Contains("{index}")) {
                return path.Replace("{index}", "" + index);
            }
            else {
                throw new NotSupportedException();
            }
        }
        public static void GetAccountIndex(string path, out int account, out int index) {
            index = 0;
            account = 0;

            if (String.IsNullOrEmpty(path)) return;

            if (path == "m") {
                return;
            }

            String indexPath = path;

            if (indexPath.EndsWith("'")) indexPath = indexPath.Substring(0, indexPath.Length - 1);

            Int32.TryParse(indexPath.Substring(indexPath.LastIndexOf("/") + 1), out index);

            if (path.StartsWith("m/44'/501'")) {
                //  SOL special case

                account = index;
                index = 0;
                return;                
            }

            if (!path.EndsWith("'")) {
                string accountPath = path.Substring(0, path.LastIndexOf("'"));

                Int32.TryParse(accountPath.Substring(accountPath.LastIndexOf("/") + 1), out account);
            }
        }

        public static string Tokenize(string path)
        {
            string p = path;

            if (String.IsNullOrEmpty(p) || !p.Contains("/") || !p.Contains("'")) return p;

            if (p == "m/44'/501'") return p;    //  SOL special case

            if (p.EndsWith("'") && !path.StartsWith("m/44'/501'/")) p = p.Substring(0, p.Length - 1);

            if (!path.Contains("{account}")) {
               string accountPath = p.Substring(0, p.LastIndexOf("'"));

               string indexPath = p.Substring(accountPath.Length + 1);

               accountPath = accountPath.Substring(0, accountPath.LastIndexOf("/") + 1);

               p = accountPath + "{account}'" + indexPath;
            }

            //  SOL is a weird exception
            if (path.StartsWith("m/44'/501'/"))
            {
                return p;
            }
            else if (!path.Contains("{index}")) {
                p = p.Substring(0, p.LastIndexOf("/")) + "/{index}";
            }

            if (path.EndsWith("'")) p += "'";

            return p;
        }
        public static string GetAccountPath(string path) {
            string p = path;

            if (!p.Contains("{account}")) p = Tokenize(path);

            return p.Substring(0, p.IndexOf("{account}'") + "{account}'".Length);
        }
        public static string GetIndexPath(string path) {
            string p = path;

            if (!p.Contains("{account}") || !p.Contains("{index}")) p = Tokenize(path);

            string a = GetAccountPath(p);

            return p.Substring(a.Length + 1);
        }
    }
    public class PathTree {
        public PathNode Root { get; }
        public PathTree() {
            Root = new PathNode(null, PathNode.RootIndex);
        }

        public PathTree(PathTree src) {
            Root = new PathNode(null, PathNode.RootIndex);
            Root.End = src.Root.End;
            foreach (PathNode child in src.Root.Children) {
                PathNode c = new PathNode(child);
                Root.AddChild(c);
            }
        }

        public void AddPath(string path, bool valid = true) {
            if (path.StartsWith("m/")) {
                Root.AddPath(path.Substring(2), valid);
            }
            else if (path.StartsWith("m'/")) {
                Root.AddPath(path.Substring(3), valid);
            }
            else if (path.Equals("m")) {
                Root.End = valid;
            }
            else {
                throw new NotSupportedException();
            }
        }

        public override string ToString() {
            return ToString(Root, "", true);
        }

        private string ToString(PathNode node, String indent, bool last) {
            string s;
            if (node == Root) {
                s = "m";
            }
            else {
                s = indent + "\\ " + PathNode.GetPath(node.Value);
            }

            if (node.End) s += " *";
            s += "\n";

            indent += last ? "  " : "| ";

            for (int i = 0; i < node.Children.Count; i++) {
                s += ToString(node.Children[i], indent, i == node.Children.Count - 1);
            }

            return s;
        }
    }

    public class PathNode {
        public static uint Hardened = 0x80000000U;
        public static uint RootIndex = 0x7fffffffU;
        public PathNode Parent { get; protected set; }
        public uint Value { get; protected set; }
        public List<PathNode> Children { get; protected set; }
        public bool End { get; set; }
        public Object Key { get; set; }
        public Object[] Keys { get; set; }

        public PathNode(PathNode parent, uint prefix) {
            this.Parent = parent;
            this.Children = new List<PathNode>();
            this.Value = prefix;
            this.End = false;
        }

        //  Copy constructor
        public PathNode(PathNode src) {
            this.Value = src.Value;
            this.End = src.End;
            this.Key = src.Key;
            this.Children = new List<PathNode>();
            foreach (var child in src.Children) {
                PathNode c = new PathNode(child);
                AddChild(c);
            }
        }

        public void AddChild(PathNode child) {
            this.Children.Add(child);
            child.Parent = this;
        }

        public static bool IsHardened(uint val) {
            return ((val & Hardened) == Hardened);
        }
        public static uint Harden(uint val) {
            return val | Hardened;
        }
        public static uint Soften(uint val) {
            return val ^ Hardened;
        }
        public uint Parse(string segment) {
            if (segment.EndsWith("'")) {
                return Harden(uint.Parse(segment.Substring(0, segment.Length - 1)));
            }
            else {
                return uint.Parse(segment);
            }
        }

        public string GetPath() {
            if (this.Parent != null) {
                return this.Parent.GetPath() + "/" + GetPath(this.Value);
            }
            else {
                return "m";
            }
        }
        public static string GetPath(uint index) {
            if (IsHardened(index)) {
                return $"{Soften(index)}'";
            }
            else {
                return $"{index}";
            }
        }
        public void AddPath(string path, bool valid = true) {
            if (!path.Contains("/")) {
                bool found = false;
                uint val = Parse(path);
                foreach (PathNode child in this.Children) {
                    if (child.Value == val) {
                        child.End = valid;
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    PathNode node = new PathNode(this, val);
                    node.End = valid;
                    this.Children.Add(node);
                    return;
                }
            }
            else {
                int ix = path.IndexOf("/");
                string prefix = path.Substring(0, ix);
                uint val = Parse(prefix);
                string post = path.Substring(ix + 1);

                bool found = false;

                foreach (PathNode child in this.Children) {
                    if (child.Value == val) {
                        if (!String.IsNullOrEmpty(post)) child.AddPath(post, valid);
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    PathNode node = new PathNode(this, val);
                    this.Children.Add(node);
                    if (!String.IsNullOrEmpty(post)) node.AddPath(post, valid);
                }
            }
        }
        public PathNode GetChild(uint val) {
            foreach (PathNode child in this.Children) {
                if (child.Value == val) return child;
            }

            return null;
       }
    }
}