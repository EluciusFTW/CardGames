using CardGames.Core.Dealer;
using CardGames.Core.Deck;
using CardGames.Core.Random;

namespace CardGames.Core.Tests.__TestImplementations
{
    internal class TestDealer : Dealer<TestCard>
    {
        public TestDealer(IDeck<TestCard> deck, IRandomNumberGenerator numberGenerator) 
            : base(deck, numberGenerator)
        {
        }
    }
}
