using PharmacyMask.Fundation.Dao;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IPurchaseRepository
    {
        int Insert(List<PurchaseDao> dao);
    }
}