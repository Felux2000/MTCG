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
        public List<Card> Stack { get; set; }
        public List<Card> Deck { get; set; }
        public int Coins { get; set; }
        public int Elo { get; }
        public int Wins { get; }
        public int Games { get; }
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

            if(StackCardNr < Stack?.Count)
            {
                if (!Stack[StackCardNr].InStore)
                {
                    if(Deck?.Count < 5)
                    {
                        Deck.Add(Stack[StackCardNr]);
                    }
                    else if(DeckCardNr < Deck?.Count && DeckCardNr < 5)
                    {
                        Deck[DeckCardNr] = Stack[StackCardNr];
                    }
                }
            }
        }
        public void ShowScore() { }
        public void UpdateScore() { }

        public User(string Username, string Password,List<Card> Stack, List<Card> Deck, int Coins,int Elo, int Games, int Wins)
        {
            this.Username = Username;
            this.Password = Password;
            this.Stack = new List<Card>();
            this.Deck = new List<Card>();
            this.Stack.AddRange(Stack);
            this.Deck.AddRange(Deck);
            this.Coins = Coins;
            this.Elo = Elo;
            this.Games = Games;
            this.Wins = Wins;
        }
    }
}
