using CardGames.Core.French.Cards;
using CardGames.Core.French.Cards.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGames.Poker.Hands.HandStrenght
{
    public static class StrengthCalculations
    {
        private static readonly Dictionary<HandType, int> ClassicHap = new Dictionary<HandType, int>
        {
            { HandType.Incomplete, -1 },
            { HandType.Highcard, 0 },
            { HandType.OnePair, 1 },
            { HandType.TwoPair, 2 },
            { HandType.Trips, 3},
            { HandType.Straight, 4 },
            { HandType.Flush, 5 },
            { HandType.FullHouse, 6 },
            { HandType.Quads, 7 },
            { HandType.StraightFlush, 8 },
        };

        private static readonly Dictionary<HandType, int> ShortDeckMap = new Dictionary<HandType, int>
        {
            { HandType.Incomplete, -1 },
            { HandType.Highcard, 0 },
            { HandType.OnePair, 1 },
            { HandType.TwoPair, 2 },
            { HandType.Trips, 3},
            { HandType.Straight, 4 },
            { HandType.FullHouse, 5 },
            { HandType.Flush, 6 },
            { HandType.Quads, 7 },
            { HandType.StraightFlush, 8 },
        };

        private static long prefixMultiplier = 10000000000;

        public static long Classic(IReadOnlyCollection<Card> cards, HandType handType)
            => Calculate(cards, handType, ClassicHap);

        public static long ShortDeck(IReadOnlyCollection<Card> cards, HandType handType)
            => Calculate(cards, handType, ShortDeckMap);

        public static long Calculate(IReadOnlyCollection<Card> cards, HandType handType, IDictionary<HandType,int> handTypeValueMap)
        {
            var valuesAsNumber = CalculateFromCards(cards);
            var handTypePrefix = prefixMultiplier * handTypeValueMap[handType];

            return handTypePrefix + valuesAsNumber;
        }

        private static int CalculateFromCards(IReadOnlyCollection<Card> cards)
            => cards
                .DescendingValues()
                .Select((value, index) => (int)(Math.Pow(10, 2 * (4 - index)) * value))
                .Sum();
    }
}
