using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Blockchain
{
    public class NumFinite: IComparable<NumFinite>
    {
        static BigInteger modulo;
        public BigInteger num;
        public NumFinite(BigInteger bigint)
        {
            num = bigint;
        }
        public NumFinite(string bigint)
        {
            num = BigInteger.Parse(bigint, System.Globalization.NumberStyles.AllowHexSpecifier);
        }
        public static implicit operator NumFinite(BigInteger bigint)
        {
            return new NumFinite(bigint);
        }
        public static implicit operator NumFinite(string bigint)
        {
            return new NumFinite(bigint);
        }
        public static implicit operator NumFinite(int integer)
        {
            return new NumFinite(integer);
        }
        public static NumFinite operator +(NumFinite n1, NumFinite n2)
        {
            return (n1.num + n2.num) % modulo;
        }
        public static NumFinite operator -(NumFinite n1, NumFinite n2)
        {
            BigInteger a = (n1.num - n2.num) % modulo;
            return a < 0 ? modulo + a : a;
        }
        public static NumFinite operator *(NumFinite n1, NumFinite n2)
        {
            return (n1.num * n2.num) % modulo;
        }
        public static NumFinite operator /(NumFinite n1, NumFinite n2)
        {
            return n1 * Inverse(n2);
        }
        public static NumFinite operator %(NumFinite n1, NumFinite n2)
        {
            return (n1.num % n2.num) % modulo;
        }
        private static NumFinite Inverse(NumFinite n)
        {
            var i = n.num;
            var j = modulo;
            var s = new BigInteger(1);
            var t = new BigInteger(0);
            var u = new BigInteger(0);
            var v = new BigInteger(1);
            while (j != 0)
            {
                var q = i / j;
                var r = i % j;
                var unew = s;
                var vnew = t;
                s = u - (q * s);
                t = v - (q * t);
                i = j;
                j = r;
                u = unew;
                v = vnew;
            }
            return v > 0 ? v : (modulo + v);
        }
        public override string ToString()
        {
            return ByteArrayToString(num.ToByteArray());
        }
        public int CompareTo(NumFinite other)
        {
            return (num % modulo).CompareTo(other.num % modulo);
        }
        public static bool operator >(NumFinite n1, NumFinite n2)
        {
            return (n1.num % modulo) > (n2.num % modulo);
        }
        public static bool operator <(NumFinite n1, NumFinite n2)
        {
            return (n1.num % modulo) < (n2.num % modulo);
        }
        private static string ByteArrayToString(byte[] ba)
        {
            ba = ba.Reverse().SkipWhile(x => x == 0).ToArray();
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public static string BigRandom(NumFinite num)
        {
            Random random = new Random();
            byte[] b = new byte[32];
            BigInteger big;
            do
            {
                random.NextBytes(b);
                byte[] a = b.Reverse().Concat(new byte[] { 0 }).ToArray();
                big = new BigInteger(a);
            } while (big >= num.num || big.IsZero);
            return ByteArrayToString(b);
        }
    }
}
