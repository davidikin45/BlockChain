using System;
using System.Linq;
using System.Text;
using BlockChainCourse.BlockWithProofOfWork.Interfaces;
using BlockChainCourse.Cryptography;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public class Input : IInput
    {
        //User specifies these
        public string FromTransactionId { get; private set; }
        public IOutput FromOutput { get; private set; }
        public string FromAddress { get; private set; }

        public string Signature { get; private set; }
        public string FromAddressPublicKey { get; private set; }

        public Input(string previousTransactionId, IOutput fromOutput)
        {
            FromTransactionId = previousTransactionId;

            if(fromOutput!= null)
            {
                FromOutput = fromOutput;
                FromAddress = fromOutput.ToAddress;
            }
            else
            {
                FromOutput = null;
                FromAddress = null;
            }
        }

        public void SetSignature(string transactionHash, IKeyStore KeyStoreFromAddress)
        {
            FromAddressPublicKey = KeyStoreFromAddress.ExportPublicKey();

            Signature = KeyStoreFromAddress.Sign(transactionHash);
        }

        public bool IsValid(string transactionHash, bool verbose)
        {
            bool validSignature = false;
            bool validPublicKey = false;

            var keyStoreFromAddress = new KeyStore(FromAddressPublicKey, "");
            if (FromTransactionId != null)
            {
                if (keyStoreFromAddress.ExportPublicAddress() == FromAddress)
                {
                    validPublicKey = true;
                }
            }
            else
            {
                validPublicKey = true;
            }

            validSignature = keyStoreFromAddress.Verify(transactionHash, Signature);

            PrintVerificationMessage(transactionHash, verbose, validSignature, validPublicKey);

            return validPublicKey && validSignature;
        }

        private void PrintVerificationMessage(string transactionId, bool verbose, bool validSignature, bool validPublicKey)
        {
            if (verbose)
            {
                if (!validSignature)
                {
                    Console.WriteLine("Transaction " + transactionId + " Input : Invalid Digital Signature");
                }

                if (!validPublicKey)
                {
                    Console.WriteLine("Transaction " + transactionId + " Input : Invalid Public Key");
                }
            }
        }

        public string GetDataForHash()
        {
           return FromTransactionId + FromAddress;
        }
    }
}
