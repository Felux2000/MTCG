using MonsterTradingCardsGame.Cards;
using Npgsql;

namespace MonsterTradingCardsGame.Daos
{
    internal class CardDao
    {
        readonly NpgsqlDataSource DbConnection;
        readonly CardBuilder CardAssembler;

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
                cmd.Parameters.AddWithValue("cardid", card.CardID);
                cmd.Parameters.AddWithValue("username", card.Username);
                cmd.Parameters.AddWithValue("cardindex", card.Index);
                cmd.Parameters.AddWithValue("indeck", card.InDeck);
                cmd.Parameters.AddWithValue("instore", card.InStore);
                cmd.ExecuteNonQuery();
            }
        }

        public Card Read(Guid cardid)
        {
            string query = "SELECT cardid, username, cardindex, indeck, instore FROM \"cards\" WHERE cardid = @cardid;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("cardid", cardid);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int cardIndex = reader.GetInt32(2);
                        Card tmpCard = new(
                            reader.GetGuid(0),
                            reader.GetString(1),
                            CardAssembler.GetCardName(cardIndex),
                            CardAssembler.GetCardDamage(cardIndex),
                            CardAssembler.GetCardElement(cardIndex),
                            CardAssembler.GetCardType(cardIndex),
                            reader.GetBoolean(3),
                            reader.GetBoolean(4),
                            cardIndex
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
                        int cardIndex = reader.GetInt32(2);
                        Card tmpCard = new(
                            reader.GetGuid(0),
                            reader.GetString(1),
                            CardAssembler.GetCardName(cardIndex),
                            CardAssembler.GetCardDamage(cardIndex),
                            CardAssembler.GetCardElement(cardIndex),
                            CardAssembler.GetCardType(cardIndex),
                            reader.GetBoolean(3),
                            reader.GetBoolean(4),
                            cardIndex,
                            CardAssembler.GetCardDescription(cardIndex)
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
                cmd.Parameters.AddWithValue("cardid", card.CardID);
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
                    batchcommand2.Parameters.AddWithValue($"cardid{i + 1}", cards[i].CardID);
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
                            reader.GetGuid(0),
                            reader.GetString(1),
                            CardAssembler.GetCardName(cardindex),
                            CardAssembler.GetCardDamage(cardindex),
                            CardAssembler.GetCardElement(cardindex),
                            CardAssembler.GetCardType(cardindex),
                            reader.GetBoolean(3),
                            reader.GetBoolean(4),
                            cardindex,
                            CardAssembler.GetCardDescription(cardindex)
                            );
                        cards.Add(tmpCard);
                    }
                }
            }
            return cards;
        }
    }
}
