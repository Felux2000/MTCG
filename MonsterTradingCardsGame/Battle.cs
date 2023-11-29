using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class Battle
    {
        Random Rand = new Random();
        private User PlayerOne;
        private User PlayerTwo;
        private const int EffectDuration = 2;
        private Dictionary<string, double> _effectDamage;
        private Dictionary<string, int> _effectDurationOne;
        private Dictionary<string, int> _effectDurationTwo;
        private double _dmgMultiplierOne;
        private double _dmgMultiplierTwo;
        public void Fight()
        {
            List<Card>? DeckOne = PlayerOne.Deck;
            List<Card>? DeckTwo = PlayerTwo.Deck;
            Card? CardOne;
            Card? CardTwo;
            int IndexOne;
            int IndexTwo;
            int GameCounter = 0;
            while (DeckOne.Count != 0 && DeckTwo.Count != 0 && GameCounter < 100)
            {
                double DamageOne;
                double DamageOneRaw;
                double DamageTwo;
                double DamageTwoRaw;
                IndexOne = Rand.Next(DeckOne.Count);
                IndexTwo = Rand.Next(DeckTwo.Count);

                CardOne = DeckOne[IndexOne];
                CardTwo = DeckTwo[IndexTwo];

                var temp = CardOne.CalcDmg(CardTwo);
                DamageOne = temp.final;
                DamageOneRaw = temp.raw;
                if (CardOne.Type == CardType.effect)
                {

                    _effectDamage[CardOne.Name] = DamageOne;

                    if (_effectDurationOne.ContainsKey(CardOne.Name))
                    {
                        _effectDurationOne[CardOne.Name] += EffectDuration;
                    }
                    else
                    {
                        _effectDurationOne[CardOne.Name] = EffectDuration;
                    }
                    DamageOne = 0;
                }
                temp = CardTwo.CalcDmg(CardOne);
                DamageTwo = temp.final;
                DamageTwoRaw = temp.raw;
                if (CardTwo.Type == CardType.effect)
                {

                    _effectDamage[CardTwo.Name] = DamageTwo;

                    if (_effectDurationOne.ContainsKey(CardTwo.Name))
                    {
                        _effectDurationTwo[CardTwo.Name] += EffectDuration;
                    }
                    else
                    {
                        _effectDurationTwo[CardTwo.Name] = EffectDuration;
                    }
                    DamageOne = 0;
                }

                CalculateDamageMultipliers();
                DamageOne *= _dmgMultiplierOne;
                DamageTwo *= _dmgMultiplierTwo;
                if (DamageOne != DamageTwo)
                {
                    if (DamageOne > DamageTwo)
                    {
                        DeckOne.Add(CardTwo);
                        DeckTwo.RemoveAt(IndexTwo);
                    }
                    else
                    {
                        DeckTwo.Add(CardOne);
                        DeckOne.RemoveAt(IndexOne);
                    }
                }
                DecreaseEffectDurations();
                GameCounter++;
            }
            if (DeckOne.Count > DeckTwo.Count)
            {
                //player one wins
            }
            else
            {
                //player two wins
            }
        }

        private void CalculateDamageMultipliers()
        {
            _dmgMultiplierOne = 1;
            foreach (KeyValuePair<string, int> effect in _effectDurationOne)
            {
                if (effect.Value != 0)
                {
                    _dmgMultiplierOne *= _effectDamage[effect.Key];
                }
            }

            _dmgMultiplierTwo = 1;
            foreach (KeyValuePair<string, int> effect in _effectDurationTwo)
            {
                if (effect.Value != 0)
                {
                    _dmgMultiplierTwo *= _effectDamage[effect.Key];
                }
            }
        }

        private void DecreaseEffectDurations()
        {
            foreach (KeyValuePair<string, int> effect in _effectDurationOne)
            {
                if (effect.Value != 0)
                {
                    _effectDurationOne[effect.Key] -= -1;
                }
            }
            foreach (KeyValuePair<string, int> effect in _effectDurationTwo)
            {
                if (effect.Value != 0)
                {
                    _effectDurationTwo[effect.Key] -= -1;
                }
            }
        }

        public Battle(User PlayerOne, User PlayerTwo)
        {
            this.PlayerOne = PlayerOne;
            this.PlayerTwo = PlayerTwo;
        }

        private enum Winner
        {
            none = 0, playerOne = 1, playerTwo = 2
        }
    }
}
