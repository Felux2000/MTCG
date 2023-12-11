using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Server;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MonsterTradingCardsGame.Cards;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json.Linq;

namespace MonsterTradingCardsGame.Controller
{
    internal class TradingController : Controller
    {
        CardDao cardDao;
        TradeDao tradeDao;
        public TradingController(NpgsqlDataSource dbConnection) : base(dbConnection)
        {
            cardDao = new(dbConnection);
            tradeDao = new(dbConnection);
        }

        public Response GetTradingDeals(string authToken)
        {
            try
            {
                if (!IsAuthorized(authToken))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                List<TradingDeal> trades = tradeDao.ReadAll();
                if (!trades.Any())
                {
                    return SendResponse("No trading deals available", "null", HttpStatusCode.NoContent, ContentType.TEXT);
                }
                string tradeDataJson = "[";
                tradeDataJson = $"{tradeDataJson}{TradeData(trades)}";
                tradeDataJson = $"{tradeDataJson}]";
                return SendResponse(tradeDataJson, "null", HttpStatusCode.OK, ContentType.JSON);

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

        public Response CreateTradingDeal(string body, string username)
        {
            try
            {
                TradingDeal trade = JsonConvert.DeserializeObject<TradingDeal>(body.ToString());

                if (!IsAuthorized(username + "-mtcgToken"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                User user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User not found", HttpStatusCode.NotFound, ContentType.TEXT);
                }

                if (trade.Id == string.Empty || trade.CardToTrade == string.Empty)
                {
                    return SendResponse("null", "TradeID or CardToTradeID not set", HttpStatusCode.BadRequest, ContentType.TEXT);
                }

                trade.Username = user.Username;
                //check if card belongs to user
                Card cardOffered = cardDao.Read(trade.CardToTrade);
                if (cardOffered == null)
                {
                    return SendResponse("null", "Card not found", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                if (cardOffered.Username != user.Username || cardOffered.InDeck || cardOffered.InStore)
                {
                    return SendResponse("null", "The card is not owned by the user or still in deck or already in store", HttpStatusCode.Forbidden, ContentType.TEXT);
                }

                TradingDeal idCheckTrade = tradeDao.Read(trade.Id);
                if (idCheckTrade != null)
                {
                    return SendResponse("null", "A deal with this deal ID already exists", HttpStatusCode.Conflict, ContentType.TEXT);

                }

                tradeDao.Create(trade);
                cardOffered.InStore = true;
                cardDao.Update(cardOffered);
                return SendResponse(trade.ShowDeal(), "null", HttpStatusCode.Created, ContentType.JSON);

            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
            /*catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }*/
        }

        private static string TradeData(List<TradingDeal> trades)
        {
            string tradeData = string.Empty;
            bool firstTrade = true;
            foreach (TradingDeal trade in trades)
            {
                if (firstTrade)
                {
                    tradeData = $"{tradeData}{trade.ShowDeal()}";
                    firstTrade = false;
                }
                else
                {
                    tradeData = $"{tradeData}, {trade.ShowDeal()}";
                }
            }
            return tradeData;
        }
    }
}

