using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public class PurchaseDetailRepository : IPurchaseDetailRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<PurchaseDetailDao> dao)
        {
            string sql = @"
INSERT INTO `purchase_detail`
(`PurchaseId`,
`PharmacyProductId`,
`DetailStatus`,
`Quantity`,
`Amount`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@PurchaseId,
@PharmacyProductId,
@DetailStatus,
@Quantity,
@Amount,
@CreateUser,
@UpdateUser
);
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }
    }
}
