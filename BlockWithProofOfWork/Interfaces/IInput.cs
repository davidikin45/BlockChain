using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChainCourse.BlockWithProofOfWork.Interfaces
{
    public interface IInput
    {
        //input
        string FromTransactionId { get;  }
        IOutput FromOutput { get; }
        string FromAddress { get;  }

        string Signature { get; } //scriptSig
        string FromAddressPublicKey { get;  } //scriptSig

        void SetSignature(string transactionHash, IKeyStore KeyStoreFromAddress);
        string GetDataForHash();

        bool IsValid(string transactionHash, bool verbose);
    }
}
