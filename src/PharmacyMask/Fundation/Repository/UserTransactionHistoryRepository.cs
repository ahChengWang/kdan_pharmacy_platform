using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Fundation.Repository
{
    public class UserTransactionHistoryRepository : IUserTransactionHistoryRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<UserTransactionHistoryDao> dao)
        {
            string sql = @"
INSERT INTO `user_transaction_history`
(`TransactionDate`,
`UserId`,
`PharmacyId`,
`ProductTypeId`,
`ProductDetailId`,
`Remark`,
`TransactionAmount`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@TransactionDate,
@UserId,
@PharmacyId,
@ProductTypeId,
@ProductDetailId,
@Remark,
@TransactionAmount,
@CreateUser,
@UpdateUser
);
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

        public List<UserTransactionHistoryDao> SelectAll()
        {
            string sql = @"SELECT * FROM user_transaction_history ";

            return _dbHelper.ExecuteQuery<UserTransactionHistoryDao>(sql);
        }

        public List<UserTransactionHistoryDao> SelectByOption(
            List<int> userIdList,
            List<int> pharmacyIdList,
            List<int> productDetailIdList,
            DateTime? transactionDateFrom,
            DateTime? transactionDateTo,
            decimal? transactionAmountFrom,
            decimal? transactionAmountTo)
        {
            string sql = @"SELECT * FROM user_transaction_history WHERE 1=1 ";

            if (userIdList != null && userIdList.Any())
            {
                sql += @" AND UserId IN @UserId ";
            }
            if (pharmacyIdList != null && pharmacyIdList.Any())
            {
                sql += @" AND PharmacyId IN @PharmacyId ";
            }
            if (productDetailIdList != null && productDetailIdList.Any())
            {
                sql += @" AND ProductDetailId IN @ProductDetailId ";
            }
            if (transactionDateFrom.HasValue)
            {
                sql += @" AND TransactionDate >= @TransactionDateFrom ";
            }
            if (transactionDateTo.HasValue)
            {
                sql += @" AND TransactionDate <= @TransactionDateTo ";
            }
            if (transactionAmountFrom.HasValue)
            {
                sql += @" AND TransactionAmount >= @TransactionAmountFrom ";
            }
            if (transactionAmountTo.HasValue)
            {
                sql += @" AND TransactionAmount <= @TransactionAmountTo ";
            }

            return _dbHelper.ExecuteQuery<UserTransactionHistoryDao>(sql, new
            {
                UserId = userIdList,
                PharmacyId = pharmacyIdList,
                ProductDetailId = productDetailIdList,
                TransactionDateFrom = transactionDateFrom,
                TransactionDateTo = transactionDateTo,
                TransactionAmountFrom = transactionAmountFrom,
                TransactionAmountTo = transactionAmountTo
            });
        }
    }
}
