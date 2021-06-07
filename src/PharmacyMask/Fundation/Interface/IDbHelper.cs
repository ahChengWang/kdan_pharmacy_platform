using System.Collections.Generic;
using System.Data;

namespace PharmacyMask.Fundation.Interface
{
    public interface IDbHelper
    {
        int ExecuteNonQuery(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null);


        T ExecuteScalar<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null);


        List<T> ExecuteQuery<T>(string sql, object param = null, CommandType? commandType = null);

    }
}
