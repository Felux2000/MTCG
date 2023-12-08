using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Package
    {
        public string[] CardID { get; set; }
        public int[] CardIndex { get; set; }

        public Package(string[] cardID, int[] cardIndex)
        {
            CardID = cardID;
            CardIndex = cardIndex;
        }
    }
}
