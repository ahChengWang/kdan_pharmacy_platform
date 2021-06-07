using Dapper;
using MySql.Data.MySqlClient;
using PharmacyMask.Fundation.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Optimus.Utility.Helper
{
    public class MySqlDbHelper : IDbHelper
    {
        private readonly string _connectionString = string.Empty;

        public MySqlDbHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int ExecuteNonQuery(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    return conn.Execute(sql, param, transaction, null, commandType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public T ExecuteScalar<T>(string sql, object param = null, CommandType? commandType = null, IDbTransaction transaction = null)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    return conn.ExecuteScalar<T>(sql, param, transaction, null, commandType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<T> ExecuteQuery<T>(string sql, object param = null, CommandType? commandType = null)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    return conn.Query<T>(sql, param, null, true, null, commandType).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}