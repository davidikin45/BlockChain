using System;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public interface IAddressTransaction : ITransaction
    {
        string ToAddress { get; }
        string PreviousTransactionId { get; set; }
        string TransactionSignature { get; }

        IKeyStore KeyStoreFromAddress { get; }

        void SetTransactionHash(ITransaction parent);
    }
}
