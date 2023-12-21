using MonsterTradingCardsGame.Controller;
using MonsterTradingCardsGame.Server.Requests;
using MonsterTradingCardsGame.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.Server.ProtocolSpecs;

namespace MonsterTradingCardsGame.Server.Responses
{
    internal class GetHandler
    {
        public NpgsqlDataSource DbConnection { get; set; }
        private readonly UserController UserController;
        private readonly CardController CardController;
        private readonly TradingController TradingController;
        private readonly TransactionController TransactionController;
        public GetHandler(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
            UserController = new(DbConnection);
            CardController = new(DbConnection);
            TradingController = new(DbConnection);
            TransactionController = new(DbConnection);
        }

        public Response Handle(Request request)
        {
            if (request.AuthToken == null)
            {
                return Response.Unauthorized();
            }
            if (request.Path != null)
            {
                string[] splitPath = request.Path.Split(PSRequestPathSeperator);

                switch (request.Path)
                {
                    case "/cards": return CardController.GetUserCards(ResponseHandler.GetUsernameFromToken(request.AuthToken));
                    case "/deck":
                        if (request.Params != null)
                        {
                            return CardController.GetUserDeck(ResponseHandler.GetUsernameFromToken(request.AuthToken), request.Params.Contains(PSParameterContentPlain));
                        }
                        break;
                    case "/stats":
                        return UserController.GetStats(request.AuthToken);
                    case "/scoreboard":
                        return UserController.GetScoreBoard(request.AuthToken);
                    case "/tradings": return TradingController.GetTradingDeals(request.AuthToken);
                    case "/transactions": return TransactionController.GetAll(request.AuthToken);
                }
                if (Regex.IsMatch(request.Path, $"/transactions/{PSUsernamePattern}"))
                {
                    if (ResponseHandler.CompareAuthTokenToUser(request.AuthToken, ResponseHandler.GetIDFromPath(splitPath)))
                    {
                        return TransactionController.GetFromUser(ResponseHandler.GetIDFromPath(splitPath), request.AuthToken);
                    }
                    return Response.Unauthorized();
                }
                else if (Regex.IsMatch(request.Path, $"/users/{PSUsernamePattern}"))
                {
                    if (ResponseHandler.CompareAuthTokenToUser(request.AuthToken, ResponseHandler.GetIDFromPath(splitPath)))
                    {
                        return UserController.GetUserByName(ResponseHandler.GetIDFromPath(splitPath), request.AuthToken);
                    }
                    return Response.Unauthorized();
                }
            }
            return Response.MethodNotFound();
        }
    }
}
