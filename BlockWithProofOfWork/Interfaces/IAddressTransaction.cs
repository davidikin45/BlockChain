using System;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public interface IAddressTransaction : ITransaction
    {
        string ToAddress { get; }
        string PreviousTransactionId { get; set; }
        IAddressTransaction PreviousTransaction { get; set; }
        string TransactionSignature { get; }

        IKeyStore KeyStoreFromAddress { get; }

        void SetTransactionHash(IAddressTransaction parent);
        bool IsValidChain(string prevBlockHash, bool verbose);
    }
}
