using CardGames.Core.French.Cards;
using CardGames.Poker.Simulations.Holdem;
using System;
using System.Collections.Generic;

namespace CardGames.Poker.Simulations.Omaha
{
    public class OmahaPlayer : CommunityCardGamePlayer
    {
        public OmahaPlayer(string name, IReadOnlyCollection<Card> holeCards)
        {
            Name = name;

            if (holeCards.Count > 4)
            {
                throw new ArgumentException($"{name} has too many hole cards to play Omaha.");
            }
            GivenHoleCards = holeCards;
        }
    }
}

