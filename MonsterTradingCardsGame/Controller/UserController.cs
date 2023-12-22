using MonsterTradingCardsGame.Classes;
using MonsterTradingCardsGame.Daos;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Server.Responses;
using Newtonsoft.Json;
using Npgsql;
using System.Net;
using static MonsterTradingCardsGame.Server.ProtocolSpecs;


namespace MonsterTradingCardsGame.Controller
{
    internal class UserController : Controller
    {
        readonly TransactionDao transactionDao;
        public UserController(NpgsqlDataSource dbConnection) : base(new(dbConnection))
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
                user = userDao.Read(authtoken.Split(PSAuthTokenSeperator)[0]);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
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
                    return Response.Unauthorized();
                }
                users = userDao.ReadAll();
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
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
                User? userModel = JsonConvert.DeserializeObject<User?>(body.ToString()); ;

                if (userModel == null || userModel.Username == string.Empty || userModel.Username == null || userModel.Password == string.Empty || userModel.Password == null)
                {
                    return SendResponse("null", "Username or password not set", HttpStatusCode.BadRequest, ContentType.TEXT);
                }

                User user = userDao.Read(userModel.Username);
                if (user != null)
                {
                    return SendResponse("null", "User with same username already registered", HttpStatusCode.Conflict, ContentType.TEXT);
                }
                user = new(userModel.Username, userModel.Password);
                userDao.Create(user);

                return SendResponse("User successfully created", "null", HttpStatusCode.Created, ContentType.TEXT);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
        }

        //POST /sessions
        public Response LoginUser(string body)
        {
            try
            {
                User? userModel = JsonConvert.DeserializeObject<User?>(body.ToString());

                if (userModel == null || userModel.Username == null || userModel.Password == null)
                {
                    return SendResponse("null", "Username or password not set", HttpStatusCode.BadRequest, ContentType.TEXT);
                }

                if (!userDao.CheckCredentials(userModel.Username, userModel.Password))
                {
                    return SendResponse("null", "Invalid username/password provided", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }

                User user = userDao.Read(userModel.Username);
                if (user == null)
                {
                    return SendResponse("null", "User not found", HttpStatusCode.Conflict, ContentType.TEXT);
                }
                user.AuthToken = $"{user.Username}{PSAuthTokenSuffix}";
                userDao.Update(user);
                return SendResponse(user.AuthToken, "null", HttpStatusCode.OK, ContentType.JSON);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
        }

        public Response AquireCoins(string username, string body)
        {
            try
            {
                AquireCoinModel? coinModel = JsonConvert.DeserializeObject<AquireCoinModel?>(body.ToString());
                User user;

                user = userDao.Read(username);
                if (user == null)
                {
                    return SendResponse("null", "User not found", HttpStatusCode.NotFound, ContentType.TEXT);
                }
                if (!IsAuthorized($"{username}{PSAuthTokenSuffix}"))
                {
                    return SendResponse("null", "Incorrect Token", HttpStatusCode.Unauthorized, ContentType.TEXT);
                }

                if (coinModel == null)
                {
                    return Response.BadRequest();
                }
                user.Coins += coinModel.Coins;
                Transaction transaction = new(user.Username, Guid.Empty, "admin", Guid.Empty, coinModel.Coins, TransactionType.coinBuy);

                userDao.Update(user);
                transactionDao.Create(transaction);

                return SendResponse("Coins sucessfully added", "null", HttpStatusCode.OK, ContentType.JSON);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
        }

        //PUT /users/username
        public Response UpdateUser(string authtoken, string username, string body)
        {
            UpdateUserModel? userModel = JsonConvert.DeserializeObject<UpdateUserModel?>(body.ToString());
            bool newName = false;
            try
            {
                if (userModel == null)
                {
                    return Response.BadRequest();
                }
                User user;
                if (userModel.Name != string.Empty && userModel.Name != username)
                {
                    User checkUser = userDao.Read(userModel.Name);
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
                    user.AuthToken = $"{userModel.Name}{PSAuthTokenSuffix}";
                }
                user.Bio = userModel.Bio;
                user.Image = userModel.Image;
                if (userModel.Name == string.Empty)
                {
                    userDao.Update(user);
                }
                else
                {
                    userDao.Update(user, userModel.Name);
                }
                return SendResponse("User sucessfully updated", "null", HttpStatusCode.OK, ContentType.JSON);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return Response.InternalServerError();
            }
        }

    }
}
