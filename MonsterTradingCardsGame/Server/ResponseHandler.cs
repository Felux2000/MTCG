using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Npgsql;
using System.Net;
using MonsterTradingCardsGame.Controller;

namespace MonsterTradingCardsGame.Server
{
    internal class ResponseHandler
    {
        public NpgsqlDataSource DbConnection { get; set; }
        private UserController UserController;
        private CardController CardController;
        private TradingController TradingController;
        private BattleController BattleController;
        public ResponseHandler(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
            UserController = new(DbConnection);
            CardController = new(DbConnection);
            TradingController = new(DbConnection);
            BattleController = new(DbConnection);
        }

        public Response CreateResponse(Request request)
        {
            switch (request.HttpMethod)
            {
                case Method.GET:
                    {
                        if (request.AuthToken == null)
                        {
                            return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, $"null error: Access token is missing or invalid \n");
                        }
                        string[] splitPath = request.Path.Split('/');

                        switch (request.Path)
                        {
                            case "/cards": return CardController.GetUserCards(GetUsernameFromToken(request.AuthToken));
                            case "/deck": return CardController.GetUserDeck(GetUsernameFromToken(request.AuthToken), request.Params.Contains("format=plain"));
                            case "/stats":
                                return UserController.GetStats(request.AuthToken);
                            case "/scoreboard":
                                return UserController.GetScoreBoard(request.AuthToken);
                            case "/tradings": return TradingController.GetTradingDeals(request.AuthToken);
                        }
                        if (Regex.IsMatch(request.Path, "/users/[a-zA-Z]+"))
                        {
                            if (CompareAuthTokenToUser(request.AuthToken, GetIDFromPath(splitPath)))
                            {
                                return UserController.GetUserByName(GetIDFromPath(splitPath), request.AuthToken);
                            }
                            return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, $"null error: Access token is missing or invalid \n");
                        }
                        break;
                    }
                case Method.POST:
                    {
                        switch (request.Path)
                        {
                            case "/packages":
                                if (CompareAuthTokenToUser(request.AuthToken, "admin"))
                                {
                                    return CardController.CreatePackage(request.Body);
                                }
                                return new Response(HttpStatusCode.Forbidden, ContentType.TEXT, $"null error: User is prohibited from action \n");

                            case "/transactions/packages":
                                if (request.AuthToken == null)
                                {
                                    return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, $"null error: Access token is missing or invalid \n");
                                }
                                return CardController.AcquirePackage(GetUsernameFromToken(request.AuthToken));
                            case "/users":
                                return UserController.CreateUser(request.Body);
                            case "/sessions":
                                return UserController.LoginUser(request.Body);
                            case "/tradings":
                                if (request.AuthToken == null)
                                {
                                    return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, $"null error: Access token is missing or invalid \n");
                                }
                                return TradingController.CreateTradingDeal(request.Body, GetUsernameFromToken(request.AuthToken));
                            case "/battles": return BattleController.HaveBattle(GetUsernameFromToken(request.AuthToken));
                        }
                        if (Regex.IsMatch(request.Path, "/tradings/([A-Za-z0-9]+(-[A-Za-z0-9]+)+)"))
                        {
                            string[] splitPath = request.Path.Split('/');
                            if (request.AuthToken == null)
                            {
                                return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, $"null error: Access token is missing or invalid \n");
                            }
                            return TradingController.TakeTradingDeal(GetIDFromPath(splitPath), request.Body, GetUsernameFromToken(request.AuthToken));
                        }

                        break;
                    }
                case Method.PUT:
                    {
                        string[] splitPath = request.Path.Split('/');
                        if (request.Path == "/deck")
                        {
                            if (request.AuthToken == null)
                            {
                                return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, $"null error: Access token is missing or invalid \n");
                            }
                            return CardController.AssembleDeck(request.Body, GetUsernameFromToken(request.AuthToken));
                        }
                        else if (request.Path == "/coins")
                        {
                            if (request.AuthToken == null)
                            {
                                return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, $"null error: Access token is missing or invalid \n");
                            }
                            return UserController.AquireCoins(GetUsernameFromToken(request.AuthToken), request.Body);
                        }
                        else if (Regex.IsMatch(request.Path, "/users/[a-zA-Z]+"))
                        {
                            if (CompareAuthTokenToUser(request.AuthToken, GetIDFromPath(splitPath)))
                            {
                                return UserController.UpdateUser(request.AuthToken, GetIDFromPath(splitPath), request.Body);
                            }
                            return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, $"null error: Access token is missing or invalid \n");
                        }
                        break;
                    }
                case Method.DELETE:
                    {
                        string[] splitPath = request.Path.Split('/');
                        if (request.AuthToken == null)
                        {
                            return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, $"null error: Access token is missing or invalid \n");
                        }
                        else if (Regex.IsMatch(request.Path, "/tradings/([A-Za-z0-9]+(-[A-Za-z0-9]+)+)"))
                        {
                            return TradingController.DeleteTradingDeal(GetIDFromPath(splitPath), GetUsernameFromToken(request.AuthToken));
                        }
                        break;

                    }

            }

            return new Response(HttpStatusCode.NotFound, ContentType.TEXT, $"null error: Method {request.Path} not found \n");
        }

        public static string GetIDFromPath(string[] split)
        {
            //get username if in path
            if (split == null)
            {
                throw new ArgumentNullException("Path is null");
            }
            //username always last
            int length = split.Length;
            if (length < 2)
            {
                //path incomplete/too short to contain username
                throw new ArgumentException("Path incomplete");
            }
            return split[length - 1];
        }
        public static string GetUsernameFromToken(string auth)
        {
            //extract username from token
            if (auth == null)
            {
                throw new ArgumentNullException("AuthToken is null");
            }
            string[] splitAuth = auth.Split("-");
            return splitAuth[0];
        }
        public static bool CompareAuthTokenToUser(string auth, string username)
        {
            //auth == username-mtcgToken
            if (auth == null)
            {
                throw new ArgumentNullException("AuthToken is null");
            }
            string[] splitAuth = auth.Split("-");
            return splitAuth[0] == username;
        }

    }
}
