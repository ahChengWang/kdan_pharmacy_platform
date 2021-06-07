using System.Collections.Generic;

namespace PharmacyMask.DomainService.Entity
{
    public class PurchaseCreateEntity
    {
        public int PharmacyId { get; set; }
        public int UserId { get; set; }
        public List<PurchaseCreateDetailEntity> CreateDetailList { get; set; }
    }
}
