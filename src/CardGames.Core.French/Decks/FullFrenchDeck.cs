using System.Collections.Generic;
using System.Linq;
using CardGames.Core.French.Cards;

namespace CardGames.Core.French.Decks;

public sealed class FullFrenchDeck : FrenchDeck
{
    private readonly IReadOnlyCollection<Card> _cards
        = Enumerable
            .Range(2, 13)
            .SelectMany(CardsOfValue)
            .ToList();
    
    private static IEnumerable<Card> CardsOfValue(int value)
        => Enumerable
            .Range(0, 4)
            .Select(suit => new Card((Suit)suit, value));

    protected override IReadOnlyCollection<Card> Cards => _cards;
}
