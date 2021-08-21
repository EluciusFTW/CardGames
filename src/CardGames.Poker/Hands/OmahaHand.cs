using CardGames.Core.French.Cards;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CardGames.Poker.Hands
{
    public sealed class OmahaHand : CommunityCardsHand
    {
        public OmahaHand(IReadOnlyCollection<Card> holeCards, IReadOnlyCollection<Card> communityCards)
            : base(2, 2, holeCards, communityCards)
        {
            if (holeCards.Count() != 4)
            {
                throw new ArgumentException("An Omaha hand needs exactly four hole cards");
            }

            var numberOfCommuntiyCards = communityCards.Count();
            if (numberOfCommuntiyCards < 3 || 5 < numberOfCommuntiyCards)
            {
                throw new ArgumentException("A Omaha board can only have three, four or five cards.");
            }
        }
    }
}
