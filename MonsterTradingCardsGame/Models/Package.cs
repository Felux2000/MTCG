using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Cards;

namespace MonsterTradingCardsGame.Models
{
    internal class Package
    {
        public Guid ID { get; }
        public List<Card> Cards { get; set; }

        public Package(List<Card> cards)
        {
            ID = Guid.NewGuid();
            Cards = cards;
        }

        public Package(List<Card> cards, Guid id)
        {
            ID = id;
            Cards = cards;
        }
    }
}
