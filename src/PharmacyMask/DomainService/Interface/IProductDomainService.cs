using PharmacyMask.DomainService.Entity;
using System.Collections.Generic;

namespace PharmacyMask.DomainService
{
    public interface IProductDomainService
    {
        List<PharmacyProductEntity> GetPharmacyProductList(PharmacyProductSearchEntity searchEntity);
        bool UpdateMask(ProductEntity productEntity);
    }
}