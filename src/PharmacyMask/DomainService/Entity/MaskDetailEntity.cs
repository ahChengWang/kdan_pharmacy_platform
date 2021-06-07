using PharmacyMask.Fundation.Definition.Enum;

namespace PharmacyMask.DomainService.Entity
{
    public class MaskDetailEntity
    {
        public int MaskId { get; set; }
        public int DetailId { get; set; }
        public string Name { get; set; }
        public MaskColorEnum ColorId { get; set; }
        public int QtyPerPack { get; set; }
    }
}
