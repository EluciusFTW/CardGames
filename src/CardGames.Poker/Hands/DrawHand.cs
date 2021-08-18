using CardGames.Core.French.Cards;
using System.Collections.Generic;
using CardGames.Poker.Hands.HandStrenght;

namespace CardGames.Poker.Hands
{
    public sealed class DrawHand : FiveCardHand
    {
        public DrawHand(IReadOnlyCollection<Card> cards)
            : base(cards)
        {
        }

        protected override long HandStrengthCalculation()
            => StrengthCalculations.Classic(Cards, HandType);
    }
}
