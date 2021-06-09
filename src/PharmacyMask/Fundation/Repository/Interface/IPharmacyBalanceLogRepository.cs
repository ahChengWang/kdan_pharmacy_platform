using PharmacyMask.Fundation.Dao;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IPharmacyBalanceLogRepository
    {
        int Insert(List<PharmacyBalanceLogDao> dao);
        List<PharmacyBalanceLogDao> SelectAll();
    }
}