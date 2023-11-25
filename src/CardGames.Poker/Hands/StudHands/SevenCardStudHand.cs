using CardGames.Core.French.Cards;
using System;
using System.Collections.Generic;

namespace CardGames.Poker.Hands.StudHands;

public class SevenCardStudHand : StudHand
{
    public SevenCardStudHand(
        IReadOnlyCollection<Card> holeCards,
        IReadOnlyCollection<Card> openCards,
        Card downCard) : base(holeCards, openCards, new[] { downCard })
    {
        if (holeCards.Count != 2)
        {
            throw new ArgumentException("A seven card Stud hand needs exactly two hole cards");
        }

        if (openCards.Count > 4)
        {
            throw new ArgumentException("A seven card Stud hand has at most four open cards");
        }
    }
}
