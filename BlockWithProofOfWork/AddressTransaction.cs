using System;
using System.Collections.Generic;
using System.Text;
using BlockChainCourse.BlockWithProofOfWork.Interfaces;
using BlockChainCourse.Cryptography;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public class AddressTransaction : IAddressTransaction
    {
        public DateTime CreatedDate { get; set; }
        public string TransactionId { get; private set; }

        public IList<IInput> Inputs {get; set; }
        public IList<IOutput> Outputs { get; set; }

        public AddressTransaction()
        {
            Inputs = new List<IInput>();
            Outputs = new List<IOutput>();
            CreatedDate = DateTime.UtcNow;
        }

        public void SetTransactionHash(IKeyStore KeyStoreFromAddress)
        {
            var transactionHash = CalculateTransactionHash(KeyStoreFromAddress);
            TransactionId = transactionHash;
        }

        public string CalculateTransactionHash(IKeyStore KeyStoreFromAddress)
        {
            string blockheader = CreatedDate.ToString();
            string transactionHash = "";
            foreach (var item in Inputs)
            {
                transactionHash += item.GetDataForHash();
            }
            foreach (var item in Outputs)
            {
                transactionHash += item.GetDataForHash();
            }
            string combined = transactionHash + blockheader;

            string completeTransactionHash;

            if (KeyStoreFromAddress == null || true)
            {
                completeTransactionHash = Convert.ToBase64String(HashData.ComputeHashSha256(Encoding.UTF8.GetBytes(combined)));
            }
            else
            {
                //Not sure need this
                completeTransactionHash = Convert.ToBase64String(Hmac.ComputeHmacSha256(Encoding.UTF8.GetBytes(combined), KeyStoreFromAddress.AuthenticatedHashKey));
            }

            return completeTransactionHash;
        }

        public bool IsValid(bool verbose)
        {
            var isValid = true;

            // Is this a valid block and transaction
            string newTransactionHash = CalculateTransactionHash(null);
            //string newTransactionHash = Convert.ToBase64String(HashData.ComputeHashSha256(Encoding.UTF8.GetBytes(CalculateTransactionHash(prevTransactionId))));

            if (newTransactionHash != TransactionId)
            {
                isValid = false;
            }
            else
            {
                // Does the previous block hash match the latest previ0ous block hash
                foreach (var input in Inputs)
                {
                    isValid = isValid && input.IsValid(newTransactionHash, verbose);
                }
            }

            PrintVerificationMessage(verbose, isValid);

            return isValid;
        }

        private void PrintVerificationMessage(bool verbose, bool isValid)
        {
            if (verbose)
            {
                if (!isValid)
                {
                    Console.WriteLine("Transaction " + TransactionId + " : FAILED VERIFICATION");
                }
                else
                {
                    Console.WriteLine("Transaction " + TransactionId + " : PASS VERIFICATION");
                }
            }
        }
    }
}
