using System;
using System.Collections.Generic;
using System.Linq;
using CardGames.Core.Deck;
using CardGames.Core.Random;

namespace CardGames.Core.Dealer;

public class Dealer<TCardKind> where TCardKind : class
{
    protected IDeck<TCardKind> Deck { get; }
    protected IRandomNumberGenerator NumberGenerator { get; }

    public Dealer(IDeck<TCardKind> deck)
    {
        Deck = deck;
        NumberGenerator = new StandardRandomNumberGenerator();
    }

    public Dealer(IDeck<TCardKind> deck, IRandomNumberGenerator numberGenerator)
    {
        Deck = deck;
        NumberGenerator = numberGenerator;
    }

    public TCardKind DealCard()
    {
        var cardsLeft = Deck.NumberOfCardsLeft();
        if (cardsLeft < 1)
        {
            throw new InvalidOperationException("There are no more cards in the deck to deal.");
        }
        var nextCardPosition = NumberGenerator.Next(cardsLeft);
        return Deck.GetFromRemaining(nextCardPosition);
    }

    public IReadOnlyCollection<TCardKind> DealCards(int amount)
        => Enumerable
            .Repeat(1, amount)
            .Select(_ => DealCard())
            .ToList();

    public void Shuffle()
        => Deck.Reset();
}
