using System;
using EluciusFTW.CardGames.Core.Tests.TestImplementations;
using EluciusFTW.CardGames.Core.Cards.Deck;
using EluciusFTW.CardGames.Core.Random;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EluciusFTW.CardGames.Core.Tests.Dealer
{
    public class DealerTests
    {
        private readonly TestDealer _dealer;
        private readonly IDeck<TestCard> _deck;
        private readonly IRandomNumberGenerator _numberGenerator;

        public DealerTests()
        {
            _deck = Substitute.For<IDeck<TestCard>>();
            _numberGenerator = Substitute.For<IRandomNumberGenerator>();

            _dealer = new TestDealer(_deck, _numberGenerator);
        }

        [Fact]
        public void Shuffle_Resets_The_Deck()
        {
            _dealer.Shuffle();

            _deck.Received(1).Reset();
        }

        [Fact]
        public void Deals_The_Card_Of_Index_Given_By_The_Number_Generator()
        {
            var expectedCard = new TestCard(777);
            _deck
                .NumberOfCardsLeft()
                .Returns(15);
            _numberGenerator
                .Next(15)
                .Returns(12);
            _deck
                .GetFromRemaining(12)
                .Returns(expectedCard);
            
            var card = _dealer.DealCard();

            card.Should().Be(expectedCard);
        }

        [Fact]
        public void Can_Not_Deal_A_Card_When_The_deck_Is_Empty()
        {
            _deck
                .NumberOfCardsLeft()
                .Returns(0);

            Action deal = () => _dealer.DealCard();

            deal
                .Should().Throw<InvalidOperationException>()
                .WithMessage("There are no more cards in the deck to deal.");
        }

        [Fact]
        public void Can_Deal_Several_Cards_At_Once()
        {
            var expectedCard = new TestCard(777);
            _deck
                .NumberOfCardsLeft()
                .Returns(21, 20, 19);
            _numberGenerator
                .Next(Arg.Any<int>())
                .Returns(x => (int)x[0]);
            _deck
                .GetFromRemaining(Arg.Any<int>())
                .Returns(x => new TestCard((int)x[0]));

            var cards = _dealer.DealCards(3);

            cards
                .Should()
                .BeEquivalentTo(new[] { new TestCard(21), new TestCard(20), new TestCard(19) });
        }
    }
}
