﻿using System;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Blockchain
{
    public class NumFinite: IComparable<NumFinite>
    {
        public BigInteger num;
        static public BigInteger modulo;
        public NumFinite(BigInteger bigint)
        {
            num = bigint;
        }
        public NumFinite(string bigint)
        {
            num = BigInteger.Parse("00" + bigint, System.Globalization.NumberStyles.AllowHexSpecifier);
        }
        public NumFinite(NumOrder numo)
        {
            num = numo.num;
        }
        public virtual BigInteger Modulo()
        {
            return modulo;
        }
        public static explicit operator NumFinite(NumOrder numo)
        {
            return new NumFinite(numo);
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
            return (n1.num + n2.num) % n1.Modulo();
        }
        public static NumFinite operator -(NumFinite n1, NumFinite n2)
        {
            BigInteger a = (n1.num - n2.num) % n1.Modulo();
            return a < 0 ? n1.Modulo() + a : a;
        }
        public static NumFinite operator *(NumFinite n1, NumFinite n2)
        {
            return (n1.num * n2.num) % n1.Modulo();
        }
        public static NumFinite operator /(NumFinite n1, NumFinite n2)
        {
            return n1 * Inverse(n2);
        }
        public static NumFinite operator %(NumFinite n1, NumFinite n2)
        {
            return (n1.num % n2.num) % n1.Modulo();
        }
        private static NumFinite Inverse(NumFinite n)
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
        public int CompareTo(NumFinite other)
        {
            return (num % Modulo()).CompareTo(other.num % Modulo());
        }
        public static bool operator >(NumFinite n1, NumFinite n2)
        {
            return (n1.num % n1.Modulo()) > (n2.num % n1.Modulo());
        }
        public static bool operator <(NumFinite n1, NumFinite n2)
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