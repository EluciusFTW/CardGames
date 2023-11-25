using System;
using BenchmarkDotNet.Running;

namespace CardGames.Poker.Benchmarks;

public class Program
{
    static void Main()
    {
        BenchmarkRunner.Run<HoldemSimulationBenchmarks>();
        Console.ReadKey();
    }
}
