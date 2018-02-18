﻿using System;

namespace BlockChainCourse.BlockWithProofOfWork
{
    public interface IClaimTransaction : IAddressTransaction
    {
        string ClaimNumber { get; set; }
        decimal SettlementAmount { get; set; }
        DateTime SettlementDate { get; set; }
        string CarRegistration { get; set; }
        int Mileage { get; set; }
        ClaimType ClaimType { get; set; }
    }
}