using CardGames.Core.French.Cards;
using CardGames.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using CardGames.Poker.Hands.Strength;

namespace CardGames.Poker.Hands.CommunityCardHands
{
    public class CommunityCardsHand : HandBase
    {
        protected readonly int LeastNumberOfHoleCardsToBeUsed;
        protected readonly int GreatestNumberOfHoleCardsToBeUsed;

        public IReadOnlyCollection<Card> HoleCards { get; }
        public IReadOnlyCollection<Card> CommunityCards { get; }
        protected override HandTypeStrengthRanking Ranking { get; }

        public CommunityCardsHand(
            int leastNumberOfHoleCardsToBeUsed,
            int greatestNumberOfHoleCardsToBeUsed,
            IReadOnlyCollection<Card> holeCards,
            IReadOnlyCollection<Card> communityCards,
            HandTypeStrengthRanking ranking)
            : base(holeCards
                  .Concat(communityCards)
                  .ToList())
        {
            LeastNumberOfHoleCardsToBeUsed = leastNumberOfHoleCardsToBeUsed;
            GreatestNumberOfHoleCardsToBeUsed = greatestNumberOfHoleCardsToBeUsed;
            HoleCards = holeCards;
            CommunityCards = communityCards;
            Ranking = ranking;
        }

        protected override IEnumerable<IReadOnlyCollection<Card>> PossibleHands()
        {
            var nrOfCombos = GreatestNumberOfHoleCardsToBeUsed - LeastNumberOfHoleCardsToBeUsed + 1;
            return Enumerable
                .Range(LeastNumberOfHoleCardsToBeUsed, nrOfCombos)
                .SelectMany(PossibleHandsWithFixedNumberOfHoleCards);
        }

        private IEnumerable<IReadOnlyCollection<Card>> PossibleHandsWithFixedNumberOfHoleCards(int numberOfHoleCards)
        {
            var communityCombos = CommunityCards.SubsetsOfSize(5 - numberOfHoleCards);
            
            return numberOfHoleCards == 0
                ? communityCombos.Select(cards => cards.ToList())
                : HoleCards.SubsetsOfSize(numberOfHoleCards)
                    .CartesianProduct(communityCombos)
                    .Select(sequences => sequences
                        .SelectMany(cards => cards)
                        .ToList());
        }
    }
}
