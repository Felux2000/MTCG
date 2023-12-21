using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MonsterTradingCardsGame.Models
{
    internal class CardModel
    {
        public Guid CardID { get; }
        public int CardIndex { get; }

        [JsonConstructor]
        public CardModel(Guid Id, int Index)
        {
            CardID = Id;
            CardIndex = Index;
        }
    }
}
