using MonsterTradingCardsGame.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MonsterTradingCardsGame.Classes
{
    public enum TransactionType
    {
        package = 0, trade = 1, coinBuy = 2
    }
    internal class Transaction
    {
        public Guid Id { get; }
        public string Username { get; }
        public Guid ObtainedID { get; }
        public string Seller { get; }
        public Guid SoldCardID { get; }
        public int Coins { get; }
        public TransactionType Type { get; }

        public Transaction(string username, Guid obtainedID, string seller, Guid soldCardID, int coins, TransactionType type)
        {
            Id = Guid.NewGuid();
            Username = username;
            ObtainedID = obtainedID;
            Seller = seller;
            SoldCardID = soldCardID;
            Coins = coins;
            Type = type;
        }

        public Transaction(Guid id, string username, Guid obtainedID, string seller, Guid soldCardID, int coins, int type)
        {
            Id = id;
            Username = username;
            ObtainedID = obtainedID;
            Seller = seller;
            SoldCardID = soldCardID;
            Coins = coins;
            switch (type)
            {
                case 0: Type = TransactionType.package; break;
                case 1: Type = TransactionType.trade; break;
                case 2: Type = TransactionType.coinBuy; break;
                default: throw new ArgumentException();
            }
        }

        public string ShowDetails()
        {
            switch (Type)
            {
                case TransactionType.package: return $"{{ \"User\": \"{Username}\", \"PackageId\": \"{ObtainedID}\", \"Coins\": \"{Coins}\", \"Type\": \"Package\" }}";
                case TransactionType.trade:
                    if (ObtainedID == Guid.Empty)
                    { return $"{{ \"User\": \"{Username}\", \"SoldCardId\": \"{SoldCardID}\", \"Buyer\": \"{Seller}\", \"Coins\": \"{Coins}\", \"Type\": \"Trade\" }}"; }
                    else if (SoldCardID == Guid.Empty)
                    { return $"{{ \"User\": \"{Username}\", \"GainedCardId\": \"{ObtainedID}\", \"Seller\": \"{Seller}\", \"Coins\": \"{Coins}\", \"Type\": \"Trade\" }}"; }
                    else
                    { return $"{{ \"User\": \"{Username}\", \"SoldCardId\": \"{SoldCardID}\", \"GainedCardId\": \"{ObtainedID}\", \"Seller\": \"{Seller}\", \"Coins\": \"{Coins}\", \"Type\": \"Trade\" }}"; }
                case TransactionType.coinBuy: return $"{{ \"User\": \"{Username}\", \"Coins\": \"{Coins}\", \"Type\": \"CoinPurchase\" }}";
                default: return $"{{ \"User\": \"{Username}\", \"SoldCardId\": \"{SoldCardID}\", \"GainedCardId\": \"{ObtainedID}\", \"Seller\": \"{Seller}\", \"Coins\": \"{Coins}\" , \"Type\": \"{Type}\"}}";
            }
        }
    }
}
