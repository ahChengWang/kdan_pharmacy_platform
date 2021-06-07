using PharmacyMask.Fundation.Definition.Enum;
using System;

namespace PharmacyMask.Fundation.Dao
{
    public class PharmacyProductDao
    {
        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public PharmacyProductTypeEnum ProductTypeId { get; set; }
        public int ProductDetailId { get; set; }
        public decimal Price { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
