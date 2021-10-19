using CardGames.Core.French.Cards.Extensions;
using CardGames.Core.Extensions;
using System;
using System.Linq;
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

            Console.ReadKey();
        }

        private static HoldemSimulationResult RunHoldemSimulation(int nrOfHands)
           => new HoldemSimulation()
               .WithPlayer("John", "Js Jd".ToCards())
               .WithPlayer("Jeremy", "8s 6d".ToCards())
               .WithPlayer("Jarvis", "Ad".ToCards())
               .WithFlop("8d 8h 4d".ToCards())
               .SimulateWithFullDeck(nrOfHands);

        private static StudSimulationResult RunStudSimulation(int nrOfHands)
            => new SevenCardStudSimulation()
                .WithPlayer(
                    new StudPlayer("John")
                        .WithHoleCards("Js Jd".ToCards())
                        .WithBoardCards("Qc".ToCards()))
                .WithPlayer(
                    new StudPlayer("Jeremy")
                        .WithHoleCards("3s 4s".ToCards())
                        .WithBoardCards("7s".ToCards()))
                .WithPlayer(
                    new StudPlayer("Jarvis")
                        .WithBoardCards("Tc".ToCards()))
                .WithDeadCards("Qs Qh Jc 6s".ToCards())
                .Simulate(nrOfHands);

        private static void PrintWinPercentages(StudSimulationResult result)
            => result
                .GroupByWins()
                .OrderByDescending(player => player.WinPercentage)
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
