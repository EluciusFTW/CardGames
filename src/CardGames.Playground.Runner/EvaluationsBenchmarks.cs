using BenchmarkDotNet.Attributes;
using CardGames.Poker.Simulations.Holdem;
using CardGames.Poker.Evaluation;

namespace CardGames.Poker.Benchmarks;

[MemoryDiagnoser]
public class EvaluationsBenchmarks
{
    [Params(100, 1000)]
    public int NumberOfHands;

    private HoldemSimulationResult _headsUpResult;
    private HoldemSimulationResult _fiveHandedResult;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _headsUpResult = HoldemSimulations
            .HeadsUp()
            .SimulateWithFullDeck(NumberOfHands);
        _fiveHandedResult = HoldemSimulations
            .FiveHanded()
            .SimulateWithFullDeck(NumberOfHands);
    }

    [Benchmark, BenchmarkCategory("HeadsUp")]
    public void EvaluateEquity_HeadsUp()
    {
        HandsEvaluation.GroupByWins(_headsUpResult.Hands);
    }

    [Benchmark, BenchmarkCategory("HeadsUp")]
    public void EvaluateMadeHandDistribution_HeadsUp()
    {
        HandsEvaluation.AllMadeHandDistributions(_headsUpResult.Hands);
    }

    [Benchmark, BenchmarkCategory("FiveHanded")]
    public void EvaluateEquity_FiveHanded()
    {
        HandsEvaluation.GroupByWins(_headsUpResult.Hands);
    }

    [Benchmark, BenchmarkCategory("FiveHanded")]
    public void EvaluateMadeHandDistribution_FiveHanded()
    {
        HandsEvaluation.AllMadeHandDistributions(_fiveHandedResult.Hands);
    }
}
