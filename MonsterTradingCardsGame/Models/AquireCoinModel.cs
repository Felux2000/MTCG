using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MonsterTradingCardsGame.Models
{
    internal class AquireCoinModel
    {
        public int Coins { get; }

        [JsonConstructor]
        public AquireCoinModel(int coins)
        {
            Coins = coins;
        }
    }
}
