using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Controller
{
    internal class UserController : Controller
    {
        TransactionDao transactionDao;
        public UserController(NpgsqlDataSource dbConnection) : base(dbConnection)
        {
            transactionDao = new(dbConnection);
        }

        //GET /users/username
        public Response GetUserByName(string username, string authtoken)
        {
            User user;
            try
            {
                user = userDao.Read(username);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
            if (user == null)
            {
                return SendResponse("null", "User not found", HttpStatusCode.NotFound, ContentType.TEXT);
            }
            if (user.AuthToken != authtoken)
            {
                return SendResponse("null", "Access token is missing or invalid", HttpStatusCode.Forbidden, ContentType.TEXT);
            }
            string userDataJson = user.ShowData();
            return SendResponse(userDataJson, "null", HttpStatusCode.OK, ContentType.JSON);
        }

        //GET /stats
        public Response GetStats(string authtoken)
        {
            User user;
            try
            {
                bool authorized = userDao.CheckAuthToken(authtoken);
                if (!authorized)
                {
                    return SendResponse("null", "Access token is missing or invalid", HttpStatusCode.Forbidden, ContentType.TEXT);
                }
                user = userDao.Read(authtoken.Split("-")[0]);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
            if (user == null)
            {
                return SendResponse("null", "User not found", HttpStatusCode.NotFound, ContentType.TEXT);
            }
            string userStatsJson = user.ShowStats();
            return SendResponse(userStatsJson, "null", HttpStatusCode.OK, ContentType.JSON);
        }

        //GET /scoreboard
        public Response GetScoreBoard(string authtoken)
        {
            List<User> users;
            bool authorized;
            try
            {
                authorized = userDao.CheckAuthToken(authtoken);
                if (!authorized)
                {
                    return SendResponse("null", "Access token is missing or invalid", HttpStatusCode.Forbidden, ContentType.TEXT);
                }
                users = userDao.ReadAll();
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
            if (!users.Any())
            {
                return SendResponse("null", "No Users found", HttpStatusCode.NotFound, ContentType.TEXT);
            }
            users = users.OrderByDescending(o => o.Elo).ToList();
            string scoreDataJson = "[";
            bool firstUser = true;
            foreach (User user in users)
            {
                if (firstUser)
                {
                    scoreDataJson = $"{scoreDataJson}{user.ShowStats()}";
                    firstUser = false;
                }
                else
                {
                    scoreDataJson = $"{scoreDataJson}, {user.ShowStats()}";
                }
            }
            scoreDataJson = $"{scoreDataJson}]";
            return SendResponse(scoreDataJson, "null", HttpStatusCode.OK, ContentType.JSON);
        }

        //POST /users
        public Response CreateUser(string body)
        {
            try
            {
                string username;
                string password;
                JObject jsonUser = JsonConvert.DeserializeObject<JObject>(body.ToString());
                username = (string)jsonUser["Username"];
                password = (string)jsonUser["Password"];

                if (username == string.Empty || password == string.Empty)
                {
                    return SendResponse("null", "Username or password not set", HttpStatusCode.BadRequest, ContentType.TEXT);
                }

                User user;
                try
                {
                    user = userDao.Read(username);
                    if (user != null)
                    {
                        return SendResponse("null", "User with same username already registered", HttpStatusCode.Conflict, ContentType.TEXT);
                    }
                    user = new(username, password);
                    userDao.Create(user);
                }
                catch (NpgsqlException e)
                {
                    Console.WriteLine(e.StackTrace);
                    return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
                }
                return SendResponse("User successfully created", "null", HttpStatusCode.Created, ContentType.TEXT);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }

        //POST /sessions
        public Response LoginUser(string body)
        {
            string username = null;
            string password = null;
            JObject jsonUser = JsonConvert.DeserializeObject<JObject>(body.ToString());
            username = (string)jsonUser["Username"];
            password = (string)jsonUser["Password"];

            if (username == null || password == null)
            {
                return SendResponse("null", "Username or password not set", HttpStatusCode.BadRequest, ContentType.TEXT);
            }

            try
            {
                if (!userDao.CheckCredentials(username, password))
                {
                    return SendResponse("null", "Invalid username/password provided", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }

                User user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User not found", HttpStatusCode.Conflict, ContentType.TEXT);
                }
                user.AuthToken = $"{username}-mtcgToken";
                userDao.Update(user);
                return SendResponse(user.AuthToken, "null", HttpStatusCode.OK, ContentType.JSON);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }

        public Response AquireCoins(string username, string body)
        {
            JObject jsonCoins = JsonConvert.DeserializeObject<JObject>(body.ToString());
            int newCoins = (int)jsonCoins["Coins"];
            try
            {
                User user;

                user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User not found", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                if (!IsAuthorized(username + "-mtcgToken"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }
                user.Coins += newCoins;
                userDao.Update(user);

                Transaction transaction = new(user.Username, Guid.Empty, "admin", Guid.Empty, newCoins, TransactionType.coinBuy);
                transactionDao.Create(transaction);

                return SendResponse("Coins sucessfully added", "null", HttpStatusCode.OK, ContentType.JSON);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }

        //PUT /users/username
        public Response UpdateUser(string authtoken, string username, string body)
        {
            JObject jsonUser = JsonConvert.DeserializeObject<JObject>(body.ToString());
            string newUsername;
            bool newName = false;
            try
            {
                User user;
                newUsername = jsonUser["Name"].ToString();
                if (newUsername != string.Empty && newUsername != username)
                {
                    User checkUser = userDao.Read(newUsername);
                    if (checkUser != null)
                    {
                        return SendResponse("null", "User with same username already registered", HttpStatusCode.Conflict, ContentType.TEXT);
                    }
                    newName = true;
                }
                user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User not found", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                if (user.AuthToken != authtoken)
                {
                    return SendResponse("null", "Access token is missing or invalid", HttpStatusCode.Forbidden, ContentType.TEXT);
                }
                if (newName)
                {
                    user.AuthToken = $"{newUsername}-mtcgToken";
                }
                user.Bio = jsonUser["Bio"].ToString();
                user.Image = jsonUser["Image"].ToString();
                if (newUsername == string.Empty)
                {
                    userDao.Update(user);
                }
                else
                {
                    userDao.Update(user, newUsername);
                }
                return SendResponse("User sucessfully updated", "null", HttpStatusCode.OK, ContentType.JSON);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return SendResponse("null", "Internal Server Error", HttpStatusCode.InternalServerError, ContentType.TEXT);
            }
        }

    }
}
