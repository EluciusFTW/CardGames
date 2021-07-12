using EluciusFTW.CardGames.Core;
using EluciusFTW.CardGames.Core.Cards.Deck;
using EluciusFTW.CardGames.Core.Random;

namespace EluciusFTW.CardGames.Core.Tests.TestImplementations
{
    internal class TestDealer : Dealer<TestCard>
    {
        public TestDealer(IDeck<TestCard> deck, IRandomNumberGenerator numberGenerator) 
            : base(deck, numberGenerator)
        {
        }
    }
}
