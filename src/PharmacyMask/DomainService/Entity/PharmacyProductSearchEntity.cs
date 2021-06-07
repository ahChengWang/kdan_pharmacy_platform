using PharmacyMask.Fundation.Definition.Enum;
using System.Collections.Generic;

namespace PharmacyMask.DomainService.Entity
{
    public class PharmacyProductSearchEntity
    {
        public List<int> Id { get; set; }
        public List<int> PharmacyId { get; set; }
        public List<PharmacyProductTypeEnum> ProductTypeId { get; set; }
        public List<int> ProductDetailId { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
    }
}
