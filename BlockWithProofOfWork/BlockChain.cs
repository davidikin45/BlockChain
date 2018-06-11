using System;
using System.Collections.Generic;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public class BlockChain : IBlockChain<IClaimTransaction>
    {
        public IBlock<IClaimTransaction> CurrentBlock { get; private set; }
        public IBlock<IClaimTransaction> HeadBlock { get; private set; }

        public List<IBlock<IClaimTransaction>> Chain { get; }

        public BlockChain()
        {
            Chain = new List<IBlock<IClaimTransaction>>();
        }

        public void AcceptBlock(IBlock<IClaimTransaction> block)
        {
            // This is the first block, so make it the genesis block.
            if (HeadBlock == null)
            {
                HeadBlock = block;
                HeadBlock.PreviousBlockHash = null;
            }

            CurrentBlock = block;
            Chain.Add(block);
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

                    if (new KeyStore(transaction.FromAddressPublicKey).ExportPublicAddress() == address)
                    {
                        balance -= transaction.SettlementAmount;
                    }

                    if (transaction.ToAddress == address)
                    {
                        balance += transaction.SettlementAmount;
                    }
                }
            }

            return balance;
        }
    }
}
