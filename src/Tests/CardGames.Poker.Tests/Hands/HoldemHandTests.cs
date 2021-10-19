using CardGames.Poker.Hands.HandTypes;
using CardGames.Core.French.Cards.Extensions;
using Xunit;
using FluentAssertions;
using CardGames.Poker.Hands.CommunityCardHands;

namespace CardGames.Poker.Tests.Hands
{
    public class HoldemHandTests
    {
        [Theory]
        [InlineData("2s 5d", "8d Js Kc 5c 5h", HandType.Trips)]
        [InlineData("2s 2d", "8d Js Kc 5c 5h", HandType.TwoPair)]
        [InlineData("2s 2d", "5d Js Kc 5c 5h", HandType.FullHouse)]
        [InlineData("2s 2h", "5d Jh Kh 6h 5h", HandType.Flush)]
        [InlineData("2s 2c", "7h Jh Kh 6h 5h", HandType.Flush)]
        [InlineData("2s 3h", "8d Jh Td Qh 9c", HandType.Straight)]
        [InlineData("2s 3h", "8d Jh 4d Qh 9c", HandType.Highcard)]
        public void Determines_Hand_Type(string holeCards, string boardCards, HandType expectedHandType)
        {
            var hand = new HoldemHand(holeCards.ToCards(), boardCards.ToCards());

            hand.Type.Should().Be(expectedHandType);
        }
    }
}
