using CardGames.Core.French.Cards.Extensions;

namespace CardGames.Core.French.Cards
{
    public readonly struct Card
    {
        public Suit Suit { get; }

        public int Value 
            => (int)Symbol;

        public Symbol Symbol { get; }

        public Card(Suit suit, int value)
        {
            Suit = suit;
            Symbol = value.ToSymbol();
        }

        public Card(Suit suit, Symbol symbol)
        {
            Suit = suit;
            Symbol = symbol;
        }

        public override string ToString() 
            => this.ToShortString();
    }
}
