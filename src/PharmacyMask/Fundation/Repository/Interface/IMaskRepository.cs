using PharmacyMask.Fundation.Dao;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IMaskRepository
    {
        int Insert(List<MaskDao> dao);
        List<MaskDao> SelectAll();
        List<MaskDao> SelectByOption(List<int> id, string name);
        int Update(MaskDao dao);
    }
}