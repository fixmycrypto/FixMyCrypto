using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography; 
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FixMyCrypto
{
    static class Global {
        public static bool done = false;

        public static bool found = false;
    }
    public enum CoinType {
        BTC,
        ADA,
        ADALedger,
        ADATrezor,
        ETH,
        DOGE,
        LTC,
        BCH,
        XRP,
        SOL,
        ALGO
    }
    public enum BtcApiType {
        blockcypher,
        mempool
    }
    public enum AdaApiType {
        rest,
        graphql
    }
    public enum EthApiType {
        gethrpc,
        blockcypher
    }
    public enum AltcoinApiType {
        dogechain,
        bitcore
    }
    public class WebClient {
        public static int retryCount = 5;
        public static readonly HttpClient client = new HttpClient();
    }

    public static class ArrayExtensions {
        private static char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        public static string ToHexString(this byte[] b) {
            return b.ToHexString(0, b.Length);
        }
        public static string ToHexString(this byte[] b, int start, int len) {
            char[] c = new char[len * 2];
            int p = 0;
            int q = start;
            int end = start + len;
            while (q < end) {
                byte a = b[q++];
                c[p++] = digits[a >> 4];
                c[p++] = digits[a & 0x0f];
            }
            return new string(c);
        }

        //  Array.Copy or for-loop copy faster than BlockCopy for most of what we do
        //  https://stackoverflow.com/questions/1389821/array-copy-vs-buffer-blockcopy
        public static T[] Slice<T>(this T[] b, int start, int len = -1) {
            if (len == -1) len = b.Length - start;
            T[] r = new T[len];

            // int i;
            // for (i = 0; i < len; i++) r[i] = b[i + start];

            // Buffer.BlockCopy(b, start, r, 0, len);
            
            Array.Copy(b, start, r, 0, len);
            return r;           
        }

        public static short[] Copy(this short[] a) {
            short[] b = new short[a.Length];
            a.CopyTo(b, 0);
            return b;
        }

        //  https://stackoverflow.com/questions/51431932/how-can-i-convert-an-arraybuffer-to-11-bits-values-and-back-again-in-javascript/51452614#51452614
        public static byte[] ElevenToEight(this short[] src) {
            byte[] b = new byte[(src.Length * 11) / 8];
            int ix = 0;
            int acc = 0;
            int accBits = 0;

            for (int i = 0; i < src.Length; i++) {
                acc |= (src[i] << accBits);
                accBits += 11;
                while (accBits >= 8) {
                    b[ix++] = (byte)(acc & 0xff);
                    acc >>= 8;
                    accBits -= 8;
                }
            }

            if (accBits > 0) {
                b[ix++] = (byte)acc;
            }

            return b;
        }
    }
    class Work {
        public Phrase phrase;
        public IList<Address> addresses;
        public Work(Phrase phrase, List<Address> addresses) {
            this.phrase = phrase;
            this.addresses = addresses;
        }

        public override string ToString() {
            string result = "";
            if (this.phrase != null) result += $"phrase: \"{phrase.ToPhrase()}\"\n";
            if (this.addresses != null) {
                foreach (Address address in addresses)
                    result += $"address: {address}\n";
            }
            return result;
        }
    }

    class FoundResult {

        public static void DoFoundResult(CoinType coin, Address addr) {
            Global.found = true;

            Log.All("\n\n\n\n");

            var resultData = new {
                instructions = "Contact help@fixmycrypto.com if you require further assistance.",
                coin = $"{coin}",
                address = addr.address,
                path = addr.path,
                wrongPhrase = Settings.phrase,
                correctedPhrase = addr.phrase.ToPhrase(),
                passphrase = addr.passphrase
            };
            string result = JsonConvert.SerializeObject(resultData, Formatting.Indented);
            StreamWriter writer = File.CreateText("results.json");
            writer.WriteLine(result);
            writer.Close();
            Log.All($"\n!!! FOUND WALLET !!!\nAddress:\n{addr.address} ({addr.path})\n\nRecovery Phrase written to: results.json\n");
            Log.All("To support the developers, please donate to one of these addresses:\nBTC: bc1q477afku8x7964gmzlsapgj8705e63ch89p8k4z\nETH: 0x0327DF6652D07eE6cc670626b034edFfceD1B20C\nDOGE: DT8iZF8RbqpRftgrWdiq34EZdJpCGiWBwG\nADA: addr1qxhjru35kv8fq66afxxdnjzf720anfcppktchh6mjuwxma3e876gh3czzkq0guls5qrkghexsuh543h7k2xqlje5lskqfp2elv\nSOL: 7ky2LTXNwPASogjMURv88LoPRHAAL4v49HeD7MYARuM4\nALGO: EPQZU6GMEMKKEQH4CP7U2U2NTQE2ZVMOYAS7F5WMCUYIAYUKNJVUHW5W5A\n");
        }

    }
}
