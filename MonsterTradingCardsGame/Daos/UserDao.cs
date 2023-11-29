using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterTradingCardsGame.Daos
{
    internal class UserDao
    {
        NpgsqlDataSource DbConnection;

        public UserDao(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
        }

        public void Create(User user)
        {
            string query = "INSERT INTO users (Username, Password, Coins, Elo, Games, Bio, Image, AuthToken) VALUES (@username,@password,@coins,@elo,@games,@bio,@image,authtoken)";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("password", user.Password);
                cmd.Parameters.AddWithValue("coins", user.Coins);
                cmd.Parameters.AddWithValue("elo", user.Elo);
                cmd.Parameters.AddWithValue("games", user.GamesPlayed);
                cmd.Parameters.AddWithValue("bio", user.Bio);
                cmd.Parameters.AddWithValue("image", user.Image);
                cmd.Parameters.AddWithValue("authtoken", user.AuthToken);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
