using System;
using System.Text;
using System.IO;
using System.Numerics;
using System.Web.Script.Serialization;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

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
            Blockchain blockchain = new Blockchain("log");
            Timer timer = new Timer(blockchain.MineBlock, new AutoResetEvent(false), 3000, 3000);

            string data = "";
            do
            {
                var serializer = new JavaScriptSerializer();
                try
                {
                    string to = Console.ReadLine();
                    decimal amount = decimal.Parse(Console.ReadLine());
                    TranType type = (TranType)Enum.Parse(typeof(TranType), Console.ReadLine());
                    string message = ReadLine();
                    string privKey = Console.ReadLine();
                    Transaction tr = new Transaction(to, amount, type, message);
                    tr = tr.Sign(privKey);
                    blockchain.AddTransaction(tr);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            } while (data != "exit");
        }

        private static string ReadLine()
        {
            Stream inputStream = Console.OpenStandardInput(8000);
            byte[] bytes = new byte[8000];
            int outputLength = inputStream.Read(bytes, 0, 8000);
            List<char> chars = Encoding.UTF7.GetChars(bytes, 0, outputLength).ToList();
            if (chars[chars.Count - 1] == '\n')
                chars.RemoveAt(chars.Count - 1);
            if (chars[chars.Count - 1] == '\r')
                chars.RemoveAt(chars.Count - 1);
            return new string(chars.ToArray());
        }
    }
}
