using MonsterTradingCardsGame.Server;
using MonsterTradingCardsGame.Cards;
using MonsterTradingCardsGame.Classes;
using Npgsql;
using NSubstitute;
using NSubstitute.Core;
using System.ComponentModel;
using System.Data.Common;
using Microsoft.VisualBasic;
using System.Drawing;

namespace TestProject
{
    public class TransactionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TransactionType_INT()
        {
            TransactionType typeExpected = TransactionType.trade;
            TransactionType typeReal;

            Transaction transaction = new(Guid.NewGuid(), "testuser", Guid.NewGuid(), "testseller", Guid.NewGuid(), 5, 1);
            typeReal = transaction.Type;

            Assert.That(typeReal, Is.EqualTo(typeExpected));
        }

        [Test]
        public void TransactionTypeINT_ArgEx()
        {
            Transaction transaction;
            int transactionType = 3;

            Assert.Throws<ArgumentException>(() => transaction = new(Guid.NewGuid(), "testuser", Guid.NewGuid(), "testseller", Guid.NewGuid(), 5, transactionType));
        }


        [Test]
        public void TransactionShowDetail_Package()
        {
            string user = "testuser";
            int coins = 5;
            Transaction transaction = new(Guid.Empty, user, Guid.Empty, "testseller", Guid.Empty, coins, 0);

            string expected = $"{{ \"User\": \"{user}\", \"PackageId\": \"{Guid.Empty}\", \"Coins\": \"{coins}\", \"Type\": \"Package\" }}";

            Assert.That(transaction.ShowDetails(), Is.EqualTo(expected));
        }

        [Test]
        public void TransactionShowDetail_Trade()
        {
            string user = "testuser";
            string seller = "testseller";
            int coins = 5;
            Guid obtained = Guid.NewGuid();
            Guid sold = Guid.NewGuid();
            Transaction transactionObtainedEmpty = new(Guid.Empty, user, Guid.Empty, seller, sold, coins, 1);
            Transaction transactionSoldEmpty = new(Guid.Empty, user, obtained, seller, Guid.Empty, coins, 1);
            Transaction transactionDefault = new(Guid.Empty, user, obtained, seller, sold, coins, 1);

            string expectedObtainesEmpty = $"{{ \"User\": \"{user}\", \"SoldCardId\": \"{sold}\", \"Buyer\": \"{seller}\", \"Coins\": \"{coins}\", \"Type\": \"Trade\" }}";
            string expectedSoldEmpty = $"{{ \"User\": \"{user}\", \"GainedCardId\": \"{obtained}\", \"Seller\": \"{seller}\", \"Coins\": \"{coins}\", \"Type\": \"Trade\" }}";
            string expectedDefault = $"{{ \"User\": \"{user}\", \"SoldCardId\": \"{sold}\", \"GainedCardId\": \"{obtained}\", \"Seller\": \"{seller}\", \"Coins\": \"{coins}\", \"Type\": \"Trade\" }}";

            Assert.Multiple(() =>
            {
                Assert.That(transactionObtainedEmpty.ShowDetails(), Is.EqualTo(expectedObtainesEmpty));
                Assert.That(transactionSoldEmpty.ShowDetails(), Is.EqualTo(expectedSoldEmpty));
                Assert.That(transactionDefault.ShowDetails(), Is.EqualTo(expectedDefault));
            });
        }

        [Test]
        public void TransactionShowDetail_CoinBuy()
        {
            string user = "testuser";
            int coins = 5;
            Transaction transaction = new(Guid.Empty, user, Guid.Empty, "testseller", Guid.Empty, coins, 2);

            string expected = $"{{ \"User\": \"{user}\", \"Coins\": \"{coins}\", \"Type\": \"CoinPurchase\" }}";

            Assert.That(transaction.ShowDetails(), Is.EqualTo(expected));
        }
    }
}