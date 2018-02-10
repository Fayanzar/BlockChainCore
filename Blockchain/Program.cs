using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Numerics;

namespace Blockchain
{
    public static class ECDSAParameters
    {
        public static BigInteger modulo;
        public static BigInteger order;
        public static Point basePoint;
    }

    class Program
    {
       
        static void Main(string[] args)
        {
            ECDSAParameters.modulo = BigInteger.Parse("0FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F", System.Globalization.NumberStyles.AllowHexSpecifier);
            ECDSAParameters.basePoint = new Point("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", "483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8");
            ECDSAParameters.order = BigInteger.Parse("0FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", System.Globalization.NumberStyles.AllowHexSpecifier);
            NumFinite.modulo = ECDSAParameters.modulo;
            NumOrder.modulo = ECDSAParameters.order;

            var privKey = NumFinite.BigRandom(ECDSAParameters.order);

            Transaction t = new Transaction("abc", 10, TranType.one, "bca");
            Transaction t1 = t.Sign(privKey);

            Console.WriteLine(t1);
            Console.WriteLine(t1.Verify());


            Console.ReadKey();
        }
    }
}
