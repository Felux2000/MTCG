using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Package
    {
        public string PackageID { get; set; }
        public string[] CardID { get; set; }
        public int[] CardIndex { get; set; }

        public Package(string packageID, string[] cardID, int[] cardIndex)
        {
            PackageID = packageID;
            CardID = cardID;
            CardIndex = cardIndex;
        }
    }
}
