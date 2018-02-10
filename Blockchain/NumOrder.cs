using System;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Blockchain
{
    public class NumOrder: IComparable<NumOrder>
    {
        public BigInteger num;
        static public BigInteger modulo;
        public NumOrder(BigInteger bigint)
        {
            num = bigint;
        }
        public NumOrder(string bigint)
        {
            num = BigInteger.Parse("00" + bigint, System.Globalization.NumberStyles.AllowHexSpecifier);
        }
        public NumOrder(NumFinite numf)
        {
            num = numf.num;
        }
        public virtual BigInteger Modulo()
        {
            return modulo;
        }
        public static explicit operator NumOrder(NumFinite numf)
        {
            return new NumOrder(numf);
        }
        public static implicit operator NumOrder(BigInteger bigint)
        {
            return new NumOrder(bigint);
        }
        public static implicit operator NumOrder(string bigint)
        {
            return new NumOrder(bigint);
        }
        public static implicit operator NumOrder(int integer)
        {
            return new NumOrder(integer);
        }
        public static NumOrder operator +(NumOrder n1, NumOrder n2)
        {
            return (n1.num + n2.num) % n1.Modulo();
        }
        public static NumOrder operator -(NumOrder n1, NumOrder n2)
        {
            BigInteger a = (n1.num - n2.num) % n1.Modulo();
            return a < 0 ? n1.Modulo() + a : a;
        }
        public static NumOrder operator *(NumOrder n1, NumOrder n2)
        {
            return (n1.num * n2.num) % n1.Modulo();
        }
        public static NumOrder operator /(NumOrder n1, NumOrder n2)
        {
            return n1 * Inverse(n2);
        }
        public static NumOrder operator %(NumOrder n1, NumOrder n2)
        {
            return (n1.num % n2.num) % n1.Modulo();
        }
        private static NumOrder Inverse(NumOrder n)
        {
            var i = n.num;
            var j = n.Modulo();
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
            return v > 0 ? v : (n.Modulo() + v);
        }
        public override string ToString()
        {
            return ByteArrayToString(num.ToByteArray());
        }
        public int CompareTo(NumOrder other)
        {
            return (num % Modulo()).CompareTo(other.num % Modulo());
        }
        public static bool operator >(NumOrder n1, NumOrder n2)
        {
            return (n1.num % n1.Modulo()) > (n2.num % n1.Modulo());
        }
        public static bool operator <(NumOrder n1, NumOrder n2)
        {
            return (n1.num % n1.Modulo()) < (n2.num % n1.Modulo());
        }
        private static string ByteArrayToString(byte[] ba)
        {
            ba = ba.Reverse().SkipWhile(x => x == 0).ToArray();
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
