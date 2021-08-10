using CardGames.Core.French.Cards.Extensions;
using System;

namespace CardGames.Core.French.Cards
{
    public readonly struct Card : IEquatable<Card>
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

        // The rest of the struct is solemnly for performance. Although a struct already comes with an implementation of 
        // equality, it is based on a generic implementation using reflection. To acheive (much) better performance, it is 
        // recommended to overwrite to implementation with a specific one, and that results in hte boilderpalte below. See, e.g.,
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/how-to-define-value-equality-for-a-type
        public override bool Equals(object obj)
            => obj is Card other && Equals(other);

        public bool Equals(Card other)
            => Suit == other.Suit && Symbol == other.Symbol;

        public static bool operator ==(Card left, Card right) 
            => left.Equals(right);

        public static bool operator !=(Card left, Card right) 
            => !(left == right);

        // This is the standard mapping of suits and values to 1..52, which is also used in the SerilaizationExtensions.
        public override int GetHashCode()
            => (int)Suit * 13 + Value - 2;
    }
}
