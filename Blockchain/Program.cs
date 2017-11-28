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
            RSAKey pr = new RSAKey(key.Modulus, key.D);
            RSAKey pu = new RSAKey(key.Modulus, key.Exponent);

            string text = "im retarded";
            string sign = Crypto.SignData(text, pr);
            Console.WriteLine(Crypto.VerifyData(sign, text, pu));

            Console.ReadKey();
        }
    }
}
