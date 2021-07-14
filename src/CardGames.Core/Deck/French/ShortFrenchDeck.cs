using System.Collections.Generic;
using System.Linq;
using CardGames.Core.Cards.French;

namespace CardGames.Core.Deck.French
{
    public class ShortFrenchDeck : FrenchDeck
    {
        private readonly IReadOnlyCollection<Card> _cards
            = Enumerable
                .Range(6, 9)
                .SelectMany(CardsOfValue)
                .ToList();

        private static IEnumerable<Card> CardsOfValue(int value)
            => Enumerable
                .Range(0, 4)
                .Select(suit => new Card((Suit)suit, value));
        
        protected override IReadOnlyCollection<Card> Cards() => _cards;
    }
}
