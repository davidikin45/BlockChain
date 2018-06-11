using System;
using System.Collections.Generic;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public interface IBlock<T> where T : ITransaction
    {
        // List of transactions
        List<T> Transactions { get; }

        // Block header data
        int BlockNumber { get; }
        DateTime CreatedDate { get; set; }
        string BlockHash { get; }
        string PreviousBlockHash { get; set; }
        string BlockSignature { get; }
        int Difficulty { get; }
        int Nonce { get; }

        void AddTransaction(T transaction);
        string CalculateBlockHash(string previousBlockHash);
        void SetBlockHash(IBlock<T> parent);
        IBlock<T> NextBlock { get; set; }
        bool IsValidChain(string prevBlockHash, bool verbose);
        IKeyStore KeyStore { get; }
    }
}
