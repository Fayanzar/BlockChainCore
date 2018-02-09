using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web.Script.Serialization;

namespace Blockchain
{
    public enum TranType { one, two, three }
    public struct Transaction
    {
        public string recipient;
        public decimal amount;
        public TranType tranType;
        public string message;
        public string v;
        public string r;
        public string s;
        public Transaction(string re, decimal a, TranType t, string m, string v0, string r0, string s0)
        {
            recipient = re;
            amount = a;
            tranType = t;
            message = m;
            v = v0;
            r = r0;
            s = s0;
        }
        public Transaction Sign(string privKey, string pubKey)
        {
            Point G = ECDSAParameters.basePoint;
            var k = NumFinite.BigRandom(ECDSAParameters.order);

            return this;
        }
        public override string ToString()
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }

        public static string Merkle(List<Transaction> trans)
        {
            var transactions = trans.Select(x => Crypto.Hash(x.ToString())).ToList();
            while (transactions.Count > 1)
            {
                List<string> level = new List<string>();
                int n = transactions.Count / 2;
                for (int i = 0; i < n - 1; i++)
                {
                    level.Add(Crypto.Hash(Crypto.ToStrASCII(transactions[0]) + Crypto.ToStrASCII(transactions[1])));
                    transactions.RemoveRange(0, 2);
                }
                if (transactions.Any())
                {
                    level.Add(Crypto.Hash(Crypto.ToStrASCII(transactions[0]) + Crypto.ToStrASCII(transactions[0])));
                    transactions.RemoveAt(0);
                }
                transactions = level;
            }
            return transactions[0];
        }
    }
    
    public struct Block
    {
        public int index;
        public string timestamp;
        public string merkleRoot;
        public string ownHash;
        public string prevHash;
        public Block(int i, string t, List<Transaction> tr, string pr)
        {
            index = i;
            timestamp = t;
            merkleRoot = Transaction.Merkle(tr);
            prevHash = pr;
            ownHash = "";
        }
        public void ComputeHash()
        {
            ownHash = "";
            ownHash = Crypto.Hash(ToString());
        }
        public override string ToString()
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }

    }
    public class Blockchain
    {
        public static List<string> chain = new List<string>();
        List<Transaction> transactions;
        public Blockchain()
        {
            transactions = new List<Transaction>();
        }

        public void AddTransaction(string re, decimal a, TranType t, string m, string v, string r, string s)
        {
            transactions.Add(new Transaction(re, a, t, m, v, r, s));        
        }

        public void MineBlock()
        {
            //have to send a message to the sender and recipient for the transaction has passed succesfully
            string prevHash = chain.Count > 0 ? chain.ElementAt(chain.Count - 2) : "0";
            string timestamp = DateTime.Now.ToString();
            Block block = new Block(chain.Count, timestamp, transactions, prevHash);
            block.ComputeHash();
            chain.Add(block.ToString());
        }
    }
}
