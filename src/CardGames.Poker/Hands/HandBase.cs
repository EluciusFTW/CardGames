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
        public long Strength { get; }
        public HandType Type { get; }
        public abstract HandTypeStrengthRanking Ranking { get; }
        public abstract IEnumerable<IReadOnlyCollection<Card>> PossibleHands();

        public HandBase(IReadOnlyCollection<Card> cards)
        {
            if (cards.Count < 5)
            {
                throw new ArgumentException("A poker hand needs at least five cards");
            }
            Cards = cards;

            var handsAndTypes = PossibleHands()
                .Select(hand => new { hand, type = HandTypeDetermination.DetermineHandType(hand) });
            Type = HandStrength.GetEffectiveType(handsAndTypes.Select(pair => pair.type), Ranking);
            Strength = handsAndTypes
                .Where(pair => pair.type == Type)
                .Select(pair => pair.hand)
                .Select(hand => HandStrength.Classic(hand, Type))
                .Max();
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