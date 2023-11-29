using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Npgsql;

namespace MonsterTradingCardsGame.Services
{
    internal class DatabaseService
    {
        public NpgsqlDataSource DbConnection { get; }
        public DatabaseService()
        {
            DbConnection = NpgsqlDataSource.Create("Host=//localhost:5432/MTCGdb;Username=mtcgUser;Password=mtcgPw;Database=database");
        }
    }
}
