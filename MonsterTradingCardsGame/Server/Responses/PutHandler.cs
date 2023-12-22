using MonsterTradingCardsGame.Controller;
using MonsterTradingCardsGame.Server.Requests;
using Npgsql;
using System.Text.RegularExpressions;
using static MonsterTradingCardsGame.Server.ProtocolSpecs;

namespace MonsterTradingCardsGame.Server.Responses
{
    internal class PutHandler
    {
        public NpgsqlDataSource DbConnection { get; set; }
        private readonly UserController UserController;
        private readonly CardController CardController;
        public PutHandler(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
            UserController = new(DbConnection);
            CardController = new(DbConnection);
        }

        public Response Handle(Request request)
        {
            if (request.Path != null)
            {
                string[] splitPath = request.Path.Split(PSRequestPathSeperator);
                if (request.Path == "/deck")
                {
                    if (request.AuthToken == null)
                    {
                        return Response.Unauthorized();
                    }
                    if (request.Body != null)
                    {
                        return CardController.AssembleDeck(request.Body, ResponseHandler.GetUsernameFromToken(request.AuthToken));
                    }
                }
                else if (request.Path == "/coins")
                {
                    if (request.AuthToken == null)
                    {
                        return Response.Unauthorized();
                    }
                    if (request.Body != null)
                    {
                        return UserController.AquireCoins(ResponseHandler.GetUsernameFromToken(request.AuthToken), request.Body);
                    }
                }
                else if (Regex.IsMatch(request.Path, $"/users/{PSUsernamePattern}"))
                {
                    if (request.AuthToken == null)
                    {
                        return Response.Unauthorized();
                    }
                    if (ResponseHandler.CompareAuthTokenToUser(request.AuthToken, ResponseHandler.GetIDFromPath(splitPath)))
                    {
                        if (request.Body != null)
                        {
                            return UserController.UpdateUser(request.AuthToken, ResponseHandler.GetIDFromPath(splitPath), request.Body);
                        }
                    }
                }
            }
            return Response.MethodNotFound();
        }
    }
}
