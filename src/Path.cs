using System;
using System.Collections.Generic;

namespace FixMyCrypto {
    public class Path {
        public static string Resolve(string path, int account, int index) {
            string p = path;

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
        public PathNode root;
        public PathTree() {
            root = new PathNode(null, PathNode.RootIndex);
        }
        public void AddPath(string path, bool valid = true) {
            if (path.StartsWith("m/")) {
                root.AddPath(path.Substring(2), valid);
            }
            else if (path.StartsWith("m'/")) {
                root.AddPath(path.Substring(3), valid);
            }
            else {
                throw new NotSupportedException();
            }
        }

        public override string ToString() {
            return ToString(root, "", true);
        }

        private string ToString(PathNode node, String indent, bool last) {
            string s;
            if (node == root) {
                s = "m\n";
            }
            else {
                s = indent + "\\ " + PathNode.GetPath(node.value);
                if (node.end) s += " *";
                s += "\n";
            }

            indent += last ? "  " : "| ";

            for (int i = 0; i < node.children.Count; i++) {
                s += ToString(node.children[i], indent, i == node.children.Count - 1);
            }

            return s;
        }
    }
    public class PathNode {
        public static uint Hardened = 0x80000000U;
        public static uint RootIndex = 0x7fffffffU;
        public PathNode parent;
        public uint value;
        public List<PathNode> children;
        public bool end;
        public Object key;

        public PathNode(PathNode parent, uint prefix) {
            this.parent = parent;
            this.children = new List<PathNode>();
            this.value = prefix;
            this.end = false;
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
            if (this.parent != null) {
                return this.parent.GetPath() + "/" + GetPath(this.value);
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
                foreach (PathNode child in this.children) {
                    if (child.value == val) {
                        child.end = valid;
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    PathNode node = new PathNode(this, val);
                    node.end = valid;
                    this.children.Add(node);
                    return;
                }
            }
            else {
                int ix = path.IndexOf("/");
                string prefix = path.Substring(0, ix);
                uint val = Parse(prefix);
                string post = path.Substring(ix + 1);

                bool found = false;

                foreach (PathNode child in this.children) {
                    if (child.value == val) {
                        if (!String.IsNullOrEmpty(post)) child.AddPath(post, valid);
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    PathNode node = new PathNode(this, val);
                    this.children.Add(node);
                    if (!String.IsNullOrEmpty(post)) node.AddPath(post, valid);
                }
            }
        }
        public PathNode GetChild(uint val) {
            foreach (PathNode child in this.children) {
                if (child.value == val) return child;
            }

            return null;
       }
    }
}