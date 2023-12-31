﻿using MonsterTradingCardsGame.Cards;
using MonsterTradingCardsGame.Classes;
using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Server.Responses;
using Newtonsoft.Json;
using Npgsql;
using System.Net;
using static MonsterTradingCardsGame.Server.ProtocolSpecs;

namespace MonsterTradingCardsGame.Controller
{
    internal class TradingController : Controller
    {
        readonly CardDao cardDao;
        readonly TradeDao tradeDao;
        readonly TransactionDao transactionDao;
        public TradingController(NpgsqlDataSource dbConnection) : base(new(dbConnection))
        {
            cardDao = new(dbConnection);
            tradeDao = new(dbConnection);
            transactionDao = new(dbConnection);
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

        public Response CreateTradingDeal(string body, string username)
        {
            try
            {
                TradingDeal? trade = JsonConvert.DeserializeObject<TradingDeal>(body.ToString());

                if (trade == null)
                {
                    return Response.BadRequest();
                }

                if (!IsAuthorized($"{username}{PSAuthTokenSuffix}"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                User user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User not found", HttpStatusCode.NotFound, ContentType.TEXT);
                }

                if (trade.Id == Guid.Empty || trade.CardToTrade == Guid.Empty)
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
                return Response.InternalServerError();
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

        public Response TakeTradingDeal(string tradeID, string? body, string username)
        {
            bool coinTrade = false;
            Card? offeredCard = null;
            Card cardInTrade;
            TradingDeal trade;
            User acceptUser;
            User offerUser;

            try
            {

                Guid offeredCardID = Guid.Empty;
                if (body != null)
                {
                    offeredCardID = Guid.Parse(body.Trim().Trim('"').Trim());
                }

                if (!IsAuthorized($"{username}{PSAuthTokenSuffix}"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                //check if tradingDeal exists
                trade = tradeDao.Read(Guid.Parse(tradeID));
                if (trade == null)
                {
                    return SendResponse("null", "TradingDeal does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                if (trade.MinimumDamage == 0)
                {
                    coinTrade = true;
                }
                if (offeredCardID == Guid.Empty && !coinTrade)
                {
                    return SendResponse("null", "No Card offered", HttpStatusCode.NotFound, ContentType.TEXT);
                }

                acceptUser = userDao.Read(username);
                if (acceptUser == null)
                {
                    return SendResponse("null", "User does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                //check if trying to trade with self
                if (trade.Username == acceptUser.Username)
                {
                    return SendResponse("null", "Trying to trade with self", HttpStatusCode.Forbidden, ContentType.TEXT);
                }
                if (!coinTrade)
                {

                    //check if Card exists
                    offeredCard = cardDao.Read(offeredCardID);
                    if (offeredCard == null)
                    {
                        return SendResponse("null", "Offered Card does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                    }
                    //check if offered Card belongs to user
                    if (offeredCard.Username != acceptUser.Username)
                    {
                        return SendResponse("null", "Offered Card does not belong to User", HttpStatusCode.Forbidden, ContentType.TEXT);
                    }
                    //check if in Deck
                    if (offeredCard.InDeck || offeredCard.InStore)
                    {
                        return SendResponse("null", "Offered Card in Deck or already in a TradingDeal", HttpStatusCode.Forbidden, ContentType.TEXT);
                    }
                    //check if requirements met
                    if (offeredCard.Damage < trade.MinimumDamage || offeredCard.Type != trade.Type || acceptUser.Coins < trade.CoinCost)
                    {
                        return SendResponse("null", "Offered Card does not meet requirements or not enough coins", HttpStatusCode.Forbidden, ContentType.TEXT);
                    }
                }
                else
                {
                    if (acceptUser.Coins < trade.CoinCost)
                    {
                        return SendResponse("null", "Not enough coins", HttpStatusCode.Forbidden, ContentType.TEXT);
                    }
                }


                //carry out trade -> update offeredCard UID and cardInTrade UID + paused + coins
                offerUser = userDao.Read(trade.Username);
                string tradeOfferUser = offerUser.Username;
                string tradeAcceptUser = acceptUser.Username;
                cardInTrade = cardDao.Read(trade.CardToTrade);

                if (!coinTrade && offeredCard != null)
                {
                    offeredCard.Username = tradeOfferUser;
                    cardDao.Update(offeredCard);
                }
                cardInTrade.Username = tradeAcceptUser;
                cardInTrade.InStore = false;
                cardDao.Update(cardInTrade);

                offerUser.Coins += trade.CoinCost;
                acceptUser.Coins -= trade.CoinCost;
                userDao.Update(offerUser);
                userDao.Update(acceptUser);


                Transaction transactionBuyer = new(acceptUser.Username, cardInTrade.CardID, offerUser.Username, coinTrade ? Guid.Empty : offeredCard.CardID, -trade.CoinCost, TransactionType.trade);
                Transaction transactionSeller = new(offerUser.Username, coinTrade ? Guid.Empty : offeredCard.CardID, acceptUser.Username, cardInTrade.CardID, trade.CoinCost, TransactionType.trade);
                transactionDao.Create(transactionBuyer);
                transactionDao.Create(transactionSeller);

                tradeDao.Delete(trade.Id);

                return SendResponse("Trading deal successfully executed", "null", HttpStatusCode.OK, ContentType.TEXT);

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

        public Response DeleteTradingDeal(string tradeID, string username)
        {
            try
            {
                //check if authtoken exists
                if (!IsAuthorized($"{username}{PSAuthTokenSuffix}"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                //check if trade with this id exists
                TradingDeal trade = tradeDao.Read(Guid.Parse(tradeID));
                if (trade == null)
                {
                    return SendResponse("null", "TradingDeal does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                //check if card in trade belongs to user
                User user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                Card card = cardDao.Read(trade.CardToTrade);
                if (card == null)
                {
                    return SendResponse("null", "Card in Trade does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                if (trade.Username != user.Username)
                {
                    return SendResponse("null", "Card in Trade does not belong to User", HttpStatusCode.Forbidden, ContentType.TEXT);
                }
                //delete TradingDeal
                tradeDao.Delete(trade.Id);
                card.InStore = false;
                cardDao.Update(card);
                return SendResponse("Trading deal successfully deleted", "null", HttpStatusCode.OK, ContentType.TEXT);
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

