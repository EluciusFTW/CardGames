using BenchmarkDotNet.Attributes;
using CardGames.Core.French.Cards.Extensions;
using CardGames.Playground.Simulations.Holdem;
using CardGames.Poker.Simulations.Holdem;

namespace CardGames.Playground.Runner
{
    [MemoryDiagnoser]
    public class HoldemSimulationBenchmarks
    {
        [Benchmark]
        public HoldemSimulationResult HoldemHeadsUp100Hands()
            => new HoldemSimulation()
                .WithPlayer("James", "8s 6d".ToCards())
                .WithPlayer("Jimmy", "Ad Kd".ToCards())
                .WithFlop("3h 6c Qd".ToCards())
                .SimulateWithFullDeck(100);

        [Benchmark]
        public HoldemSimulationResult HoldemFiveHanded100Hands()
            => new HoldemSimulation()
                .WithPlayer("Jill", "Qh Td".ToCards())
                .WithPlayer("Jonas", "8s 9d".ToCards())
                .WithPlayer("James", "2h 2c".ToCards())
                .WithPlayer("John", "Ks Js".ToCards())
                .WithPlayer("Jimmy", "Ad Kd".ToCards())
                .SimulateWithFullDeck(100);
    }
}
