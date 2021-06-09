using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public class UserBalanceRepository : IUserBalanceRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<UserBalanceDao> dao)
        {
            string sql = @"
INSERT INTO `user_balance`
(`UserId`,
`CashBalance`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@UserId,
@CashBalance,
@CreateUser,
@UpdateUser
);
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

        public List<UserBalanceDao> SelectAll()
        {
            string sql = @"SELECT * FROM user_balance ";

            return _dbHelper.ExecuteQuery<UserBalanceDao>(sql);
        }


        public List<UserBalanceDao> SelectById(int userId)
        {
            string sql = @"SELECT * FROM user_balance WHERE UserId = @UserId ";

            return _dbHelper.ExecuteQuery<UserBalanceDao>(sql, new
            {
                UserId = userId
            });
        }

        public int Update(UserBalanceDao dao)
        {
            string sql = @"
            UPDATE user_balance 
               SET 
       CashBalance = @CashBalance 
             WHERE Id = @Id;";

            return _dbHelper.ExecuteNonQuery(sql, dao);
        }
    }
}
