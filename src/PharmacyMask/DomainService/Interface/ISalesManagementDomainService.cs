using PharmacyMask.DomainService.Entity;
using System.Collections.Generic;

namespace PharmacyMask.DomainService
{
    public interface ISalesManagementDomainService
    {
        bool DeleteProduct(PharmacyProductEntity maskEntity);
        List<PharmacyProductEntity> GetPharmacyProductList(PharmacyProductSearchEntity optionEntity);
        List<PharmacyProductMaskEntity> GetPharmacyProductMaskList(ProductSearchEntity searchEntity, List<PharmacyEntity> pharmacyList);
        List<PharmacyProductMaskSummaryEntity> GetPharmacyProductMaskSummary(ProductSearchEntity searchEntity, List<PharmacyEntity> pharmacyList);
        void MigrationMask(List<MaskMigrationEntity> maskMigraEntity);
        bool UpdatePrice(PharmacyProductEntity maskEntity);
    }
}