using MonsterTradingCardsGame.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class TradingDeal
    {
        public string TradeID { get; set; }
        public string Username { get; set; }
        public string OfferedCardID { get; set; }
        public double MinimumDmg { get; set; }
        public CardType CardType { get; set; }

        public TradingDeal(string tradeID, string username, string offeredCardID, double minimumDmg, int cardType)
        {
            TradeID = tradeID;
            Username = username;
            OfferedCardID = offeredCardID;
            MinimumDmg = minimumDmg;
            switch (cardType)
            {
                case 0: CardType = CardType.monster; break;
                case 1: CardType = CardType.spell; break;
                case 2: CardType = CardType.effect; break;
            }
        }
    }
}
