using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Fundation.Repository
{
    public class PharmacyDetailRepository : IPharmacyDetailRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<PharmacyDetailDao> dao)
        {
            string sql = @"
INSERT INTO `pharmacy_detail`
(`PharmacyId`,
`DayOfWeek`,
`OpenTime`,
`CloseTime`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@PharmacyId,
@DayOfWeek,
@OpenTime,
@CloseTime,
@CreateUser,
@UpdateUser
);
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

        public List<PharmacyDetailDao> SelectAll()
        {
            string sql = @"SELECT * FROM pharmacy_detail ";

            return _dbHelper.ExecuteQuery<PharmacyDetailDao>(sql);
        }

        public List<PharmacyDetailDao> SelectByDay(List<DayOfWeek> dayOfWeek = null)
        {
            string sql = @"SELECT * FROM pharmacy_detail WHERE 1=1 ";

            if (dayOfWeek != null && dayOfWeek.Any())
            {
                sql += "AND DayOfWeek IN @DayOfWeek";
            }

            return _dbHelper.ExecuteQuery<PharmacyDetailDao>(sql, new
            {
                DayOfWeek = dayOfWeek
            });
        }
    }
}
