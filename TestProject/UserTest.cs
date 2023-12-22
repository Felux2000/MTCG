using MonsterTradingCardsGame.Classes;

namespace TestProject
{
    public class UserTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NewUserDefaultValues()
        {
            User user = new("testuser", "testpass");

            int coinsExpected = 20;
            int EloExpected = 100;
            int WinsExpected = 0;
            int LossesExpected = 0;
            int GamesPlayedExpected = 0;

            Assert.Multiple(() =>
            {
                Assert.That(user.Coins, Is.EqualTo(coinsExpected));
                Assert.That(user.Elo, Is.EqualTo(EloExpected));
                Assert.That(user.Wins, Is.EqualTo(WinsExpected));
                Assert.That(user.Losses, Is.EqualTo(LossesExpected));
                Assert.That(user.GamesPlayed, Is.EqualTo(GamesPlayedExpected));
            });
        }

        [Test]
        public void UserShowData()
        {
            User user = new("testuser", "testpass");

            string expected = $"{{ \"Username\": \"{user.Username}\", \"Bio\": \"{user.Bio}\", \"Image\": \"{user.Image}\", \"Coins\": \"{user.Coins}\" }}";

            Assert.That(user.ShowData(), Is.EqualTo(expected));
        }

        [Test]
        public void UserShowElo()
        {
            User user = new("testuser", "testpass");

            string expected = $"{{ \"Username\": \"{user.Username}\", \"Elo\": \"{user.Elo}\" }}";

            Assert.That(user.ShowElo(), Is.EqualTo(expected));
        }

        [Test]
        public void UserShowScore()
        {
            User user = new("testuser", "testpass");

            string expected = $"{{ \"Name\": \"{user.Username}\", \"Elo\": \"{user.Elo}\", \"Wins\": \"{user.Wins}\", \"Losses\": \"{user.Losses}\", \"W / L\": \"{(user.GamesPlayed == 0 ? 0 : ((user.Losses) == 0 ? user.Wins : Math.Round((double)user.Wins / ((double)user.Losses), 2).ToString().Replace(',', '.')))}\" }}";

            Assert.That(user.ShowStats(), Is.EqualTo(expected));
        }
    }
}