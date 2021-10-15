using CardGames.Core.French.Cards.Extensions;
using CardGames.Core.Extensions;
using System;
using CardGames.Playground.Simulations.Holdem;
using CardGames.Playground.Simulations.Stud;

namespace CardGames.Playground.Runner
{

    public class Program
    {
        static void Main()
        {
            
            var results = RunStudSimulation(10000);

            PrintWinPercentages(results);
            PrintHandDistributions(results);

            // var results = RunHoldemSimulation(10000);
            // BenchmarkRunner.Run<HoldemSimulations>();

            Console.ReadKey();
        }
               
        private static HoldemSimulationResult RunHoldemSimulation(int nrOfHAnds)
            => new HoldemSimulation()
                .WithPlayer("Stefan", "Js Jd".ToCards())
                .WithPlayer("Matthias", "8s 6d".ToCards())
                .WithPlayer("Guy", "Ad Kd".ToCards())
                .WithFlop("8d 8h 4d".ToCards())
                .SimulateWithFullDeck(nrOfHAnds);

        private static StudSimulationResult RunStudSimulation(int nrOfHAnds)
            => new SevenCardStudSimulation()
                .WithPlayer(
                    new StudPlayer("Stefan")
                        .WithHoleCards("Js Jd".ToCards())
                        .WithBoardCards("Qc".ToCards()))
                .WithPlayer(
                    new StudPlayer("Matthias")
                        .WithHoleCards("3s 4s".ToCards())
                        .WithBoardCards("7s".ToCards()))
                .WithPlayer(
                    new StudPlayer("Guy")
                        .WithHoleCards("Ad Kd".ToCards())
                        .WithBoardCards("Tc".ToCards()))
                .Simulate(nrOfHAnds);

        private static void PrintWinPercentages(StudSimulationResult result)
            => result
                .GroupByWins()
                .ForEach(player =>
                {
                    Console.WriteLine($"{player.Name} won {player.Wins} hands => {player.WinPercentage:P2}");
                });

        private static void PrintHandDistributions(StudSimulationResult results)
            => results
                .AllMadeHandDistributions()
                .ForEach(distribution =>
                {
                    Console.WriteLine();
                    Console.WriteLine($"{distribution.Key} made:");
                    distribution.Value.ForEach(typeDistribution =>
                    {
                        Console.WriteLine($" * {typeDistribution.Type} - {typeDistribution.Occurences} times ({typeDistribution.Frequency:P2})");
                    });
                });

    }
}
