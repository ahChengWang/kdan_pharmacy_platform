using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public class PharmacyBalanceRepository : IPharmacyBalanceRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<PharmacyBalanceDao> dao)
        {
            string sql = @"
INSERT INTO `pharmacy_balance`
(`PharmacyId`,
`CashBalance`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@PharmacyId,
@CashBalance,
@CreateUser,
@UpdateUser
);
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

        public List<PharmacyBalanceDao> SelectAll()
        {
            string sql = @"SELECT * FROM pharmacy_balance ";

            return _dbHelper.ExecuteQuery<PharmacyBalanceDao>(sql);
        }

        public List<PharmacyBalanceDao> SelectById(int pharmacyId)
        {
            string sql = @"SELECT * FROM pharmacy_balance WHERE PharmacyId = @PharmacyId ";

            return _dbHelper.ExecuteQuery<PharmacyBalanceDao>(sql, new
            {
                PharmacyId = pharmacyId
            });
        }

        public int Update(PharmacyBalanceDao dao)
        {
            string sql = @"
            UPDATE pharmacy_balance 
               SET 
       CashBalance = @CashBalance 
             WHERE Id = @Id;";

            return _dbHelper.ExecuteNonQuery(sql, dao);
        }
    }
}
