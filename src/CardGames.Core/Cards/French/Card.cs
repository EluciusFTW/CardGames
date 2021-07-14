using CardGames.Core.Cards.French.Extensions;

namespace CardGames.Core.Cards.French
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
