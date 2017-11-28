using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Numerics;

namespace Blockchain
{
    public struct RSAKey
    {
        public BigInteger modulus;
        public BigInteger exponent;
        public RSAKey(BigInteger n, BigInteger e)
        {
            modulus = n;
            exponent = e;
        }
        public RSAKey(byte[] n, byte[] e)
        {
            modulus = new BigInteger(Crypto.Correct(n.Reverse().ToArray()));
            exponent = new BigInteger(Crypto.Correct(e.Reverse().ToArray()));
        }
    }

    static public class Crypto
    {
        public static byte[] Correct(byte[] bytes)
        {
            if ((bytes[bytes.Length - 1] & 0x80) > 0)
            {
                byte[] temp = new byte[bytes.Length];
                Array.Copy(bytes, temp, bytes.Length);
                bytes = new byte[temp.Length + 1];
                Array.Copy(temp, bytes, temp.Length);
            }
            return bytes;
        }

      /*  public static RSAKey[] GenerateKey(int length)
        {
            Random random = new Random();
            byte[] pBytes = new byte[length], qBytes = new byte[length];
            random.NextBytes(pBytes);
            random.NextBytes(qBytes);
            BigInteger
        }*/

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
            var sha = new SHA512CryptoServiceProvider();
            Encoding asc = Encoding.ASCII;
            byte[] bl = asc.GetBytes(data);
            byte[] br = sha.ComputeHash(bl);
            return ToStrHex(br, false);
        }

        public static string SignData(string data, RSAKey key)
        {
            BigInteger d = key.exponent;
            BigInteger n = key.modulus;
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            Encoding asc = Encoding.ASCII;
            byte[] textBytes = asc.GetBytes(data);
            byte[] hash = sha.ComputeHash(textBytes);
            BigInteger m = new BigInteger(Correct(hash.Reverse().ToArray()));
            BigInteger s = BigInteger.ModPow(m, d, n);
            byte[] signed = s.ToByteArray().Reverse().ToArray();
            return ToStrHex(signed, false);
        }

        public static bool VerifyData(string verData, string sigData, RSAKey key)
        {
            BigInteger e = key.exponent;
            BigInteger n = key.modulus;
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            Encoding asc = Encoding.ASCII;
            byte[] textBytes = asc.GetBytes(sigData);
            byte[] hash = sha.ComputeHash(textBytes);
            byte[] signed = HexToByteArray(verData);
            BigInteger s = new BigInteger(Correct(signed.Reverse().ToArray()));
            BigInteger m = BigInteger.ModPow(s, e, n);
            return (m == new BigInteger(Correct(hash.Reverse().ToArray())));
        }
    }
}
