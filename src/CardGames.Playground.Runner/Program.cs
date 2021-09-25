﻿using CardGames.Core.French.Cards.Extensions;
using CardGames.Core.Extensions;
using System;
using CardGames.Playground.Simulations.Holdem;

namespace CardGames.Playground.Runner
{

    public class Program
    {
        static void Main()
        {
            var results = RunHoldemSimulation(10000);

            PrintWinPercentages(results);
            PrintHandDistributions(results);

            Console.ReadKey();
        }

        private static SimulationResult RunHoldemSimulation(int nrOfHAnds)
            => new HoldemSimulation()
                .WithPlayer("Stefan", "Js Jd".ToCards())
                .WithPlayer("Matthias", "8s 6d".ToCards())
                .WithPlayer("Guy", "Ad Kd".ToCards())
                .WithFlop("8d 8c 4d".ToCards())
                .SimulateWithFullDeck(nrOfHAnds);

        private static void PrintWinPercentages(SimulationResult result)
            => result
                .GroupByWins()
                .ForEach(player =>
                {
                    Console.WriteLine($"{player.Name} won {player.Wins} hands => {player.WinPercentage:P2}");
                });

        private static void PrintHandDistributions(SimulationResult results)
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
