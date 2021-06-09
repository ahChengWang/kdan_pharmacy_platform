using PharmacyMask.Fundation.Dao;
using System;
using System.Collections.Generic;

namespace PharmacyMask.Fundation.Repository
{
    public interface IPharmacyDetailRepository
    {
        int Insert(List<PharmacyDetailDao> dao);
        List<PharmacyDetailDao> SelectAll();
        List<PharmacyDetailDao> SelectByDay(List<DayOfWeek> dayOfWeek = null);
    }
}