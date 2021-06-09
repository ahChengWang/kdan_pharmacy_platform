using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public class PharmacyBalanceLogRepository : IPharmacyBalanceLogRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<PharmacyBalanceLogDao> dao)
        {
            string sql = @"
INSERT INTO `pharmacy_balance_log`
(`PharmacyId`,
`Amount`,
`PreBalance`,
`LastBalance`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@PharmacyId,
@Amount,
@PreBalance,
@LastBalance,
@CreateUser,
@UpdateUser
);
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

        public List<PharmacyBalanceLogDao> SelectAll()
        {
            string sql = @"SELECT * FROM pharmacy_balance_log ";

            return _dbHelper.ExecuteQuery<PharmacyBalanceLogDao>(sql);
        }
    }
}
