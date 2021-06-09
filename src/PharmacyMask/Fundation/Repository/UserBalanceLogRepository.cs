using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public class UserBalanceLogRepository : IUserBalanceLogRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<UserBalanceLogDao> dao)
        {
            string sql = @"
INSERT INTO `user_balance_log`
(`UserId`,
`Amount`,
`PreBalance`,
`LastBalance`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@UserId,
@Amount,
@PreBalance,
@LastBalance,
@CreateUser,
@UpdateUser
);
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

        public List<UserBalanceLogDao> SelectAll()
        {
            string sql = @"SELECT * FROM user_balance_log ";

            return _dbHelper.ExecuteQuery<UserBalanceLogDao>(sql);
        }
    }
}
