using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Core.French.Cards.Extensions;

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
    {
        if (cardsExpression.Length == 0) return [];

        var remaining = cardsExpression.AsSpan();
        List<Card> cards = [];
        while (remaining.Length > 2)
        {
            cards.Add(new Card(remaining[1].ToSuit(), remaining[0].ToSymbol()));
            remaining = remaining[3..];
        }
        cards.Add(new Card(remaining[1].ToSuit(), remaining[0].ToSymbol()));
        return cards;
    }

    public static string ToStringRepresentation(this IEnumerable<Card> cards)
       => string.Join(" ", cards.OrderByDescending(card => card.Value).Select(card => card.ToString()));

    public static Card ToCard(this string cardExpression)
    {
        if (cardExpression.Length != 2)
        {
            throw new ArgumentException($"{cardExpression} is not a valid representation of a card.");
        }
        var cardsSpan = cardExpression.AsSpan();
        return new Card(cardsSpan[1].ToSuit(), cardsSpan[0].ToSymbol());
    }

    /// <summary>
    /// Pre-span implementation which is much slower than the current one.
    /// </summary>
    public static Card ToCardObsolete(this string cardExpression)
    {
        var hash = Array.IndexOf(CardDeck, cardExpression);

        return hash >= 0
            ? new Card(hash.ToSuit(), hash.ToValue())
            : throw new ArgumentException($"{cardExpression} is not a valid representation of a card.");
    }

    /// <summary>
    /// Pre-span implementation which is much slower than the current one.
    /// </summary>
    public static IReadOnlyCollection<Card> ToCardsObsolete(this string cardsExpression)
        => cardsExpression
            .Split(' ')
            .Select(expression => expression.ToCardObsolete())
            .ToList();
}
