using PharmacyMask.DomainService.Entity;
using System.Collections.Generic;

namespace PharmacyMask.DomainService
{
    public interface IMaskService
    {
        List<MaskDetailEntity> GetMaskDetail(MaskSearchOptionEntity optionEntity);
        bool MigrationMask(List<MaskMigrationEntity> maskMigraEntity);
        bool UpdateMask(MaskEntity maskEntity);
    }
}