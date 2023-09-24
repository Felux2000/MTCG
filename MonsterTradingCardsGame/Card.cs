using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    public enum EElementType
    {
        normal=0, fire=1, water=2
    }
    public enum ECardType
    {
        monster=0, spell=1
    }

    abstract internal class Card
    {
        public string Name { get; }
        public int Damage { get; }
        public EElementType EType { get; }
        public ECardType CType { get; }
        public bool InStore { get; set; }
        public abstract int CalcDmg(EElementType EnemyEle, ECardType EnemyType);
        public Card(string Name, int Damage, EElementType EType, ECardType CType) {
            this.Name = Name;
            this.Damage = Damage;
            this.EType = EType;
            this.CType = CType;
        }
    }
}
