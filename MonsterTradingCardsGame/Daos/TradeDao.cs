using MonsterTradingCardsGame.Cards;
using MonsterTradingCardsGame.Classes;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Daos
{
    internal class TradeDao
    {
        readonly NpgsqlDataSource DbConnection;

        public TradeDao(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
        }

        public void Create(TradingDeal trade)
        {
            string query = "INSERT INTO tradingdeals (tradeid, offeredcardid, cardtype, mindamage, coins, username) VALUES (@tradeid, @offeredcardid, @cardtype, @mindamage, @coins, @username);";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("tradeid", trade.Id);
                cmd.Parameters.AddWithValue("offeredcardid", trade.CardToTrade);
                cmd.Parameters.AddWithValue("cardtype", (int)trade.Type);
                cmd.Parameters.AddWithValue("mindamage", trade.MinimumDamage);
                cmd.Parameters.AddWithValue("coins", trade.CoinCost);
                cmd.Parameters.AddWithValue("username", trade.Username);
                cmd.ExecuteNonQuery();
            }
        }

        public TradingDeal Read(Guid tradeid)
        {
            string query = "SELECT tradeid, offeredcardid, cardtype, mindamage, coins, username FROM tradingdeals WHERE tradeid = @tradeid;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("tradeid", tradeid);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        TradingDeal tmpDeal = new(
                            reader.GetGuid(0),
                            reader.GetGuid(1),
                            reader.GetInt32(2),
                            reader.GetDouble(3),
                            reader.GetInt32(4),
                            reader.GetString(5)
                            );
                        return tmpDeal;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public List<TradingDeal> ReadAll()
        {
            List<TradingDeal> tradeList = new();
            string query = "SELECT tradeid, offeredcardid, cardtype, mindamage, coins, username FROM tradingdeals;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TradingDeal tmpDeal = new(
                            reader.GetGuid(0),
                            reader.GetGuid(1),
                            reader.GetInt32(2),
                            reader.GetDouble(3),
                            reader.GetInt32(4),
                            reader.GetString(5)
                            );
                        tradeList.Add(tmpDeal);
                    }
                }
            }
            return tradeList;
        }

        public void Delete(Guid tradeid)
        {
            string query = "DELETE FROM tradingdeals WHERE tradeid = @tradeid;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("tradeid", tradeid);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
