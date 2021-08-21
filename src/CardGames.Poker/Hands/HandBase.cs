using CardGames.Core.French.Cards;
using CardGames.Core.French.Cards.Extensions;
using System;
using System.Collections.Generic;
using CardGames.Poker.Hands.HandTypes;

namespace CardGames.Poker.Hands
{
    public abstract class HandBase : IComparable<HandBase>
    {
        public IReadOnlyCollection<Card> Cards { get; }
        public abstract long HandStrength { get; }
        public abstract HandType HandType { get; }

        public HandBase(IReadOnlyCollection<Card> cards)
        {
            Cards = cards;
        }

        public static bool operator >(HandBase thisHand, HandBase otherHand)
            => thisHand.CompareTo(otherHand) > 0;

        public static bool operator <(HandBase thisHand, HandBase otherHand)
            => thisHand.CompareTo(otherHand) < 0;

        public int CompareTo(HandBase other)
            => HandStrength.CompareTo(other.HandStrength);

        public override string ToString()
            => Cards.ToStringRepresentation();
    }
}