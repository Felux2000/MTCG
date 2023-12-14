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
        private List<string> _cardDescription;
        public CardBuilder()
        {
            _cardName = new List<string>() { "Kraken", "Goblin", "Knight", "Wizard", "Dragon", "FireElf", "Ork", "Archer", "Mage", "Farmer", "Wyvern", "Fireball", "WaterGun", "WindCutter", "EarthSpike", "Bless", "Amplify", "Depression", "Diminish" };
            _cardDamage = new List<double>() { 25, 5, 15, 30, 50, 20, 20, 15, 40, 5, 40, 30, 20, 25, 35, 1.2, 1.35, 0.85, 0.75 };
            _cardElement = new List<DamageType>() { DamageType.water, DamageType.normal, DamageType.normal, DamageType.air, DamageType.fire, DamageType.fire, DamageType.earth, DamageType.air, DamageType.fire, DamageType.normal, DamageType.earth, DamageType.fire, DamageType.water, DamageType.air, DamageType.earth, DamageType.normal, DamageType.normal, DamageType.normal, DamageType.normal };
            _cardType = new List<CardType>() { CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.monster, CardType.spell, CardType.spell, CardType.spell, CardType.spell, CardType.effect, CardType.effect, CardType.effect, CardType.effect };
            _cardDescription = new List<string> { "An ancient sea monster capable of unleashing devastating water-based attacks.", "A mischievous creature relying on quick but relatively weak attacks.", "A noble warrior skilled in combat, dealing moderate damage with traditional techniques.", "A master of the arcane, wielding powerful air-based spells to overwhelm opponents.", "The epitome of destruction, breathing intense flames to decimate foes.",
                                                  "Agile and fiery, this elf harnesses fire to strike with speed and precision.","A formidable creature with earth-based attacks, capable of brute force.","A skilled marksman using air-based arrows to strike accurately from a distance.","A powerful spellcaster specializing in devastating fire-based magic.","A humble character with limited combat skills, dealing minimal damage.",
                                                  "A fierce beast wielding earth-based attacks, known for its ferocity.","Unleashes a potent fireball, causing significant damage upon impact.","Shoots a pressurized jet of water, dealing moderate damage to foes.","Conjures razor-sharp air currents to slice through enemies.","Summons spikes from the earth to impale and damage opponents.",
                                                  "Bestows a minor boost to allied creatures, enhancing their capabilities.","Augments the power of allied creatures, significantly boosting their abilities.","Inflicts a sense of despair, weakening enemy creatures to reduce their effectiveness.","Substantially reduces the strength and impact of enemy creatures, hindering their performance."};
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
        public string GetCardDescription(int index)
        {
            if (index < 0 || index >= _cardType.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return _cardDescription[index];
        }
    }
}
