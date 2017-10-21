using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web.Script.Serialization;

namespace Blockchain
{
    public struct Transaction
    {
        public string sender;
        public string recipient;
        public decimal amount;
        public Transaction(string s, string r, decimal a)
        {
            sender = s;
            recipient = r;
            amount = a;
            
        }
        public override string ToString()
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }
    }

    
    public struct Block
    {
        public int index;
        public string timestamp;
        public List<Transaction> transactions;
        public string prevHash;
        public Block(int i, string t, List<Transaction> tr, string pr)
        {
            index = i;
            timestamp = t;
            transactions = tr.Select(x => x).ToList();
            prevHash = pr;
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

        public void AddTransaction(string s, string r, decimal a)
        {
            transactions.Add(new Transaction(s, r, a));        
        }
        static string ToStr(byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }

        public Block MineBlock()
        {
            //have to send a message to the sender and recipient for the transaction has passed succesfully
            string prevHash = chain.Count > 0 ? chain.ElementAt(chain.Count - 2) : "0";
            string timestamp = DateTime.Now.ToString();
            Block block = new Block(chain.Count, timestamp, transactions, prevHash);
            var sha = new SHA512CryptoServiceProvider();
            Encoding u8 = Encoding.ASCII;
            byte[] bl = u8.GetBytes(block.ToString());
            string hash = ToStr(sha.ComputeHash(bl), false);
            Console.WriteLine(hash);
            chain.Add(hash);
            return block;
        }
    }
}
