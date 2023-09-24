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
        public void BuyPackage() { 
            if(Coins > 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    //Stack.Add(new Card());
                }
            }
        }
        public void ManageCards(int StackCardNr, int DeckCardNr) {
            if(StackCardNr < Stack.Count && DeckCardNr <= Deck.Count && DeckCardNr < 5)
            {
                if (!Stack[StackCardNr].InStore)
                {
                    Deck[DeckCardNr] = Stack[StackCardNr];
                }
            }
        }
        public void ShowScore() { }

        public User(string Username, string Password,List<Card> Stack, List<Card> Deck, int Coins)
        {
            this.Username = Username;
            this.Password = Password;
            this.Stack.AddRange(Stack);
            this.Deck.AddRange(Deck);
            this.Coins = Coins;
        }
    }
}
