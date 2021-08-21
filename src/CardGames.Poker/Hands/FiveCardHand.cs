using CardGames.Core.French.Cards;
using System;
using System.Collections.Generic;

namespace CardGames.Poker.Hands
{
    public abstract class FiveCardHand : HandBase
    {
        protected FiveCardHand(IReadOnlyCollection<Card> cards)
            : base(cards)
        {
            if (cards.Count != 5)
            {
                throw new ArgumentException("A five card hand requires exactly 5 cards!");
            }
        } 
    }
}