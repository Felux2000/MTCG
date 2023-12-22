using MonsterTradingCardsGame.Cards;

namespace MonsterTradingCardsGame.Classes
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
