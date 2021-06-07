using System;

namespace PharmacyMask.DomainService.Entity
{
    public class UserTransactionHistoryEntity
    {
        public DateTime TransactionDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public int MaskId { get; set; }
        public string MaskName { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
