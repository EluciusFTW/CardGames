using CardGames.Core.French.Cards;
using CardGames.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using CardGames.Poker.Hands.HandTypes;
using CardGames.Poker.Hands.Strength;

namespace CardGames.Poker.Hands
{
    public class CommunityCardsHand : HandBase
    {
        protected int _leastNumberOfHoleCardsToBeUsed;
        protected int _greatestNumberOfHoleCardsToBeUsed;

        public IReadOnlyCollection<Card> HoleCards { get; }
        public IReadOnlyCollection<Card> CommunityCards { get; }

        public override long Strength { get; }
        public override HandType Type { get; }

        public CommunityCardsHand(
            int leastNumberOfHoleCardsToBeUsed,
            int greatestNumberOfHoleCardsToBeUsed,
            IReadOnlyCollection<Card> holeCards,
            IReadOnlyCollection<Card> communityCards)
            : base(holeCards
                  .Concat(communityCards)
                  .ToList())
        {
            _leastNumberOfHoleCardsToBeUsed = leastNumberOfHoleCardsToBeUsed;
            _greatestNumberOfHoleCardsToBeUsed = greatestNumberOfHoleCardsToBeUsed;
            HoleCards = holeCards;
            CommunityCards = communityCards;

            var handsAndTypes = PossibleHands()
                .Select(hand => new { hand, type = HandTypeDetermination.DetermineHandType(hand) });

            Type = HandStrength
                .GetEffectiveType(handsAndTypes.Select(pair => pair.type).ToList());

            var handsOfEffectiveType = handsAndTypes
                .Where(pair => pair.type == Type)
                .Select(pair => pair.hand);

            Strength = handsOfEffectiveType
                .Select(hand => HandStrength.Classic(hand, Type))
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
