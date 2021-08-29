using CardGames.Poker.Hands;
using CardGames.Poker.Hands.HandTypes;
using CardGames.Core.French.Cards.Extensions;
using Xunit;
using FluentAssertions;

namespace CardGames.Poker.Tests.Hands
{
    public class DrawHandTests
    {

        [Theory]
        [InlineData("2s 3s 7h 9h Kd", HandType.Highcard)]
        [InlineData("Td 3s 7h 3h Kd", HandType.OnePair)]
        [InlineData("2s Js Jh Kh Kd", HandType.TwoPair)]
        [InlineData("2s 9s 9h 9c Kd", HandType.Trips)]
        [InlineData("Jd 8s 7h 9h Tc", HandType.Straight)]
        [InlineData("2d 3d 7d 9d Kd", HandType.Flush)]
        [InlineData("Ks 9s 9h 9c Kd", HandType.FullHouse)]
        [InlineData("2s 9s 9h 9c 9d", HandType.Quads)]
        [InlineData("2s 3s 4s 5s 6s", HandType.StraightFlush)]
        public void Determines_Hand_Type(string cardString, HandType expectedHandType)
        {
            var hand = new DrawHand(cardString.ToCards());

            hand.Type.Should().Be(expectedHandType);
        }
    }
}
