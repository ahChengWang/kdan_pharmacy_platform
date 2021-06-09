using PharmacyMask.DomainService.Entity;
using PharmacyMask.Fundation.Repository;
using System.Collections.Generic;

namespace PharmacyMask.DomainService
{
    public class SalesManagementDomainServiceForTest : SalesManagementDomainService
    {
        private readonly List<MaskDetailEntity> _maskDetailList = new List<MaskDetailEntity>();
        private readonly List<PharmacyProductEntity> _pharmacyProductList = new List<PharmacyProductEntity>();

        public SalesManagementDomainServiceForTest(
            IMaskRepository maskRepository,
            IMaskDetailRepository maskInfoRepository,
            IPharmacyProductRepository pharmacyProductRepository,
            IPharmacyRepository pharmacyRepository,
            IMaskService maskDomainService
            ) : base(maskRepository,
            maskInfoRepository,
            pharmacyProductRepository,
            pharmacyRepository,
            maskDomainService)
        { }

        public void AddMsakDetailList(List<MaskDetailEntity> entityList)
        {
            _maskDetailList.AddRange(entityList);
        }
        public void AddPharmacyProduct(List<PharmacyProductEntity> entityList)
        {
            _pharmacyProductList.AddRange(entityList);
        }



        protected override List<MaskDetailEntity> GetMaskDetail(MaskSearchOptionEntity optionEntity)
        {
            return _maskDetailList;
        }

        protected override List<PharmacyProductEntity> GetPharmacyProduct(PharmacyProductOptionEntity optionEntity)
        {
            return _pharmacyProductList;
        }

    }
}
