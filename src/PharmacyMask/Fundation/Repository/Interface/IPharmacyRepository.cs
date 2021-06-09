using PharmacyMask.Fundation.Dao;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IPharmacyRepository
    {
        int Insert(List<PharmacyDao> dao);
        List<PharmacyDao> SelectAll();
        List<PharmacyDao> SelectByOption(List<int> idList, string pharmacyName);
        int UpdateName(int id, string pharmacyName);
    }
}