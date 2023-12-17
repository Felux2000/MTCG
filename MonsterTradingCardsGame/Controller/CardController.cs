using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Cards;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonsterTradingCardsGame.Controller
{
    internal class CardController : Controller
    {
        CardDao cardDao;
        PackageDao packageDao;
        TransactionDao transactionDao;
        public CardController(NpgsqlDataSource dbConnection) : base(dbConnection)
        {
            cardDao = new(dbConnection);
            packageDao = new(dbConnection);
            transactionDao = new(dbConnection);
        }

        public Response GetUserCards(string username)
        {
            try
            {
                //check if Token ok
                if (!IsAuthorized(username + "-mtcgToken"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                User user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                List<Card> cards = cardDao.ReadAllFromUser(user.Username);
                if (cards.Count == 0)
                {
                    return SendResponse("No Cards for this User", "null", HttpStatusCode.NoContent, ContentType.TEXT);
                }
                string cardDataJson = "[";
                cardDataJson = $"{cardDataJson}{CardData(cards)}";
                cardDataJson = $"{cardDataJson}]";
                return SendResponse(cardDataJson, "null", HttpStatusCode.OK, ContentType.JSON);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }

        public Response GetUserDeck(string username, bool plain)
        {
            try
            {
                //check if Token ok
                if (!IsAuthorized(username + "-mtcgToken"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                User user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                List<Card> deck = cardDao.ReadDeck(user.Username);
                Console.WriteLine(deck.Count);
                if (deck.Count == 0)
                {
                    return SendResponse("No Cards in the Deck", "null", HttpStatusCode.NoContent, ContentType.TEXT);
                }
                string deckDataJson;
                if (plain)
                {
                    string cardDataJson = CardData(deck, plain);
                    return SendResponse(cardDataJson, "null", HttpStatusCode.OK, ContentType.TEXT);
                }
                else
                {
                    string cardDataJson = "[";
                    cardDataJson = $"{cardDataJson}{CardData(deck)}";
                    cardDataJson = $"{cardDataJson}]";
                    return SendResponse(cardDataJson, "null", HttpStatusCode.OK, ContentType.JSON);
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }

        public Response CreatePackage(string body)
        {
            try
            {
                List<Card> cards = new();
                string[] cardId = new string[5];
                int cardIndex;
                JObject[] jsonCard = JsonConvert.DeserializeObject<JObject[]>(body.ToString());
                int cardCount = jsonCard.Length;
                if (cardCount != 5)
                {
                    return SendResponse("null", "Not the appropriate number of cards", HttpStatusCode.Conflict, ContentType.TEXT);
                }
                for (int i = 0; i < cardCount; i++)
                {
                    cardId[i] = (string)jsonCard[i]["Id"];
                    cardIndex = (int)jsonCard[i]["Index"];
                    if (cardId[i] == string.Empty || cardIndex == null)
                    {
                        return SendResponse("null", "Invalid Information for card declaration", HttpStatusCode.BadRequest, ContentType.TEXT);
                    }
                    Card tmpCard = new(cardId[i], "admin", false, false, cardIndex);
                    cards.Add(tmpCard);
                }
                foreach (string id in cardId)
                {
                    if (cardDao.Read(id) != null)
                    {
                        return SendResponse("null", "At least one card in the packages already exists", HttpStatusCode.Conflict, ContentType.TEXT);
                    }
                }
                foreach (Card card in cards)
                {
                    cardDao.Create(card);
                }
                Package newPackage = new(cards);
                packageDao.Create(newPackage);
                return SendResponse("Package and cards successfully created", "null", HttpStatusCode.Created, ContentType.TEXT);

            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }

        public Response AcquirePackage(string username)
        {
            try
            {
                int packageCost = 5;
                if (!IsAuthorized(username + "-mtcgToken"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                User user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                if (user.Coins < 5)
                {
                    return SendResponse("null", "Not enough money for buying a card package", HttpStatusCode.Forbidden, ContentType.TEXT);
                }
                //get one package
                Package package = packageDao.ReadPackage();
                if (package == null)
                {
                    return SendResponse("null", "No card package available for buying", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                //change UID of cards in cards in cards table
                foreach (Card card in package.Cards)
                {
                    card.Username = user.Username;
                    cardDao.Update(card);
                }
                //deduct user money
                user.Coins -= packageCost;
                userDao.Update(user);

                Transaction transaction = new(user.Username, package.ID, "admin", Guid.Empty, -packageCost, TransactionType.package);
                transactionDao.Create(transaction);

                string packageDataJson = "[";
                packageDataJson = $"{packageDataJson}{CardData(package.Cards)}";
                packageDataJson = $"{packageDataJson}]";
                return SendResponse(packageDataJson, "null", HttpStatusCode.OK, ContentType.JSON);

            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }

        public Response AssembleDeck(string body, string username)
        {
            try
            {
                //check if Token ok
                if (!IsAuthorized(username + "-mtcgToken"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                body = body.Trim().TrimStart('[').TrimEnd(']');
                string[] splitByCardID = body.Split(",");
                List<string> cardIDs = new();
                foreach (string splitString in splitByCardID)
                {
                    string trimmed = splitString.Trim().Trim('"');
                    if (trimmed != string.Empty && !cardIDs.Contains(trimmed))
                    {
                        cardIDs.Add(trimmed);
                    }
                }
                if (cardIDs.Count != 4)
                {
                    return SendResponse("null", "The provided deck did not include the required amount of cards", HttpStatusCode.BadRequest, ContentType.TEXT);
                }

                //check if user exists
                User user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                }

                //check if Cards belong to right user
                List<Card> chosenCards = new();
                foreach (string cardID in cardIDs)
                {
                    Card tmpCard = cardDao.Read(cardID);
                    if (tmpCard == null)
                    {
                        return SendResponse("null", "Card does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                    }
                    else if (tmpCard.Username != user.Username || tmpCard.InStore)
                    {
                        return SendResponse("null", "At least one of the provided cards does not belong to the user or is not available.", HttpStatusCode.Forbidden, ContentType.TEXT);
                    }
                    chosenCards.Add(tmpCard);
                }

                user.HasDeck = true;
                //create new deck
                cardDao.UpdateDeck(chosenCards, user.Username);
                userDao.Update(user);
                return SendResponse("The deck has been successfully configured", "null", HttpStatusCode.OK, ContentType.TEXT);

            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }

        private static string CardData(List<Card> cards, bool plain = false)
        {
            string cardData = string.Empty;
            bool firstCard = true;
            foreach (Card card in cards)
            {
                if (firstCard)
                {
                    cardData = $"{cardData}{card.ShowCard(plain)}";
                    firstCard = false;
                }
                else
                {
                    cardData = $"{cardData}, {card.ShowCard(plain)}";
                }
            }
            return cardData;
        }
    }
}
