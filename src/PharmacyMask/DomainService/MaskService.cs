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
    public class MaskService
    {
        private readonly MaskRepository _maskRepository;
        private readonly MaskDetailRepository _maskInfoRepository;
        private readonly PharmacyProductRepository _pharmacyProductRepository;
        private readonly PharmacyRepository _pharmacyRepository;

        public MaskService(
            MaskRepository maskRepository,
            MaskDetailRepository maskInfoRepository,
            PharmacyProductRepository pharmacyProductRepository,
            PharmacyRepository pharmacyRepository
            )
        {
            _maskRepository = maskRepository;
            _maskInfoRepository = maskInfoRepository;
            _pharmacyProductRepository = pharmacyProductRepository;
            _pharmacyRepository = pharmacyRepository;
        }


        public List<MaskDetailEntity> GetMaskDetail(MaskSearchOptionEntity optionEntity)
        {
            var maskList = _maskRepository.SelectByOption(optionEntity?.MaskIdList, optionEntity?.MaskName);
            var maskDetailList = _maskInfoRepository.SelectByOption(optionEntity?.MaskDetailIdList, maskList.Select(s => s.Id).ToList());

            return (from md in maskDetailList
                    join m in maskList
                    on md.MaskId equals m.Id
                    select new MaskDetailEntity
                    {
                        MaskId = md.MaskId,
                        DetailId = md.Id,
                        Name = m.Name,
                        ColorId = md.ColorId,
                        QtyPerPack = md.QtyPerPack
                    }).ToList();
        }

        public bool UpdateMask(MaskEntity maskEntity)
        {
            var updateMaskResult = true;

            using (var scope = new TransactionScope())
            {
                updateMaskResult  = _maskRepository.Update(maskEntity.Adapt<MaskDao>()) == 1;

                if (updateMaskResult)
                    scope.Complete();
            }

            return updateMaskResult;
        }

        public bool MigrationMask(List<MaskMigrationEntity> maskMigraEntity)
        {
            var migraResult = true;
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
                if (!insResult) migraResult = false;

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
                if (!insResult) migraResult = false;

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
                if (!insResult) migraResult = false;

                if (migraResult)
                    scope.Complete();
            }

            return migraResult;
        }
    }
}
