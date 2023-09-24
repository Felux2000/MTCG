using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class Game
    {
        private User PlayerOne;
        private User PlayerTwo;
        public void Battle()
        {

        }
        public Game(User PlayerOne , User PlayerTwo)
        {
            this.PlayerOne = PlayerOne;
            this.PlayerTwo = PlayerTwo;
        }
    }
}
