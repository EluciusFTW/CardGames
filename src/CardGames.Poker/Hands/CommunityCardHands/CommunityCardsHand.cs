using CardGames.Core.French.Cards;
using CardGames.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using CardGames.Poker.Hands.HandTypes;
using CardGames.Poker.Hands.Strength;

namespace CardGames.Poker.Hands.CommunityCardHands
{
    public class CommunityCardsHand : HandBase
    {
        protected int _leastNumberOfHoleCardsToBeUsed;
        protected int _greatestNumberOfHoleCardsToBeUsed;

        public IReadOnlyCollection<Card> HoleCards { get; }
        public IReadOnlyCollection<Card> CommunityCards { get; }
        public override HandTypeStrengthRanking Ranking { get; }
       
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
            _leastNumberOfHoleCardsToBeUsed = leastNumberOfHoleCardsToBeUsed;
            _greatestNumberOfHoleCardsToBeUsed = greatestNumberOfHoleCardsToBeUsed;
            HoleCards = holeCards;
            CommunityCards = communityCards;
            Ranking = ranking;
        }

        public override IEnumerable<IReadOnlyCollection<Card>> PossibleHands()
        {
            var nrOfCombos = _greatestNumberOfHoleCardsToBeUsed - _leastNumberOfHoleCardsToBeUsed + 1;
            return Enumerable
                .Range(_leastNumberOfHoleCardsToBeUsed, nrOfCombos)
                .SelectMany(PossibleHandsWithFixedNumberOfHoleCards);
        }

        private IEnumerable<IReadOnlyCollection<Card>> PossibleHandsWithFixedNumberOfHoleCards(int numberOfHoleCards)
        {
            var holeCardCombos = HoleCards.SubsetsOfSize(numberOfHoleCards);
            var communityCombos = CommunityCards.SubsetsOfSize(5 - numberOfHoleCards);

            if (!holeCardCombos.Any())
            {
                return communityCombos.Select(cards => cards.ToList());
            }

            return holeCardCombos
                .CartesianProduct(communityCombos)
                .Select(sequences => sequences
                    .SelectMany(cards => cards)
                    .ToList());
        }
    }
}
