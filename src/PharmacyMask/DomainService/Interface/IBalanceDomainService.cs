using PharmacyMask.DomainService.Entity;
using System;
using System.Collections.Generic;

namespace PharmacyMask.DomainService
{
    public interface IBalanceDomainService
    {
        List<MaskTranSummaryEntity> GetMasksTranSummary(DateTime tranDateFrom, DateTime tranDateTo);
        List<UserTransactionHistoryEntity> GetUserTranHistoryByOption(List<int> userIdList, DateTime tranDateFrom, DateTime tranDateTo);
        List<UserTransactionSummaryEntity> GetUserTranSummaryByOption(DateTime tranDateFrom, DateTime tranDateTo);
        bool UpdateBalance(BalanceUpdateEntity balUpdateEntity);
    }
}