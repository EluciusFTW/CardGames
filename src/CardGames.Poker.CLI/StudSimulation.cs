using CardGames.Playground.Simulations.Stud;
using Spectre.Console.Cli;
using CardGames.Core.French.Cards.Extensions;
using CardGames.Core.Extensions;
using System.Linq;
using System;
using Spectre.Console;
using System.Collections.Generic;
using CardGames.Core.French.Cards;
using CardGames.Poker.CLI.Logging;

namespace CardGames.Poker.CLI
{
    internal class StudSimulation : Command<StudSimulationSettings>
    {
        private static readonly SpectreLogger Logger = new();

        public override int Execute(CommandContext context, StudSimulationSettings settings)
        {
            LogApplicationStart();
            var simulation = new SevenCardStudSimulation();

            do
            {
                simulation.WithPlayer(GetStudPlayer());
            }
            while (AnsiConsole.Confirm("Do you want to add another player?"));

            Logger.Paragraph("Other configuration");
            var deadCards = PromptForCards("Dead cards:");
            var numberOfHands = AnsiConsole.Ask<int>("How many hands?");

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
            var holeCards = PromptForCards("Hole Cards: ");
            var openCards = PromptForCards("Board Cards: ");

            return new StudPlayer(playerName)
                .WithHoleCards(holeCards)
                .WithBoardCards(openCards);
        }

        private static void LogApplicationStart()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            var title = "Poker-CLI";
            var contents = new[]
            {
                "Stay up to date with newest development and features by",
                "- following me on Twitter (@EluciusFTW)",
                "- visiting the GitHub page (https://github.com/EluciusFTW/CardGames)"
            };
            Logger.LogTitle(title, contents);
        }

        public static IReadOnlyCollection<Card> PromptForCards(string message)
        {
            var cardsPrompt = new TextPrompt<string>(message)
                .Validate(input =>
                {
                    try
                    {
                        input.ToCards();
                        return ValidationResult.Success();
                    }
                    catch
                    {
                        return ValidationResult.Error("Can't parse those cards. Please enter cards in the format like '3d 5h Jc'");
                    }
                });

            return AnsiConsole
                .Prompt(cardsPrompt)
                .ToCards();
        }

        private void PrintResults(StudSimulationResult results)
        {
            Logger.Paragraph("Results!");

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

    internal class StudSimulationSettings : CommandSettings
    {
    }
}