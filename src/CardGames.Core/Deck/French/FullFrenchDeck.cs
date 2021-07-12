using EluciusFTW.CardGames.Core.Cards.French;
using System.Collections.Generic;
using System.Linq;

namespace EluciusFTW.CardGames.Core.Deck.French
{
    public class FullFrenchDeck : FrenchDeck
    {
        private readonly IReadOnlyCollection<Card> _cards
            = Enumerable
                .Range(2, 13)
                .SelectMany(value => GetAllsuits(value))
                .ToList();

        protected override IReadOnlyCollection<Card> Cards() => _cards;
    }
}
