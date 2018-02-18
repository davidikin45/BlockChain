using System;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public interface ITransaction
    {
        string TransactionId { get; }
        DateTime CreatedDate { get; set; }
        void SetTransactionHash();
        string CalculateTransactionHash();
    }
}
