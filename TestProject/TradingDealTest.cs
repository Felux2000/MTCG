using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Cards;
using MonsterTradingCardsGame.Models;
using Npgsql;
using NSubstitute;
using NSubstitute.Core;
using System.ComponentModel;
using System.Data.Common;
using Npgsql;
using Microsoft.VisualBasic;

namespace TestProject
{
    public class TradingDealTest
    {
        private TradingDeal deal;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TradingDealCardType_INT()
        {
            int cardType = 0;
            CardType typeExpected = CardType.monster;
            CardType typeReal;

            deal = new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), cardType, 15, 5);
            typeReal = deal.Type;

            Assert.That(typeReal, Is.EqualTo(typeExpected));
        }

        [Test]
        public void TradingDealCardType_STRING()
        {
            string cardType = "spell";
            CardType typeExpected = CardType.spell;
            CardType typeReal;

            deal = new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), cardType, 15, 5);
            typeReal = deal.Type;

            Assert.That(typeReal, Is.EqualTo(typeExpected));
        }

        [Test]
        public void TradingDealCardTypeINT_ArgEx()
        {
            int cardType = 3;

            Assert.Throws<ArgumentException>(() => deal = new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), cardType, 15, 5));
        }

        [Test]
        public void TradingDealCardTypeSTRING_ArgEx()
        {
            string cardType = "spells";

            Assert.Throws<ArgumentException>(() => deal = new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), cardType, 15, 5));
        }

        [Test]
        public void TradingDealShowDeal()
        {
            string id = Guid.NewGuid().ToString();
            deal = new(id, Guid.Empty.ToString(), 0, 15, 5);

            string expected = $"{{ \"Id\": \"{id}\", \"CardToTrade\": \"{Guid.Empty}\", \"Type\": \"monster\", \"MinimumDamage\": \"15\", \"CoinCost\": \"5\" }}";

            Assert.That(deal.ShowDeal(), Is.EqualTo(expected));
        }
    }
}