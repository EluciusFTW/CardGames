using CardGames.Core.French.Cards;
using CardGames.Core.French.Cards.Extensions;
using CardGames.Poker.Hands.HandTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Poker.Hands.Strength
{
    public static class HandStrength
    {
        private static long prefixMultiplier = 10000000000;

        public static HandType GetEffectiveType(IEnumerable<HandType> handTypes)
            => handTypes
                .Select(type => new { type, Value = HandTypeStrength.Classic(type) })
                .OrderByDescending(pair => pair.Value)
                .First().type;

        public static HandType GetEffectiveType(IEnumerable<HandType> handTypes, HandTypeStrengthRanking ranking)
            => handTypes
                .Select(type => new { type, Value = HandTypeStrength.ByRanking(ranking, type) })
                .OrderByDescending(pair => pair.Value)
                .First().type;

        public static long Classic(IReadOnlyCollection<Card> cards, HandType handType)
            => Calculate(cards, HandTypeStrength.Classic(handType));

        public static long ShortDeck(IReadOnlyCollection<Card> cards, HandType handType)
            => Calculate(cards, HandTypeStrength.ShortDeck(handType));

        public static long Calculate(IReadOnlyCollection<Card> cards, HandType handType, Func<HandType, int> handTypeOrderMap)
            => Calculate(cards, handTypeOrderMap(handType));

        private static long Calculate(IReadOnlyCollection<Card> cards, int handTypeStrengthMultiplier)
            => prefixMultiplier * handTypeStrengthMultiplier + CalculateFromCards(cards);

        private static int CalculateFromCards(IReadOnlyCollection<Card> cards)
            => cards
                .DescendingValues()
                .Select((value, index) => (int)(Math.Pow(10, 2 * (4 - index)) * value))
                .Sum();
    }
}
