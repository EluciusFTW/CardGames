using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Core.Cards.French.Extensions
{
    public static class SerializationExtensions
    {
        private static readonly string[] CardDeck = 
        {
            "2h", "3h", "4h", "5h", "6h", "7h", "8h", "9h", "Th", "Jh", "Qh", "Kh", "Ah",
            "2d", "3d", "4d", "5d", "6d", "7d", "8d", "9d", "Td", "Jd", "Qd", "Kd", "Ad",
            "2s", "3s", "4s", "5s", "6s", "7s", "8s", "9s", "Ts", "Js", "Qs", "Ks", "As",
            "2c", "3c", "4c", "5c", "6c", "7c", "8c", "9c", "Tc", "Jc", "Qc", "Kc", "Ac"
        };

        private static int Hash(Card card) 
            => (int)card.Suit * 13 + card.Value - 2;
        
        private static Suit ToSuit(this int hash) 
            => (Suit)(hash / 13);

        private static int ToValue(this int hash) 
            => hash % 13 + 2;

        public static string ToShortString(this Card card) 
            => CardDeck[Hash(card)];

        public static IReadOnlyCollection<Card> ToCards(this string cardsExpression)
            => cardsExpression
                .Split(' ')
                .Select(expression => expression.ToCard())
                .ToList();

        public static string ToStringRepresentation(this IEnumerable<Card> cards)
           => string.Join(" ", cards.OrderBy(card => -card.Value).Select(card => card.ToString()));

        public static Card ToCard(this string cardExpression)
        {
            var hash = Array.IndexOf(CardDeck, cardExpression);

            return hash >= 0
                ? new Card(hash.ToSuit(), hash.ToValue())
                : throw new ArgumentException($"{cardExpression} is not a valid representation of a card.");
        }
    }
}
