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
        private User PlayerOne;
        private User PlayerTwo;
        private const int EffectDuration = 2;
        private Dictionary<string, double>? _effectDamage;
        private Dictionary<string, int>? _effectDurationOne;
        private Dictionary<string, int>? _effectDurationTwo;
        private double _dmgMultiplierOne = 1;
        private double _dmgMultiplierTwo = 1;
        private int RoundNum = 0;
        private string battleLog = string.Empty;

        private Winner RoundWinner(Card cardOne, Card cardTwo)
        {
            double DamageOne;
            double DamageOneRaw;
            double DamageTwo;
            double DamageTwoRaw;
            battleLog = $"{battleLog}\nRound {RoundNum}:\nCard {cardOne.Name} and Card {cardTwo.Name} have been called into battle\nDamage raw: ";
            DamageOne = cardOne.CalcDmg(cardTwo);
            DamageOneRaw = cardOne.Damage;
            if (cardOne.Type == CardType.effect)
            {
                _effectDamage[cardOne.Name] = DamageOne;
                if (DamageOne > 1)
                {
                    battleLog = $"{battleLog} {cardOne.Name} -> boosts {PlayerOne.Username}'s cards damage by {(DamageOne - 1) * 100}% for two rounds, ";
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
                    battleLog = $"{battleLog} {cardOne.Name} -> decreases {PlayerTwo.Username}'s cards damage by {(1 - DamageOne) * 100}% for two rounds, ";
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
                battleLog = $"{battleLog} {cardOne.Name} -> {DamageOneRaw}, ";
            }
            DamageTwo = cardTwo.CalcDmg(cardOne);
            DamageTwoRaw = cardOne.Damage;
            if (cardTwo.Type == CardType.effect)
            {

                _effectDamage[cardTwo.Name] = DamageTwo;
                if (DamageTwo > 1)
                {
                    battleLog = $"{battleLog} {cardTwo.Name} -> boosts {PlayerTwo.Username}'s cards damage by {(DamageTwo - 1) * 100}% for two rounds\n";
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
                    battleLog = $"{battleLog} {cardTwo.Name} -> decreases {PlayerOne.Username}'s cards damage by {(1 - DamageTwo) * 100}% for two rounds\n";
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
                battleLog = $"{battleLog} {cardTwo.Name} -> {DamageOneRaw}\n";
            }

            CalculateDamageMultipliers();
            DamageOne *= _dmgMultiplierOne;
            DamageTwo *= _dmgMultiplierTwo;

            if (_dmgMultiplierOne != 1 || _dmgMultiplierTwo != 1)
            {
                battleLog = $"{battleLog}This rounds damage effectiveness: {PlayerOne.Username} -> {_dmgMultiplierOne * 100}%, {PlayerTwo.Username} -> {_dmgMultiplierTwo * 100}%\n";
            }
            battleLog = $"{battleLog}Final damage: {cardOne.Name} -> {DamageOne}, {cardTwo.Name} -> {DamageTwo}\n";

            if (DamageOne > DamageTwo)
            {
                return Winner.playerOne;
            }
            else if (DamageTwo > DamageOne)
            {
                return Winner.playerTwo;
            }
            else
            {
                return Winner.none;
            }

        }

        private int CalculateElo(int eloCalc, int eloRef, bool winner)
        {
            int eloIncrease = 10;
            int eloDecrease = 8;
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

        private void UpdateStats(Winner winner)
        {
            switch (winner)
            {
                case Winner.playerOne:
                    PlayerOne.Elo = CalculateElo(PlayerOne.Elo, PlayerTwo.Elo, true);
                    battleLog = $"{battleLog}User {PlayerOne.Username} won the battle!\n";
                    break;
                case Winner.playerTwo:
                    PlayerOne.Elo = CalculateElo(PlayerOne.Elo, PlayerTwo.Elo, false);
                    battleLog = $"{battleLog}User {PlayerTwo.Username} won the battle!\n";
                    break;
                case Winner.none:
                    battleLog = $"{battleLog}The battle ended in a draw!\n";
                    break;
            }

            //BattleController UpdatePlayerStats(PlayerOne)
        }
        public void Fight()
        {
            List<Card>? DeckOne = PlayerOne.Deck;
            List<Card>? DeckTwo = PlayerTwo.Deck;
            Card? CardOne;
            Card? CardTwo;
            int IndexOne;
            int IndexTwo;
            battleLog = $"NEW BATTLE\n\nUser {PlayerOne.Username} and User {PlayerTwo.Username} are battling!!!\n\n";

            while (DeckOne.Count != 0 && DeckTwo.Count != 0 && RoundNum < 100)
            {
                RoundNum++;

                IndexOne = Rand.Next(DeckOne.Count);
                IndexTwo = Rand.Next(DeckTwo.Count);

                CardOne = DeckOne[IndexOne];
                CardTwo = DeckTwo[IndexTwo];

                switch (RoundWinner(CardOne, CardTwo))
                {
                    case Winner.playerOne:
                        DeckOne.Add(CardTwo);
                        DeckTwo.RemoveAt(IndexTwo);
                        battleLog = $"{battleLog}Card {CardOne.Name} of User {PlayerOne.Username} won\nUser {PlayerOne.Username} gains Card {CardTwo.Name}\n";
                        break;
                    case Winner.playerTwo:
                        DeckTwo.Add(CardOne);
                        DeckOne.RemoveAt(IndexOne);
                        battleLog = $"{battleLog}Card {CardTwo.Name} of User {PlayerTwo.Username} won\nUser {PlayerTwo.Username} gains Card {CardOne.Name}\n";
                        break;
                    case Winner.none:
                        battleLog = $"{battleLog}The round ended in a draw\n";
                        break;
                }

                DecreaseEffectDurations();
            }

            Winner winner;
            if (DeckOne.Count > DeckTwo.Count)
            {
                winner = Winner.playerOne;
            }
            else if (DeckTwo.Count > DeckOne.Count)
            {
                winner = Winner.playerTwo;
            }
            else
            {
                winner = Winner.none;
            }

            UpdateStats(winner);
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
