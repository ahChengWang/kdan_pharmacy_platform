using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Definition.Enum;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IPharmacyProductRepository
    {
        int Delete(List<PharmacyProductDao> dao);
        int Insert(List<PharmacyProductDao> dao);
        List<PharmacyProductDao> SelectAll();
        List<PharmacyProductDao> SelectByOption(List<int> idList, List<int> pharmacyIdList, List<int> productDetailIdList, List<PharmacyProductTypeEnum> productTypeIdList, decimal? priceFrom, decimal? priceTo);
        int Update(PharmacyProductDao dao);
    }
}