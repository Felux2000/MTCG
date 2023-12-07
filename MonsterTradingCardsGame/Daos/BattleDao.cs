using MonsterTradingCardsGame.Cards;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;


namespace MonsterTradingCardsGame.Daos
{
    internal class BattleDao
    {
        NpgsqlDataSource DbConnection;

        public BattleDao(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
        }

        public User GetOpponent(int elo, string username)
        {
            User opponent = null;
            string query = "SELECT username, coins, elo, wins, games, bio, image, token FROM \"users\" WHERE elo >= @elo AND username != @username ORDER BY elo ASC;";
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
                            reader.GetString(5),
                            reader.GetString(6),
                            reader.GetString(7)
                            );
                        return opponent;
                    }
                }
            }
            query = "SELECT username, coins, elo, wins, games, bio, image, token FROM \"users\" WHERE elo <= @elo username != @username ORDER BY elo DESC;";
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
                            reader.GetString(5),
                            reader.GetString(6),
                            reader.GetString(7)
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
