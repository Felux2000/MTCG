using Npgsql;

namespace MonsterTradingCardsGame.Services
{
    internal class DatabaseService
    {
        public NpgsqlDataSource DbConnection { get; }
        public DatabaseService()
        {
            DbConnection = NpgsqlDataSource.Create("Server=localhost;Port=5432;Username=mtcgUser;Password=mtcgPw;Database=mtcgdb");
        }
    }
}
