﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Cards;

namespace MonsterTradingCardsGame.Models
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; }
        public List<Card>? Stack { get; set; }
        public List<Card>? Deck { get; set; }
        public int Coins { get; set; }
        public int Elo { get; set; }
        public int Wins { get; set; }
        public int GamesPlayed { get; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public string AuthToken { get; set; }

        /*public void BuyPackage()
        {
            if (Coins > 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    //Stack.Add(new Card());
                }
            }
        }
        public void ManageCards(int StackCardNr, int DeckCardNr)
        {

            if (StackCardNr < Stack?.Count)
            {
                if (!Stack[StackCardNr].InStore)
                {
                    if (Deck?.Count < 5)
                    {
                        Deck.Add(Stack[StackCardNr]);
                    }
                    else if (DeckCardNr < Deck?.Count && DeckCardNr < 5)
                    {
                        Deck[DeckCardNr] = Stack[StackCardNr];
                    }
                }
            }
        }
        */

        //new user
        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Coins = 20;
            Elo = 1000;
            Wins = 0;
            GamesPlayed = 0;
            Bio = string.Empty;
            Image = string.Empty;
            AuthToken = string.Empty;
            Deck = new List<Card>();
            Stack = new List<Card>();
        }
        //existing user
        public User(string username, int coins, int elo, int wins, int gamesPlayed, List<Card> deck, List<Card> stack, string bio, string image, string authToken)
        {
            Username = username;
            Password = string.Empty;
            Coins = coins;
            Elo = elo;
            GamesPlayed = gamesPlayed;
            Deck = deck;
            Stack = stack;
            Bio = bio;
            Image = image;
            AuthToken = authToken;
        }


        public User(string username, int coins, int elo, int wins, int gamesPlayed, string bio, string image, string authToken)
        {
            Username = username;
            Password = string.Empty;
            Coins = coins;
            Elo = elo;
            Wins = wins;
            GamesPlayed = gamesPlayed;
            Bio = bio;
            Image = image;
            AuthToken = authToken;
        }

        public string ShowData()
        {
            return $"{{ \"Username\": \"{Username}\", \"Bio\": \"{Bio}\", \"Image\": \"{Image}\" }}";
        }
        public string ShowElo()
        {
            return $"{{ \"Username\": \"{Username}\", \"Elo\": \"{Elo}\" }}";
        }
        public string ShowStats()
        {
            return $"{{ \"Name\": \"{Username}\", \"Elo\": \"{Elo}\", \"Wins\": \"{Wins}\", \"Losses\": \"{GamesPlayed - Wins}\"}}";
        }

    }
}