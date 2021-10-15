using CardGames.Poker.Hands.HandTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Poker.Hands.Strength
{
    public static class HandTypeStrength
    {
        public static int ByRanking(HandTypeStrengthRanking ranking, HandType type)
            => ranking switch
            {
                HandTypeStrengthRanking.Classic => Classic(type),
                HandTypeStrengthRanking.ShortDeck => ShortDeck(type),
                _ => throw new NotImplementedException("The ranking is not implemented")
            };

        public static int Classic(HandType type)
            => type switch
            {
                HandType.Highcard => 0,
                HandType.OnePair => 1,
                HandType.TwoPair => 2,
                HandType.Trips => 3,
                HandType.Straight => 4,
                HandType.Flush => 5,
                HandType.FullHouse => 6,
                HandType.Quads => 7,
                HandType.StraightFlush => 8,
                _ => -1,
            };

        public static int ShortDeck(HandType type)
            => type switch
            {
                HandType.Highcard => 0,
                HandType.OnePair => 1,
                HandType.TwoPair => 2,
                HandType.Trips => 3,
                HandType.Straight => 4,
                HandType.Flush => 6,
                HandType.FullHouse => 5,
                HandType.Quads => 7,
                HandType.StraightFlush => 8,
                _ => -1,
            };

        public static HandType GetEffectiveForClassicOrder(IReadOnlyCollection<HandType> handTypes)
            => GetEffective(handTypes, Classic);

        public static HandType GetEffectiveForShortDeckOrder(IReadOnlyCollection<HandType> handTypes)
            => GetEffective(handTypes, ShortDeck);

        public static HandType GetEffective(IReadOnlyCollection<HandType> handTypes, Func<HandType, int> handTypeStrengthMap)
            => handTypes
                .Select(type => new { type, Value = handTypeStrengthMap(type) })
                .OrderByDescending(pair => pair.Value)
                .First().type;
    }
}
