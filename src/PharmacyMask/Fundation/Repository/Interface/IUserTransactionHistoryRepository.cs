using PharmacyMask.Fundation.Dao;
using System;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IUserTransactionHistoryRepository
    {
        int Insert(List<UserTransactionHistoryDao> dao);
        List<UserTransactionHistoryDao> SelectAll();
        List<UserTransactionHistoryDao> SelectByOption(List<int> userIdList, List<int> pharmacyIdList, List<int> productDetailIdList, DateTime? transactionDateFrom, DateTime? transactionDateTo, decimal? transactionAmountFrom, decimal? transactionAmountTo);
    }
}