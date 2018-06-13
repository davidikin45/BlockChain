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

        static string pubAddress1;

        static void Main(string[] args)
        {
            IAddressTransaction txn5 = SetupTransactions();

            //Only needed if we want it to be a private blockchain
            //IKeyStore keyStore = new KeyStore(Hmac.GenerateKey());
            IKeyStore keyStore = null;

            IBlock<IAddressTransaction> block1 = new Block(0, keyStore, 3);
            IBlock<IAddressTransaction> block2 = new Block(1, keyStore, 3);
            IBlock<IAddressTransaction> block3 = new Block(2, keyStore, 3);
            IBlock<IAddressTransaction> block4 = new Block(3, keyStore, 3);

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
            Console.WriteLine($"{pubAddress1}' balance: {chain.GetBalance(pubAddress1)}");

            ((IClaimOutput)txn5.Outputs[0]).ClaimNumber = "weqwewe";
            chain.VerifyChain();

            Console.WriteLine();

            Console.WriteLine("=========================");
            Console.WriteLine($"Chain");
            Console.WriteLine(JsonConvert.SerializeObject(chain, Formatting.Indented));

            Console.ReadKey();
        }

        private static void AddTransactionsToBlocksAndCalculateHashes(IBlock<IAddressTransaction> block1, IBlock<IAddressTransaction> block2, IBlock<IAddressTransaction> block3, IBlock<IAddressTransaction> block4)
        {
            block1.AddTransaction(txnPool.GetTransaction());

            block2.AddTransaction(txnPool.GetTransaction());

            block3.AddTransaction(txnPool.GetTransaction());

            block4.AddTransaction(txnPool.GetTransaction());

            block1.SetBlockHash(null);
            block2.SetBlockHash(block1);
            block3.SetBlockHash(block2);
            block4.SetBlockHash(block3);
        }

        private static IAddressTransaction SetupTransactions()
        {
            IKeyStore address0 = new KeyStore(Hmac.GenerateKey());
            var pubAddress0 = address0.ExportPublicAddress();

            IKeyStore address1 = new KeyStore(Hmac.GenerateKey());
            pubAddress1 = address1.ExportPublicAddress();

            IKeyStore address2 = new KeyStore(Hmac.GenerateKey());
            var pubAddress2 = address2.ExportPublicAddress();

            IKeyStore address3 = new KeyStore(Hmac.GenerateKey());
            var pubAddress3 = address3.ExportPublicAddress();

            IKeyStore address4 = new KeyStore(Hmac.GenerateKey());
            var pubAddress4 = address4.ExportPublicAddress();

            IAddressTransaction txn1 = new AddressTransaction();
            txn1.Inputs.Add(new Input(null,null));
            txn1.Outputs.Add(new ClaimOutput(pubAddress1, "ABC123", 100.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss));
            txn1.Outputs.Add(new ClaimOutput(pubAddress1, "ABC123", 200.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss));
            txn1.SetTransactionHash(null);
            txn1.Inputs[0].SetSignature(txn1.TransactionId, address0);

           IAddressTransaction txn2 = new AddressTransaction();
            txn2.Inputs.Add(new Input(txn1.TransactionId, txn1.Outputs[0]));
            txn2.Outputs.Add(new ClaimOutput(pubAddress2, "ABC123", 100.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss));
            txn2.SetTransactionHash(null);
            txn2.Inputs[0].SetSignature(txn2.TransactionId, address1);

            IAddressTransaction txn3 = new AddressTransaction();
            txn3.Inputs.Add(new Input(txn1.TransactionId, txn2.Outputs[0]));
            txn3.Outputs.Add(new ClaimOutput(pubAddress3, "ABC123", 100.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss));
            txn3.SetTransactionHash(null);
            txn3.Inputs[0].SetSignature(txn3.TransactionId, address2);

            IAddressTransaction txn4 = new AddressTransaction();
            txn4.Inputs.Add(new Input(txn1.TransactionId, txn3.Outputs[0]));
            txn4.Outputs.Add(new ClaimOutput(pubAddress4, "ABC123", 100.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss));
            txn4.SetTransactionHash(null);
            txn4.Inputs[0].SetSignature(txn4.TransactionId, address3);

            txn1.IsValid(true);
            txn2.IsValid(true);
            txn3.IsValid(true);
            txn4.IsValid(true);


            txnPool.AddTransaction(txn1);
            txnPool.AddTransaction(txn2);
            txnPool.AddTransaction(txn3);
            txnPool.AddTransaction(txn4);

            return txn4;
        }
    }
}
