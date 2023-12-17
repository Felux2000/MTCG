using MonsterTradingCardsGame.Cards;
using MonsterTradingCardsGame.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Daos
{
    internal class TransactionDao
    {
        NpgsqlDataSource DbConnection;

        public TransactionDao(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
        }

        public void Create(Transaction transaction)
        {
            string query = "INSERT INTO \"transactions\" (transactionid, username, obtainedid, seller, soldcardid, coins, type) VALUES (@transactionid, @username, @obtainedid, @seller, @soldcardid, @coins, @type)";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("transactionid", transaction.Id);
                cmd.Parameters.AddWithValue("username", transaction.Username);
                cmd.Parameters.AddWithValue("obtainedid", transaction.ObtainedID);
                cmd.Parameters.AddWithValue("seller", transaction.Seller);
                cmd.Parameters.AddWithValue("soldcardid", transaction.SoldCardID);
                cmd.Parameters.AddWithValue("coins", transaction.Coins);
                cmd.Parameters.AddWithValue("type", (int)transaction.Type);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Transaction> ReadAll()
        {
            List<Transaction> transactions = new();
            string query = "SELECT transactionid, username, obtainedid, seller, soldcardid, coins, type FROM \"transactions\" ORDER BY username, type;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Transaction tmpTransaction = new(
                            reader.GetGuid(0),
                            reader.GetString(1),
                            reader.GetGuid(2),
                            reader.GetString(3),
                            reader.GetGuid(4),
                            reader.GetInt32(5),
                            reader.GetInt32(6)
                            );
                        transactions.Add(tmpTransaction);
                    }
                }
            }
            return transactions;
        }

        public List<Transaction> ReadFromUser(string username)
        {
            List<Transaction> transactions = new();
            string query = "SELECT transactionid, username, obtainedid, seller, soldcardid, coins, type FROM \"transactions\" WHERE username = @username ORDER BY type;";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Transaction tmpTransaction = new(
                            reader.GetGuid(0),
                            reader.GetString(1),
                            reader.GetGuid(2),
                            reader.GetString(3),
                            reader.GetGuid(4),
                            reader.GetInt32(5),
                            reader.GetInt32(6)
                            );
                        transactions.Add(tmpTransaction);
                    }
                }
            }
            return transactions;
        }
    }
}
