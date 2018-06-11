using System;
using BlockChainCourse.Cryptography;
using Newtonsoft.Json;

namespace BlockChainCourse.BlockWithProofOfWork
{
    class Program
    {
        static readonly TransactionPool txnPool = new TransactionPool();

        ///
        /// Single block with a multiple transactions, in an immutable chain with transactions taken from a transaction pool.
        /// Hashing with HMAC(SHA-256) and a Digital Signature
        ///

        static string pubAddress2;

        static void Main(string[] args)
        {
            IClaimTransaction txn5 = SetupTransactions();

            //Only needed if we want it to be a private blockchain
            //IKeyStore keyStore = new KeyStore(Hmac.GenerateKey());
            IKeyStore keyStore = null;

            IBlock<IClaimTransaction> block1 = new Block(0, keyStore, 3);
            IBlock<IClaimTransaction> block2 = new Block(1, keyStore, 3);
            IBlock<IClaimTransaction> block3 = new Block(2, keyStore, 3);
            IBlock<IClaimTransaction> block4 = new Block(3, keyStore, 3);

            AddTransactionsToBlocksAndCalculateHashes(block1, block2, block3, block4);

            BlockChain chain = new BlockChain();
            chain.AcceptBlock(block1);
            chain.AcceptBlock(block2);
            chain.AcceptBlock(block3);
            chain.AcceptBlock(block4);

            chain.VerifyChain();

            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine("=========================");
            Console.WriteLine($"{pubAddress2}' balance: {chain.GetBalance(pubAddress2)}");

            txn5.ClaimNumber = "weqwewe";
            chain.VerifyChain();

            Console.WriteLine();

            Console.WriteLine("=========================");
            Console.WriteLine($"Chain");
            Console.WriteLine(JsonConvert.SerializeObject(chain, Formatting.Indented));

            Console.ReadKey();
        }

        private static void AddTransactionsToBlocksAndCalculateHashes(IBlock<IClaimTransaction> block1, IBlock<IClaimTransaction> block2, IBlock<IClaimTransaction> block3, IBlock<IClaimTransaction> block4)
        {
            block1.AddTransaction(txnPool.GetTransaction());

            block2.AddTransaction(txnPool.GetTransaction());
            block2.AddTransaction(txnPool.GetTransaction());
            block2.AddTransaction(txnPool.GetTransaction());
            block2.AddTransaction(txnPool.GetTransaction());
            block2.AddTransaction(txnPool.GetTransaction());
            block2.AddTransaction(txnPool.GetTransaction());
            block2.AddTransaction(txnPool.GetTransaction());

            block3.AddTransaction(txnPool.GetTransaction());
            block3.AddTransaction(txnPool.GetTransaction());
            block3.AddTransaction(txnPool.GetTransaction());
            block3.AddTransaction(txnPool.GetTransaction());

            block4.AddTransaction(txnPool.GetTransaction());
            block4.AddTransaction(txnPool.GetTransaction());
            block4.AddTransaction(txnPool.GetTransaction());
            block4.AddTransaction(txnPool.GetTransaction());

            block1.SetBlockHash(null);
            block2.SetBlockHash(block1);
            block3.SetBlockHash(block2);
            block4.SetBlockHash(block3);
        }

        private static IClaimTransaction SetupTransactions()
        {
            IKeyStore address0 = new KeyStore(Hmac.GenerateKey());
            var pubAddress0 = address0.ExportPublicAddress();

            IKeyStore address1 = new KeyStore(Hmac.GenerateKey());
            var pubAddress1 = address1.ExportPublicAddress();

            IKeyStore address2 = new KeyStore(Hmac.GenerateKey());
            pubAddress2 = address2.ExportPublicAddress();

            IClaimTransaction txn1 = new ClaimTransaction(pubAddress0, pubAddress1, "ABC123", 1000000000.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss);
            IClaimTransaction txn2 = new ClaimTransaction(pubAddress1, pubAddress2, "VBG345", 2000.00m, DateTime.Now, "JKH567", 20000, ClaimType.TotalLoss);
            IClaimTransaction txn3 = new ClaimTransaction(pubAddress1, pubAddress2, "XCF234", 3000.00m, DateTime.Now, "DH23ED", 30000, ClaimType.TotalLoss);
            IClaimTransaction txn4 = new ClaimTransaction(pubAddress1, pubAddress2, "CBHD45", 4000.00m, DateTime.Now, "DH34K6", 40000, ClaimType.TotalLoss);
            IClaimTransaction txn5 = new ClaimTransaction(pubAddress1, pubAddress2, "AJD345", 5000.00m, DateTime.Now, "28FNF4", 50000, ClaimType.TotalLoss);
            IClaimTransaction txn6 = new ClaimTransaction(pubAddress1, pubAddress2, "QAX367", 6000.00m, DateTime.Now, "FJK676", 60000, ClaimType.TotalLoss);
            IClaimTransaction txn7 = new ClaimTransaction(pubAddress1, pubAddress2, "CGO444", 7000.00m, DateTime.Now, "LKU234", 70000, ClaimType.TotalLoss);
            IClaimTransaction txn8 = new ClaimTransaction(pubAddress1, pubAddress2, "PLO254", 8000.00m, DateTime.Now, "VBN456", 80000, ClaimType.TotalLoss);
            IClaimTransaction txn9 = new ClaimTransaction(pubAddress1, pubAddress2, "ABC123", 1000.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss);
            IClaimTransaction txn10 = new ClaimTransaction(pubAddress1, pubAddress2, "VBG345", 2000.00m, DateTime.Now, "JKH567", 20000, ClaimType.TotalLoss);
            IClaimTransaction txn11 = new ClaimTransaction(pubAddress1, pubAddress2, "XCF234", 3000.00m, DateTime.Now, "DH23ED", 30000, ClaimType.TotalLoss);
            IClaimTransaction txn12 = new ClaimTransaction(pubAddress1, pubAddress2, "CBHD45", 4000.00m, DateTime.Now, "DH34K6", 40000, ClaimType.TotalLoss);
            IClaimTransaction txn13 = new ClaimTransaction(pubAddress1, pubAddress2, "AJD345", 5000.00m, DateTime.Now, "28FNF4", 50000, ClaimType.TotalLoss);
            IClaimTransaction txn14 = new ClaimTransaction(pubAddress1, pubAddress2, "QAX367", 6000.00m, DateTime.Now, "FJK676", 60000, ClaimType.TotalLoss);
            IClaimTransaction txn15 = new ClaimTransaction(pubAddress1, pubAddress2, "CGO444", 7000.00m, DateTime.Now, "LKU234", 70000, ClaimType.TotalLoss);
            IClaimTransaction txn16 = new ClaimTransaction(pubAddress1, pubAddress2, "PLO254", 8000.00m, DateTime.Now, "VBN456", 80000, ClaimType.TotalLoss);

            txn1.SetTransactionHash(null, address0);
            txn2.SetTransactionHash(txn1, address1);
            txn3.SetTransactionHash(txn1, address1);
            txn4.SetTransactionHash(txn1, address1);
            txn5.SetTransactionHash(txn1, address1);
            txn6.SetTransactionHash(txn1, address1);
            txn7.SetTransactionHash(txn1, address1);
            txn8.SetTransactionHash(txn1, address1);
            txn9.SetTransactionHash(txn1, address1);
            txn10.SetTransactionHash(txn1, address1);
            txn11.SetTransactionHash(txn1, address1);
            txn12.SetTransactionHash(txn1, address1);
            txn13.SetTransactionHash(txn1, address1);
            txn14.SetTransactionHash(txn1, address1);
            txn15.SetTransactionHash(txn1, address1);
            txn16.SetTransactionHash(txn1, address1);

            txnPool.AddTransaction(txn1);
            txnPool.AddTransaction(txn2);
            txnPool.AddTransaction(txn3);
            txnPool.AddTransaction(txn4);
            txnPool.AddTransaction(txn5);
            txnPool.AddTransaction(txn6);
            txnPool.AddTransaction(txn7);
            txnPool.AddTransaction(txn8);
            txnPool.AddTransaction(txn9);
            txnPool.AddTransaction(txn10);
            txnPool.AddTransaction(txn11);
            txnPool.AddTransaction(txn12);
            txnPool.AddTransaction(txn13);
            txnPool.AddTransaction(txn14);
            txnPool.AddTransaction(txn15);
            txnPool.AddTransaction(txn16);

            return txn5;
        }
    }
}
