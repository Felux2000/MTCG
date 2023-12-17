using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Cards;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace MonsterTradingCardsGame.Controller
{
    internal class BattleController : Controller
    {
        CardDao cardDao;
        public BattleController(NpgsqlDataSource dbConnection) : base(new(dbConnection))
        {
            cardDao = new(dbConnection);
        }

        public Response HaveBattle(string username)
        {
            try
            {
                if (!IsAuthorized(username + "-mtcgToken"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                User requestor = userDao.Read(username);
                if (requestor == null)
                {
                    return SendResponse("null", "User not found", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                List<Card> deckRequestor = cardDao.ReadDeck(requestor.Username);
                if (!requestor.HasDeck)
                {
                    return SendResponse("null", $"User has valid deck", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                if (deckRequestor.Count != 4)
                {
                    return SendResponse("null", $"Deck has the wrong amount of cards", HttpStatusCode.Conflict, ContentType.TEXT);
                }
                requestor.Deck = deckRequestor;

                User opponent;
                bool opponentFound = false;
                List<Card> deckOpponent;
                do
                {
                    opponent = userDao.GetOpponent(requestor.Elo, requestor.Username);
                    if (opponent == null)
                    {
                        return SendResponse("null", "No opponent found", HttpStatusCode.NotFound, ContentType.TEXT);
                    }
                    opponentFound = true;
                    deckOpponent = cardDao.ReadDeck(opponent.Username);
                    if (!opponent.HasDeck || deckOpponent.Count != 4)
                    {
                        opponent.HasDeck = false;
                        userDao.Update(opponent);
                        opponentFound = false;
                    }
                } while (!opponentFound);

                opponent.Deck = deckOpponent;

                Battle battle = new(requestor, opponent);
                battle.Fight();
                battle.UpdateStats();
                userDao.Update(battle.Requestor);
                return SendResponse(battle.BattleLog, "null", HttpStatusCode.OK, ContentType.TEXT);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }
    }
}
