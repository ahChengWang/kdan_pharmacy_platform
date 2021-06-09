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
    public class PurchaseDomainService : IPurchaseDomainService
    {
        private readonly PurchaseRepository _purchaseRepository;
        private readonly PurchaseDetailRepository _purchaseDetailRepository;
        private readonly UserTransactionHistoryRepository _userTransactionHistoryRepository;
        private readonly IBalanceDomainService _balanceDomainService;
        private readonly ISalesManagementDomainService _salesManagementDomainService;
        private readonly IMaskService _maskService;
        private readonly DateTime _dateTimeNow = DateTime.Now;

        public PurchaseDomainService(
            PurchaseRepository purchaseRepository,
            PurchaseDetailRepository purchaseDetailRepository,
            UserTransactionHistoryRepository userTransactionHistoryRepository,
            IBalanceDomainService balanceDomainServic,
            ISalesManagementDomainService salesManagementDomainService,
            IMaskService maskService
            )
        {
            _purchaseRepository = purchaseRepository;
            _purchaseDetailRepository = purchaseDetailRepository;
            _userTransactionHistoryRepository = userTransactionHistoryRepository;
            _balanceDomainService = balanceDomainServic;
            _salesManagementDomainService = salesManagementDomainService;
            _maskService = maskService;
        }

        public bool CreatePurchase(PurchaseCreateEntity createListEntity)
        {
            var createResult = true;

            var pharmacyProductList = _salesManagementDomainService.GetPharmacyProductList(
                new PharmacyProductOptionEntity
                {
                    IdList = createListEntity.CreateDetailList.Select(s => s.PharmacyProductId).ToList()
                });

            var maskDetailList = _maskService.GetMaskDetail(new MaskSearchOptionEntity
            {
                MaskDetailIdList = pharmacyProductList.Select(s => s.ProductDetailId).ToList()
            });

            var insPurchase =
                new PurchaseDao
                {
                    OrderNo = $"P{_dateTimeNow.ToString("HHmmssff")}",
                    PharmacyId = createListEntity.PharmacyId,
                    UserId = createListEntity.UserId,
                    Status = OrderStatusEnum.Complete,
                    TotalQuantity = createListEntity.CreateDetailList.Sum(sum => sum.Quantity),
                    TotalAmount = createListEntity.CreateDetailList.Sum(sum => sum.Amount),
                    CreateUser = "system",
                    UpdateUser = "system"
                };

            var insPurchaseDetailList = createListEntity.CreateDetailList.Select(s =>
            {
                return new PurchaseDetailDao
                {
                    PharmacyProductId = s.PharmacyProductId,
                    Quantity = s.Quantity,
                    Amount = s.Amount,
                    DetailStatus = OrderStatusEnum.Complete,
                    CreateUser = "system",
                    UpdateUser = "system"
                };
            }).ToList();

            var insUserTranHistoryList = createListEntity.CreateDetailList.Select(s =>
            {
                var pharmacyProduct = pharmacyProductList.FirstOrDefault(f => f.Id == s.PharmacyProductId);
                var maskInfo = maskDetailList.FirstOrDefault(f => f.DetailId == pharmacyProduct.ProductDetailId);

                return new UserTransactionHistoryDao
                {
                    TransactionDate = _dateTimeNow,
                    PharmacyId = createListEntity.PharmacyId,
                    ProductTypeId = PharmacyProductTypeEnum.Mask,
                    ProductDetailId = s.PharmacyProductId,
                    Remark = $"{maskInfo.Name} ({maskInfo.ColorId}) ({maskInfo.QtyPerPack} per pack)",
                    TransactionAmount = pharmacyProduct.Price,
                    CreateUser = "system",
                    UpdateUser = "system"
                };
            }).ToList();

            using (var scope = new TransactionScope())
            {
                var insResult = false;

                var insPurchaseId = _purchaseRepository.Insert(new List<PurchaseDao> { insPurchase });

                if (insPurchaseId == 0) createResult = false;

                insPurchaseDetailList.ForEach(fe => fe.PurchaseId = insPurchaseId);

                insResult = _purchaseDetailRepository.Insert(insPurchaseDetailList) == insPurchaseDetailList.Count;

                if (!insResult) createResult = false;

                insResult = _userTransactionHistoryRepository.Insert(insUserTranHistoryList) == insUserTranHistoryList.Count;

                if (insResult)
                    scope.Complete();
            }

            createResult = _balanceDomainService.UpdateBalance(new BalanceUpdateEntity
            {
                PharmacyId = createListEntity.PharmacyId,
                UserId = createListEntity.UserId,
                Amount = insPurchase.TotalAmount
            });

            return createResult;
        }

    }
}
