using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Server;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MonsterTradingCardsGame.Controller
{
    internal class TransactionController : Controller
    {
        TransactionDao transactionDao;
        public TransactionController(NpgsqlDataSource dbConnection) : base(dbConnection)
        {
            transactionDao = new(dbConnection);
        }

        public Response GetAll(string authToken)
        {
            try
            {
                if (!IsAuthorized(authToken))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                List<Transaction> transactions = transactionDao.ReadAll();
                if (!transactions.Any())
                {
                    return SendResponse("No transactions available", "null", HttpStatusCode.NoContent, ContentType.TEXT);
                }
                string transactionDataJson = "[";
                transactionDataJson = $"{transactionDataJson}{TransactionData(transactions)}";
                transactionDataJson = $"{transactionDataJson}]";
                return SendResponse(transactionDataJson, "null", HttpStatusCode.OK, ContentType.JSON);

            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }

        public Response GetFromUser(string username, string authToken)
        {
            User user;
            try
            {
                user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User not found", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                if (user.AuthToken != authToken)
                {
                    return SendResponse("null", "Access token is missing or invalid", HttpStatusCode.Forbidden, ContentType.TEXT);
                }
                List<Transaction> transactions = transactionDao.ReadFromUser(authToken.Split("-")[0]);
                if (!transactions.Any())
                {
                    return SendResponse("No transactions available", "null", HttpStatusCode.NoContent, ContentType.TEXT);
                }
                string transactionDataJson = "[";
                transactionDataJson = $"{transactionDataJson}{TransactionData(transactions)}";
                transactionDataJson = $"{transactionDataJson}]";
                return SendResponse(transactionDataJson, "null", HttpStatusCode.OK, ContentType.JSON);

            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }

        private static string TransactionData(List<Transaction> transactions)
        {
            string transactionData = string.Empty;
            bool firstTransaction = true;
            foreach (Transaction transaction in transactions)
            {
                if (firstTransaction)
                {
                    transactionData = $"{transactionData}{transaction.ShowDetails()}";
                    firstTransaction = false;
                }
                else
                {
                    transactionData = $"{transactionData}, {transaction.ShowDetails()}";
                }
            }
            return transactionData;
        }
    }
}
