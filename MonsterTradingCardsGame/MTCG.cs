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
            try
            {
                ResponsHandler = new ResponseHandler(new DatabaseService().DbConnection);
                Server = new(ResponsHandler);
                Server.Start();
            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message);
            }

            /*
            UserDao testdao = new UserDao(new DatabaseService().DbConnection);
            //testdao.Create(new User("Jonas", "Jonas"));
            foreach (var item in testdao.ReadAll())
            {
                Console.WriteLine(item.Username + " " + item.Coins);
            }
            */
        }
    }
}