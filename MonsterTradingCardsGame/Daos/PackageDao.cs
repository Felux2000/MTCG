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
    internal class PackageDao
    {
        NpgsqlDataSource DbConnection;
        CardBuilder CardAssembler;
        public PackageDao(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
            CardAssembler = new();
        }

        public void Create(Package package)
        {
            string query = "INSERT INTO \"packages\" (packageid, idone, indexone, idtwo, indextwo, idthreem, indexthree, idfour, indexfour, idfive, indexfive) VALUES (@packageid, @idone, @indexone, @idtwo, @indextwo, @idthreem, @indexthree, @idfour, @indexfour, @idfive, @indexfive)";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("packageid", package.PackageID);
                cmd.Parameters.AddWithValue("idone", package.CardID[0]);
                cmd.Parameters.AddWithValue("indexone", package.CardIndex[0]);
                cmd.Parameters.AddWithValue("idtwo", package.CardID[1]);
                cmd.Parameters.AddWithValue("indextwo", package.CardIndex[1]);
                cmd.Parameters.AddWithValue("idthreem", package.CardID[2]);
                cmd.Parameters.AddWithValue("indexthree", package.CardIndex[2]);
                cmd.Parameters.AddWithValue("idfour", package.CardID[3]);
                cmd.Parameters.AddWithValue("indexfour", package.CardIndex[3]);
                cmd.Parameters.AddWithValue("idfive", package.CardID[4]);
                cmd.Parameters.AddWithValue("indexfive", package.CardIndex[4]);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Card> ReadPackage()
        {
            List<Card> cards = new();

            Monitor.Enter(this);
            string query = "SELECT packageid, idone, indexone, idtwo, indextwo, idthreem, indexthree, idfour, indexfour, idfive, indexfive FROM packages LIMIT 1;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            int cardindex = reader.GetInt32(i + 2);
                            Card tmpCard = new(
                                reader.GetString(i + 1),
                                string.Empty,
                                CardAssembler.GetCardName(cardindex),
                                CardAssembler.GetCardDamage(cardindex),
                                CardAssembler.GetCardElement(cardindex),
                                CardAssembler.GetCardType(cardindex),
                                false,
                                false,
                                cardindex
                                );
                            cards.Add(tmpCard);

                        }
                        Delete(reader.GetString(0));
                        Monitor.Exit(this);
                        return cards;
                    }
                    else
                    {
                        Monitor.Exit(this);
                        return null;
                    }
                }
            }
        }

        public void Delete(string packageid)
        {
            string query = "DELETE FROM packages WHERE packageid = @packageid;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("packageid", packageid);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
