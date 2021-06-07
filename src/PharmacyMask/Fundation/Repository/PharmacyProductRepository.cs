using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Definition.Enum;
using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Fundation.Repository
{
    public class PharmacyProductRepository
    {
        private readonly IDbHelper _dbHelper = DbHelperFactory.Get();

        public int Insert(List<PharmacyProductDao> dao)
        {
            string sql = @"
INSERT INTO `pharmacy_product`
(`PharmacyId`,
`ProductTypeId`,
`ProductDetailId`,
`Price`,
`CreateUser`,
`UpdateUser`
)
VALUES
(@PharmacyId,
@ProductTypeId,
@ProductDetailId,
@Price,
@CreateUser,
@UpdateUser
);
";
            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

        public List<PharmacyProductDao> SelectAll()
        {
            string sql = @"SELECT * FROM pharmacy_product ";

            return _dbHelper.ExecuteQuery<PharmacyProductDao>(sql);
        }

        public List<PharmacyProductDao> SelectByOption(
            List<int> idList,
            List<int> pharmacyIdList,
            List<int> productDetailIdList,
            List<PharmacyProductTypeEnum> productTypeIdList,
            decimal? priceFrom,
            decimal? priceTo
            )
        {
            string sql = @"SELECT * FROM pharmacy_product WHERE ProductTypeId = 1 ";

            if (idList != null && idList.Any())
            {
                sql += @" AND Id IN @Id ";
            }
            if (pharmacyIdList != null && pharmacyIdList.Any())
            {
                sql += @" AND PharmacyId IN @PharmacyId ";
            }
            if (productTypeIdList != null && productTypeIdList.Any())
            {
                sql += @" AND ProductTypeId IN @ProductTypeId ";
            }
            if (productDetailIdList != null && productDetailIdList.Any())
            {
                sql += @" AND ProductDetailId IN @ProductDetailId ";
            }
            if (priceFrom.HasValue)
            {
                sql += @" AND Price >= @PriceFrom ";
            }
            if (priceTo.HasValue)
            {
                sql += @" AND Price <= @PriceTo ";
            }

            return _dbHelper.ExecuteQuery<PharmacyProductDao>(sql, new
            {
                Id = idList,
                PharmacyId = pharmacyIdList,
                ProductTypeId = productTypeIdList,
                ProductDetailId = productDetailIdList,
                PriceFrom = priceFrom,
                PriceTo = priceTo
            });
        }


        public int Update(PharmacyProductDao dao)
        {
            string sql = @"
            UPDATE pharmacy_product 
               SET 
             Price = @Price 
             WHERE Id = @Id;";

            return _dbHelper.ExecuteNonQuery(sql, dao);
        }

        public int Delete(List<PharmacyProductDao> dao)
        {
            string sql = @"
            DELETE 
              FROM pharmacy_product 
             WHERE Id = @Id 
               AND PharmacyId = @PharmacyId
               AND ProductDetailId = @ProductDetailId; ";

            return _dbHelper.ExecuteNonQuery(sql, dao);
        }
    }
}
