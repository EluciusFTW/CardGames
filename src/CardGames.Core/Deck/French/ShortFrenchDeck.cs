using EluciusFTW.CardGames.Core.Cards.French;
using System.Collections.Generic;
using System.Linq;

namespace EluciusFTW.CardGames.Core.Deck.French
{
    public class ShortFrenchDeck : FrenchDeck
    {
        private readonly IReadOnlyCollection<Card> _cards
            = Enumerable
                .Range(6, 9)
                .SelectMany(value => GetAllsuits(value))
                .ToList();

        protected override IReadOnlyCollection<Card> Cards() => _cards;
    }
}
