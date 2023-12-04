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
        public ResponseHandler(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
            UserController = new(DbConnection);
        }

        /* public Response CreateResponse(Request request)
         { return new Response(HttpStatusCode.Accepted, ContentType.JSON, "test"); }
        */
        public Response CreateResponse(Request request)
        {
            switch (request.HttpMethod)
            {
                case Method.GET:
                    {
                        if (request.AuthToken == null)
                        {
                            return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, "Access token is missing or invalid");
                        }
                        string[] splitPath = request.Path.Split('/');

                        switch (request.Path)
                        {
                            //      case "/cards": return;//CardController needed
                            //      case "/decks": return;//CardController needed
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
                            return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, "Access token is missing or invalid");
                        }
                        break;
                    }
                case Method.POST:
                    {
                        switch (request.Path)
                        {
                            /*  case "/packages":
                                  if (CompareAuthTokenToUser(request.AuthToken, GetUsernameFromPath(splitPath)))
                                  {
                                      return;//CardController needed
                                  }
                                  return new Response(HttpStatusCode.Forbidden, "{ \"error\": \"Incorrect Token\", \"data\": null }");
                            */
                            //    case "/transaction/packages":return;//CardController needed
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
                        /*  if (request.Path == "/decks")
                          {
                              if (request.AuthToken == null)
                              {
                                  return new Response(HttpStatusCode.Unauthorized, "{ \"error\": \"No Token set\", \"data\": null }");
                              }
                              return; //CardController needed
                          }
                          else*/
                        if (Regex.IsMatch(request.Path, "/users/[a-zA-Z]+"))
                        {
                            if (CompareAuthTokenToUser(request.AuthToken, GetUsernameFromPath(splitPath)))
                            {
                                return UserController.UpdateUser(request.AuthToken, GetUsernameFromPath(splitPath), request.Body);
                            }
                            return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, "Access token is missing or invalid");
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
            return new Response(HttpStatusCode.NotFound, ContentType.TEXT, "Method Not Found");
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
