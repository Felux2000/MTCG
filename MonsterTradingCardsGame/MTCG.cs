using MonsterTradingCardsGame.Controller;
using MonsterTradingCardsGame.Services;
using System.Data.SqlClient;

namespace MonsterTradingCardsGame
{
    internal class MTCG
    {
        static void Main(string[] args)
        {
            AppController AppController = null;
            try
            {
                AppController = new AppController(new DatabaseService().DbConnection);
            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message);
            }
        }
    }
}