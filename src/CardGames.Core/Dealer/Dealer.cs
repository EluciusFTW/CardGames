using EluciusFTW.CardGames.Core.Cards.Deck;
using EluciusFTW.CardGames.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EluciusFTW.CardGames.Core
{
    public class Dealer<TCard> where TCard : struct
    {
        protected IDeck<TCard> Deck { get; }
        protected IRandomNumberGenerator NumberGenerator { get; }

        public Dealer(IDeck<TCard> deck)
        {
            Deck = deck;
            NumberGenerator = new StandardRandomNumberGenerator();
        }

        public Dealer(IDeck<TCard> deck, IRandomNumberGenerator numberGenerator)
        {
            Deck = deck;
            NumberGenerator = numberGenerator;
        }

        public TCard DealCard()
        {
            var cardsLeft = Deck.NumberOfCardsLeft();
            if (cardsLeft < 1)
            {
                throw new InvalidOperationException("There are no more cards in the deck to deal.");
            }
            var nextCardPosition = NumberGenerator.Next(cardsLeft);
            return Deck.GetFromRemaining(nextCardPosition);
        }

        public IReadOnlyCollection<TCard> DealCards(int amount)
            => Enumerable
                .Repeat(1, amount)
                .Select(_ => DealCard())
                .ToList();

        public void Shuffle()
            => Deck.Reset();
    }
}
