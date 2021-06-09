using PharmacyMask.Fundation.Dao;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IUserBalanceLogRepository
    {
        int Insert(List<UserBalanceLogDao> dao);
        List<UserBalanceLogDao> SelectAll();
    }
}