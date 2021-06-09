using PharmacyMask.Fundation.Dao;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IPharmacyBalanceRepository
    {
        int Insert(List<PharmacyBalanceDao> dao);
        List<PharmacyBalanceDao> SelectAll();
        List<PharmacyBalanceDao> SelectById(int pharmacyId);
        int Update(PharmacyBalanceDao dao);
    }
}