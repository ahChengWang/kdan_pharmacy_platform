using PharmacyMask.DomainService.Entity;
using System;
using System.Collections.Generic;

namespace PharmacyMask.DomainService
{
    public interface IPharmacyDomainService
    {
        List<PharmacyOpenTimeEntity> GetOpenDayTime(List<DayOfWeek> dayOfWeek);
        List<PharmacyEntity> GetPharmacyInfo(List<int> pharmacyId, string pharmacyName);
        bool MigrationPharmacy(List<PharmacyMigrationEntity> pharmacyMigraEntity);
        bool UpdatePharmacyName(int pharmacyId, string pharmacyName);
    }
}