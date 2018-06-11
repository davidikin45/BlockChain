using BlockChainCourse.Cryptography.AssymetricEncryption;
using System;
using System.Security.Cryptography;

namespace BlockChainCourse.Cryptography
{
    public class DigitalSignature
    {
        private string _publicKey;
        private string _privateKey;

        public void AssignNewKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;

                _publicKey = Crypto.ExportPublicKeyToX509PEM(rsa);
                _privateKey = Crypto.ExportPrivateKeyToRSAPEM(rsa);
            }
        }

        public void AssignPublicKey(string publicKeyToX509PEM)
        {
            _publicKey = publicKeyToX509PEM;
        }

        public string ExportPublicKeyToX509PEM()
        {
            return _publicKey;
        }

        public string ExportPrivateKeyToRSAPEM()
        {
            return _privateKey;
        }

        public byte[] SignData(byte[] hashOfDataToSign)
        {
            using (var rsa = Crypto.DecodeRsaPrivateKey(_privateKey))
            {
                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");

                return rsaFormatter.CreateSignature(hashOfDataToSign);
            }
        }

        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
        {
            using (var rsa = Crypto.DecodeRsaPublicKey(_publicKey))
            {
                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");

                return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
            }
        }
    }
}
