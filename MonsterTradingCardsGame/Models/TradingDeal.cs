using MonsterTradingCardsGame.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace MonsterTradingCardsGame.Models
{
    internal class TradingDeal
    {
        public string Id { get; set; }
        public string CardToTrade { get; set; }
        public string Type { get; set; }
        public double MinimumDamage { get; set; }
        public string Username { get; set; }

        public TradingDeal(string tradeID, string offeredCardID, double minimumDmg, string cardType, string username = "")
        {
            Id = tradeID;
            CardToTrade = offeredCardID;
            Type = cardType;
            MinimumDamage = minimumDmg;
            Username = username;
        }

        public string ShowDeal()
        {
            return $"{{ \"Id\": \"{Id}\", \"CardToTrade\": \"{CardToTrade}\", \"Type\": \"{Type}\", \"MinimumDamage\": \"{MinimumDamage}\" }}";
        }
    }
}
