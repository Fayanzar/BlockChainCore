using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Blockchain
{
    static public class Crypto
    {
        static string ToStrHex(byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }

        public static string ToStrASCII(string hex)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hex.Length; i += 2)
            {
                string hs = hex.Substring(i, i + 2);
                Convert.ToChar(Convert.ToUInt32(hex.Substring(0, 2), 16)).ToString();
            }
            return sb.ToString();
        }
        static public string Hash(string data)
        {
            var sha = new SHA512CryptoServiceProvider();
            Encoding asc = Encoding.ASCII;
            byte[] bl = asc.GetBytes(data);
            byte[] br = sha.ComputeHash(bl);
            return ToStrHex(br, false);
        }
    }
}
