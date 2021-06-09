using PharmacyMask.DomainService.Entity;
using System.Collections.Generic;

namespace PharmacyMask.DomainService
{
    public interface IUserDomainService
    {
        List<UserEntity> GetUserInfo(List<int> userIdList, string userName);
        bool MigrationUser(List<UserMigrationEntity> userMigraEntity);
    }
}