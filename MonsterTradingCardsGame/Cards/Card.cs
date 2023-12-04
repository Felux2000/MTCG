using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Cards
{
    public enum DamageType
    {
        normal = 0, fire = 1, water = 2, earth = 3, air = 4
    }
    public enum CardType
    {
        monster = 0, spell = 1, effect = 2
    }

    internal class Card
    {
        public string CardID { get; set; }
        public string Name { get; }
        public double Damage { get; }
        public DamageType Element { get; }
        public CardType Type { get; }
        public bool InStore { get; set; }
        public bool InDeck { get; set; }
        public int Index { get; }

        public (double final, double raw) CalcDmg(Card EnemyCard)
        {
            DamageType EnemyElement = EnemyCard.Element;
            CardType EnemyType = EnemyCard.Type;
            string EnemyName = EnemyCard.Name;

            if (Type == CardType.effect || EnemyCard.Type == CardType.effect)
            {
                return (Damage, Damage);
            }

            if (Type == CardType.spell && EnemyName == "Kraken")
            {
                return (0, Damage);
            }

            switch (Name)
            {
                case "Goblin": if (EnemyName == "Dragon") return (0, Damage); ; ; break;
                case "Knight": if (EnemyType == CardType.spell && EnemyElement == DamageType.water) return (0, Damage); ; ; break;
                case "Ork": if (EnemyName == "Wizard") return (0, Damage); ; ; break;
                case "Dragon": if (EnemyName == "FireElf") return (0, Damage); ; ; break;
                default:
                    break;
            }

            if (EnemyType != Type)
            {
                double EffectiveDmg = 0;
                switch (Element)
                {
                    case DamageType.normal:
                        switch (EnemyElement)
                        {
                            case DamageType.normal: EffectiveDmg = Damage; break;
                            case DamageType.fire: EffectiveDmg = Damage / 2; break;
                            case DamageType.water: EffectiveDmg = Damage * 2; break;
                            case DamageType.earth: EffectiveDmg = Damage * 0.25; break;
                            case DamageType.air: EffectiveDmg = Damage * 0.75; break;
                        }
                        break;
                    case DamageType.fire:
                        switch (EnemyElement)
                        {
                            case DamageType.normal: EffectiveDmg = Damage * 2; break;
                            case DamageType.fire: EffectiveDmg = Damage; break;
                            case DamageType.water: EffectiveDmg = Damage / 2; break;
                            case DamageType.earth: EffectiveDmg = 0.25 * Damage; break;
                            case DamageType.air: EffectiveDmg = Damage * 1.5; break;
                        }
                        break;
                    case DamageType.water:
                        switch (EnemyElement)
                        {
                            case DamageType.normal: EffectiveDmg = Damage / 2; break;
                            case DamageType.fire: EffectiveDmg = Damage * 2; break;
                            case DamageType.water: EffectiveDmg = Damage; break;
                            case DamageType.earth: EffectiveDmg = Damage * 1.5; break;
                            case DamageType.air: EffectiveDmg = Damage * 0.75; break;
                        }
                        break;
                    case DamageType.earth:
                        switch (EnemyElement)
                        {
                            case DamageType.normal: EffectiveDmg = Damage * 1.75; break;
                            case DamageType.fire: EffectiveDmg = Damage * 5; break;
                            case DamageType.water: EffectiveDmg = Damage * 0.75; break;
                            case DamageType.earth: EffectiveDmg = Damage; break;
                            case DamageType.air: EffectiveDmg = Damage * 2; break;
                        }
                        break;
                    case DamageType.air:
                        switch (EnemyElement)
                        {
                            case DamageType.normal: EffectiveDmg = Damage * 1.5; break;
                            case DamageType.fire: EffectiveDmg = Damage * 0.75; break;
                            case DamageType.water: EffectiveDmg = Damage * 1.25; break;
                            case DamageType.earth: EffectiveDmg = Damage / 2; break;
                            case DamageType.air: EffectiveDmg = Damage; break;
                        }
                        break;
                    default: EffectiveDmg = Damage; break;
                }
                return (EffectiveDmg, Damage); ;
            }
            return (Damage, Damage);
        }
        public Card(string cardID, string name, double damage, DamageType element, CardType type, bool inStore, bool inDeck, int index)
        {
            CardID = cardID;
            Name = name;
            Damage = damage;
            Element = element;
            Type = type;
            InStore = inStore;
            InDeck = inDeck;
            Index = index;
        }

        public string ShowCard()
        {
            return $"{{ \"ID\": \"{CardID}\", \"Name\": \"{Name}\", \"Damage\": \"{Damage}\" }}";
        }
    }
}
