using MonsterTradingCardsGame.Controller;
using MonsterTradingCardsGame.Server.Requests;
using MonsterTradingCardsGame.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.Server.ProtocolSpecs;

namespace MonsterTradingCardsGame.Server.Responses
{
    internal class PostHandler
    {
        public NpgsqlDataSource DbConnection { get; set; }
        private readonly UserController UserController;
        private readonly CardController CardController;
        private readonly BattleController BattleController;
        private readonly TradingController TradingController;
        public PostHandler(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
            UserController = new(DbConnection);
            CardController = new(DbConnection);
            BattleController = new(DbConnection);
            TradingController = new(DbConnection);
        }

        public Response Handle(Request request)
        {
            if (request.Path != null)
            {
                switch (request.Path)
                {
                    case "/packages":
                        if (request.AuthToken == null)
                        {
                            return Response.Unauthorized();
                        }
                        if (request.Body != null)
                        {

                            if (ResponseHandler.CompareAuthTokenToUser(request.AuthToken, "admin"))
                            {
                                return CardController.CreatePackage(request.Body);
                            }
                            return new Response(HttpStatusCode.Forbidden, ContentType.TEXT, $"null error: User is prohibited from action \n");
                        }
                        break;

                    case "/transactions/packages":
                        if (request.AuthToken == null)
                        {
                            return Response.Unauthorized();
                        }
                        return CardController.AcquirePackage(ResponseHandler.GetUsernameFromToken(request.AuthToken));
                    case "/users":
                        if (request.Body != null)
                        {
                            return UserController.CreateUser(request.Body);
                        }
                        break;
                    case "/sessions":
                        if (request.Body != null)
                        {
                            return UserController.LoginUser(request.Body);
                        }
                        break;
                    case "/tradings":
                        if (request.AuthToken == null)
                        {
                            return Response.Unauthorized();
                        }
                        if (request.Body != null)
                        {
                            return TradingController.CreateTradingDeal(request.Body, ResponseHandler.GetUsernameFromToken(request.AuthToken));
                        }
                        break;
                    case "/battles":
                        if (request.AuthToken == null)
                        {
                            return Response.Unauthorized();
                        }
                        return BattleController.HaveBattle(ResponseHandler.GetUsernameFromToken(request.AuthToken));
                }
                if (Regex.IsMatch(request.Path, $"/tradings/{PSTradingIdPattern}"))
                {
                    string[] splitPath = request.Path.Split(PSRequestPathSeperator);

                    if (request.AuthToken == null)
                    {
                        return Response.Unauthorized();
                    }
                    return TradingController.TakeTradingDeal(ResponseHandler.GetIDFromPath(splitPath), request.Body, ResponseHandler.GetUsernameFromToken(request.AuthToken));
                }
            }
            return Response.MethodNotFound();
        }
    }
}
