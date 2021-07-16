# CardGames
### Introduction
After writing simulations and tools for card games (poker in particular) in the last years, I have decided to take a step back, sort through the code bases, clean up here and there, and distill out small, reusable packages that hopefully can be useful for the open source community.

### About the project
I assume this project will be slow, because of several reasons. First of all, it is a pure leisure project. Secondly, because I value design. I try to model the entities and their apis as closely as possible to the real world concepts they represent (albeit adding some convenience apis if they are very useful). I value code quality and readability a lot as well. I'll spend lots of time rewriting algorithmically simple things if I feel I can express them even cleaner. And finally, I also am a big believer in unit tests, especially when writing libraries. You put yourself in the role of a client and try to interact with the entities the library provides. They not only provide confidence that everything works the way it should, they make you really think about the design an usability of the entities the library provides. 

# CardGames.Core
There are thousands of different [card games](https://en.wikipedia.org/wiki/Card_game), with many different cards and collections of cards, with different rules and purposes. 

In this package, we tried to follow a domain-driven approach to card games in general<sup>1</sup>. 
It contains the basic entities needed for such a game: _Cards_ (duh!), _Card decks_ (which are the finite collections of all possible cards of given type) and _Dealers_.

We do provide implementations for the entities for the arguably the most well-known card: the [french-suited playing card](https://en.wikipedia.org/wiki/French-suited_playing_cards).

<sup>1</sup> Actually, up to abuse of language, any game of chance that contains a finite set of possibilities can be modelled using this package, e.g., we can interpret a die as a deck of six cards called: 1,2,3,4,5,6. We can implement a `DiceDealer` who 'shuffles the deck' (i.e. returns the dealt/rolled card/value back to the deck immediately) after dealing a card (rolling the die).
## Card
The most elemental part of a card game is the card. 

### French-suited playing cards
This package currently provides the arguably the most used type of card, namely the [french-suited playing card](https://en.wikipedia.org/wiki/French-suited_playing_cards). They currently can be constructed in two ways. Firstly using the constructors,

```cs
// Using the Suit and Symbol enum
var card = new Card(Suit.Hearts, Symbol.Deuce);

// Using the value instead of the Symbol. 
// It is simply the integer value underlying the enum value, but very useful in simulations
var card = new Card(Suit.Hearts, 8);
````

and secondly, using deserialization extensions from the most common card representation,
```cs
// String extension expecting the format {symbol char}{suit char}
var card = "Jc".ToCard();

// String extension expecting the one or more cards separated by a space
var cards = "2h 5d Qs".ToCards();
````

### Custom cards
You can define your own card type. The only restriction for it to work together with the _Deck_ and the _Dealer_ is that cards have to be 
**structs**, and the set of all of them (i.e., the content of the deck) should be **finite** (sorry, you can't implement natural number bingo). 

## Deck
The collection of all different cards, in a bunch, is called a deck.

The package provides a generic interface for a deck which holds cards of the generic struct type `TCardKind`
```cs
// Interface definition for a generic deck
public interface IDeck<TCardKind> where TCardKind : struct
{
    int NumberOfCardsLeft();
    TCardKind GetFromRemaining(int index);
    TCardKind GetSpecific(TCardKind specificCard);
    void Reset();
};
````
### French decks
Of course, as we have provided the french-suited card, we also provide some decks in this package.

First of all, there is a base class which provides a few more useful methods already using the fact that a french card has a `Symbol` (resp. `Value`) and a `Suit`. The only thing an implementing class must provide is the collection of _all cards_ in the deck:
```cs
public abstract class FrenchDeck : IDeck<Cards.French.Card>
{
    protected abstract IReadOnlyCollection<Card> Cards();
    public IReadOnlyCollection<Card> CardsLeft() {...}
    public IReadOnlyCollection<Card> CardsLeftOfValue(int value) {...}
    public IReadOnlyCollection<Card> CardsLeftOfSuit(Suit suit) {...}
    public IReadOnlyCollection<Card> CardsLeftWith(Func<Card, bool> predicate) {...}
}
````
There are two implementations in the package:
- `FullFrenchDeck`: The standard 52-card deck consisting of Deuce-to-Ace of all four suits.
- `ShortFrenchDeck`: A 36-card deck consisting of Six-to-Ace of all four suits (like is used in [Short-deck poker](https://en.wikipedia.org/wiki/Six-plus_hold_%27em)).

## Dealer
The dealer is the person handling the deck. 

We provide a generic dealer as well:
```cs
public class Dealer<TCardKind> where TCardKind : struct
{
    public Dealer(IDeck<TCardKind> deck) {...}
    public Dealer(IDeck<TCardKind> deck, IRandomNumberGenerator numberGenerator) {...}

    public TCardKind DealCard() {...}
    public IReadOnlyCollection<TCardKind> DealCards(int amount) {...}
    public void Shuffle() {...}
}
````
The dealer can deal one or many cards at once, and shuffle the deck. In order to do that, he needs a deck (duh!). In order to shuffle, he needs some source of randomness. We provide an interface you can implement,
```cs
public interface IRandomNumberGenerator
{
    public int Next(int upperBound);
}
````
and a standand implementation (which is just a wrapper holding an instance of `System.Random`).

### French-deck dealer
We provide a dealer implementation for the french card an deck as well, the `FrenchDeckDealer`, including two factory methods for the decks we have defined earlier, and convenience methods useful for simulation purposes, which is possible since we know the french card has a value property:
```cs
// provides a dealer with full deck (or use .WithShortDeck() for a short deck)
var dealer = FrenchDeckDealer.WithfullDeck();

// deals a random card of given value, suit or symbol. 
// Succeeds if there are still some in the deck, else fails.
_ = dealer.TryDealCardOfValue(7, out var card);
_ = dealer.TryDealCardOfSymbol(Symbol.King, out var card);
_ = dealer.TryDealCardOfSuit(Suit.Spades, out var card);
````

### Disclaimer
:hand: Although this description uses the word _package_ multiple times, it is not yet published as a nuget package, as it is still under development.


## Feedback and Contributing
All feedback welcome!
All contributions are welcome!
