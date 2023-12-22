using MonsterTradingCardsGame.Cards;
using MonsterTradingCardsGame.Classes;
using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Server.Responses;
using Newtonsoft.Json;
using Npgsql;
using System.Net;
using static MonsterTradingCardsGame.Server.ProtocolSpecs;

namespace MonsterTradingCardsGame.Controller
{
    internal class CardController : Controller
    {
        readonly CardDao cardDao;
        readonly PackageDao packageDao;
        readonly TransactionDao transactionDao;
        readonly CardBuilder cardBuilder;
        public CardController(NpgsqlDataSource dbConnection) : base(new(dbConnection))
        {
            cardDao = new(dbConnection);
            packageDao = new(dbConnection);
            transactionDao = new(dbConnection);
            cardBuilder = new();
        }

        public Response GetUserCards(string username)
        {
            try
            {
                //check if Token ok
                if (!IsAuthorized($"{username}{PSAuthTokenSuffix}"))
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
                return Response.InternalServerError();
            }
        }

        public Response GetUserDeck(string username, bool plain)
        {
            try
            {
                //check if Token ok
                if (!IsAuthorized($"{username}{PSAuthTokenSuffix}"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                User user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User does not exist", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                List<Card> deck = cardDao.ReadDeck(user.Username);
                if (deck.Count == 0)
                {
                    return SendResponse("No Cards in the Deck", "null", HttpStatusCode.NoContent, ContentType.TEXT);
                }
                if (plain)
                {
                    string deckDataJson = CardData(deck, plain);
                    return SendResponse(deckDataJson, "null", HttpStatusCode.OK, ContentType.TEXT);
                }
                else
                {
                    string deckDataJson = "[";
                    deckDataJson = $"{deckDataJson}{CardData(deck)}";
                    deckDataJson = $"{deckDataJson}]";
                    return SendResponse(deckDataJson, "null", HttpStatusCode.OK, ContentType.JSON);
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
        }

        public Response CreatePackage(string body)
        {
            try
            {
                List<Card> cards = new();
                CardModel[]? cardModel = JsonConvert.DeserializeObject<CardModel[]?>(body.ToString());
                if (cardModel == null)
                {
                    return Response.BadRequest();
                }
                int cardCount = cardModel.Length;
                if (cardCount != 5)
                {
                    return SendResponse("null", "Not the appropriate number of cards", HttpStatusCode.Conflict, ContentType.TEXT);
                }
                for (int i = 0; i < cardCount; i++)
                {
                    if (cardModel[i].CardID != Guid.Empty)
                    {
                        Card tmpCard = new(cardModel[i].CardID, "admin", cardBuilder.GetCardName(cardModel[i].CardIndex), cardBuilder.GetCardDamage(cardModel[i].CardIndex), cardBuilder.GetCardElement(cardModel[i].CardIndex), cardBuilder.GetCardType(cardModel[i].CardIndex), false, false, cardModel[i].CardIndex);
                        cards.Add(tmpCard);
                    }
                    else
                    {
                        return SendResponse("null", "Invalid Information for card declaration", HttpStatusCode.BadRequest, ContentType.TEXT);
                    }
                }
                foreach (Card card in cards)
                {
                    if (cardDao.Read(card.CardID) != null)
                    {
                        return SendResponse("null", "At least one card in the packages already exists", HttpStatusCode.Conflict, ContentType.TEXT);
                    }
                    cardDao.Create(card);
                }
                Package newPackage = new(cards);
                packageDao.Create(newPackage);
                return SendResponse("Package and cards successfully created", "null", HttpStatusCode.Created, ContentType.TEXT);

            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Card index unknown", HttpStatusCode.Conflict, ContentType.TEXT);
            }
        }

        public Response AcquirePackage(string username)
        {
            try
            {
                int packageCost = 5;
                if (!IsAuthorized($"{username}{PSAuthTokenSuffix}"))
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
                return Response.InternalServerError();
            }
        }

        public Response AssembleDeck(string body, string username)
        {
            try
            {
                //check if Token ok
                if (!IsAuthorized($"{username}{PSAuthTokenSuffix}"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                body = body.Trim().TrimStart('[').TrimEnd(']');
                string[] splitByCardID = body.Split(",");
                List<Guid> cardIDs = new();
                foreach (string splitString in splitByCardID)
                {
                    Guid trimmed = Guid.Parse(splitString.Trim().Trim('"'));
                    if (trimmed != Guid.Empty && !cardIDs.Contains(trimmed))
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
                foreach (Guid cardID in cardIDs)
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
                return Response.InternalServerError();
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
