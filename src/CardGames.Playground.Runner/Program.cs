using BenchmarkDotNet.Running;
using System;

namespace CardGames.Playground.Runner
{

    public class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<HeadsUpSimulations>();
            Console.ReadKey();
        }
    }
}
