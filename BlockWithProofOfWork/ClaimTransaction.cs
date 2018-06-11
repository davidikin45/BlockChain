using System;
using System.Text;
using BlockChainCourse.Cryptography;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public class ClaimTransaction : IClaimTransaction
    {
        public DateTime CreatedDate { get; set; }
        public string TransactionId { get; private set; }
        public string PreviousTransactionId { get; set; }
        public IAddressTransaction PreviousTransaction { get; set; }
        public string TransactionSignature { get; private set; }
      //  public IKeyStore KeyStoreFromAddress { get; private set; }
        public string FromAddressPublicKey { get; private set; }

        public string ToAddress { get; set; }

        public string ClaimNumber { get; set; }
        public decimal SettlementAmount { get; set; }
        public DateTime SettlementDate { get; set; }
        public string CarRegistration { get; set; }
        public int Mileage { get; set; }
        public ClaimType ClaimType { get; set; }

        public ClaimTransaction(string toAddress,
                           string claimNumber,
                           decimal settlementAmount,
                           DateTime settlementDate,
                           string carRegistration,
                           int mileage,
                           ClaimType claimType)
        : this(null, toAddress, claimNumber, settlementAmount, settlementDate, carRegistration, mileage, claimType)
        {

        }

        public ClaimTransaction(string fromAddressPublicKey,
                        string toAddress,
                        string claimNumber,
                        decimal settlementAmount,
                        DateTime settlementDate,
                        string carRegistration,
                        int mileage,
                        ClaimType claimType)
        {
            FromAddressPublicKey = fromAddressPublicKey;

            ToAddress = toAddress;

            ClaimNumber = claimNumber;
            SettlementAmount = settlementAmount;
            SettlementDate = settlementDate;
            CarRegistration = carRegistration;
            Mileage = mileage;
            ClaimType = claimType;

            CreatedDate = DateTime.UtcNow;
        }

        public void SetTransactionHash(IKeyStore KeyStoreFromAddress)
        {
            SetTransactionHash(null, KeyStoreFromAddress);
        }

        public void SetTransactionHash(IAddressTransaction parent, IKeyStore KeyStoreFromAddress)
        {
            if (parent != null)
            {
                PreviousTransactionId = parent.TransactionId;
                PreviousTransaction = parent;
            }
            else
            {
                // Previous transactionid is the genesis block.
                PreviousTransactionId = null;
                PreviousTransaction = null;
            }

            var transactionHash = CalculateTransactionHash(PreviousTransactionId, KeyStoreFromAddress);
            TransactionId = transactionHash;

            if (KeyStoreFromAddress != null)
            {
                TransactionSignature = KeyStoreFromAddress.Sign(transactionHash);
            }
        }

        public string CalculateTransactionHash(IKeyStore keyStoreFromAddress)
        {
            return CalculateTransactionHash(null, keyStoreFromAddress);
        }

        public string CalculateTransactionHash(string previousTransactionId, IKeyStore KeyStoreFromAddress)
        {
            string blockheader = CreatedDate.ToString() + previousTransactionId;
            string transactionHash = ToAddress + FromAddressPublicKey + ClaimNumber + SettlementAmount + SettlementDate + CarRegistration + Mileage + ClaimType;
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

        public bool IsValidChain(bool verbose)
        {
            var root = (IAddressTransaction)this;
            while (root.PreviousTransaction != null)
            {
                root = root.PreviousTransaction;
            }

            return root.IsValidChain(null, verbose);
        }

        public bool IsValidChain(string prevTransactionId, bool verbose)
        {
            bool isValid = true;
            bool validSignature = false;
            bool validPublicKey = false;

            var keyStoreFromAddress = new KeyStore(FromAddressPublicKey);
            if (PreviousTransaction != null)
            {
                if (keyStoreFromAddress.ExportPublicKeyToX509PEM() == PreviousTransaction.ToAddress)
                {
                    validPublicKey = true;
                }
            }
            else
            {
                validPublicKey = true;
            }

            validSignature = keyStoreFromAddress.Verify(TransactionId, TransactionSignature);

            // Is this a valid block and transaction
            string newTransactionHash = CalculateTransactionHash(prevTransactionId, null);
            //string newTransactionHash = Convert.ToBase64String(HashData.ComputeHashSha256(Encoding.UTF8.GetBytes(CalculateTransactionHash(prevTransactionId))));

            validSignature = keyStoreFromAddress.Verify(newTransactionHash, TransactionSignature);

            if (newTransactionHash != TransactionId)
            {
                isValid = false;
            }
            else
            {
                // Does the previous block hash match the latest previ0ous block hash
                isValid |= PreviousTransactionId == prevTransactionId;
            }

            PrintVerificationMessage(verbose, isValid, validSignature, validPublicKey);

            // Check the next block by passing in our newly calculated blockhash. This will be compared to the previous
            // hash in the next block. They should match for the chain to be valid.
            if (PreviousTransaction != null)
            {
                return PreviousTransaction.IsValidChain(newTransactionHash, verbose);
            }

            return isValid;
        }

        private void PrintVerificationMessage(bool verbose, bool isValid, bool validSignature, bool validPublicKey)
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

                if (!validSignature)
                {
                    Console.WriteLine("Transaction " + TransactionId + " : Invalid Digital Signature");
                }

                if (!validPublicKey)
                {
                    Console.WriteLine("Transaction " + TransactionId + " : Invalid Public Key");
                }
            }
        }
    }
}
