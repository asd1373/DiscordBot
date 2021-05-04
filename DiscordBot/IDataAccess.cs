using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot
{
    public interface IDataAccess
    {
        void Insert<T>(string sql, T param, string connectionString);
        List<T> Select<T, U>(string sql, U param, string connectionString);
    }
}
