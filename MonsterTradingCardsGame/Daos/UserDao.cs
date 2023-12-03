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
            string query = "INSERT INTO \"users\" (username, password, coins, elo, games, bio, image, token) VALUES (@username,@password,@coins,@elo,@games,@bio,@image,@authtoken)";
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

        public List<User> ReadAll()
        {
            List<User> userList = new List<User>();
            string query = "SELECT * FROM users;";
            using (var cmd = DbConnection.CreateCommand(query))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    User tmpUser = new(
                        reader.GetString(0),
                        reader.GetInt32(2),
                        reader.GetInt32(3),
                        reader.GetInt32(4),
                        reader.GetString(5),
                        reader.GetString(6),
                        reader.GetString(7)
                        );
                    userList.Add(tmpUser);
                }
            }
            return userList;
        }

        public bool CheckAuthToken(string authToken)
        {
            return false;
        }
    }
}
