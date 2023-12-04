using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Cards
{
    internal class CardBuilder
    {
        private List<string> _cardName;
        private List<double> _cardDamage;
        private List<DamageType> _cardElement;
        private List<CardType> _cardType;
        public CardBuilder()
        {
            _cardName = new List<string>() { "Kraken", "Goblin", "Knight", "Wizard", "Dragon", "FireElf", "Ork", "Archer", "Mage", "Farmer", "Wyvern", "Fireball", "WaterGun", "WindCutter", "EarthSpike", "Bless", "Amplify", "Depression", "Diminish" };
            _cardDamage = new List<double>() { 25, 5, 15, 30, 50, 20, 20, 15, 40, 5, 40, 30, 20, 25, 35, 1.2, 1.35, 1.15, 1.3 };
            _cardElement = new List<DamageType>() { DamageType.water, DamageType.normal, DamageType.normal, DamageType.air, DamageType.fire, DamageType.fire, DamageType.earth, DamageType.air, DamageType.fire, DamageType.normal, DamageType.earth, DamageType.fire, DamageType.water, DamageType.air, DamageType.earth, DamageType.normal, DamageType.normal, DamageType.normal, DamageType.normal };
            _cardType = new List<CardType>() { CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.spell, CardType.spell, CardType.spell, CardType.spell, CardType.effect, CardType.effect, CardType.effect, CardType.effect };
        }

        public string GetCardName(int index)
        {
            if (index < 0 || index >= _cardName.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return _cardName[index];
        }
        public double GetCardDamage(int index)
        {
            if (index < 0 || index >= _cardDamage.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return _cardDamage[index];
        }
        public DamageType GetCardElement(int index)
        {
            if (index < 0 || index >= _cardElement.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return _cardElement[index];
        }
        public CardType GetCardType(int index)
        {
            if (index < 0 || index >= _cardType.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return _cardType[index];
        }
    }
}
