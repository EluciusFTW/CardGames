using CardGames.Core.French.Cards.Extensions;
using System;

namespace CardGames.Core.French.Cards
{
    public class Card : IEquatable<Card>
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

        public override bool Equals(object obj)
            => obj is Card other && Equals(other);

        public bool Equals(Card other) 
            => other is not null 
                && Suit == other.Suit 
                && Symbol == other.Symbol;

        public static bool operator ==(Card left, Card right)
            => left?.Equals(right) ?? false;

        public static bool operator !=(Card left, Card right) 
            => !(left == right);

        // This is the standard mapping of suits and values to 1..52, which is also used in the SerilaizationExtensions.
        public override int GetHashCode()
            => (int)Suit * 13 + Value - 2;
    }
}
