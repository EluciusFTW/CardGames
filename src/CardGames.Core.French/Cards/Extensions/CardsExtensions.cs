using System.Collections.Generic;
using System.Linq;

namespace CardGames.Core.French.Cards.Extensions
{
    public static class CardsExtensions
    {
        public static IReadOnlyCollection<Suit> Suits(this IEnumerable<Card> cards)
            => cards
                .Select(card => card.Suit)
                .Distinct()
                .ToList();

        public static IReadOnlyCollection<int> Values(this IEnumerable<Card> cards)
            => cards
                .Select(card => card.Value)
                .ToList();

        public static IReadOnlyCollection<Card> ByDescendingValue(this IEnumerable<Card> cards)
            => cards
                .OrderByDescending(card => card.Value)
                .ToList();

        public static IReadOnlyCollection<int> DescendingValues(this IEnumerable<Card> cards)
            => cards
                .ByDescendingValue()
                .Values();

        public static IReadOnlyCollection<int> DistinctDescendingValues(this IEnumerable<Card> cards)
            => cards
                .DescendingValues()
                .Distinct()
                .ToList();

        public static int ValueOfBiggestPair(this IEnumerable<Card> cards) 
            => cards.HighestValueOfNFolds(2);

        public static int ValueOfTrips(this IEnumerable<Card> cards) 
            => cards.HighestValueOfNFolds(3);
                
        public static bool ContainsValues(this IEnumerable<Card> cards, IEnumerable<int> valuesToContain)
            => valuesToContain
                .All(cards.Values().Contains);

        public static bool ContainsValue(this IEnumerable<Card> cards, int value)
            => cards
                .Values()
                .Contains(value);

        private static int HighestValueOfNFolds(this IEnumerable<Card> cards, int nFold)
        {
            var nfolds = cards
                .GroupBy(card => card.Value)
                .Where(group => group.Count() >= nFold)
                .ToList();

            return nfolds.Any()
                ? nfolds.Max(group => group.First().Value)
                : 0;
        }
    }
}
