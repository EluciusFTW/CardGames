using BenchmarkDotNet.Attributes;
using CardGames.Poker.Simulations.Holdem;
using CardGames.Core.French.Cards.Extensions;

namespace CardGames.Poker.Benchmarks;

[MemoryDiagnoser]
public class HoldemSimulationBenchmarks
{
    [Benchmark]
    public HoldemSimulationResult Holdem_HeadsUp_100_Hands()
        => HeadsUpSimulation().SimulateWithFullDeck(100);

    [Benchmark]
    public HoldemSimulationResult Holdem_HeadsUp_1000_Hands()
        => HeadsUpSimulation().SimulateWithFullDeck(1000);

    [Benchmark]
    public HoldemSimulationResult Holdem_FiveHanded_100_Hands()
        => FiveHandedSimulation().SimulateWithFullDeck(100);

    [Benchmark]
    public HoldemSimulationResult Holdem_FiveHanded_1000_Hands()
        => FiveHandedSimulation().SimulateWithFullDeck(1000);

    private HoldemSimulation HeadsUpSimulation()
        => new HoldemSimulation()
            .WithPlayer("James", "8s 6d".ToCards())
            .WithPlayer("Jimmy", "Ad Kd".ToCards())
            .WithFlop("3h 6c Qd".ToCards());

    private HoldemSimulation FiveHandedSimulation()
        => new HoldemSimulation()
            .WithPlayer("James", "8s 6d".ToCards())
            .WithPlayer("Jimmy", "Ad Kd".ToCards())
            .WithFlop("3h 6c Qd".ToCards());
}
