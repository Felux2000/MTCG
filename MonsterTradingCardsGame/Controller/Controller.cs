using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Daos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data.Common;

namespace MonsterTradingCardsGame.Controller
{
    internal class Controller
    {
        NpgsqlDataSource DbConnection;
        protected UserDao userDao;
        public Controller(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
            userDao = new UserDao(DbConnection);
        }

        public bool IsAuthorized(string authToken)
        {
            bool contains = userDao.CheckAuthToken(authToken);
            return contains;
        }

        public Response SendResponse(string data, string error, HttpStatusCode statusCode, ContentType contetnType)
        {
            if (statusCode == null)
            {
                throw new ArgumentException("Status cannot be null");
            }
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
