using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Npgsql;
using System.Net;
using MonsterTradingCardsGame.Controller;
using MonsterTradingCardsGame.Services;
using MonsterTradingCardsGame.Server.Requests;
using static MonsterTradingCardsGame.Server.ProtocolSpecs;

namespace MonsterTradingCardsGame.Server.Responses
{
    internal class ResponseHandler
    {
        public NpgsqlDataSource DbConnection { get; set; }
        private readonly DatabaseCreator DatabaseCreator;
        private readonly GetHandler GetHandler;
        private readonly PostHandler PostHandler;
        private readonly PutHandler PutHandler;
        private readonly DeleteHandler DeleteHandler;
        public ResponseHandler(NpgsqlDataSource dbConnection, bool dbRefresh)
        {
            DbConnection = dbConnection;
            DatabaseCreator = new(DbConnection);
            DatabaseCreator.PrepareDatabase(dbRefresh);
            GetHandler = new(dbConnection);
            PostHandler = new(dbConnection);
            PutHandler = new(dbConnection);
            DeleteHandler = new(dbConnection);
        }
        //for testing
        public ResponseHandler(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
            DatabaseCreator = new(DbConnection);
            GetHandler = new(dbConnection);
            PostHandler = new(dbConnection);
            PutHandler = new(dbConnection);
            DeleteHandler = new(dbConnection);
        }

        public Response CreateResponse(Request request)
        {
            if (request != null)
            {
                switch (request.HttpMethod)
                {
                    case Method.GET:
                        {
                            return GetHandler.Handle(request);
                        }
                    case Method.POST:
                        {
                            return PostHandler.Handle(request);
                        }
                    case Method.PUT:
                        {
                            return PutHandler.Handle(request);
                        }
                    case Method.DELETE:
                        {
                            return DeleteHandler.Handle(request);
                        }
                }
                return Response.MethodNotFound();
            }
            throw new ArgumentNullException();
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
            string[] splitAuth = auth.Split(PSAuthTokenSeperator);
            return splitAuth[0];
        }
        public static bool CompareAuthTokenToUser(string auth, string username)
        {
            //auth == username-mtcgToken
            if (auth == null)
            {
                throw new ArgumentNullException("AuthToken is null");
            }
            string[] splitAuth = auth.Split(PSAuthTokenSeperator);
            return splitAuth[0] == username;
        }

    }
}
