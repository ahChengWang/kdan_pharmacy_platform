using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Fundation.Repository
{
    public class UserRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<UserDao> dao)
        {
            string sql = @"
INSERT INTO `user`
(`Name`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@Name,
@CreateUser,
@UpdateUser
);
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

        public List<UserDao> SelectAll()
        {
            string sql = @"SELECT * FROM user ";

            return _dbHelper.ExecuteQuery<UserDao>(sql);
        }


        public List<UserDao> SelectByOption(List<int> idList, string name)
        {
            string sql = @"SELECT * FROM user WHERE 1=1 ";

            if (idList != null && idList.Any())
            {
                sql += @" AND Id IN @Id";
            }
            if (!string.IsNullOrEmpty(name))
            {
                sql += @" AND Name = @Name ";
            }

            return _dbHelper.ExecuteQuery<UserDao>(sql, new
            {
                Id = idList,
                Name = name
            });
        }
    }
}
