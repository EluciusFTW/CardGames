using EluciusFTW.CardGames.Core.Cards.French;
using EluciusFTW.CardGames.Core.Dealer;
using EluciusFTW.CardGames.Core.Extensions;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace EluciusFTW.CardGames.Core.Tests.Dealer
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
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Short_Deck_Dealer_Can_Not_Deal_Removed_Cards(int value)
        {
            var dealer = FrenchDeckDealer.WithShortDeck();

            var success = dealer.TryDealCardOfValue(value, out _);

            success.Should().BeFalse();
        }
    }
}
