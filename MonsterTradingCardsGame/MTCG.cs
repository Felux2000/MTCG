using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Services;
using System.Data.SqlClient;

namespace MonsterTradingCardsGame
{
    internal class MTCG
    {
        static void Main(string[] args)
        {
            ResponseHandler ResponsHandler;
            MtcgServer Server;

            ResponsHandler = new ResponseHandler(new DatabaseService().DbConnection);
            Server = new(ResponsHandler);
            Server.Start();
        }
    }
}