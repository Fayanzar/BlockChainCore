using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Blockchain
{
    public class A
    {
        public A() { }
        public A(B b) { }
        public virtual void Method()
        {
            Console.WriteLine("A");
        }

    }

    public class B : A
    {
        public override void Method()
        {
            Console.WriteLine("B");
        }
    }
}
