using CardGames.Core.French.Cards;
using System.Collections.Generic;
using CardGames.Poker.Hands.HandTypes;
using CardGames.Poker.Hands.Strength;

namespace CardGames.Poker.Hands
{
    public sealed class DrawHand : FiveCardHand
    {
        public DrawHand(IReadOnlyCollection<Card> cards)
            : base(cards)
        {
            Strength = HandStrength.Classic(Cards, Type);
            Type = HandTypeDetermination.DetermineHandType(Cards);
        }

        public override long Strength { get; }

        public override HandType Type { get; }
    }
}
