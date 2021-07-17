using System;

namespace CardGames.Core.French.Cards.Extensions
{
    public static class MappingExtensions
    {
        public static Symbol ToSymbol(this int value)
            => value switch
                {
                    2 => Symbol.Deuce,
                    3 => Symbol.Three,
                    4 => Symbol.Four,
                    5 => Symbol.Five,
                    6 => Symbol.Six,
                    7 => Symbol.Seven,
                    8 => Symbol.Eight,
                    9 => Symbol.Nine,
                    10 => Symbol.Ten,
                    11 => Symbol.Jack,
                    12 => Symbol.Queen,
                    13 => Symbol.King,
                    14 => Symbol.Ace,
                    _ => throw new ArgumentException($"{value} is not a valid card value.")
                };
    }
}
