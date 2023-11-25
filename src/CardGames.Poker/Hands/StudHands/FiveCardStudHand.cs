using CardGames.Core.French.Cards;
using System;
using System.Collections.Generic;

namespace CardGames.Poker.Hands.StudHands;

public class FiveCardStudHand : StudHand
{
    public FiveCardStudHand(
        Card holeCard,
        IReadOnlyCollection<Card> openCards)
        : base(new[] { holeCard }, openCards, Array.Empty<Card>())
    {
        if (openCards.Count > 4)
        {
            throw new ArgumentException("A five card Stud hand has at most four open cards");
        }
    }
}
