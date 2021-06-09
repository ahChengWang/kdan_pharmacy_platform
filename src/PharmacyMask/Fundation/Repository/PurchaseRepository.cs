using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<PurchaseDao> dao)
        {
            string sql = @"
INSERT INTO `purchase`
(`OrderNo`,
`PharmacyId`,
`UserId`,
`TotalQuantity`,
`TotalAmount`,
`Status`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@OrderNo,
@PharmacyId,
@UserId,
@TotalQuantity,
@TotalAmount,
@Status,
@CreateUser,
@UpdateUser
);
SELECT LAST_INSERT_ID();
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }
    }
}
