using CardGames.Core.French.Cards;
using System.Collections.Generic;
using System.Linq;
using System;
using CardGames.Poker.Hands.Strength;

namespace CardGames.Poker.Hands.CommunityCardHands
{
    public sealed class HoldemHand : CommunityCardsHand
    {
        public HoldemHand(IReadOnlyCollection<Card> holeCards, IReadOnlyCollection<Card> communityCards)
            : base(0, 2, holeCards, communityCards, HandTypeStrengthRanking.Classic)
        {
            if (holeCards.Count() != 2)
            {
                throw new ArgumentException("A Holdem hand needs exactly two hole cards");
            }

            var numberOfCommuntiyCards = communityCards.Count();
            if (numberOfCommuntiyCards < 3 || 5 < numberOfCommuntiyCards)
            {
                throw new ArgumentException("A Holdem board can only have three, four or five cards.");
            }
        }
    }
}
