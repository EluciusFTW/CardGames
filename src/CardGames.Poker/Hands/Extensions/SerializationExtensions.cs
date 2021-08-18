using System.Linq;

namespace CardGames.Poker.Hands.Extensions
{
    public static class SerializationExtensions
    {
        public static string HandStrengthStringRepresentation(this FiveCardHand hand)
            => hand.HandType switch
            {
                HandType.Highcard => hand.Cards.First().Value + "Low",
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
