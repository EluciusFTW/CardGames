using System.Linq;
using CardGames.Core.Cards.French;
using CardGames.Core.Dealer;
using CardGames.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CardGames.Core.Tests.Dealer
{
    public class FrenchDeckDealerTests
    {
        [Fact]
        public void Can_Deal_Specific_Value()
        {
            var dealer = FrenchDeckDealer.WithFullDeck();

            var success = dealer.TryDealCardOfValue(7, out var card);

            success.Should().BeTrue();
            card.Symbol.Should().Be(Symbol.Seven);
        }
        
        [Fact]
        public void Can_Deal_Specific_Symbol()
        {
            var dealer = FrenchDeckDealer.WithFullDeck();

            var success = dealer.TryDealCardOfSymbol(Symbol.Ace, out var card);

            success.Should().BeTrue();
            card.Symbol.Should().Be(Symbol.Ace);
        }
        
        [Fact]
        public void Can_Deal_Specific_Suit()
        {
            var dealer = FrenchDeckDealer.WithFullDeck();

            var success = dealer.TryDealCardOfSuit(Suit.Diamonds, out var card);

            success.Should().BeTrue();
            card.Suit.Should().Be(Suit.Diamonds);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(9)]
        [InlineData(14)]
        public void Can_Deal_Exactly_Four_Cards_Of_Given_Value(int value)
        {
            var dealer = FrenchDeckDealer.WithFullDeck();
            Enumerable.Repeat(true, 4)
                .ForEach(__ =>
                {
                    var success = dealer.TryDealCardOfValue(value, out _);
                    success.Should().BeTrue();
                });

            var success = dealer.TryDealCardOfValue(value, out _);

            success.Should().BeFalse();
        }
        
        
        [Theory]
        [InlineData(Suit.Diamonds)]
        [InlineData(Suit.Hearts)]
        public void Can_Deal_Exactly_Nine_Cards_Of_Given_Suit_From_A_Short_Deck(Suit suit)
        {
            var dealer = FrenchDeckDealer.WithShortDeck();
            Enumerable.Repeat(true, 9)
                .ForEach(__ =>
                {
                    var success = dealer.TryDealCardOfSuit(suit, out _);
                    success.Should().BeTrue();
                });

            var success = dealer.TryDealCardOfSuit(suit, out _);

            success.Should().BeFalse();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Short_Deck_Dealer_Can_Not_Deal_Cards_Missing_From_Full_Deck(int value)
        {
            var dealer = FrenchDeckDealer.WithShortDeck();

            var success = dealer.TryDealCardOfValue(value, out _);

            success.Should().BeFalse();
        }
    }
}
