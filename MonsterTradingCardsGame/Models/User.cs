using System;
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
        public List<Card>? Deck { get; set; }
        public int Coins { get; set; }
        public int Elo { get; set; }
        public int Wins { get; set; }
        public int GamesPlayed { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public string AuthToken { get; set; }
        public bool HasDeck { get; set; }


        //new user
        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Coins = 20;
            Elo = 100;
            Wins = 0;
            GamesPlayed = 0;
            Bio = string.Empty;
            Image = string.Empty;
            AuthToken = string.Empty;
            Deck = new List<Card>();
            HasDeck = false;
        }
        //existing user
        public User(string username, int coins, int elo, int wins, int gamesPlayed, string bio, string image, string authToken, bool hasDeck)
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
            HasDeck = hasDeck;
        }

        public string ShowData()
        {
            return $"{{ \"Username\": \"{Username}\", \"Bio\": \"{Bio}\", \"Image\": \"{Image}\", \"Coins\": \"{Coins}\" }}";
        }
        public string ShowElo()
        {
            return $"{{ \"Username\": \"{Username}\", \"Elo\": \"{Elo}\" }}";
        }
        public string ShowStats()
        {
            return $"{{ \"Name\": \"{Username}\", \"Elo\": \"{Elo}\", \"Wins\": \"{Wins}\", \"Losses\": \"{GamesPlayed - Wins}\", \"W / L\": \"{(GamesPlayed == 0 ? 0 : ((GamesPlayed - Wins) == 0 ? Wins : Math.Round((double)Wins / ((double)GamesPlayed - (double)Wins), 2).ToString().Replace(',', '.')))}\" }}";
        }

    }
}
