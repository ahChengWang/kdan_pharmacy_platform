using System;

namespace PharmacyMask.DomainService.Entity
{
    public class UserTransactionSummaryEntity
    {
        public DateTime TransactionDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal TotalTransactionAmount { get; set; }
    }
}
