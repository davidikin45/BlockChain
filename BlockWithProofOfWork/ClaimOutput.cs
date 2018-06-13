using System;
using System.Text;
using BlockChainCourse.Cryptography;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public class ClaimOutput : IClaimOutput
    {
        public string ToAddress { get; set; }

        public string ClaimNumber { get; set; }
        public decimal SettlementAmount { get; set; }
        public DateTime SettlementDate { get; set; }
        public string CarRegistration { get; set; }
        public int Mileage { get; set; }
        public ClaimType ClaimType { get; set; }

        public ClaimOutput(string toAddress,
                        string claimNumber,
                        decimal settlementAmount,
                        DateTime settlementDate,
                        string carRegistration,
                        int mileage,
                        ClaimType claimType)
        {

            ToAddress = toAddress;

            ClaimNumber = claimNumber;
            SettlementAmount = settlementAmount;
            SettlementDate = settlementDate;
            CarRegistration = carRegistration;
            Mileage = mileage;
            ClaimType = claimType;
        }

        public string GetDataForHash()
        {
           return ToAddress + ClaimNumber + SettlementAmount + SettlementDate + CarRegistration + Mileage + ClaimType;
        }
    }
}
