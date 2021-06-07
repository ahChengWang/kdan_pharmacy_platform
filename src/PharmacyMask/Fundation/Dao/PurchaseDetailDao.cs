using PharmacyMask.Fundation.Definition.Enum;
using System;

namespace PharmacyMask.Fundation.Dao
{
    public class PurchaseDetailDao
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int PharmacyProductId { get; set; }
        public OrderStatusEnum DetailStatus { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
