using MonsterTradingCardsGame.Server;
using Npgsql;
using NSubstitute;
using NSubstitute.Core;
using System.ComponentModel;
using System.Data.Common;
using Npgsql;
using MonsterTradingCardsGame.Cards;

namespace TestProject
{
    public class CardTest
    {
        CardBuilder cardBuilder;

        [SetUp]
        public void Setup()
        {
            cardBuilder = new();
        }

        [TestCase(1, 4)]
        [TestCase(6, 3)]
        [TestCase(2, 12)]
        [TestCase(4, 5)]
        public void SpecialityTest_Zero(int cardIndex1, int cardIndex2)
        {
            int expectedDamage = 0;
            Card card1 = new(Guid.NewGuid().ToString(), "testuser", cardBuilder.GetCardName(cardIndex1), cardBuilder.GetCardDamage(cardIndex1), cardBuilder.GetCardElement(cardIndex1), cardBuilder.GetCardType(cardIndex1), false, false, cardIndex1);
            Card card2 = new(Guid.NewGuid().ToString(), "testuser", cardBuilder.GetCardName(cardIndex2), cardBuilder.GetCardDamage(cardIndex2), cardBuilder.GetCardElement(cardIndex2), cardBuilder.GetCardType(cardIndex2), false, false, cardIndex2);

            Assert.That(card1.CalcDmg(card2), Is.EqualTo(expectedDamage));
        }

        [TestCase(0, 11)]
        [TestCase(11, 1)]
        public void ElementTest_Greater(int cardIndex1, int cardIndex2)
        {
            Card card1 = new(Guid.NewGuid().ToString(), "testuser", cardBuilder.GetCardName(cardIndex1), cardBuilder.GetCardDamage(cardIndex1), cardBuilder.GetCardElement(cardIndex1), cardBuilder.GetCardType(cardIndex1), false, false, cardIndex1);
            Card card2 = new(Guid.NewGuid().ToString(), "testuser", cardBuilder.GetCardName(cardIndex2), cardBuilder.GetCardDamage(cardIndex2), cardBuilder.GetCardElement(cardIndex2), cardBuilder.GetCardType(cardIndex2), false, false, cardIndex2);

            Assert.That(card1.CalcDmg(card2), Is.GreaterThan(card1.Damage));
        }

        [TestCase(1, 11)]
        [TestCase(12, 1)]
        public void ElementTest_Less(int cardIndex1, int cardIndex2)
        {
            Card card1 = new(Guid.NewGuid().ToString(), "testuser", cardBuilder.GetCardName(cardIndex1), cardBuilder.GetCardDamage(cardIndex1), cardBuilder.GetCardElement(cardIndex1), cardBuilder.GetCardType(cardIndex1), false, false, cardIndex1);
            Card card2 = new(Guid.NewGuid().ToString(), "testuser", cardBuilder.GetCardName(cardIndex2), cardBuilder.GetCardDamage(cardIndex2), cardBuilder.GetCardElement(cardIndex2), cardBuilder.GetCardType(cardIndex2), false, false, cardIndex2);

            Assert.That(card1.CalcDmg(card2), Is.LessThan(card1.Damage));
        }

        [Test]
        public void ShowCardTest_Json()
        {
            int cardIndex = 0;
            string CardID = Guid.NewGuid().ToString();
            Card card1 = new(CardID, "testuser", cardBuilder.GetCardName(cardIndex), cardBuilder.GetCardDamage(cardIndex), cardBuilder.GetCardElement(cardIndex), cardBuilder.GetCardType(cardIndex), false, false, cardIndex);
            string expected = $"{{ \"Id\": \"{CardID}\", \"Name\": \"{cardBuilder.GetCardName(cardIndex)}\", \"Damage\": \"{cardBuilder.GetCardDamage(cardIndex)}\", \"Description\": \"\" }}";

            Assert.That(card1.ShowCard(), Is.EqualTo(expected));
        }

        [Test]
        public void ShowCardTest_Plain()
        {
            int cardIndex = 0;
            string CardID = Guid.NewGuid().ToString();
            Card card1 = new(CardID, "testuser", cardBuilder.GetCardName(cardIndex), cardBuilder.GetCardDamage(cardIndex), cardBuilder.GetCardElement(cardIndex), cardBuilder.GetCardType(cardIndex), false, false, cardIndex);
            string expected = $"Id: {CardID}, Name: {cardBuilder.GetCardName(cardIndex)}, Damage: {cardBuilder.GetCardDamage(cardIndex)}, Description: ";

            Assert.That(card1.ShowCard(true), Is.EqualTo(expected));
        }
    }
}