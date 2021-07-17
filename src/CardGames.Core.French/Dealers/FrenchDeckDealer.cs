using System.Linq;
using CardGames.Core.Dealer;
using CardGames.Core.French.Cards;
using CardGames.Core.French.Decks;
using CardGames.Core.Random;

namespace CardGames.Core.French.Dealers
{
    public class FrenchDeckDealer : Dealer<Card>
    {
        private FrenchDeck SpecificDeck => Deck as FrenchDeck;

        public FrenchDeckDealer(FrenchDeck deck) 
            : base(deck)
        {
        }

        public FrenchDeckDealer(FrenchDeck deck, IRandomNumberGenerator numberGenerator) 
            : base(deck, numberGenerator)
        {
        }

        public static FrenchDeckDealer WithShortDeck() 
            => new(new ShortFrenchDeck());
        
        public static FrenchDeckDealer WithFullDeck() 
            => new(new FullFrenchDeck());

        public bool TryDealCardOfValue(int value, out Card card)
        {
            var availableCards = SpecificDeck
                .CardsLeftOfValue(value)
                .ToArray();

            return TryDealCardFrom(availableCards, out card);
        }
        
        public bool TryDealCardOfSymbol(Symbol symbol, out Card card)
            => TryDealCardOfValue((int)symbol, out card);
        
        public bool TryDealCardOfSuit(Suit suit, out Card card)
        {
            var availableCards = SpecificDeck
                .CardsLeftOfSuit(suit)
                .ToArray();
            
            return TryDealCardFrom(availableCards, out card);
        }
        
        private bool TryDealCardFrom(Card[] cards, out Card card)
        {
            if (!cards.Any())
            {
                card = default;
                return false;
            }

            card = cards[NumberGenerator.Next(cards.Length)];
            _ = Deck.GetSpecific(card);
            return true;
        }
    }
}
