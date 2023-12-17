using MonsterTradingCardsGame.Server;
using Npgsql;
using NSubstitute;
using NSubstitute.Core;
using System.ComponentModel;
using System.Data.Common;
using Npgsql;

namespace TestProject
{
    public class ResponseHandlerTest
    {
        private NpgsqlDataSource connection;
        private ResponseHandler responseHandler;

        [SetUp]
        public void Setup()
        {
            connection = NpgsqlDataSource.Create("Server=localhost;");
            responseHandler = new(connection);
        }

        [Test]
        public void GetIDFromPath_match()
        {
            string path = "/users/testname";
            string[] split = path.Split("/");
            string expectedUsername = "testname";
            string realUsername;

            realUsername = ResponseHandler.GetIDFromPath(split);

            Assert.That(realUsername, Is.EqualTo(expectedUsername));
        }

        [Test]
        public void GetIDFromPathNull_ArgNullEx()
        {
            Assert.Throws<ArgumentNullException>(() => ResponseHandler.GetIDFromPath(null));
        }

        [Test]
        public void GetIDFromPathEmpty_ArgEx()
        {
            string[] splitPath = "".Split("/");
            Assert.Throws<ArgumentException>(() => ResponseHandler.GetIDFromPath(splitPath));
        }

        [Test]
        public void GetUsernameFromToken_match()
        {
            string authToken = "testname-token";
            string expectedUsername = "testname";
            string realUsername;

            realUsername = ResponseHandler.GetUsernameFromToken(authToken);

            Assert.That(realUsername, Is.EqualTo(expectedUsername));
        }

        [Test]
        public void GetUsernameFromTokenNull_ArgNullEx()
        {
            Assert.Throws<ArgumentNullException>(() => ResponseHandler.GetUsernameFromToken(null));
        }

        [Test]
        public void CompareAuthTokenToUser_match()
        {
            string authToken = "testname-token";
            string user = "testname";
            bool expected = true;
            bool real;

            real = ResponseHandler.CompareAuthTokenToUser(authToken, user);

            Assert.That(real, Is.EqualTo(expected));
        }

        [Test]
        public void CompareAuthTokenToUser_noMatch()
        {
            string authToken = "test-token";
            string user = "testname";
            bool expected = false;
            bool real;

            real = ResponseHandler.CompareAuthTokenToUser(authToken, user);

            Assert.That(real, Is.EqualTo(expected));
        }

        [Test]
        public void CompareAuthTokenToUser_ArgNullExh()
        {
            string user = "testname";

            Assert.Throws<ArgumentNullException>(() => ResponseHandler.CompareAuthTokenToUser(null, user));
        }
    }
}