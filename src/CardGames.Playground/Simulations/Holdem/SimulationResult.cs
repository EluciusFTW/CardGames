using System.Collections.Generic;
using System.Linq;
using CardGames.Poker.Hands.CommunityCardHands;
using CardGames.Poker.Hands.HandTypes;
using static CardGames.Playground.Simulations.Holdem.HoldemSimulation;

namespace CardGames.Playground.Simulations.Holdem
{
    public class SimulationResult
    {
        private readonly int _nrOfHands;

        public SimulationResult(int nrOfHands, IReadOnlyCollection<IDictionary<string, HoldemHand>> hands)
        {
            _nrOfHands = nrOfHands;
            Hands = hands;
        }

        public IReadOnlyCollection<IDictionary<string, HoldemHand>> Hands { get; init; }

        public IEnumerable<string> GetPlayers => Hands.First().Keys;

        public IEnumerable<(string Name, int Wins, decimal WinPercentage)> GroupByWins()
            => Hands
                .Select(playerHands => playerHands.OrderByDescending(hand => hand.Value.Strength).First())
                .GroupBy(playerHands => playerHands.Key)
                .Select(grp => (grp.Key, grp.Count(), (decimal)grp.Count() / _nrOfHands))
                .OrderBy(res => res.Item2);

        public IDictionary<string, IEnumerable<(HandType Type, int Occurences, decimal Frequency)>> AllMadeHandDistributions()
            => GetPlayers
                .ToDictionary(player => player, player => MadeHandDistributionOf(player));

        public IEnumerable<(HandType Type, int Occurences, decimal Frequency)> MadeHandDistributionOf(string name)
            => Hands
                .Select(playerHands => playerHands[name].Type)
                .GroupBy(type => type)
                .Select(grp => (grp.Key, grp.Count(), (decimal)grp.Count() / _nrOfHands));

    }

    public class SimulationResultEncapsulated
    {
        private readonly int _nrOfHands;

        public IReadOnlyCollection<Hands> Hands { get; }

        public SimulationResultEncapsulated(int nrOfHands, IReadOnlyCollection<Hands> hands)
        {
            _nrOfHands = nrOfHands;
            Hands = hands;
        }

        public IEnumerable<(string Name, int Wins, decimal WinPercentage)> GroupByWins()
            => Hands
                .Where(hand => !hand.Tie())
                .GroupBy(hand => hand.GetWinner())
                .Select(grp => (grp.Key, grp.Count(), (decimal)grp.Count() / _nrOfHands));

        public IEnumerable<string> GetPlayers => Hands.First().GetPlayers();

        public IDictionary<string, IEnumerable<(HandType Type, int Occurences, decimal Frequency)>> AllMadeHandDistributions()
            => GetPlayers
                .ToDictionary(player => player, player => MadeHandDistributionOf(player));

        public IEnumerable<(HandType Type, int Occurences, decimal Frequency)> MadeHandDistributionOf(string name)
            => Hands
                .Select(hands => hands.HandOfPlayer(name).Type)
                .GroupBy(type => type)
                .Select(grp => (grp.Key, grp.Count(), (decimal)grp.Count() / _nrOfHands));
    }
}
