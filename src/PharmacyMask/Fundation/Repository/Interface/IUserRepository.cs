using PharmacyMask.Fundation.Dao;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IUserRepository
    {
        int Insert(List<UserDao> dao);
        List<UserDao> SelectAll();
        List<UserDao> SelectByOption(List<int> idList, string name);
    }
}