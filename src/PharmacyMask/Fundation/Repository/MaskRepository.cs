using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Fundation.Repository
{
    public class MaskRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<MaskDao> dao)
        {
            string sql = @"
INSERT INTO `mask`
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

        public List<MaskDao> SelectAll()
        {
            string sql = @"SELECT * FROM mask ";

            return _dbHelper.ExecuteQuery<MaskDao>(sql);
        }

        public List<MaskDao> SelectByOption(List<int> id, string name)
        {
            string sql = @"SELECT * FROM mask where 1=1 ";

            if (id != null && id.Any())
            {
                sql += @" AND Id IN @Id ";
            }
            if (!string.IsNullOrEmpty(name))
            {
                sql += @$" AND Name like '%{name}%' ";
            }

            return _dbHelper.ExecuteQuery<MaskDao>(sql, new
            {
                Id = id,
                Name = name
            });
        }

        public int Update(MaskDao dao)
        {
            string sql = @"
            UPDATE mask 
               SET 
              Name = @Name 
             WHERE Id = @Id;";

            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

    }
}
