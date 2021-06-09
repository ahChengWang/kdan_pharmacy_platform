using PharmacyMask.DomainService.Entity;
using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Repository;
using System;
using System.Collections.Generic;

namespace PharmacyMask.DomainService
{
    public class BalanceDomainServiceForTest : BalanceDomainService
    {
        private readonly List<UserEntity> _userInfoList = new List<UserEntity>();
        private readonly List<UserTransactionHistoryDao> _userTransactionHistoryList = new List<UserTransactionHistoryDao>();
        private readonly List<MaskDetailEntity> _maskDetailList = new List<MaskDetailEntity>();
        private readonly List<PharmacyEntity> _pharmacyInfoList = new List<PharmacyEntity>();

        public BalanceDomainServiceForTest(
            IUserDomainService userDomainService,
            IUserBalanceRepository userBalanceRepository,
            IUserBalanceLogRepository userBalanceLogRepository,
            IUserTransactionHistoryRepository userTransactionHistoryRepository,
            IPharmacyBalanceRepository pharmacyBalanceRepository,
            IPharmacyBalanceLogRepository pharmacyBalanceLogRepository,
            IMaskService maskService,
            IPharmacyDomainService pharmacyDomainService
            ) : base(
            userDomainService,
            userBalanceRepository,
            userBalanceLogRepository,
            userTransactionHistoryRepository,
            pharmacyBalanceRepository,
            pharmacyBalanceLogRepository,
            maskService,
            pharmacyDomainService)
        { }

        public void AddUserInfo(List<UserEntity> entityList)
        {
            _userInfoList.AddRange(entityList);
        }

        public void AddUserTransactionHistory(List<UserTransactionHistoryDao> entityList)
        {
            _userTransactionHistoryList.AddRange(entityList);
        }

        public void AddMaskDetail(List<MaskDetailEntity> entityList)
        {
            _maskDetailList.AddRange(entityList);
        }

        public void AddPharmacyInfo(List<PharmacyEntity> entityList)
        {
            _pharmacyInfoList.AddRange(entityList);
        }




        protected override List<UserEntity> GetUserInfo(List<int> userIdList, string userName)
        {
            return _userInfoList;
        }

        protected override List<UserTransactionHistoryDao> GetUserTransactionHistory(
            List<int> userIdList,
            List<int> pharmacyIdList,
            List<int> productDetailIdList,
            DateTime? transactionDateFrom,
            DateTime? transactionDateTo,
            decimal? transactionAmountFrom,
            decimal? transactionAmountTo)
        {
            return _userTransactionHistoryList;
        }

        protected override List<MaskDetailEntity> GetMaskDetail(MaskSearchOptionEntity optionEntity)
        {
            return _maskDetailList;
        }

        protected override List<PharmacyEntity> GetPharmacyInfo(List<int> pharmacyId, string pharmacyName)
        {
            return _pharmacyInfoList;
        }

    }
}
