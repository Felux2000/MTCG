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
