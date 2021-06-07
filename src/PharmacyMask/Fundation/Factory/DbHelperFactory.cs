using Microsoft.Extensions.Configuration;
using Optimus.Utility.Helper;
using PharmacyMask.Fundation.Interface;

namespace PharmacyMask.Fundation.Factory
{
    public class DbHelperFactory
    {
        private static string _connectionString;
        private static IDbHelper _helper;

        public DbHelperFactory(IConfiguration config)
        {
            _connectionString = config.GetSection("MySql").GetValue<string>("ConnectionString");
            CreateHelper();
        }

        public static IDbHelper Get()
        {
            if (_helper == null)
            {
                CreateHelper();
            }
            return _helper;
        }

        private static void CreateHelper()
        {
            _helper = new MySqlDbHelper(_connectionString);
        }
    }
}
