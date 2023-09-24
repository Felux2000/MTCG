using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class User
    {
        public string Username { get; }
        public string Password { get; }
        public List<Card>? Stack { get; set; }
        public List<Card>? Deck { get; set; }
        private int Coins { get; set; }
        public void BuyPackage() { }
        public void ManageCards() { }
        public void ShowScore() { }
    }
}
