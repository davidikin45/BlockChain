using System;
using System.Text;
using BlockChainCourse.Cryptography;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public class KeyStore : IKeyStore
    {
        private DigitalSignature DigitalSignature { get; set; }
        public byte[] AuthenticatedHashKey { get; private set; }

        public KeyStore(byte[] authenticatedHashKey)
        {
            AuthenticatedHashKey = authenticatedHashKey;
            DigitalSignature = new DigitalSignature();
            DigitalSignature.AssignNewKey();
        }

        public string Sign(string hash)
        {
            return Convert.ToBase64String(DigitalSignature.SignData(Convert.FromBase64String(hash)));
        }

        public bool Verify(string hash, string signature)
        {
            return DigitalSignature.VerifySignature(Convert.FromBase64String(hash), Convert.FromBase64String(signature));  
        }
    }
}
