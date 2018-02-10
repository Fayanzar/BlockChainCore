using System;
using System.Collections.Generic;
using System.Linq;
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
        public string from;
        public string r;
        public string s;
        public Transaction(string re, decimal a, TranType t, string m)
        {
            recipient = re;
            amount = a;
            tranType = t;
            message = m;
            r = "";
            s = "";
            from = "";
        }
        public Transaction Sign(string privKey)
        {
            Point G = ECDSAParameters.basePoint;
            NumFinite n = ECDSAParameters.order;
            NumFinite rn = 0;
            NumOrder sn = 0;
            NumFinite k = 0;
            NumFinite z = Crypto.Hash(ToString());
            do
            {
                do
                {
                    k = NumFinite.BigRandom(ECDSAParameters.order);
                    var XY = G * k;
                    rn = XY.x % n;
                } while (rn.num == 0);
                sn = (((NumOrder)z + (NumOrder)rn * privKey) / (NumOrder)k);
            } while (sn.num == 0);
            Transaction tr = new Transaction(recipient, amount, tranType, message);
            tr.r = rn.ToString();
            tr.s = sn.ToString();

            var pubKey = G * privKey;
            string x = pubKey.x.ToString().Length == 64 ? pubKey.x.ToString() : new string(Enumerable.Repeat('0', 64 - pubKey.x.ToString().Length).ToArray()) + pubKey.x.ToString();
            string y = pubKey.y.ToString().Length == 64 ? pubKey.y.ToString() : new string(Enumerable.Repeat('0', 64 - pubKey.y.ToString().Length).ToArray()) + pubKey.y.ToString();
            tr.from = x + y;

            return tr;
        }

        public bool Verify(string privKey)
        {
            Transaction add = new Transaction(recipient, amount, tranType, message);
            NumOrder z = Crypto.Hash(add.ToString());
            var G = ECDSAParameters.basePoint;
            NumOrder rn = r;
            NumOrder sn = s;
            if (rn.num.IsZero || rn.num >= ECDSAParameters.order)
                return false;
            if (sn.num.IsZero || sn.num >= ECDSAParameters.order)
                return false;

            NumOrder n = ECDSAParameters.order;
            var w = (1 / sn) % n;
            var u = (z * w) % n;
            var v = (rn * w) % n;
            string x = from.Remove(64, 64);
            string y = from.Substring(64);
            Point Q = new Point(x, y);

            var XY = G * (NumFinite)u + Q * (NumFinite)v;

            return (rn.num == (XY.x % (NumFinite)n).num);
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
        public List<Transaction> transactions;
        public Block(int i, string t, List<Transaction> tr, string pr)
        {
            index = i;
            timestamp = t;
            merkleRoot = Transaction.Merkle(tr);
            prevHash = pr;
            ownHash = "";
            transactions = tr;
        }
        public void ComputeHash()
        {
            ownHash = "";
            List<Transaction> trAdd = transactions.Select(x => x).ToList();
            transactions = new List<Transaction>();
            ownHash = Crypto.Hash(ToString());
            transactions = trAdd;
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
            transactions.Add(new Transaction(re, a, t, m));        
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
