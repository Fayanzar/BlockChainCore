using System;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Blockchain
{
    public class Point
    {
        public NumFinite x;
        public NumFinite y;
        public Point(string xs, string ys)
        {
            x = xs;
            y = ys;
        }
        public Point(NumFinite xb, NumFinite yb)
        {
            x = xb;
            y = yb;
        }
        public static Point operator +(Point p1, Point p2)
        {
            if (p1.x.num.IsZero && p1.y.num.IsZero)
                return p2;
            if (p2.x.num.IsZero && p2.y.num.IsZero)
                return p1;
            if (p1.x.num == p2.x.num)
            {
                if (p1.y.num == p2.y.num)
                    return Double(p1);
                else
                    return new Point(0, 0);

            }
            else
            {
                var s = (p1.y - p2.y) / (p1.x - p2.x);
                var x3 = s * s - p1.x - p2.x;
                var y3 = s * (p1.x - x3) - p1.y;
                return new Point(x3, y3);
            }
        }
        public static Point Double(Point p)
        {
            var s = 3 * p.x * p.x / (2 * p.y);
            var x3 = s * s - 2 * p.x;
            var y3 = s * (p.x - x3) - p.y;
            return new Point(x3, y3);
        }
        public static Point operator *(Point p, NumFinite k)
        {
            Point R = new Point(0, 0);
            Point N = new Point(p.x, p.y);
            char[] bits = ToBinaryString(k.num).Reverse().ToArray();
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == '1')
                {
                    R += N;
                }
                N += N;
            }
            return R;
        }
        private static string ToBinaryString(BigInteger bigint)
        {
            var bytes = bigint.ToByteArray();
            var idx = bytes.Length - 1;
            var base2 = new StringBuilder(bytes.Length * 8);
            var binary = Convert.ToString(bytes[idx], 2);
            if (binary[0] != '0' && bigint.Sign == 1)
            {
                base2.Append('0');
            }

            base2.Append(binary);
            for (idx--; idx >= 0; idx--)
            {
                base2.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));
            }
            return new string(base2.ToString().SkipWhile(x => x == '0').ToArray());
        }
        public override string ToString()
        {
            return "(" + x.ToString() + "; " + y.ToString() + ")";
        }

    }
}
