using BlockChainCourse.BlockWithProofOfWork.Interfaces;
using System;
using System.Collections.Generic;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public interface IAddressTransaction : ITransaction
    {
        IList<IInput> Inputs { get; }
        IList<IOutput> Outputs { get; }

        void SetTransactionHash(IKeyStore KeyStoreFromAddress);
        bool IsValid(bool verbose);
    }
}
