using CardGames.Playground.Simulations.Holdem;
using CardGames.Core.Extensions;
using CardGames.Poker.CLI.Logging;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Linq;

namespace CardGames.Poker.CLI
{
    internal class HoldemSimulationCommand : Command<SimulationSettings>
    {
        private static readonly SpectreLogger Logger = new();

        public override int Execute(CommandContext context, SimulationSettings settings)
        {
            Logger.LogApplicationStart();
            var simulation = new HoldemSimulation();

            do
            {
                simulation.WithPlayer(GetPlayer());
            }
            while (AnsiConsole.Confirm("Do you want to add another player?"));
            
            var flop = Prompt.PromptForCards("Flop: ");
            if (flop.Any())
            {
                simulation.WithFlop(flop);
                var turn = Prompt.PromptForCard("Turn: ");
                if(turn != default)
                {
                    simulation.WithTurn(turn);
                }
            }

            var numberOfHands = settings.NumberOfhands == default
                ? AnsiConsole.Ask<int>("How many hands?")
                : settings.NumberOfhands;

            PrintResults(simulation.SimulateWithFullDeck(numberOfHands));
            return 0;
        }

        private static HoldemPlayer GetPlayer()
        {
            Logger.Paragraph("Add Player");

            var name = AnsiConsole.Ask<string>("Player Name: ");
            var holeCards = Prompt.PromptForCards("Hole Cards: ");

            return new HoldemPlayer(name, holeCards);
        }

        private void PrintResults(HoldemSimulationResult results)
        {
            Logger.Headline("Wins");
            PrintWinPercentages(results);

            Logger.Headline("Made hand distributions");
            PrintHandDistributions(results);
        }

        private static void PrintWinPercentages(HoldemSimulationResult result)
            => result
                .GroupByWins()
                .OrderByDescending(player => player.WinPercentage)
                .ForEach(player =>
                {
                    Console.WriteLine($"{player.Name} won {player.Wins} hands => {player.WinPercentage:P2}");
                });

        private static void PrintHandDistributions(HoldemSimulationResult results)
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
