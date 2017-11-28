using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Numerics;

namespace Blockchain
{
    class Program
    {

        static void Main(string[] args)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            RSAParameters key = rsa.ExportParameters(true);
            BigInteger d = new BigInteger(key.D);
            BigInteger n = new BigInteger(key.Modulus);
            BigInteger e = new BigInteger(key.Exponent);
            string text = "text";
            Encoding enc = Encoding.ASCII;
            byte[] br = enc.GetBytes(text);
            BigInteger m = new BigInteger(br);
            BigInteger c = BigInteger.ModPow(m, e, n);
            BigInteger m1 = BigInteger.ModPow(c, d, n);
            Console.WriteLine(m1);
            Console.WriteLine(m);
            Console.ReadKey();
        }
    }
}
