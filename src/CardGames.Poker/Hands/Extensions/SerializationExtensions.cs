using CardGames.Poker.Hands.DrawHands;
using CardGames.Poker.Hands.HandTypes;
using System.Linq;

namespace CardGames.Poker.Hands.Extensions
{
    public static class SerializationExtensions
    {
        public static string HandStrengthStringRepresentation(this FiveCardHand hand)
            => hand.Type switch
            {
                HandType.HighCard => hand.Cards.First().Value + "Low",
                HandType.OnePair => "One Pair",
                HandType.TwoPair => "Two Pair",
                HandType.Trips => "Trips",
                HandType.Straight => "Straight",
                HandType.Flush => "Flush",
                HandType.FullHouse => "Full House",
                HandType.Quads => "Quads",
                HandType.StraightFlush => "Straight Flush",
                _ => "Incomplete Hand",
            };
    }
}
