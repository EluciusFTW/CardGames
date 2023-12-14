using BenchmarkDotNet.Attributes;
using CardGames.Core.French.Cards.Extensions;

namespace CardGames.Core.Benchmarks;

[MemoryDiagnoser]
public class FrenchBenchmarks
{
    [Benchmark]
    public void ToCards_Obsolete_Method()
    {
        "2h 7c Ks 9d 3c".ToCardsObsolete();
    }

    [Benchmark]
    public void ToCards_Using_Spans()
    {
        "2h 7c Ks 9d 3c".ToCards();
    }

    [Benchmark]
    public void ToCard_Obsolete_Method()
    {
        "2h".ToCardObsolete();
    }

    [Benchmark]
    public void ToCard_Using_Spans()
    {
        "2h".ToCard();
    }
}
