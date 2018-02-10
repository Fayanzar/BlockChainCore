using System;
using System.Text;
using System.Security.Cryptography;

namespace Blockchain
{
    static public class Crypto
    {
        public static string ToStrHex(byte[] bytes, bool upperCase)
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

        public static byte[] HexToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        static public string Hash(string data)
        {
            var sha = new SHA256CryptoServiceProvider();
            Encoding asc = Encoding.ASCII;
            byte[] bl = asc.GetBytes(data);
            byte[] br = sha.ComputeHash(bl);
            return ToStrHex(br, false);
        }

    }
}
