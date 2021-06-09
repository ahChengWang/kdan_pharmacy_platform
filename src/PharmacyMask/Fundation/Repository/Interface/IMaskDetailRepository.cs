using PharmacyMask.Fundation.Dao;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IMaskDetailRepository
    {
        int Insert(List<MaskDetailDao> dao);
        List<MaskDetailDao> SelectAll();
        List<MaskDetailDao> SelectByOption(List<int> idList, List<int> maskIdList);
    }
}