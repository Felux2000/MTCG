using MonsterTradingCardsGame.Services;
using System.Data.SqlClient;

namespace MonsterTradingCardsGame
{
    internal class MTCG
    {
        static void Main(string[] args)
        {
            Game Game;
            try
            {
                Game = new Game(new DatabaseService().DbConnection);
            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message);
            }
        }
    }
}