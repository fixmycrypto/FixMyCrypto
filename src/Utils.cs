﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;

namespace FixMyCrypto
{
    static class Global {
        public static bool Done = false;

        public static bool Found = false;

        public static System.Diagnostics.Stopwatch sw = new();
    }
    public enum CoinType {
        BTC,
        ADA,
        ADALedger,
        ADATrezor,
        ATOM,
        ETH,
        DOGE,
        LTC,
        BCH,
        XRP,
        SOL,
        ALGO,
        DOT,
        DOTLedger,
        CRO
    }

    static class CoinTypeExtensions {
        public static CoinType GetCanonical(this CoinType coin) {
            switch (coin) {
                case CoinType.ADALedger:
                case CoinType.ADATrezor:

                return CoinType.ADA;

                case CoinType.DOTLedger:

                return CoinType.DOT;

                default:

                return coin;
            }
        }
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

    public static class StringExtensions {
        public static byte[] ToASCIIBytes(this string s) {
            return Encoding.ASCII.GetBytes(s);
        }

        public static byte[] ToUTF8Bytes(this string s) {
            return Encoding.UTF8.GetBytes(s);
        }
    }

    public static class ArrayExtensions {
        private static char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        public static string ToHexString(this byte[] b) {
            return b.ToHexString(0, b.Length);
        }
        public static string ToHexString(this ReadOnlyMemory<byte> b) {
            return Convert.ToHexString(b.Span);
        }
        public static string ToHexString(this ReadOnlySpan<byte> b) {
            return Convert.ToHexString(b);
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

        //  From CardanoSharp
        //  Big Endian bit order
        public static byte[] ElevenToEight(this short[] indices, bool includeChecksum = false)
        {
            // Compute and check checksum
            int MS = indices.Length;
            int ENTCS = MS * 11;
            int CS = ENTCS % 32;
            int ENT = includeChecksum ? ENTCS : ENTCS - CS;

            var entropy = new byte[ENT / 8];

            int itemIndex = 0;
            int bitIndex = 0;
            // Number of bits in a word
            int toTake = 8;
            // Indexes are held in a UInt32 but they are only 11 bits
            int maxBits = 11;
            for (int i = 0; i < entropy.Length; i++)
            {
                if (bitIndex + toTake <= maxBits)
                {
                    // All 8 bits are in one item

                    // To take 8 bits (*) out of 00000000 00000000 00000xx* *******x:
                    // 1. Shift right to get rid of extra bits on right, then cast to byte to get rid of the rest
                    // >> maxBits - toTake - bitIndex
                    entropy[i] = (byte)(indices[itemIndex] >> (3 - bitIndex));
                }
                else
                {
                    // Only a part of 8 bits are in this item, the rest is in the next.
                    // Since items are only 32 bits there is no other possibility (8<32)

                    // To take 8 bits(*) out of [00000000 00000000 00000xxx xxxx****] [00000000 00000000 00000*** *xxxxxxx]:
                    // Take first item at itemIndex [00000000 00000000 00000xxx xxxx****]: 
                    //    * At most 7 bits and at least 1 bit should be taken
                    // 1. Shift left [00000000 00000000 0xxxxxxx ****0000] (<< 8 - (maxBits - bitIndex)) 8-max+bi
                    // 2. Zero the rest of the bits (& (00000000 00000000 00000000 11111111))

                    // Take next item at itemIndex+1 [00000000 00000000 00000*** *xxxxxxx]
                    // 3. Shift right [00000000 00000000 00000000 0000****]
                    // number of bits already taken = maxBits - bitIndex
                    // nuber of bits to take = toTake - (maxBits - bitIndex)
                    // Number of bits on the right to get rid of= maxBits - (toTake - (maxBits - bitIndex))
                    // 4. Add two values to each other using bitwise OR [****0000] | [0000****]
                    entropy[i] = (byte)(((indices[itemIndex] << (bitIndex - 3)) & 0xff) |
                                         (indices[itemIndex + 1] >> (14 - bitIndex)));
                }

                bitIndex += toTake;
                if (bitIndex >= maxBits)
                {
                    bitIndex -= maxBits;
                    itemIndex++;
                }
            }

            return entropy;
        }

        //  For ALGO - little endian bit order
        //  https://stackoverflow.com/questions/51431932/how-can-i-convert-an-arraybuffer-to-11-bits-values-and-back-again-in-javascript/51452614#51452614
        public static byte[] ElevenToEightReverse(this short[] src) {
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

        public static short[] EightToEleven(this byte[] src) {
            short[] b = new short[(src.Length * 8) / 11];
            int ix = 0;
            int acc = 0;
            int accBits = 0;

            for (int i = 0; i < src.Length; i++) {
                acc <<= 8;
                acc |= src[i];
                accBits += 8;
                while (accBits >= 11) {
                    int val = (acc >> (accBits - 11));
                    b[ix++] = (short)(val & 0xffff);
                    acc &= (1 << (accBits - 11)) - 1;
                    accBits -= 11;
                }
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

        public static Dictionary<CoinType, string> DonationAddresses = new Dictionary<CoinType, string>() {
            { CoinType.BTC, "bc1q477afku8x7964gmzlsapgj8705e63ch89p8k4z" },
            { CoinType.ETH, "0x0327DF6652D07eE6cc670626b034edFfceD1B20C" },
            { CoinType.DOGE, "DT8iZF8RbqpRftgrWdiq34EZdJpCGiWBwG" },
            { CoinType.ADA, "addr1qxhjru35kv8fq66afxxdnjzf720anfcppktchh6mjuwxma3e876gh3czzkq0guls5qrkghexsuh543h7k2xqlje5lskqfp2elv" },
            { CoinType.SOL, "7ky2LTXNwPASogjMURv88LoPRHAAL4v49HeD7MYARuM4" },
            { CoinType.ALGO, "EPQZU6GMEMKKEQH4CP7U2U2NTQE2ZVMOYAS7F5WMCUYIAYUKNJVUHW5W5A" },
            { CoinType.DOT, "14jUHiE429X8HwPRmj2Sy4Xvo5Z9ewJJJ273ctvmQgxgTJ4b" },
            { CoinType.XRP, "rJtr6VfAP5Qmp2abKUfdJEGtHEckRpcKHk" },
            { CoinType.ATOM, "cosmos159zgwcx454uqrtrva8nk3sdrlegdxxln2w20s3" },
            { CoinType.CRO, "cro1yk3yuul2f60mfqy93fs8dcf3flt40xv9mtqef6" }
        };

        public static void DoFoundResult(CoinType coin, Address addr) {
            Global.Found = true;
            Global.Done = true;

            Log.All("\n\n\n\n");

            var resultData = new {
                instructions = "Contact help@fixmycrypto.com if you require further assistance.",
                coin = $"{coin}",
                address = addr.address,
                path = addr.path,
                phrase = addr.phrase.ToPhrase(),
                passphrase = addr.passphrase
            };
            string result = JsonConvert.SerializeObject(resultData, Formatting.Indented);
            StreamWriter writer = File.CreateText("results.json");
            writer.WriteLine(result);
            writer.Close();
            Log.All($"\n!!! FOUND WALLET !!!\nAddress:\n{addr.address} ({addr.path})\n\nRecovery Phrase written to: results.json\n");
            Log.All($"Please consider donating to fund future development!");
            CoinType ct = coin.GetCanonical();
            if (DonationAddresses.ContainsKey(ct)) {
                Log.All($"Donation address {ct}: {DonationAddresses[ct]}");
            }
            else {
                foreach (KeyValuePair<CoinType, string> p in DonationAddresses) {
                    Log.All($"Donation address {p.Key}: {p.Value}");
                }
            }
            Log.All();
        }

    }

    class Utils {
        public static int Factorial(int i) {
            if (i == 0) return 1;

            return i * Factorial(i - 1);
        }

    }
}
