﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var sha = new SHA512CryptoServiceProvider();
            Encoding asc = Encoding.ASCII;
            byte[] bl = asc.GetBytes(data);
            byte[] br = sha.ComputeHash(bl);
            return ToStrHex(br, false);
        }

        public static string SignData(string data, RSAParameters Key)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(Key);
            Encoding asc = Encoding.ASCII;
            byte[] bl = asc.GetBytes(data);
            byte[] br = rsa.SignData(bl, new SHA512CryptoServiceProvider());
            return ToStrHex(br, false);
        }

        public static bool VerifyData(string verData, string sigData, RSAParameters Key)
        {
            Encoding asc = Encoding.ASCII;
            byte[] bv = HexToByteArray(verData);
            byte[] bs = asc.GetBytes(sigData);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(Key);
            return rsa.VerifyData(bs, new SHA512CryptoServiceProvider(), bv);
        }
    }
}
