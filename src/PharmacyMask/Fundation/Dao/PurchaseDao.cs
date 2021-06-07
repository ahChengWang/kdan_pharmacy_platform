using PharmacyMask.Fundation.Definition.Enum;
using System;

namespace PharmacyMask.Fundation.Dao
{
    public class PurchaseDao
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public int PharmacyId { get; set; }
        public int UserId { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatusEnum Status { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
