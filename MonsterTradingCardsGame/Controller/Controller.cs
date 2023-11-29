using MonsterTradingCardsGame.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Controller
{
    internal class Controller
    {

        public bool IsAuthorized(string authToken)
        {
            bool contains = UserDao.checkAuthToken(authToken);
            return contains;
        }

        public Response SendResponse(string data, string error, HttpStatusCode statusCode)
        {
            if (statusCode == null)
            {
                throw new ArgumentException("Status cannot be null");
            }
            return new(statusCode, data);
        }
    }
}
