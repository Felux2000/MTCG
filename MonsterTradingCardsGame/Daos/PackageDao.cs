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
            string query = "INSERT INTO \"packages\" (idone, idtwo, idthree, idfour, idfive) VALUES (@idone, @idtwo, @idthree, @idfour, @idfive)";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("idone", Guid.Parse(package.CardID[0]));
                cmd.Parameters.AddWithValue("idtwo", Guid.Parse(package.CardID[1]));
                cmd.Parameters.AddWithValue("idthree", Guid.Parse(package.CardID[2]));
                cmd.Parameters.AddWithValue("idfour", Guid.Parse(package.CardID[3]));
                cmd.Parameters.AddWithValue("idfive", Guid.Parse(package.CardID[4]));
                cmd.ExecuteNonQuery();
            }
        }

        public List<Card> ReadPackage()
        {
            List<Card> cards = new();

            Monitor.Enter(this);
            string query = "SELECT packageid, cardid, username, cardindex FROM cards JOIN packages ON cardid = idone OR cardid = idtwo OR cardid = idthree OR cardid = idfour OR cardid = idfive ORDER BY packageid LIMIT 5;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    int packageID = -1;
                    while (reader.Read())
                    {
                        packageID = reader.GetInt32(0);
                        int cardIndex = reader.GetInt32(3);
                        Card tmpCard = new(
                            reader.GetGuid(1).ToString(),
                            reader.GetString(2),
                            CardAssembler.GetCardName(cardIndex),
                            CardAssembler.GetCardDamage(cardIndex),
                            CardAssembler.GetCardElement(cardIndex),
                            CardAssembler.GetCardType(cardIndex),
                            false,
                            false,
                            cardIndex,
                            CardAssembler.GetCardDescription(cardIndex)
                            );
                        cards.Add(tmpCard);

                    }
                    if (cards.Count != 5)
                    {
                        Monitor.Exit(this);
                        return null;
                    }
                    Delete(packageID);
                    Monitor.Exit(this);
                    return cards;
                }
            }
        }

        public void Delete(int packageid)
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
