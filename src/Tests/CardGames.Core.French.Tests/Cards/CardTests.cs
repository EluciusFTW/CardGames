using CardGames.Core.French.Cards;
using FluentAssertions;
using Xunit;

namespace CardGames.Core.French.Tests.Cards
{
    public class CardTests
    {
        [Theory]
        [InlineData(2, Symbol.Deuce)]
        [InlineData(9, Symbol.Nine)]
        [InlineData(11, Symbol.Jack)]
        [InlineData(14, Symbol.Ace)]
        public void Determines_Symbol_From_Value(int value, Symbol expectedSymbol)
        {
            var card = new Card(Suit.Hearts, value);

            card.Symbol.Should().Be(expectedSymbol);
        }

        [Theory]
        [InlineData(Symbol.Deuce, 2)]
        [InlineData(Symbol.Nine, 9)]
        [InlineData(Symbol.Jack, 11)]
        [InlineData(Symbol.Ace, 14)]
        public void Determines_Value_From_Symbol(Symbol symbol, int expectedValue)
        {
            var card = new Card(Suit.Hearts, symbol);

            card.Value.Should().Be(expectedValue);
        }
    }
}
