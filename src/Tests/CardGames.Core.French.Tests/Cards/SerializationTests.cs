using CardGames.Core.French.Cards;
using CardGames.Core.French.Cards.Extensions;
using FluentAssertions;
using Xunit;

namespace CardGames.Core.French.Tests.Cards;

public class SerializationTests
{
    [Theory]
    [InlineData(Suit.Hearts, Symbol.Deuce, "2h")]
    [InlineData(Suit.Hearts, Symbol.Jack, "Jh")]
    [InlineData(Suit.Spades, Symbol.Ace, "As")]
    [InlineData(Suit.Clubs, Symbol.King, "Kc")]
    [InlineData(Suit.Diamonds, Symbol.Ten, "Td")]
    public void Serializes_Card_Correctly(Suit suit, Symbol symbol, string expected)
    {
        var label = new Card(suit, symbol).ToShortString();

        label.Should().Be(expected);
    }

    [Theory]
    [InlineData("3h", Suit.Hearts, Symbol.Three)]
    [InlineData("8s", Suit.Spades, Symbol.Eight)]
    [InlineData("Kc", Suit.Clubs, Symbol.King)]
    public void Deserializes_Card_Correctly(string expression, Suit expectedSuit, Symbol expectedSymbol)
    {
        var card = expression.ToCard();
        
        card.Suit.Should().Be(expectedSuit);
        card.Symbol.Should().Be(expectedSymbol);
    }
    
    [Fact]
    public void Deserializes_Multiple_Cards_Correctly()
    {
        var expectedCards = new[]
        {
            new Card(Suit.Hearts, Symbol.Deuce),
            new Card(Suit.Hearts, Symbol.Seven),
            new Card(Suit.Spades, Symbol.Jack),
            new Card(Suit.Clubs, Symbol.King)
        };
        
        var cards = "2h 7h Js Kc".ToCards();
        
        cards.Should().BeEquivalentTo(expectedCards);
    }
}
