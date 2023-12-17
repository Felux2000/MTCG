using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;
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
            string query = "INSERT INTO \"users\" (username, password, coins, elo, wins, losses, games, bio, image, deck, token) VALUES (@username,@password,@coins,@elo,@wins,@losses,@games,@bio,@image,@deck,@authtoken)";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("password", user.Password);
                cmd.Parameters.AddWithValue("coins", user.Coins);
                cmd.Parameters.AddWithValue("elo", user.Elo);
                cmd.Parameters.AddWithValue("wins", user.Wins);
                cmd.Parameters.AddWithValue("losses", user.Losses);
                cmd.Parameters.AddWithValue("games", user.GamesPlayed);
                cmd.Parameters.AddWithValue("bio", user.Bio);
                cmd.Parameters.AddWithValue("image", user.Image);
                cmd.Parameters.AddWithValue("deck", user.HasDeck);
                cmd.Parameters.AddWithValue("authtoken", user.AuthToken);
                cmd.ExecuteNonQuery();
            }
        }

        public List<User> ReadAll()
        {
            List<User> userList = new List<User>();
            string query = "SELECT username, coins, elo, wins, losses, games, bio, image, token, deck FROM \"users\";";
            using (var cmd = DbConnection.CreateCommand(query))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    User tmpUser = new(
                        reader.GetString(0),
                        reader.GetInt32(1),
                        reader.GetInt32(2),
                        reader.GetInt32(3),
                        reader.GetInt32(4),
                        reader.GetInt32(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        reader.GetString(8),
                        reader.GetBoolean(9)
                        );
                    userList.Add(tmpUser);
                }
            }
            return userList;
        }

        public User Read(string username)
        {
            string query = "SELECT username, coins, elo, wins, losses, games, bio, image, token, deck FROM \"users\" WHERE username = @username;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        User tmpUser = new(
                        reader.GetString(0),
                        reader.GetInt32(1),
                        reader.GetInt32(2),
                        reader.GetInt32(3),
                        reader.GetInt32(4),
                        reader.GetInt32(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        reader.GetString(8),
                        reader.GetBoolean(9)
                        );
                        return tmpUser;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public bool CheckCredentials(string username, string password)
        {
            string query = "SELECT * FROM \"users\" WHERE username = @username AND password = @password;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool CheckAuthToken(string authToken)
        {
            string query = "SELECT * FROM \"users\" WHERE token = @authtoken;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("authtoken", authToken);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public void Update(User user, string newname = null)
        {
            string query = "UPDATE \"users\" SET username = @newname, coins = @coins, elo = @elo, games = @games, bio = @bio, image = @image, deck=@deck, wins=@wins, losses=@losses, token = @authtoken WHERE username = @username;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                if (newname == null)
                {
                    cmd.Parameters.AddWithValue("newname", user.Username);
                }
                else
                {
                    cmd.Parameters.AddWithValue("newname", newname);
                }
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("coins", user.Coins);
                cmd.Parameters.AddWithValue("elo", user.Elo);
                cmd.Parameters.AddWithValue("games", user.GamesPlayed);
                cmd.Parameters.AddWithValue("bio", user.Bio);
                cmd.Parameters.AddWithValue("image", user.Image);
                cmd.Parameters.AddWithValue("deck", user.HasDeck);
                cmd.Parameters.AddWithValue("wins", user.Wins);
                cmd.Parameters.AddWithValue("losses", user.Losses);
                cmd.Parameters.AddWithValue("authtoken", user.AuthToken);
                cmd.ExecuteNonQuery();
            }
        }
        public User GetOpponent(int elo, string username)
        {
            User opponent = null;
            string query = "SELECT username, coins, elo, wins, losses, games, bio, image, token, deck FROM \"users\" WHERE elo > @elo AND username != @username AND deck = true ORDER BY elo ASC;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("elo", elo);
                cmd.Parameters.AddWithValue("username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        opponent = new(
                        reader.GetString(0),
                        reader.GetInt32(1),
                        reader.GetInt32(2),
                        reader.GetInt32(3),
                        reader.GetInt32(4),
                        reader.GetInt32(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        reader.GetString(8),
                        reader.GetBoolean(9)
                        );
                        return opponent;
                    }
                }
            }
            query = "SELECT username, coins, elo, wins, losses, games, bio, image, token, deck FROM \"users\" WHERE elo <= @elo AND username != @username AND deck = true ORDER BY elo DESC;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("elo", elo);
                cmd.Parameters.AddWithValue("username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        opponent = new(
                        reader.GetString(0),
                        reader.GetInt32(1),
                        reader.GetInt32(2),
                        reader.GetInt32(3),
                        reader.GetInt32(4),
                        reader.GetInt32(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        reader.GetString(8),
                        reader.GetBoolean(9)
                        );
                        return opponent;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
