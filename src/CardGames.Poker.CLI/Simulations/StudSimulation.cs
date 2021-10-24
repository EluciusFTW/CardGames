using CardGames.Playground.Simulations.Stud;
using Spectre.Console.Cli;
using CardGames.Core.Extensions;
using System.Linq;
using System;
using Spectre.Console;
using CardGames.Poker.CLI.Logging;

namespace CardGames.Poker.CLI
{
    internal class StudSimulation : Command<SimulationSettings>
    {
        private static readonly SpectreLogger Logger = new();

        public override int Execute(CommandContext context, SimulationSettings settings)
        {
            Logger.LogApplicationStart();
            var simulation = new SevenCardStudSimulation();

            do
            {
                simulation.WithPlayer(GetStudPlayer());
            }
            while (AnsiConsole.Confirm("Do you want to add another player?"));

            Logger.Paragraph("Other configuration");
            var deadCards = Prompt.PromptForCards("Dead cards:");

            var numberOfHands = settings.NumberOfhands == default
                ? AnsiConsole.Ask<int>("How many hands?")
                : settings.NumberOfhands;

            var results = simulation
                .WithDeadCards(deadCards)
                .Simulate(numberOfHands);

            PrintResults(results);

            return 0;
        }

        private static StudPlayer GetStudPlayer()
        {
            Logger.Paragraph("Add Player");

            var playerName = AnsiConsole.Ask<string>("Player Name: ");
            var holeCards = Prompt.PromptForCards("Hole Cards: ");
            var openCards = Prompt.PromptForCards("Board Cards: ");

            return new StudPlayer(playerName)
                .WithHoleCards(holeCards)
                .WithBoardCards(openCards);
        }

        private void PrintResults(StudSimulationResult results)
        {
            Logger.Headline("Wins");
            PrintWinPercentages(results);

            Logger.Headline("Made hand distributions");
            PrintHandDistributions(results);
        }

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