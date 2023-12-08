using MonsterTradingCardsGame.Cards;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Daos
{
    internal class CardDao
    {
        NpgsqlDataSource DbConnection;
        CardBuilder CardAssembler;

        public CardDao(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
            CardAssembler = new();
        }
        public void Create(Card card)
        {
            string query = "INSERT INTO \"cards\" (cardid, username, cardindex, indeck, instore) VALUES (@cardid, @username, @cardindex, @indeck, @instore)";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("cardid", Guid.Parse(card.CardID));
                cmd.Parameters.AddWithValue("username", card.Username);
                cmd.Parameters.AddWithValue("cardindex", card.Index);
                cmd.Parameters.AddWithValue("indeck", card.InDeck);
                cmd.Parameters.AddWithValue("instore", card.InStore);
                cmd.ExecuteNonQuery();
            }
        }

        public Card Read(string cardid)
        {
            Console.WriteLine($" card id {cardid}\n");
            string query = "SELECT cardid, username, cardindex, indeck, instore FROM \"cards\" WHERE cardid = @cardid;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("cardid", Guid.Parse(cardid));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int cardindex = reader.GetInt32(2);
                        Card tmpCard = new(
                            reader.GetGuid(0).ToString(),
                            reader.GetString(1),
                            CardAssembler.GetCardName(cardindex),
                            CardAssembler.GetCardDamage(cardindex),
                            CardAssembler.GetCardElement(cardindex),
                            CardAssembler.GetCardType(cardindex),
                            reader.GetBoolean(3),
                            reader.GetBoolean(4),
                            cardindex
                            );
                        return tmpCard;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public List<Card> ReadAllFromUser(string username)
        {
            List<Card> cards = new();
            string query = "SELECT cardid, username, cardindex, indeck, instore FROM \"cards\" WHERE username = @username;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int cardindex = reader.GetInt32(2);
                        Card tmpCard = new(
                            reader.GetGuid(0).ToString(),
                            reader.GetString(1),
                            CardAssembler.GetCardName(cardindex),
                            CardAssembler.GetCardDamage(cardindex),
                            CardAssembler.GetCardElement(cardindex),
                            CardAssembler.GetCardType(cardindex),
                            reader.GetBoolean(3),
                            reader.GetBoolean(4),
                            cardindex
                            );
                        cards.Add(tmpCard);
                    }
                }
            }
            return cards;
        }


        public void Update(Card card)
        {
            string query = "UPDATE \"cards\" SET username = @username, indeck = @indeck, instore = @instore WHERE cardid = @cardid;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("username", card.Username);
                cmd.Parameters.AddWithValue("indeck", card.InDeck);
                cmd.Parameters.AddWithValue("instore", card.InStore);
                cmd.Parameters.AddWithValue("cardid", Guid.Parse(card.CardID));
                cmd.ExecuteNonQuery();
            }
        }


        public void UpdateDeck(List<Card> cards, string username)
        {
            using (var batch = DbConnection.CreateBatch())
            {
                var batchcommand1 = new NpgsqlBatchCommand("UPDATE \"cards\" SET indeck = false WHERE username = @username;");
                batchcommand1.Parameters.AddWithValue("username", username);
                var batchcommand2 = new NpgsqlBatchCommand("UPDATE \"cards\" SET indeck = true WHERE username = @username and (cardid = @cardid1 OR cardid = @cardid2 OR cardid = @cardid3 OR cardid = @cardid4);");
                batchcommand2.Parameters.AddWithValue("username", username);
                for (int i = 0; i < 4; i++)
                {
                    batchcommand2.Parameters.AddWithValue($"cardid{i + 1}", Guid.Parse(cards[i].CardID));
                }
                batch.BatchCommands.Add(batchcommand1);
                batch.BatchCommands.Add(batchcommand2);
                batch.ExecuteNonQuery();
            }
        }

        public List<Card> ReadDeck(string username)
        {
            List<Card> cards = new();
            string query = "SELECT cardid, username, cardindex, indeck, instore FROM \"cards\" WHERE username = @username AND indeck = true;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int cardindex = reader.GetInt32(2);
                        Card tmpCard = new(
                            reader.GetGuid(0).ToString(),
                            reader.GetString(1),
                            CardAssembler.GetCardName(cardindex),
                            CardAssembler.GetCardDamage(cardindex),
                            CardAssembler.GetCardElement(cardindex),
                            CardAssembler.GetCardType(cardindex),
                            reader.GetBoolean(3),
                            reader.GetBoolean(4),
                            cardindex
                            );
                        cards.Add(tmpCard);
                    }
                }
            }
            return cards;
        }
    }
}
