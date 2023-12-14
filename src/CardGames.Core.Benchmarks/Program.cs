using BenchmarkDotNet.Running;
using CardGames.Core.Benchmarks;

BenchmarkRunner.Run<FrenchBenchmarks>();
System.Console.ReadKey();