using PharmacyMask.Fundation.Factory;
using PharmacyMask.Fundation.Interface;

namespace PharmacyMask.Fundation.Repository
{
    public class BaseRepository
    {
        protected IDbHelper _dbHelper;

        public BaseRepository()
        {
            _dbHelper = DbHelperFactory.Get();
        }
    }
}
