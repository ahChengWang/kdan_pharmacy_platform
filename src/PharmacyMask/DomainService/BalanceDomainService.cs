using PharmacyMask.DomainService.Entity;
using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace PharmacyMask.DomainService
{
    public class BalanceDomainService : IBalanceDomainService
    {
        private readonly IUserDomainService _userDomainService;
        private readonly UserBalanceRepository _userBalanceRepository;
        private readonly UserBalanceLogRepository _userBalanceLogRepository;
        private readonly UserTransactionHistoryRepository _userTransactionHistoryRepository;
        private readonly PharmacyBalanceRepository _pharmacyBalanceRepository;
        private readonly PharmacyBalanceLogRepository _pharmacyBalanceLogRepository;
        private readonly IMaskService _maskService;
        private readonly IPharmacyDomainService _pharmacyDomainService;

        public BalanceDomainService(
            IUserDomainService userDomainService,
            UserBalanceRepository userBalanceRepository,
            UserBalanceLogRepository userBalanceLogRepository,
            UserTransactionHistoryRepository userTransactionHistoryRepository,
            PharmacyBalanceRepository pharmacyBalanceRepository,
            PharmacyBalanceLogRepository pharmacyBalanceLogRepository,
            IMaskService maskService,
            IPharmacyDomainService pharmacyDomainService)
        {
            _userDomainService = userDomainService;
            _userBalanceRepository = userBalanceRepository;
            _userBalanceLogRepository = userBalanceLogRepository;
            _userTransactionHistoryRepository = userTransactionHistoryRepository;
            _pharmacyBalanceRepository = pharmacyBalanceRepository;
            _pharmacyBalanceLogRepository = pharmacyBalanceLogRepository;
            _maskService = maskService;
            _pharmacyDomainService = pharmacyDomainService;
        }

        /// <summary>
        /// The top x users by total transaction amount of masks within a date range
        /// </summary>
        /// <param name="tranDateFrom"></param>
        /// <param name="tranDateTo"></param>
        /// <returns></returns>
        public List<UserTransactionSummaryEntity> GetUserTranSummaryByOption(DateTime tranDateFrom, DateTime tranDateTo)
        {
            var userTranHisList = GetUserTranHistoryByOption(null, tranDateFrom, tranDateTo);

            return userTranHisList.GroupBy(gb => gb.UserName).Select(s => new UserTransactionSummaryEntity
            {
                UserName = s.Key,
                TotalTransactionAmount = s.Sum(sum => sum.TransactionAmount)
            }).OrderByDescending(ob => ob.TotalTransactionAmount).ToList();
        }

        public List<MaskTranSummaryEntity> GetMasksTranSummary(DateTime tranDateFrom, DateTime tranDateTo)
        {
            var userTranHisList = GetUserTranHistoryByOption(null, tranDateFrom, tranDateTo);

            return userTranHisList.GroupBy(gb => gb.MaskName).Select(s => new MaskTranSummaryEntity
            {
                MaskName = s.Key,
                TotalMaskTranAmount = s.Sum(sum => sum.TransactionAmount)
            }).ToList();
        }

        public List<UserTransactionHistoryEntity> GetUserTranHistoryByOption(
            List<int> userIdList,
            DateTime tranDateFrom,
            DateTime tranDateTo)
        {
            var userList = _userDomainService.GetUserInfo(userIdList, null);
            var userTranHistoryId = _userTransactionHistoryRepository.SelectByOption(
                userList.Select(s => s.Id).ToList(),
                null,
                null,
                tranDateFrom,
                tranDateTo,
                null,
                null);

            var maskList = _maskService.GetMaskDetail(new MaskSearchOptionEntity
            {
                MaskDetailIdList = userTranHistoryId.Select(s => s.ProductDetailId).ToList(),
            });

            var pharmacyList = _pharmacyDomainService.GetPharmacyInfo(userTranHistoryId.Select(s => s.PharmacyId).ToList(), null);

            return (from uth in userTranHistoryId
                    join u in userList
                    on uth.UserId equals u.Id
                    join m in maskList
                    on uth.ProductDetailId equals m.MaskId
                    join p in pharmacyList
                    on uth.PharmacyId equals p.Id
                    select new UserTransactionHistoryEntity
                    {
                        TransactionDate = uth.TransactionDate,
                        UserId = uth.UserId,
                        UserName = u.Name,
                        PharmacyId = uth.PharmacyId,
                        PharmacyName = p.Name,
                        MaskId = m.MaskId,
                        MaskName = m.Name,
                        TransactionAmount = uth.TransactionAmount
                    }).ToList();
        }

        public bool UpdateBalance(BalanceUpdateEntity balUpdateEntity)
        {
            var updBalResult = true;
            var pharmacyBal = _pharmacyBalanceRepository.SelectById(balUpdateEntity.PharmacyId).FirstOrDefault();

            var userBal = _userBalanceRepository.SelectById(balUpdateEntity.UserId).FirstOrDefault();

            var updPharmacyBal = new PharmacyBalanceDao
            {
                Id = pharmacyBal.Id,
                CashBalance = pharmacyBal.CashBalance + balUpdateEntity.Amount,
            };

            var insPharmacyBalLog = new PharmacyBalanceLogDao
            {
                PharmacyId = balUpdateEntity.PharmacyId,
                Amount = balUpdateEntity.Amount,
                PreBalance = pharmacyBal.CashBalance,
                LastBalance = updPharmacyBal.CashBalance,
                CreateUser = "system",
                UpdateUser = "system"
            };

            var updUserBal = new UserBalanceDao
            {
                Id = userBal.Id,
                CashBalance = userBal.CashBalance - balUpdateEntity.Amount,
            };

            var insUserBalLog = new UserBalanceLogDao
            {
                UserId = balUpdateEntity.UserId,
                Amount = balUpdateEntity.Amount,
                PreBalance = userBal.CashBalance,
                LastBalance = updUserBal.CashBalance,
                CreateUser = "system",
                UpdateUser = "system"
            };

            using (var scope = new TransactionScope())
            {
                var result = false;

                result = _pharmacyBalanceRepository.Update(updPharmacyBal) == 1;

                if (!result) updBalResult = false;

                result = _pharmacyBalanceLogRepository.Insert(new List<PharmacyBalanceLogDao> { insPharmacyBalLog }) == 1;

                if (!result) updBalResult = false;

                result = _userBalanceRepository.Update(updUserBal) == 1;

                if (!result) updBalResult = false;

                result = _userBalanceLogRepository.Insert(new List<UserBalanceLogDao> { insUserBalLog }) == 1;

                if (result)
                    scope.Complete();
            }
            return updBalResult;
        }

    }
}
