using MonsterTradingCardsGame.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MonsterTradingCardsGame.Server;
using Newtonsoft.Json;

namespace MonsterTradingCardsGame.Classes
{
    internal class TradingDeal
    {
        private Dictionary<CardType, string> typeToString = new Dictionary<CardType, string>
        {
            {CardType.monster , "monster"},
            {CardType.spell ,"spell"},
            {CardType.effect ,"effect"}
        };

        public Guid Id { get; }
        public Guid CardToTrade { get; }
        public CardType Type { get; }
        public double MinimumDamage { get; }
        public int CoinCost { get; }
        public string Username { get; set; }

        public TradingDeal(Guid Id, Guid CardToTrade, int Type, double MinimumDamage, int CoinCost, string username = "")
        {
            this.Id = Id;
            this.CardToTrade = CardToTrade;
            this.MinimumDamage = MinimumDamage;
            this.CoinCost = CoinCost;
            Username = username;
            switch (Type)
            {
                case 0: this.Type = CardType.monster; break;
                case 1: this.Type = CardType.spell; break;
                case 2: this.Type = CardType.effect; break;
                default: throw new ArgumentException();
            }
        }
        [JsonConstructor]
        public TradingDeal(Guid Id, Guid CardToTrade, string Type, double MinimumDamage, int CoinCost, string username = "")
        {
            this.Id = Id;
            this.CardToTrade = CardToTrade;
            this.MinimumDamage = MinimumDamage >= 0 ? MinimumDamage : 0;
            this.CoinCost = CoinCost >= 0 ? CoinCost : 0;
            Username = username;
            switch (Type)
            {
                case "monster": this.Type = CardType.monster; break;
                case "spell": this.Type = CardType.spell; break;
                case "effect": this.Type = CardType.effect; break;
                default: throw new ArgumentException();
            }
        }

        public string ShowDeal()
        {
            return $"{{ \"Id\": \"{Id}\", \"CardToTrade\": \"{CardToTrade}\", \"Type\": \"{typeToString[Type]}\", \"MinimumDamage\": \"{MinimumDamage}\", \"CoinCost\": \"{CoinCost}\" }}";
        }
    }
}
