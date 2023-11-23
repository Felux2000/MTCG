using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MonsterTradingCardsGame.Services
{
    internal class DatabaseService
    {
        public SqlConnection DbConnection { get; }
        public DatabaseService()
        {
            DbConnection = new SqlConnection("user id=mtcgUser;password=mtcgPw;server=//localhost:5432/MTCGdb;Trusted_Connection=no;database=database;connection timeout=30");
        }
    }
}
