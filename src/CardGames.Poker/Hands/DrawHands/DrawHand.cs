using CardGames.Core.French.Cards;
using System.Collections.Generic;
using CardGames.Poker.Hands.Strength;

namespace CardGames.Poker.Hands.DrawHands
{
    public sealed class DrawHand : FiveCardHand
    {
        public DrawHand(IReadOnlyCollection<Card> cards)
            : base(cards)
        {
            Ranking = HandTypeStrengthRanking.Classic;
        }

        protected override HandTypeStrengthRanking Ranking { get; }

        protected override IEnumerable<IReadOnlyCollection<Card>> PossibleHands()
        {
            yield return Cards;
        }
    }
}
