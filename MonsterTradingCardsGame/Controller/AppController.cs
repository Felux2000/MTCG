using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MonsterTradingCardsGame.Server;
using System.Net.Mime;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonsterTradingCardsGame.Controller
{
    internal class AppController
    {
        public SqlConnection DbConnection { get; set; }
        public AppController(SqlConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public Response HandleRequest(Request request)
        {
            switch (request.HttpMethod)
            {
                case Method.GET:
                    {
                        if (request.AuthToken == null)
                        {
                            return new Response(System.Net.HttpStatusCode.Unauthorized, "{ \"error\": \"No Token set\", \"data\": null }");
                        }
                        string[] splitPath = request.Path.Split('/');
                        //get cards from user
                        switch (request.Path)
                        {
                            case "/cards":
                                return;//CardController needed
                            case "/decks":
                                return;//CardController needed
                            case "/users":
                                return;//UserController needed
                            case "/stats":
                                return;//UserController needed
                            case "/scores":
                                return;//UserController needed
                            case "/tradings":
                                return;//TradinController needed
                        }
                        if (Regex.IsMatch(request.Path, "/users/[a-zA-Z]+"))
                        {
                            if (CompareAuthTokenToUser(request.AuthToken, GetUsernameFromPath(splitPath)))
                            {
                                return; //UserCOntroller needed
                            }
                            return new Response(System.Net.HttpStatusCode.Forbidden, "{ \"error\": \"Incorrect Token\", \"data\": null }");
                        }
                        break;
                    }
                case Method.POST:
                    {

                        break;
                    }
                case Method.PUT:
                    {

                        break;
                    }
                case Method.DELETE:
                    {

                        break;
                    }
            }
            return new Response(System.Net.HttpStatusCode.NotFound, "{ \"error\": \"Method Not Found\", \"data\": null }");
        }

        public string GetUsernameFromPath(string[] split)
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
        public string GetUsernameFromToken(string auth)
        {
            //extract username from token
            if (auth == null)
            {
                throw new ArgumentNullException("AuthToken is null");
            }
            string[] splitAuth = auth.Split("-");
            return splitAuth[0];
        }
        public bool CompareAuthTokenToUser(string auth, string username)
        {
            //auth == username-mtcgToken
            if (auth == null)
            {
                throw new ArgumentNullException("AuthToken is null");
            }
            string[] splitAuth = auth.Split("-");
            return (splitAuth[0] == username);
        }

    }
}
