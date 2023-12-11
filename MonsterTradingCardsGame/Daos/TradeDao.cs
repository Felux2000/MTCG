using MonsterTradingCardsGame.Cards;
using MonsterTradingCardsGame.Models;
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
        NpgsqlDataSource DbConnection;

        public TradeDao(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
        }

        public void Create(TradingDeal trade)
        {
            string query = "INSERT INTO tradingdeals (tradeid, username, offeredcardid, mindamage, cardtype) VALUES (@tradeid, @username, @offeredcardid, @mindamage, @cardtype);";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("tradeid", Guid.Parse(trade.Id));
                cmd.Parameters.AddWithValue("username", trade.Username);
                cmd.Parameters.AddWithValue("offeredcardid", Guid.Parse(trade.CardToTrade));
                cmd.Parameters.AddWithValue("mindamage", trade.MinimumDamage);
                cmd.Parameters.AddWithValue("cardtype", trade.Type);
                cmd.ExecuteNonQuery();
            }
        }

        public TradingDeal Read(string tradeid)
        {
            string query = "SELECT tradeid, username, offeredcardid, mindamage, cardtype FROM tradingdeals WHERE tradeid = @tradeid;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("tradeid", Guid.Parse(tradeid));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        TradingDeal tmpDeal = new(
                            reader.GetGuid(0).ToString(),
                            reader.GetGuid(2).ToString(),
                            reader.GetInt32(3),
                            reader.GetString(4),
                            reader.GetString(1)
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
            string query = "SELECT tradeid, username, offeredcardid, mindamage, cardtype FROM tradingdeals;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TradingDeal tmpDeal = new(
                            reader.GetGuid(0).ToString(),
                            reader.GetGuid(2).ToString(),
                            reader.GetDouble(3),
                            reader.GetString(4),
                            reader.GetString(1)
                            );
                        tradeList.Add(tmpDeal);
                    }
                }
            }
            return tradeList;
        }

        public void Delete(string tradeid)
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
