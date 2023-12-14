using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Cards;
using MonsterTradingCardsGame.Daos;

namespace MonsterTradingCardsGame.Models
{
    internal class Battle
    {
        Random Rand = new Random();
        public User Requestor { get; set; }
        public User Opponent { get; set; }
        private const int EffectDuration = 2;
        private Dictionary<string, double> _effectDamage;
        private Dictionary<string, int> _effectDurationOne;
        private Dictionary<string, int> _effectDurationTwo;
        private double _dmgMultiplierOne = 1;
        private double _dmgMultiplierTwo = 1;
        private int RoundNum = 0;
        public string BattleLog { get; set; }
        private Winner BattleWinner;

        private Winner RoundWinner(Card cardOne, Card cardTwo)
        {
            double DamageOne;
            double DamageOneRaw;
            double DamageTwo;
            double DamageTwoRaw;
            BattleLog = $"{BattleLog}\nRound {RoundNum}:\nCard {cardOne.Name} and Card {cardTwo.Name} have been called into battle\nDamage raw: ";
            DamageOne = cardOne.CalcDmg(cardTwo);
            DamageOneRaw = cardOne.Damage;
            if (cardOne.Type == CardType.effect)
            {
                _effectDamage[cardOne.Name] = DamageOne;
                if (DamageOne > 1)
                {
                    BattleLog = $"{BattleLog}{cardOne.Name} -> boosts {Requestor.Username}'s cards damage by {(DamageOne - 1) * 100}% for two rounds, ";
                    if (_effectDurationOne.ContainsKey(cardOne.Name))
                    {
                        _effectDurationOne[cardOne.Name] += EffectDuration;
                    }
                    else
                    {
                        _effectDurationOne[cardOne.Name] = EffectDuration;
                    }
                }
                else
                {
                    BattleLog = $"{BattleLog}{cardOne.Name} -> decreases {Opponent.Username}'s cards damage by {(1 - DamageOne) * 100}% for two rounds, ";
                    if (_effectDurationTwo.ContainsKey(cardOne.Name))
                    {
                        _effectDurationTwo[cardOne.Name] += EffectDuration;
                    }
                    else
                    {
                        _effectDurationTwo[cardOne.Name] = EffectDuration;
                    }
                }
                DamageOne = 0;
            }
            else
            {
                BattleLog = $"{BattleLog}{cardOne.Name} -> {DecimalReplace(DamageOneRaw)} --- ";
            }
            DamageTwo = cardTwo.CalcDmg(cardOne);
            DamageTwoRaw = cardOne.Damage;
            if (cardTwo.Type == CardType.effect)
            {

                _effectDamage[cardTwo.Name] = DamageTwo;
                if (DamageTwo > 1)
                {
                    BattleLog = $"{BattleLog}{cardTwo.Name} -> boosts {Opponent.Username}'s cards damage by {(DamageTwo - 1) * 100}% for two rounds\n";
                    if (_effectDurationTwo.ContainsKey(cardTwo.Name))
                    {
                        _effectDurationTwo[cardTwo.Name] += EffectDuration;
                    }
                    else
                    {
                        _effectDurationTwo[cardTwo.Name] = EffectDuration;
                    }
                }
                else
                {
                    BattleLog = $"{BattleLog}{cardTwo.Name} -> decreases {Requestor.Username}'s cards damage by {(1 - DamageTwo) * 100}% for two rounds\n";
                    if (_effectDurationOne.ContainsKey(cardTwo.Name))
                    {
                        _effectDurationOne[cardTwo.Name] += EffectDuration;
                    }
                    else
                    {
                        _effectDurationOne[cardTwo.Name] = EffectDuration;
                    }
                }
                DamageTwo = 0;
            }
            else
            {
                BattleLog = $"{BattleLog}{cardTwo.Name} -> {DecimalReplace(DamageOneRaw)}\n";
            }

            CalculateDamageMultipliers();
            DamageOne *= _dmgMultiplierOne;
            DamageTwo *= _dmgMultiplierTwo;

            if (_dmgMultiplierOne != 1 || _dmgMultiplierTwo != 1)
            {
                BattleLog = $"{BattleLog}This rounds damage effectiveness: {Requestor.Username} -> {_dmgMultiplierOne * 100}% --- {Opponent.Username} -> {_dmgMultiplierTwo * 100}%\n";
            }
            BattleLog = $"{BattleLog}Final damage: {cardOne.Name} -> {DecimalReplace(DamageOne)} --- {cardTwo.Name} -> {DecimalReplace(DamageTwo)}\n";

            if (DamageOne > DamageTwo)
            {
                return Winner.requestor;
            }
            else if (DamageTwo > DamageOne)
            {
                return Winner.opponent;
            }
            else
            {
                return Winner.none;
            }

        }

        private static string DecimalReplace(double damage)
        {
            return $"{damage}".Replace(',', '.');
        }

        private static int CalculateElo(int eloCalc, int eloRef, bool winner)
        {
            int eloIncrease = 5;
            int eloDecrease = 3;
            if (winner)
            {
                if (eloCalc < eloRef)
                {
                    double eloBoost = (eloRef - eloCalc) / 10;
                    eloBoost += eloBoost < 1 ? 1 : 0;
                    eloIncrease = (int)(eloIncrease * eloBoost);
                }
                eloCalc += eloIncrease;
            }
            else
            {
                if (eloCalc > eloRef)
                {
                    double eloBoost = (eloCalc - eloRef) / 10;
                    eloBoost += eloBoost < 1 ? 1 : 0;
                    eloDecrease = (int)(eloDecrease * eloBoost);
                }
                eloCalc -= eloDecrease;
            }
            return eloCalc;
        }

        public void UpdateStats()
        {
            Requestor.GamesPlayed++;
            switch (BattleWinner)
            {
                case Winner.requestor:
                    Requestor.Wins++;
                    Requestor.Elo = CalculateElo(Requestor.Elo, Opponent.Elo, true);
                    BattleLog = $"{BattleLog}User {Requestor.Username} won the battle!\n";
                    break;
                case Winner.opponent:
                    Requestor.Elo = CalculateElo(Requestor.Elo, Opponent.Elo, false);
                    BattleLog = $"{BattleLog}User {Opponent.Username} won the battle!\n";
                    break;
                case Winner.none:
                    BattleLog = $"{BattleLog}The battle ended in a draw!\n";
                    break;
            }
        }
        public void Fight()
        {
            List<Card>? DeckOne = Requestor.Deck;
            List<Card>? DeckTwo = Opponent.Deck;
            Card? CardOne;
            Card? CardTwo;
            int IndexOne;
            int IndexTwo;
            BattleLog = $"NEW BATTLE\n\nUser {Requestor.Username} and User {Opponent.Username} are battling!!!\n\n";

            while (DeckOne.Count != 0 && DeckTwo.Count != 0 && RoundNum < 100)
            {
                RoundNum++;

                IndexOne = Rand.Next(DeckOne.Count);
                IndexTwo = Rand.Next(DeckTwo.Count);

                CardOne = DeckOne[IndexOne];
                CardTwo = DeckTwo[IndexTwo];

                switch (RoundWinner(CardOne, CardTwo))
                {
                    case Winner.requestor:
                        DeckOne.Add(CardTwo);
                        DeckTwo.RemoveAt(IndexTwo);
                        BattleLog = $"{BattleLog}Card {CardOne.Name} of User {Requestor.Username} won\nUser {Requestor.Username} gains Card {CardTwo.Name}\n";
                        break;
                    case Winner.opponent:
                        DeckTwo.Add(CardOne);
                        DeckOne.RemoveAt(IndexOne);
                        BattleLog = $"{BattleLog}Card {CardTwo.Name} of User {Opponent.Username} won\nUser {Opponent.Username} gains Card {CardOne.Name}\n";
                        break;
                    case Winner.none:
                        BattleLog = $"{BattleLog}The round ended in a draw\n";
                        break;
                }

                DecreaseEffectDurations();
            }

            if (DeckOne.Count > DeckTwo.Count)
            {
                BattleWinner = Winner.requestor;
            }
            else if (DeckTwo.Count > DeckOne.Count)
            {
                BattleWinner = Winner.opponent;
            }
            else
            {
                BattleWinner = Winner.none;
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

        public Battle(User requestor, User opponent)
        {
            Requestor = requestor;
            Opponent = opponent;
            BattleLog = string.Empty;
            _effectDamage = new();
            _effectDurationOne = new();
            _effectDurationTwo = new();
        }

        private enum Winner
        {
            none = 0, requestor = 1, opponent = 2
        }
    }
}
