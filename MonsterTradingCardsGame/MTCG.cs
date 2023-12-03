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
            Game Game;
            int Port = 10001;
            mtcgServer Server;
            try
            {
                Game = new Game(new DatabaseService().DbConnection);
                Server = new mtcgServer(Game, Port);
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