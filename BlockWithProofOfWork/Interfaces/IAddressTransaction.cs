using System;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public interface IAddressTransaction : ITransaction
    {
        //input
        string PreviousTransactionId { get; set; }
        IAddressTransaction PreviousTransaction { get; set; }
        string TransactionSignature { get; }
        //IKeyStore KeyStoreFromAddress { get; } //This should be 
        string FromAddressPublicKey { get; }

        //output
        string ToAddress { get; } //ToAddress publickey

        void SetTransactionHash(IAddressTransaction parent, IKeyStore KeyStoreFromAddress);
        bool IsValidChain(string prevBlockHash, bool verbose);
    }
}
