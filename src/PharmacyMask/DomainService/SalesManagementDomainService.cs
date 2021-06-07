using Mapster;
using PharmacyMask.DomainService.Entity;
using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Definition.Enum;
using PharmacyMask.Fundation.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace PharmacyMask.DomainService
{
    public class SalesManagementDomainService
    {
        private readonly MaskRepository _maskRepository;
        private readonly MaskDetailRepository _maskInfoRepository;
        private readonly PharmacyProductRepository _pharmacyProductRepository;
        private readonly PharmacyRepository _pharmacyRepository;
        private readonly MaskService _maskDomainService;

        public SalesManagementDomainService(
            MaskRepository maskRepository,
            MaskDetailRepository maskInfoRepository,
            PharmacyProductRepository pharmacyProductRepository,
            PharmacyRepository pharmacyRepository,
            MaskService maskDomainService
            )
        {
            _maskRepository = maskRepository;
            _maskInfoRepository = maskInfoRepository;
            _pharmacyProductRepository = pharmacyProductRepository;
            _pharmacyRepository = pharmacyRepository;
            _maskDomainService = maskDomainService;
        }

        public List<PharmacyProductEntity> GetPharmacyProductList(PharmacyProductOptionEntity optionEntity)
        => _pharmacyProductRepository.SelectByOption(
                optionEntity.IdList,
                optionEntity.PharmacyIdList,
                optionEntity.TypeDetailIdList,
                optionEntity.ProductTypeIdList,
                optionEntity.PriceFrom,
                optionEntity.PriceTo
                ).Select(s => s.Adapt<PharmacyProductEntity>()).ToList();

        public List<PharmacyProductMaskEntity> GetPharmacyProductMaskList(
            ProductSearchEntity searchEntity,
            List<PharmacyEntity> pharmacyList)
        {
            // 檢核金額區間
            if (searchEntity.PriceFrom.HasValue && searchEntity.PriceTo.HasValue &&
                searchEntity.PriceFrom > searchEntity.PriceTo)
                throw new Exception();

            var msakDetailList = _maskDomainService.GetMaskDetail(new MaskSearchOptionEntity
            {
                MaskIdList = null,
                MaskName = searchEntity.ProductName
            });

            var pharmacyProductMaskList = GetPharmacyProductList(new PharmacyProductOptionEntity
            {
                IdList = null,
                PharmacyIdList = null,
                TypeDetailIdList = msakDetailList.Select(s => s.DetailId).ToList(),
                ProductTypeIdList = new List<PharmacyProductTypeEnum> { PharmacyProductTypeEnum.Mask },
                PriceFrom = searchEntity.PriceFrom,
                PriceTo = searchEntity.PriceTo
            });

            return (from ppm in pharmacyProductMaskList
                    join m in msakDetailList
                    on ppm.ProductDetailId equals m.DetailId
                    join p in pharmacyList
                    on ppm.PharmacyId equals p.Id
                    select new PharmacyProductMaskEntity
                    {
                        Id = ppm.Id,
                        PharmacyId = ppm.PharmacyId,
                        PharmacyName = p.Name,
                        MaskId = m.MaskId,
                        MaskName = m.Name,
                        ColorId = m.ColorId,
                        Color = m.ColorId.ToString(),
                        QtyPerPack = m.QtyPerPack,
                        Price = ppm.Price
                    }).ToList();
        }

        public List<PharmacyProductMaskSummaryEntity> GetPharmacyProductMaskSummary(
            ProductSearchEntity searchEntity,
            List<PharmacyEntity> pharmacyList)
        {
            var productMaskList = GetPharmacyProductMaskList(searchEntity, pharmacyList);

            return productMaskList.GroupBy(gb => gb.PharmacyName)
            .Select(s => new PharmacyProductMaskSummaryEntity
            {
                PharmacyName = s.Key,
                MaskProductCnt = s.Count()
            }).ToList();
        }

        public bool UpdatePrice(PharmacyProductEntity maskEntity)
        {
            var updResult = true;

            using (var scope = new TransactionScope())
            {
                updResult = _pharmacyProductRepository.Update(maskEntity.Adapt<PharmacyProductDao>()) == 1;

                if (updResult)
                    scope.Complete();
            }

            return updResult;
        }

        public bool DeleteProduct(PharmacyProductEntity maskEntity)
        {
            var delResult = true;

            var msakData = _maskDomainService.GetMaskDetail(new MaskSearchOptionEntity
            {
                MaskName = maskEntity.ProductName
            });

            var pharmacyProductData = _pharmacyProductRepository.SelectByOption(
                null,
                new List<int> { maskEntity.PharmacyId },
                msakData.Select(s => s.DetailId).ToList(),
                new List<PharmacyProductTypeEnum> { PharmacyProductTypeEnum.Mask },
                null,
                null);

            using (var scope = new TransactionScope())
            {
                delResult = _pharmacyProductRepository.Delete(pharmacyProductData) == pharmacyProductData.Count;

                if (delResult)
                    scope.Complete();
            }

            return delResult;
        }

        public void MigrationMask(List<MaskMigrationEntity> maskMigraEntity)
        {
            var pharmacyProductList = new List<PharmacyProductDao>();
            var maskList = new List<MaskDao>();
            var maskInfoList = new List<MaskDetailDao>();

            var pharmacyList = _pharmacyRepository.SelectAll();

            // mask data
            maskList = maskMigraEntity.Select(s => s.MaskDesc.Split('(')[0].TrimEnd())
                    .GroupBy(gb => gb)
                    .Select(se =>
                    {
                        return new MaskDao
                        {
                            Name = se.Key,
                            CreateUser = "system",
                            UpdateUser = "system"
                        };
                    }).ToList();

            // pharmacy_product_mask data
            var tmpPharmacyProductList =
                maskMigraEntity.Select(s =>
            {
                var maskDesc = s.MaskDesc.Split('(').ToArray();

                return new
                {
                    PharmacyId = pharmacyList.FirstOrDefault(f => f.Name == s.PharmacyName.Trim())?.Id ?? 0,
                    ProductTypeId = PharmacyProductTypeEnum.Mask,
                    MaskName = maskDesc[0].TrimEnd(),
                    MaskColor = (MaskColorEnum)Enum.Parse(typeof(MaskColorEnum), maskDesc[1].Trim().Replace(")", "")),
                    MaskPerPack = Convert.ToInt32(maskDesc[2].Split(" ")[0]),
                    Price = s.Price,
                    CreateUser = "system",
                    UpdateUser = "system"
                };
            }).ToList();

            // mask_info data
            var tmpMaskInfoList =
                maskMigraEntity.Select(s =>
                {
                    var maskDesc = s.MaskDesc.Split('(').ToArray();

                    return new
                    {
                        MaskName = maskDesc[0].TrimEnd(),
                        MaskColor = (MaskColorEnum)Enum.Parse(typeof(MaskColorEnum), maskDesc[1].Trim().Replace(")", "")),
                        MaskPerPack = Convert.ToInt32(maskDesc[2].Split(" ")[0])
                    };
                }).GroupBy(gb => new { gb.MaskName, gb.MaskColor, gb.MaskPerPack })
                .Select(se => new
                {
                    MaskName = se.Key.MaskName,
                    MaskColor = se.Key.MaskColor,
                    MaskPerPack = se.Key.MaskPerPack,
                    CreateUser = "system",
                    UpdateUser = "system"
                }).ToList();


            using (var scope = new TransactionScope())
            {
                bool insResult = false;

                insResult = _maskRepository.Insert(maskList) == maskList.Count();
                if (!insResult)
                    //例外處理
                    throw new Exception();

                var maskData = _maskRepository.SelectAll();

                maskInfoList = tmpMaskInfoList.Select(s =>
                {
                    return new MaskDetailDao
                    {
                        MaskId = maskData.FirstOrDefault(f => f.Name == s.MaskName).Id,
                        ColorId = s.MaskColor,
                        QtyPerPack = s.MaskPerPack,
                        CreateUser = s.CreateUser,
                        UpdateUser = s.UpdateUser
                    };
                }).ToList();

                insResult = _maskInfoRepository.Insert(maskInfoList) == maskInfoList.Count();
                if (!insResult)
                    //例外處理
                    throw new Exception();

                var maskInfoData = _maskInfoRepository.SelectAll();

                pharmacyProductList = tmpPharmacyProductList.Select(s =>
                {
                    return new PharmacyProductDao
                    {
                        PharmacyId = s.PharmacyId,
                        ProductTypeId = s.ProductTypeId,
                        ProductDetailId = maskInfoData.FirstOrDefault(f =>
                            f.MaskId == maskData.FirstOrDefault(fo => fo.Name == s.MaskName).Id &&
                            f.ColorId == s.MaskColor &&
                            f.QtyPerPack == s.MaskPerPack).Id,
                        Price = s.Price,
                        CreateUser = s.CreateUser,
                        UpdateUser = s.UpdateUser
                    };
                }).ToList();

                insResult = _pharmacyProductRepository.Insert(pharmacyProductList) == pharmacyProductList.Count();
                if (!insResult)
                    //例外處理
                    throw new Exception();

                if (insResult)
                    scope.Complete();
            }
        }
    }
}
