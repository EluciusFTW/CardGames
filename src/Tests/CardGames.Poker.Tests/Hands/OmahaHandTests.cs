using CardGames.Poker.Hands;
using CardGames.Poker.Hands.HandTypes;
using CardGames.Core.French.Cards.Extensions;
using Xunit;
using FluentAssertions;
using CardGames.Poker.Hands.CommunityCardHands;

namespace CardGames.Poker.Tests.Hands
{
    public class OmahaHandTests
    {
        [Theory]
        [InlineData("2s 5d 6d Qh", "8d Js Kc 5c 5h", HandType.Trips)]
        [InlineData("2s 5d 5h Qh", "8d Js Kc 7c 6h", HandType.OnePair)]
        public void Determines_Hand_Type(string holeCards, string boardCards, HandType expectedHandType)
        {
            var hand = new OmahaHand(holeCards.ToCards(), boardCards.ToCards());

            hand.Type.Should().Be(expectedHandType);
        }

        [Theory]
        [InlineData("2s 5d 6d Qh", "8d Js Kc 5c")]
        [InlineData("2s 5d Qd As", "8d Qs Kc Ah")]
        public void Determines_Winner(string holeCardsOne, string holeCardsTwo)
        {
            var board = "Ac Ad Jd 6c 3s".ToCards();
            var handOne = new OmahaHand(holeCardsOne.ToCards(), board);
            var handTwo = new OmahaHand(holeCardsTwo.ToCards(), board);

            (handOne < handTwo).Should().BeTrue();
        }
    }
}
