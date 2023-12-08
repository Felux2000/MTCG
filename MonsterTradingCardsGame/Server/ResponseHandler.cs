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
        public ResponseHandler(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
            UserController = new(DbConnection);
            CardController = new(DbConnection);
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
                                //        case "/tradings":return;//TradinController needed
                        }
                        if (Regex.IsMatch(request.Path, "/users/[a-zA-Z]+"))
                        {
                            if (CompareAuthTokenToUser(request.AuthToken, GetUsernameFromPath(splitPath)))
                            {
                                return UserController.GetUserByName(GetUsernameFromPath(splitPath), request.AuthToken);
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
                                return new Response(HttpStatusCode.Forbidden, ContentType.TEXT, $"null error: Provided user is not \"admin\" \n");

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
                                //  case "/tradings": return;//TradinController needed
                                //    case "/battles":return;//BattleController needed
                        }
                        /*    if (Regex.IsMatch(request.Path, "/tradings/([A-Za-z0-9]+(-[A-Za-z0-9]+)+)"))
                            {
                                string[] splitPath = request.Path.Split('/');
                                if (request.AuthToken == null)
                                {
                                    return new Response(HttpStatusCode.Unauthorized, "{ \"error\": \"No Token set\", \"data\": null }");
                                }
                                return;//Tradingcontroller needed
                            }*/

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
                        else if (Regex.IsMatch(request.Path, "/users/[a-zA-Z]+"))
                        {
                            if (CompareAuthTokenToUser(request.AuthToken, GetUsernameFromPath(splitPath)))
                            {
                                return UserController.UpdateUser(request.AuthToken, GetUsernameFromPath(splitPath), request.Body);
                            }
                            return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, $"null error: Access token is missing or invalid \n");
                        }
                        break;
                    }/*
                case Method.DELETE:
                    {
                        string[] splitPath = request.Path.Split('/');
                        if (request.AuthToken == null)
                        {
                            return new Response(HttpStatusCode.Unauthorized, "{ \"error\": \"No Token set\", \"data\": null }");
                        }
                        else if (Regex.IsMatch(request.Path, "/tradings/([A-Za-z0-9]+(-[A-Za-z0-9]+)+)"))
                        {
                            return; //TradingController needed
                        }
                    break;

                    }
                    */
            }
            return new Response(HttpStatusCode.NotFound, ContentType.TEXT, $"null error: Method not found \n");
        }

        public static string GetUsernameFromPath(string[] split)
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
