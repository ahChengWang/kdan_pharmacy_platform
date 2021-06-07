using Mapster;
using PharmacyMask.DomainService.Entity;
using PharmacyMask.Fundation.Repository;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.DomainService
{
    public class ProductDomainService
    {
        private readonly PharmacyProductRepository _pharmacyProductRepository;
        private readonly MaskService _maskDomainService;

        public ProductDomainService(
            PharmacyProductRepository pharmacyProductRepository,
            MaskService maskDomainService
            )
        {
            _pharmacyProductRepository = pharmacyProductRepository;
            _maskDomainService = maskDomainService;
        }

        public List<PharmacyProductEntity> GetPharmacyProductList(PharmacyProductSearchEntity searchEntity)
        => _pharmacyProductRepository.SelectByOption(
                searchEntity.Id,
                searchEntity.PharmacyId,
                searchEntity.ProductDetailId,
                searchEntity.ProductTypeId,
                searchEntity.PriceFrom,
                searchEntity.PriceTo
                ).Select(s => s.Adapt<PharmacyProductEntity>()).ToList();

        public bool UpdateMask(ProductEntity productEntity)
        {
            return _maskDomainService.UpdateMask(new MaskEntity
            {
                Id = productEntity.Id,
                Name = productEntity.ProductName
            });

        }

    }
}
