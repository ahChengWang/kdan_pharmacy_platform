using System;

namespace PharmacyMask.DomainService.Entity
{
    public class PurchaseHistoryEntity
    {
        public string PharmacyName { get; set; }
        public string Remark { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
