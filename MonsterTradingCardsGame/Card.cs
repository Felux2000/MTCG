using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    public enum EElementType
    {
        normal = 0, fire = 1, water = 2, earth = 3, air = 4
    }
    public enum ECardType
    {
        monster = 0, spell = 1
    }

    internal class Card
    {
        public string Name { get; }
        public int Damage { get; }
        public EElementType EType { get; }
        public ECardType CType { get; }
        public bool InStore { get; set; }

        public int CalcDmg(EElementType EnemyEle, ECardType EnemyType)
        {
            if (EnemyType != CType)
            {
                int EffectiveDmg = 0;
                switch (EType)
                {
                    case EElementType.normal:
                        switch (EnemyEle)
                        {
                            case EElementType.normal: EffectiveDmg = Damage; break;
                            case EElementType.fire: EffectiveDmg = Damage / 2; break;
                            case EElementType.water: EffectiveDmg = Damage * 2; break;
                            case EElementType.earth: EffectiveDmg = (int)(float)(Damage * 0.25); break;
                            case EElementType.air: EffectiveDmg = (int)(float)(Damage * 0.75); break;
                        }
                        break;
                    case EElementType.fire:
                        switch (EnemyEle)
                        {
                            case EElementType.normal: EffectiveDmg = Damage * 2; break;
                            case EElementType.fire: EffectiveDmg = Damage; break;
                            case EElementType.water: EffectiveDmg = Damage / 2; break;
                            case EElementType.earth: EffectiveDmg = 0; break;
                            case EElementType.air: EffectiveDmg = (int)(float)(Damage * 1.5); break;
                        }
                        break;
                    case EElementType.water:
                        switch (EnemyEle)
                        {
                            case EElementType.normal: EffectiveDmg = Damage / 2; break;
                            case EElementType.fire: EffectiveDmg = Damage * 2; break;
                            case EElementType.water: EffectiveDmg = Damage; break;
                            case EElementType.earth: EffectiveDmg = (int)(float)(Damage * 1.5); break;
                            case EElementType.air: EffectiveDmg = (int)(float)(Damage * 0.75); break;
                        }
                        break;
                    case EElementType.earth:
                        switch (EnemyEle)
                        {
                            case EElementType.normal: EffectiveDmg = (int)(float)(Damage * 1.75); break;
                            case EElementType.fire: EffectiveDmg = Damage * 5; break;
                            case EElementType.water: EffectiveDmg = (int)(float)(Damage * 0.75); break;
                            case EElementType.earth: EffectiveDmg = Damage; break;
                            case EElementType.air: EffectiveDmg = Damage * 2; break;
                        }
                        break;
                    case EElementType.air:
                        switch (EnemyEle)
                        {
                            case EElementType.normal: EffectiveDmg = (int)(float)(Damage * 1.5); break;
                            case EElementType.fire: EffectiveDmg = (int)(float)(Damage * 0.75); break;
                            case EElementType.water: EffectiveDmg = (int)(float)(Damage * 1.25); break;
                            case EElementType.earth: EffectiveDmg = Damage / 2; break;
                            case EElementType.air: EffectiveDmg = Damage; break;
                        }
                        break;
                    default: EffectiveDmg = Damage; break;
                }
                return EffectiveDmg;
            }
            return Damage;
        }
        public Card(string Name, int Damage, EElementType EType, ECardType CType)
        {
            this.Name = Name;
            this.Damage = Damage;
            this.EType = EType;
            this.CType = CType;
        }
    }
}
