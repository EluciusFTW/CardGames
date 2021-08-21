using CardGames.Core.French.Cards;
using CardGames.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using CardGames.Poker.Hands.HandTypes;
using CardGames.Poker.Hands.HandStrenght;

namespace CardGames.Poker.Hands
{
    public class CommunityCardsHand : HandBase
    {
        protected int _leastNumberOfHoleCardsToBeUsed;
        protected int _greatestNumberOfHoleCardsToBeUsed;

        public IReadOnlyCollection<Card> HoleCards { get; }
        public IReadOnlyCollection<Card> CommunityCards { get; }

        public override long HandStrength { get; }
        public override HandType HandType { get; }

        public CommunityCardsHand(
            int leastNumberOfHoleCardsToBeUsed,
            int greatestNumberOfHoleCardsToBeUsed,
            IReadOnlyCollection<Card> holeCards,
            IReadOnlyCollection<Card> communityCards)
            : base(holeCards.Concat(communityCards).ToList())
        {
            _leastNumberOfHoleCardsToBeUsed = leastNumberOfHoleCardsToBeUsed;
            _greatestNumberOfHoleCardsToBeUsed = greatestNumberOfHoleCardsToBeUsed;
            HoleCards = holeCards;
            CommunityCards = communityCards;

            var handsAndTypes = PossibleHands()
                .Select(hand => new { hand, type = HandTypeDetermination.DetermineHandType(hand) });

            HandType = StrengthCalculations
                .GetEffectiveType(handsAndTypes.Select(pair => pair.type).ToList());

            var handsOfEffectiveType = handsAndTypes
                .Where(pair => pair.type == HandType)
                .Select(pair => pair.hand);

            HandStrength = handsOfEffectiveType
                .Select(hand => StrengthCalculations.Classic(hand, HandType))
                .Max();
        }

        private IEnumerable<IReadOnlyCollection<Card>> PossibleHands()
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
