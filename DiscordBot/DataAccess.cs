using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DiscordBot
{
    class DataAccess
    {

        public static readonly string CONNECTION_STRING_IATU =
                                      "Server=95.104.192.212;    " +
                                      "User=aist;                " +
                                      "database=raspisanie;      " +
                                      "port=3306;                " +
                                      "CharSet=utf8;            " +
                                      "password=123;             ";

        public List<T> Select<T, U>(string sql, U param, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sql, param).ToList();

                return rows;
            }
        }
    
        

        public void Insert<T>(string sql, T param, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                connection.Execute(sql, param);
            }
        }

        internal object Select<T1, T2>(object wEEKDAY, object p, string cONNECTION_STRING_IATU)
        {
            throw new NotImplementedException();
        }
    }
}
