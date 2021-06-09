using PharmacyMask.Fundation.Dao;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IPurchaseDetailRepository
    {
        int Insert(List<PurchaseDetailDao> dao);
    }
}