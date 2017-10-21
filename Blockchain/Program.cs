using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain
{
    class Program
    {
        static void Main(string[] args)
        {
            Blockchain BCh = new Blockchain();
            BCh.AddTransaction("35", "342", 10);
            Block b = BCh.MineBlock();
            //Console.WriteLine(b.ToString());
            Console.ReadKey();
        }
    }
}
