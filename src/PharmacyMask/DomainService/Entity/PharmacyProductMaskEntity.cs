using PharmacyMask.Fundation.Definition.Enum;

namespace PharmacyMask.DomainService.Entity
{
    public class PharmacyProductMaskEntity
    {
        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public int MaskId { get; set; }
        public string MaskName { get; set; }
        public MaskColorEnum ColorId { get; set; }
        public string Color { get; set; }
        public int QtyPerPack { get; set; }
        public decimal Price { get; set; }
    }
}
