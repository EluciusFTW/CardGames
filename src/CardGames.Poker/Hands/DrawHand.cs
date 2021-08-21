using CardGames.Core.French.Cards;
using System.Collections.Generic;
using CardGames.Poker.Hands.HandStrenght;
using CardGames.Poker.Hands.HandTypes;

namespace CardGames.Poker.Hands
{
    public sealed class DrawHand : FiveCardHand
    {
        public DrawHand(IReadOnlyCollection<Card> cards)
            : base(cards)
        {
            HandStrength = StrengthCalculations.Classic(Cards, HandType);
            HandType = HandTypeDetermination.DetermineHandType(Cards);
        }

        public override long HandStrength { get; }

        public override HandType HandType { get; }
    }
}
