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
            string query = "INSERT INTO \"packages\" (packageid, idone, idtwo, idthree, idfour, idfive) VALUES (@packageid, @idone, @idtwo, @idthree, @idfour, @idfive)";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("packageid", package.ID);
                cmd.Parameters.AddWithValue("idone", Guid.Parse(package.Cards[0].CardID));
                cmd.Parameters.AddWithValue("idtwo", Guid.Parse(package.Cards[1].CardID));
                cmd.Parameters.AddWithValue("idthree", Guid.Parse(package.Cards[2].CardID));
                cmd.Parameters.AddWithValue("idfour", Guid.Parse(package.Cards[3].CardID));
                cmd.Parameters.AddWithValue("idfive", Guid.Parse(package.Cards[4].CardID));
                cmd.ExecuteNonQuery();
            }
        }

        public Package ReadPackage()
        {
            Package package;
            List<Card> cards = new();

            Monitor.Enter(this);
            string query = "SELECT packageid, cardid, username, cardindex FROM cards JOIN packages ON cardid = idone OR cardid = idtwo OR cardid = idthree OR cardid = idfour OR cardid = idfive ORDER BY timestamp LIMIT 5;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    Guid packageID = Guid.Empty;
                    while (reader.Read())
                    {
                        packageID = reader.GetGuid(0);
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
                    package = new(cards, packageID);
                    Delete(packageID);
                    Monitor.Exit(this);
                    return package;
                }
            }
        }

        public void Delete(Guid packageid)
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
