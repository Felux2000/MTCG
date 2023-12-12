using MonsterTradingCardsGame.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MonsterTradingCardsGame.Server;
using Newtonsoft.Json;

namespace MonsterTradingCardsGame.Models
{
    internal class TradingDeal
    {
        private Dictionary<CardType, string> typeToString = new Dictionary<CardType, string>
        {
            {CardType.monster , "monster"},
            {CardType.spell ,"spell"},
            {CardType.effect ,"effect"}
        };

        public string Id { get; }
        public string CardToTrade { get; }
        public CardType Type { get; }
        public double MinimumDamage { get; }
        public string Username { get; set; }

        public TradingDeal(string Id, string CardToTrade, int Type, double MinimumDamage, string username = "")
        {
            this.Id = Id;
            this.CardToTrade = CardToTrade;
            this.MinimumDamage = MinimumDamage;
            Username = username;
            switch (Type)
            {
                case 0: this.Type = CardType.monster; break;
                case 1: this.Type = CardType.spell; break;
                case 2: this.Type = CardType.effect; break;
            }
        }
        [JsonConstructor]
        public TradingDeal(string Id, string CardToTrade, string Type, double MinimumDamage, string username = "")
        {
            this.Id = Id;
            this.CardToTrade = CardToTrade;
            this.MinimumDamage = MinimumDamage;
            Username = username;
            switch (Type)
            {
                case "monster": this.Type = CardType.monster; break;
                case "spell": this.Type = CardType.spell; break;
                case "effect": this.Type = CardType.effect; break;
            }
        }

        public string ShowDeal()
        {
            return $"{{ \"Id\": \"{Id}\", \"CardToTrade\": \"{CardToTrade}\", \"Type\": \"{typeToString[Type]}\", \"MinimumDamage\": \"{MinimumDamage}\" }}";
        }
    }
}
