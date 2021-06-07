using PharmacyMask.Fundation.Definition.Enum;
using System.Collections.Generic;

namespace PharmacyMask.DomainService.Entity
{
    public class PharmacyProductOptionEntity
    {
        public List<int> IdList { get; set; }
        public List<int> PharmacyIdList { get; set; }
        public List<int> TypeDetailIdList { get; set; }
        public List<PharmacyProductTypeEnum> ProductTypeIdList { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }
}