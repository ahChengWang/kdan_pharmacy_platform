using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Fundation.Repository
{
    public class MaskDetailRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<MaskDetailDao> dao)
        {
            string sql = @"
INSERT INTO `mask_detail`
(`MaskId`,
`ColorId`,
`QtyPerPack`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@MaskId,
@ColorId,
@QtyPerPack,
@CreateUser,
@UpdateUser
);
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

        public List<MaskDetailDao> SelectAll()
        {
            string sql = @"SELECT * FROM mask_detail ";

            return _dbHelper.ExecuteQuery<MaskDetailDao>(sql);
        }

        public List<MaskDetailDao> SelectByOption(List<int> idList, List<int> maskIdList)
        {
            string sql = @"SELECT * FROM mask_detail WHERE 1=1 ";

            if (idList != null && idList.Any())
            {
                sql += @" AND Id IN @Id ";
            }
            if (maskIdList != null && maskIdList.Any())
            {
                sql += @" AND MaskId IN @MaskId ";
            }

            return _dbHelper.ExecuteQuery<MaskDetailDao>(sql,new 
            {
                Id = idList,
                MaskId = maskIdList
            });
        }
    }
}
