using System;
using System.Collections.Generic;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public class BlockChain : IBlockChain<IAddressTransaction>
    {
        public Dictionary<string, IAddressTransaction> Transactions { get; private set; }

        public IBlock<IAddressTransaction> CurrentBlock { get; private set; }
        public IBlock<IAddressTransaction> HeadBlock { get; private set; }

        public List<IBlock<IAddressTransaction>> Chain { get; }

        public BlockChain()
        {
            Transactions = new Dictionary<string, IAddressTransaction>();
            Chain = new List<IBlock<IAddressTransaction>>();
        }

        public void AcceptBlock(IBlock<IAddressTransaction> block)
        {
            // This is the first block, so make it the genesis block.
            if (HeadBlock == null)
            {
                HeadBlock = block;
                HeadBlock.PreviousBlockHash = null;
            }

            foreach (var transaction in block.Transactions)
            {
                Transactions.Add(transaction.TransactionId, transaction);
            }

            CurrentBlock = block;
            Chain.Add(block);
        }

        public IAddressTransaction GetTransaction(string transactionId)
        {
            return Transactions[transactionId];
        }

        public int NextBlockNumber
        {
            get
            {
                if (HeadBlock == null)
                { 
                    return 0; 
                }

                return CurrentBlock.BlockNumber + 1;
            }
        }

        public void VerifyChain()
        {
            if (HeadBlock == null)
            {
                throw new InvalidOperationException("Genesis block not set.");
            }

            bool isValid = HeadBlock.IsValidChain(null, true);

            if (isValid)
            {
                Console.WriteLine("Blockchain integrity intact.");
            }
            else
            {
                Console.WriteLine("Blockchain integrity NOT intact.");
            }
        }

        public decimal GetBalance(string address)
        {
            decimal balance = 0;

            for (int i = 0; i < Chain.Count; i++)
            {
                for (int j = 0; j < Chain[i].Transactions.Count; j++)
                {
                    var transaction = Chain[i].Transactions[j];

                    foreach (var input in transaction.Inputs)
                    {
                        if (input.FromAddress == address)
                        {
                            balance -= ((IClaimOutput)input.FromOutput).SettlementAmount;
                        }
                    }

                    foreach (var output in transaction.Outputs)
                    {
                        if (output.ToAddress == address)
                        {
                            balance += ((IClaimOutput)output).SettlementAmount;
                        }
                    }
                }
            }

            return balance;
        }
    }
}
