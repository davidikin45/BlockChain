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
        public string TransactionSignature { get; private set; }
        public IKeyStore KeyStoreFromAddress { get; private set; }

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

        public ClaimTransaction(IKeyStore keyStoreFromAddress,
                        string toAddress,
                        string claimNumber,
                        decimal settlementAmount,
                        DateTime settlementDate,
                        string carRegistration,
                        int mileage,
                        ClaimType claimType)
        {
            KeyStoreFromAddress = keyStoreFromAddress;

            ToAddress = toAddress;

            ClaimNumber = claimNumber;
            SettlementAmount = settlementAmount;
            SettlementDate = settlementDate;
            CarRegistration = carRegistration;
            Mileage = mileage;
            ClaimType = claimType;

            CreatedDate = DateTime.UtcNow;
        }

        public void SetTransactionHash()
        {
            SetTransactionHash(null);
        }

        public void SetTransactionHash(ITransaction parent)
        {
            if (parent != null)
            {
                PreviousTransactionId = parent.TransactionId;
            }
            else
            {
                // Previous transactionid is the genesis block.
                PreviousTransactionId = null;
            }

            var transactionHash = CalculateTransactionHash(PreviousTransactionId);
            TransactionId = transactionHash;

            if (KeyStoreFromAddress != null)
            {
                TransactionSignature = KeyStoreFromAddress.Sign(transactionHash);
            }
        }

        public string CalculateTransactionHash()
        {
            return CalculateTransactionHash(null);
        }

        public string CalculateTransactionHash(string previousTransactionId)
        {
            string blockheader = CreatedDate.ToString() + previousTransactionId;
            string transactionHash = ToAddress + ClaimNumber + SettlementAmount + SettlementDate + CarRegistration + Mileage + ClaimType;
            string combined = transactionHash + blockheader;

            string completeTransactionHash;

            if (KeyStoreFromAddress == null)
            {
                completeTransactionHash = Convert.ToBase64String(HashData.ComputeHashSha256(Encoding.UTF8.GetBytes(combined)));
            }
            else
            {
                completeTransactionHash = Convert.ToBase64String(Hmac.ComputeHmacsha256(Encoding.UTF8.GetBytes(combined), KeyStoreFromAddress.AuthenticatedHashKey));
            }

            return completeTransactionHash;
        }
    }
}
