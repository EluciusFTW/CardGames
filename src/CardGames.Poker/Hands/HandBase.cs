using CardGames.Core.French.Cards;
using CardGames.Core.French.Cards.Extensions;
using System;
using System.Collections.Generic;
using CardGames.Poker.Hands.HandTypes;
using CardGames.Poker.Hands.Strength;
using System.Linq;

namespace CardGames.Poker.Hands
{
    public abstract class HandBase : IComparable<HandBase>
    {
        public IReadOnlyCollection<Card> Cards { get; }

        private long _strength;
        public long Strength => _strength != default
            ? _strength
            : _strength = CalculateStrength();

        private HandType _type;
        public HandType Type => _type != HandType.Incomplete
            ? _type
            : _type = DetermineType();


        public abstract HandTypeStrengthRanking Ranking { get; }
        public abstract IEnumerable<IReadOnlyCollection<Card>> PossibleHands();

        public HandBase(IReadOnlyCollection<Card> cards)
        {
            if (cards.Count < 5)
            {
                throw new ArgumentException("A poker hand needs at least five cards");
            }
            Cards = cards;
        }

        protected virtual long CalculateStrength()
        {
            var handsAndTypes = PossibleHands()
                .Select(hand => new { hand, type = HandTypeDetermination.DetermineHandType(hand) });
            
            return handsAndTypes
                .Where(pair => pair.type == Type)
                .Select(pair => pair.hand)
                .Select(hand => HandStrength.Classic(hand, Type))
                .Max();
        }

        protected virtual HandType DetermineType()
        {
            var handsAndTypes = PossibleHands()
                .Select(hand => new { hand, type = HandTypeDetermination.DetermineHandType(hand) });
            
            return HandStrength.GetEffectiveType(handsAndTypes.Select(pair => pair.type), Ranking);
        }

        public static bool operator >(HandBase thisHand, HandBase otherHand)
            => thisHand.CompareTo(otherHand) > 0;

        public static bool operator <(HandBase thisHand, HandBase otherHand)
            => thisHand.CompareTo(otherHand) < 0;

        public int CompareTo(HandBase other)
            => Strength.CompareTo(other.Strength);

        public override string ToString()
            => Cards.ToStringRepresentation();
    }
}