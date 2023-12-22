using MonsterTradingCardsGame.Classes;
using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Server.Responses;
using Npgsql;
using System.Net;
using static MonsterTradingCardsGame.Server.ProtocolSpecs;

namespace MonsterTradingCardsGame.Controller
{
    internal class TransactionController : Controller
    {
        readonly TransactionDao transactionDao;
        public TransactionController(NpgsqlDataSource dbConnection) : base(new(dbConnection))
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
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
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
                List<Transaction> transactions = transactionDao.ReadFromUser(authToken.Split(PSAuthTokenSeperator)[0]);
                if (!transactions.Any())
                {
                    return SendResponse("No transactions available", "null", HttpStatusCode.NoContent, ContentType.TEXT);
                }
                string transactionDataJson = "[";
                transactionDataJson = $"{transactionDataJson}{TransactionData(transactions)}";
                transactionDataJson = $"{transactionDataJson}]";
                return SendResponse(transactionDataJson, "null", HttpStatusCode.OK, ContentType.JSON);

            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
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
