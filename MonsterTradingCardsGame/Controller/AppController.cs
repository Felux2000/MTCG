using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MonsterTradingCardsGame.Server;

namespace MonsterTradingCardsGame.Controller
{
    internal class AppController
    {
        private SqlConnection DbConnection;
        public AppController(SqlConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public Response handleRequest(Request request)
        {

        }
    }
}
