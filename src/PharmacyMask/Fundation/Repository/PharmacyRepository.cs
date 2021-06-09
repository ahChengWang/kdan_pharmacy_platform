using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Fundation.Repository
{
    public class PharmacyRepository : IPharmacyRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<PharmacyDao> dao)
        {
            string sql = @"
INSERT INTO `pharmacy`
(`Name`,
`Status`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@Name,
@Status,
@CreateUser,
@UpdateUser
);
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

        public List<PharmacyDao> SelectAll()
        {
            string sql = @"SELECT * FROM pharmacy ";

            return _dbHelper.ExecuteQuery<PharmacyDao>(sql);
        }

        public List<PharmacyDao> SelectByOption(List<int> idList, string pharmacyName)
        {
            string sql = @"SELECT * FROM pharmacy WHERE 1=1 ";

            if (idList != null && idList.Any())
            {
                sql += @"AND Id IN @Id ";
            }
            if (!string.IsNullOrEmpty(pharmacyName))
            {
                sql += $@"AND Name like '%{pharmacyName}%' ";
            }

            return _dbHelper.ExecuteQuery<PharmacyDao>(sql, new
            {
                Id = idList,
                pharmacyName
            });
        }


        public int UpdateName(int id, string pharmacyName)
        {
            string sql = @"UPDATE pharmacy SET Name = @Name WHERE Id = @Id;";

            return _dbHelper.ExecuteNonQuery(sql, new
            {
                Id = id,
                Name = pharmacyName
            });
        }
    }
}
