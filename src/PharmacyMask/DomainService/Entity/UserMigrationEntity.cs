using System.Collections.Generic;

namespace PharmacyMask.DomainService.Entity
{
    public class UserMigrationEntity
    {
        public string Name { get; set; }
        public decimal CashBalance { get; set; }
        public List<PurchaseHistoryEntity> PurchaseHistories { get; set; }
    }
}
