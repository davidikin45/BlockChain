using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChainCourse.BlockWithProofOfWork.Interfaces
{
    public interface IOutput
    {
        string ToAddress { get; } //ToAddress publickey scriptPubKey
        string GetDataForHash();
    }
}
