using System;
using BenchmarkDotNet.Running;
using CardGames.Core.Benchmarks;

BenchmarkRunner.Run<FrenchBenchmarks>();

Console.ReadKey();