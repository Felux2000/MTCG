using MonsterTradingCardsGame.Daos;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Controller
{
    internal class CardController : Controller
    {
        CardDao CardDao;
        public CardController(NpgsqlDataSource dbConnection) : base(dbConnection)
        {
            CardDao = new(dbConnection);
        }


    }
}
