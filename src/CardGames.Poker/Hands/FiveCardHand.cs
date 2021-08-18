using CardGames.Core.French.Cards.Extensions;
using CardGames.Core.French.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Poker.Hands
{
    public abstract class FiveCardHand : IComparable<FiveCardHand>
    {
        public IReadOnlyCollection<Card> Cards { get; }
        public long HandStrength { get; }
        public HandType HandType { get; }

        public static bool operator >(FiveCardHand thisHand, FiveCardHand otherHand)
            => thisHand.CompareTo(otherHand) > 0;

        public static bool operator <(FiveCardHand thisHand, FiveCardHand otherHand)
            => thisHand.CompareTo(otherHand) < 0;

        public int CompareTo(FiveCardHand other)
            => HandStrength.CompareTo(other.HandStrength);

        public override string ToString()
            => Cards.ToStringRepresentation();

        protected FiveCardHand(IReadOnlyCollection<Card> cards)
        {
            if (cards.Count != 5)
            {
                throw new ArgumentException("A five card hand requires exactly 5 cards!");
            }

            Cards = cards.ByDescendingValue();
            HandType = DetermineHandType();
            HandStrength = HandStrengthCalculation();
        }

        protected abstract long HandStrengthCalculation();

        private HandType DetermineHandType()
        {
            var numberOfDistinctValues = Cards.DistinctValues().Count;
            return numberOfDistinctValues == 5
                ? HandTypeOfDistinctValueHand()
                : HandTypeOfDuplicateValueHand(numberOfDistinctValues);
        }

        private HandType HandTypeOfDuplicateValueHand(int numberOfDistinctValues)
             => numberOfDistinctValues switch
             {
                 4 => HandType.OnePair,
                 3 => Cards.ValueOfBiggestTrips() > 1
                         ? HandType.Trips
                         : HandType.TwoPair,
                 2 => Cards.Count(card => card.Value != Cards.ValueOfBiggestTrips()) == 2
                         ? HandType.FullHouse
                         : HandType.Quads,
                 _ => throw new ArgumentException()
             };

        private HandType HandTypeOfDistinctValueHand()
        {
            var isStraight = IsStraight();
            var isFlush = IsFlush();

            return isStraight && isFlush
                ? HandType.StraightFlush
                : isStraight
                    ? HandType.Straight
                    : isFlush
                        ? HandType.Flush
                        : HandType.Highcard;
        }

        private bool IsFlush() => Cards.Suits().Count == 1;

        private bool IsStraight()
        {
            var values = Cards.DistinctDescendingValues();
            return values.Count == 5 && values.Max() - values.Min() == 4;
        }
    }
}
