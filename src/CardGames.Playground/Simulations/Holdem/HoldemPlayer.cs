using System;
using System.Collections.Generic;
using CardGames.Core.French.Cards;

namespace CardGames.Playground.Simulations.Holdem
{
    public class HoldemPlayer : CommunityCardGamePlayer
    {
        public HoldemPlayer(string name, IReadOnlyCollection<Card> holeCards)
        {
            Name = name;
            
            if (holeCards.Count > 2)
            {
                throw new ArgumentException($"{name} has too many hole cards to play Holdem.");
            }
            GivenHoleCards = holeCards;
        }
    }
}       
