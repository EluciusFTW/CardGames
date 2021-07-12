using EluciusFTW.CardGames.Core.Cards.Deck;
using EluciusFTW.CardGames.Core.Cards.French;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EluciusFTW.CardGames.Core.Deck.French
{
    public abstract class FrenchDeck : IDeck<Card>
    {
        private IList<Card> _cardsOut
            = new List<Card>();

        private Card this[int index]
             => CardsLeft()
                .ToArray()[index];

        public int NumberOfCardsLeft()
            => 52 - _cardsOut.Count;

        protected abstract IReadOnlyCollection<Card> Cards();

        protected static IEnumerable<Card> GetAllsuits(int value)
            => Enumerable
                .Range(0, 4)
                .Select(suit => new Card((Suit)suit, value));

        public IReadOnlyCollection<Card> CardsLeft()
            => Cards()
                .Except(_cardsOut)
                .ToArray();

        public IReadOnlyCollection<Card> CardsLeftOfValue(int value)
            => CardsLeftWith(card => card.Value == value);

        public IReadOnlyCollection<Card> CardsLeftOfSuit(Suit suit)
            => CardsLeftWith(card => card.Suit == suit);

        public IReadOnlyCollection<Card> CardsLeftWith(Func<Card, bool> predicate)
            => CardsLeft()
                .Where(predicate)
                .ToArray();

        public Card GetSpecific(Card card)
        {   
            if (!Cards().Contains(card))
            {
                throw new ArgumentException($"The card {card} is not part of the deck at all!");
            }

            if (_cardsOut.Contains(card))
            {
                throw new ArgumentException($"The card {card} has already been dealt!");
            }
            _cardsOut.Add(card);
            return card;
        }

        public Card GetFromRemaining(int index)
        {
            var card = this[index];
            _cardsOut.Add(card);
            return card;
        }

        public void Reset()
            => _cardsOut = Array.Empty<Card>();
    }
}
