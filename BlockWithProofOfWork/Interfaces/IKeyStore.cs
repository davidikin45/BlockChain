using System.Security.Cryptography;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public interface IKeyStore
    {
        byte[] AuthenticatedHashKey { get; }
        string Sign(string hash);
        bool Verify(string hash, string signature);
        string ExportPublicAddress();
    }
}
