using PharmacyMask.Fundation.Dao;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IUserBalanceRepository
    {
        int Insert(List<UserBalanceDao> dao);
        List<UserBalanceDao> SelectAll();
        List<UserBalanceDao> SelectById(int userId);
        int Update(UserBalanceDao dao);
    }
}