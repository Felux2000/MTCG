using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Server.Responses;
using MonsterTradingCardsGame.Services;
using System.Data.SqlClient;


[assembly: System.Runtime.CompilerServices.InternalsVisibleToAttribute("TestProject")]
namespace MonsterTradingCardsGame
{
    internal class MTCG
    {
        static void Main(string[] args)
        {
            ResponseHandler ResponsHandler;
            MtcgServer Server;

            ResponsHandler = new ResponseHandler(new DatabaseService().DbConnection, true);
            Server = new(ResponsHandler);
            Server.Start();
        }
    }
}