using MonsterTradingCardsGame.Cards;
using MonsterTradingCardsGame.Classes;

namespace TestProject
{
    public class BattleTest
    {
        private Battle battle;
        private CardBuilder cardBuilder;

        [SetUp]
        public void Setup()
        {
            battle = new(new("user1", "pass1"), new("user2", "pass2"));
            cardBuilder = new();
        }

        [Test]
        public void RoundWinner_user1()
        {
            int cardUser1Index = 0;
            int cardUser2Index = 1;
            Battle.Winner expectedWinner = Battle.Winner.requestor;
            Battle.Winner realWinner;

            realWinner = battle.RoundWinner(new(Guid.NewGuid(), battle.Requestor.Username, cardBuilder.GetCardName(cardUser1Index), cardBuilder.GetCardDamage(cardUser1Index), cardBuilder.GetCardElement(cardUser1Index), cardBuilder.GetCardType(cardUser1Index), false, false, cardUser1Index), new(Guid.NewGuid(), battle.Opponent.Username, cardBuilder.GetCardName(cardUser2Index), cardBuilder.GetCardDamage(cardUser2Index), cardBuilder.GetCardElement(cardUser2Index), cardBuilder.GetCardType(cardUser2Index), false, false, cardUser2Index));

            Assert.That(realWinner, Is.EqualTo(expectedWinner));
        }

        [Test]
        public void RoundWinner_user2()
        {
            int cardUser1Index = 1;
            int cardUser2Index = 0;
            Battle.Winner expectedWinner = Battle.Winner.opponent;
            Battle.Winner realWinner;

            realWinner = battle.RoundWinner(new(Guid.NewGuid(), battle.Requestor.Username, cardBuilder.GetCardName(cardUser1Index), cardBuilder.GetCardDamage(cardUser1Index), cardBuilder.GetCardElement(cardUser1Index), cardBuilder.GetCardType(cardUser1Index), false, false, cardUser1Index), new(Guid.NewGuid(), battle.Opponent.Username, cardBuilder.GetCardName(cardUser2Index), cardBuilder.GetCardDamage(cardUser2Index), cardBuilder.GetCardElement(cardUser2Index), cardBuilder.GetCardType(cardUser2Index), false, false, cardUser2Index));

            Assert.That(realWinner, Is.EqualTo(expectedWinner));
        }

        [Test]
        public void RoundWinner_draw()
        {
            int cardUser1Index = 1;
            int cardUser2Index = 1;
            Battle.Winner expectedWinner = Battle.Winner.none;
            Battle.Winner realWinner;

            realWinner = battle.RoundWinner(new(Guid.NewGuid(), battle.Requestor.Username, cardBuilder.GetCardName(cardUser1Index), cardBuilder.GetCardDamage(cardUser1Index), cardBuilder.GetCardElement(cardUser1Index), cardBuilder.GetCardType(cardUser1Index), false, false, cardUser1Index), new(Guid.NewGuid(), battle.Opponent.Username, cardBuilder.GetCardName(cardUser2Index), cardBuilder.GetCardDamage(cardUser2Index), cardBuilder.GetCardElement(cardUser2Index), cardBuilder.GetCardType(cardUser2Index), false, false, cardUser2Index));

            Assert.That(realWinner, Is.EqualTo(expectedWinner));
        }

        [Test]
        public void CalculateEloWIN_same()
        {
            int expectedElo = 105;
            int realElo;

            realElo = Battle.CalculateElo(battle.Requestor.Elo, battle.Opponent.Elo, true);

            Assert.That(realElo, Is.EqualTo(expectedElo));
        }

        [Test]
        public void CalculateEloWIN_underOpponent()
        {
            battle.Opponent.Elo = 200;
            int expectedElo = 150;
            int realElo;

            realElo = Battle.CalculateElo(battle.Requestor.Elo, battle.Opponent.Elo, true);

            Assert.That(realElo, Is.EqualTo(expectedElo));
        }

        [Test]
        public void CalculateEloLOSE_overOpponent()
        {
            battle.Opponent.Elo = 0;
            int expectedElo = 70;
            int realElo;

            realElo = Battle.CalculateElo(battle.Requestor.Elo, battle.Opponent.Elo, false);

            Assert.That(realElo, Is.EqualTo(expectedElo));
        }

        [Test]
        public void CalculateEloLOSE_same()
        {
            int expectedElo = 97;
            int realElo;

            realElo = Battle.CalculateElo(battle.Requestor.Elo, battle.Opponent.Elo, false);

            Assert.That(realElo, Is.EqualTo(expectedElo));
        }

        [Test]
        public void UpdateStatsWIN()
        {
            battle.BattleWinner = Battle.Winner.requestor;
            int gamesplayedExpected = 1;
            int winsExpected = 1;

            battle.UpdateStats();

            Assert.Multiple(() =>
            {
                Assert.That(battle.Requestor.GamesPlayed, Is.EqualTo(gamesplayedExpected));
                Assert.That(battle.Requestor.Wins, Is.EqualTo(winsExpected));
            });
        }

        [Test]
        public void UpdateStatsLOSE()
        {
            battle.BattleWinner = Battle.Winner.opponent;
            int gamesplayedExpected = 1;
            int lossesExpected = 1;

            battle.UpdateStats();

            Assert.Multiple(() =>
            {
                Assert.That(battle.Requestor.GamesPlayed, Is.EqualTo(gamesplayedExpected));
                Assert.That(battle.Requestor.Losses, Is.EqualTo(lossesExpected));
            });
        }

        [Test]
        public void UpdateStatsDRAW()
        {
            battle.BattleWinner = Battle.Winner.none;
            int gamesplayedExpected = 1;
            int lossesExpected = 0;
            int winsExpected = 0;

            battle.UpdateStats();

            Assert.Multiple(() =>
            {
                Assert.That(battle.Requestor.GamesPlayed, Is.EqualTo(gamesplayedExpected));
                Assert.That(battle.Requestor.Losses, Is.EqualTo(lossesExpected));
                Assert.That(battle.Requestor.Wins, Is.EqualTo(winsExpected));
            });
        }
    }
}