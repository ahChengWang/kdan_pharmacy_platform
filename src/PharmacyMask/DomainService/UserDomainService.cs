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
    public class UserDomainService
    {
        private readonly UserRepository _userRepository;
        private readonly UserTransactionHistoryRepository _userTransactionHistoryRepository;
        private readonly UserBalanceRepository _userWalletRepository;
        private readonly MaskRepository _maskRepository;
        private readonly MaskDetailRepository _maskInfoRepository;
        private readonly PharmacyRepository _pharmacyRepository;

        public UserDomainService(
            UserRepository userRepository,
            UserTransactionHistoryRepository userTransactionHistoryRepository,
            UserBalanceRepository userWalletRepository,
            MaskRepository maskRepository,
            MaskDetailRepository maskInfoRepository,
            PharmacyRepository pharmacyRepository)
        {
            _userRepository = userRepository;
            _userTransactionHistoryRepository = userTransactionHistoryRepository;
            _userWalletRepository = userWalletRepository;
            _maskRepository = maskRepository;
            _maskInfoRepository = maskInfoRepository;
            _pharmacyRepository = pharmacyRepository;
        }

        public List<UserEntity> GetUserInfo(List<int> userIdList, string userName)
        => _userRepository.SelectByOption(userIdList, userName).Select(s => new UserEntity
        {
            Id = s.Id,
            Name = s.Name
        }).ToList();


        public bool MigrationUser(List<UserMigrationEntity> userMigraEntity)
        {
            var migraResult = true;
            var userList = new List<UserDao>();
            var userWalletList = new List<UserBalanceDao>();
            var userWalletLogList = new List<UserBalanceLogDao>();
            var userTransactionHistoryList = new List<UserTransactionHistoryDao>();

            var maskList = _maskRepository.SelectAll();
            var maskInfoList = _maskInfoRepository.SelectAll();
            var pharmacyList = _pharmacyRepository.SelectAll();

            userList = userMigraEntity.Select(s =>
            {
                return new UserDao
                {
                    Name = s.Name,
                    CreateUser = "system",
                    UpdateUser = "system"
                };
            }).ToList();

            var tmpUserWallet = userMigraEntity.Select(s =>
            {
                return new
                {
                    UserName = s.Name,
                    CashBalance = s.CashBalance,
                    CreateUser = "system",
                    UpdateUser = "system"
                };
            }).ToList();

            var tmpUserTransactionHistoryList = userMigraEntity.SelectMany(sm =>
            {
                return sm.PurchaseHistories.Select(se =>
                {
                    var maskDesc = se.Remark.Split('(').ToArray();
                    var maskId = maskList.FirstOrDefault(fo => fo.Name == maskDesc[0].TrimEnd()).Id;

                    return new
                    {
                        TransactionDate = se.TransactionDate,
                        PharmacyId = pharmacyList.FirstOrDefault(f => f.Name == se.PharmacyName).Id,
                        UserName = sm.Name,
                        ProductTypeId = PharmacyProductTypeEnum.Mask,
                        DetailId = maskInfoList.FirstOrDefault(f =>
                            f.MaskId == maskId &&
                            f.ColorId == (MaskColorEnum)Enum.Parse(typeof(MaskColorEnum), maskDesc[1].Trim().Replace(")", "")) &&
                            f.QtyPerPack == Convert.ToInt32(maskDesc[2].Split(" ")[0])).Id,
                        Remark = se.Remark,
                        TransactionAmount = se.TransactionAmount,
                        CreateUser = "system",
                        UpdateUser = "system"
                    };
                });
            }).ToList();

            using (var scope = new TransactionScope())
            {
                bool insResult = false;

                insResult = _userRepository.Insert(userList) == userList.Count();
                if (!insResult) migraResult = false;

                var userData = _userRepository.SelectAll();

                userWalletList = tmpUserWallet.Select(s =>
                {
                    return new UserBalanceDao
                    {
                        UserId = userData.FirstOrDefault(f => f.Name == s.UserName).Id,
                        CashBalance = s.CashBalance,
                        CreateUser = s.CreateUser,
                        UpdateUser = s.UpdateUser
                    };
                }).ToList();

                insResult = _userWalletRepository.Insert(userWalletList) == userWalletList.Count();
                if (!insResult) migraResult = false;

                userTransactionHistoryList = tmpUserTransactionHistoryList.Select(s =>
                {
                    return new UserTransactionHistoryDao
                    {
                        TransactionDate = s.TransactionDate,
                        PharmacyId = s.PharmacyId,
                        ProductTypeId = s.ProductTypeId,
                        ProductDetailId = s.DetailId,
                        UserId = userData.FirstOrDefault(f => f.Name == s.UserName).Id,
                        Remark = s.Remark,
                        TransactionAmount = s.TransactionAmount,
                        CreateUser = s.CreateUser,
                        UpdateUser = s.UpdateUser
                    };
                }).ToList();

                insResult = _userTransactionHistoryRepository.Insert(userTransactionHistoryList) == userTransactionHistoryList.Count();
                if (!insResult) migraResult = false;

                if (migraResult)
                    scope.Complete();
            }

            return migraResult;
        }
    }
}
