using System;
using CardGames.Core.Cards.French;
using CardGames.Core.Deck.French;
using FluentAssertions;
using Xunit;

namespace CardGames.Core.Tests.Deck
{
    public class FrenchDeckTests
    {   
        [Fact]
        public void Full_French_Deck_Has_52_Cards()
        {
            var fullDeck = new FullFrenchDeck();

            fullDeck.CardsLeft().Should().HaveCount(52);
        }

        [Fact]
        public void Short_French_Deck_Has_36_Cards()
        {
            var shortDeck = new ShortFrenchDeck();

            shortDeck.CardsLeft().Should().HaveCount(36);
        }

        [Fact]
        public void Returns_Number_Of_Cards_Left()
        {
            var deck = new ShortFrenchDeck();
            _ = deck.GetFromRemaining(1);
            _ = deck.GetFromRemaining(12);
            _ = deck.GetFromRemaining(14);

            deck.CardsLeft().Should().HaveCount(33);
        }

        [Fact]
        public void Returns_Specified_Card()
        {
            var deck = new ShortFrenchDeck();
            var desiredCard = new Card(Suit.Clubs, Symbol.Jack);

            var card = deck.GetSpecific(desiredCard);

            card.Should().Be(desiredCard);
        }

        [Fact]
        public void Returns_Cards_Left_Of_Suit()
        {
            var deck = new ShortFrenchDeck();
            _ = deck.GetSpecific(new Card(Suit.Spades, Symbol.Nine));
            _ = deck.GetSpecific(new Card(Suit.Spades, Symbol.King));
            _ = deck.GetSpecific(new Card(Suit.Diamonds, Symbol.Ten));

            deck.CardsLeftOfSuit(Suit.Spades).Should().HaveCount(7);
            deck.CardsLeftOfSuit(Suit.Diamonds).Should().HaveCount(8);
            deck.CardsLeftOfSuit(Suit.Hearts).Should().HaveCount(9);
        }

        [Fact]
        public void Throws_When_Getting_A_Card_Thats_Not_Pert_of_The_Deck()
        {
            var deck = new ShortFrenchDeck();
            
            Action getInvalidCard = () => deck.GetSpecific(new Card(Suit.Spades, Symbol.Three));

            getInvalidCard
                .Should().Throw<ArgumentException>()
                .WithMessage("The card 3s is not part of the deck at all!");
        }

        [Fact]
        public void Throws_When_Getting_A_Card_That_Has_Already_Been_Dealt_Out()
        {
            var deck = new FullFrenchDeck();
            deck.GetSpecific(new Card(Suit.Spades, Symbol.Three));

            Action getSameCardAgain = () => deck.GetSpecific(new Card(Suit.Spades, Symbol.Three));

            getSameCardAgain
                .Should().Throw<ArgumentException>()
                .WithMessage("The card 3s has already been dealt!");
        }
    }
}
