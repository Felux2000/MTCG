using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Server.Responses;
using System.Net;

namespace MonsterTradingCardsGame.Controller
{
    internal class Controller
    {
        protected UserDao userDao;
        public Controller(UserDao userdao)
        {
            userDao = userdao;
        }

        public bool IsAuthorized(string authToken)
        {
            bool contains = userDao.CheckAuthToken(authToken);
            return contains;
        }

        public Response SendResponse(string data, string error, HttpStatusCode statusCode, ContentType contetnType)
        {
            if (contetnType == ContentType.JSON)
            {
                return new(statusCode, ContentType.JSON, $"{{ \"data\": {data}, \"error\": {error} }}\n");
            }
            else
            {
                return new(statusCode, ContentType.TEXT, $"{data} error: {error} \n");
            }
        }
    }
}
