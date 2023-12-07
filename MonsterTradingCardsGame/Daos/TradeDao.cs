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
            string query = "INSERT INTO tradingdeals (tradeid, username, offeredcardid, mindamage, cardtype) VALUES (@tradid, @username, @offeredcardid, @mindamage, @cardtype)";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("tradeid", trade.TradeID);
                cmd.Parameters.AddWithValue("username", trade.Username);
                cmd.Parameters.AddWithValue("offeredcardid", trade.OfferedCardID);
                cmd.Parameters.AddWithValue("mindamage", trade.MinimumDmg);
                cmd.Parameters.AddWithValue("cardtype", trade.CardType);
                cmd.ExecuteNonQuery();
            }
        }

        public List<TradingDeal> ReadAll()
        {
            List<TradingDeal> tradeList = new();
            string query = "SELECT tradeid, username, offeredcardid, mindamage, cardtype FROM tradingdeals";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TradingDeal tmpDeal = new(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetInt32(3),
                            reader.GetInt32(4)
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
