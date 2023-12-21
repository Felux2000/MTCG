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
    internal class DeleteHandler
    {
        public NpgsqlDataSource DbConnection { get; set; }
        private readonly TradingController TradingController;
        public DeleteHandler(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
            TradingController = new(DbConnection);
        }

        public Response Handle(Request request)
        {
            if (request.Path != null)
            {
                string[] splitPath = request.Path.Split(PSRequestPathSeperator);
                if (request.AuthToken == null)
                {
                    return Response.Unauthorized();
                }
                else if (Regex.IsMatch(request.Path, $"/tradings/{PSTradingIdPattern}"))
                {
                    return TradingController.DeleteTradingDeal(ResponseHandler.GetIDFromPath(splitPath), ResponseHandler.GetUsernameFromToken(request.AuthToken));
                }
            }
            return Response.MethodNotFound();
        }
    }
}
