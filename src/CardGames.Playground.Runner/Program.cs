using System;
using BenchmarkDotNet.Running;
using CardGames.Poker.Benchmarks;

BenchmarkRunner.Run<HoldemSimulationBenchmarks>();
BenchmarkRunner.Run<EvaluationsBenchmarks>();
        
Console.ReadKey();
