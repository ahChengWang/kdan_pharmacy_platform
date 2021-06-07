using PharmacyMask.Fundation.Definition.Enum;

namespace PharmacyMask.DomainService.Entity
{
    public class PharmacyProductEntity
    {
        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public PharmacyProductTypeEnum ProductTypeId { get; set; }
        public int ProductDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
