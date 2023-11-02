using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class Game
    {
        Random Rand = new Random();
        private User PlayerOne;
        private User PlayerTwo;

        public void Battle()
        {
            List<Card>? DeckOne = PlayerOne.Deck;
            List<Card>? DeckTwo = PlayerTwo.Deck;
            Card? CardOne;
            Card? CardTwo;
            int IndexOne;
            int IndexTwo;
            double DamageOne;
            double DamageTwo;
            int GameCounter = 0;
            while (DeckOne.Count != 0 && DeckTwo.Count != 0 && GameCounter < 100)
            {
                IndexOne = Rand.Next(DeckOne.Count);
                IndexTwo = Rand.Next(DeckTwo.Count);

                CardOne = DeckOne[IndexOne];
                CardTwo = DeckTwo[IndexTwo];

                DamageOne = CardOne.CalcDmg(CardTwo);
                DamageTwo = CardTwo.CalcDmg(CardOne);

                if (DamageOne != DamageTwo)
                {
                    if (DamageOne > DamageTwo)
                    {
                        DeckOne.Add(CardTwo);
                        DeckTwo.RemoveAt(IndexTwo);
                    }
                    else
                    {
                        DeckTwo.Add(CardOne);
                        DeckOne.RemoveAt(IndexOne);
                    }
                }
                GameCounter++;
            }
            if (DeckOne.Count > DeckTwo.Count)
            {
                //player one wins
            }
            else
            {
                //player two wins
            }
        }
        public Game(User PlayerOne, User PlayerTwo)
        {
            this.PlayerOne = PlayerOne;
            this.PlayerTwo = PlayerTwo;
        }
    }
}
