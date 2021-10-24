using CardGames.Playground.Simulations.Holdem;
using CardGames.Core.Extensions;
using CardGames.Poker.CLI.Logging;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Linq;
using CardGames.Poker.CLI.Evaluation;

namespace CardGames.Poker.CLI.Simulation
{
    internal class HoldemSimulationCommand : Command<SimulationSettings>
    {
        private static readonly SpectreLogger Logger = new();

        public override int Execute(CommandContext context, SimulationSettings settings)
        {
            Logger.LogApplicationStart();

            var simulation = ConfigureSimulation();
            var numberOfHands = settings.NumberOfhands == default
                ? AnsiConsole.Ask<int>("How many hands?")
                : settings.NumberOfhands;

            PrintResults(simulation.SimulateWithFullDeck(numberOfHands));
            return 0;
        }

        private static HoldemSimulation ConfigureSimulation()
        {
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
                if (turn != default)
                {
                    simulation.WithTurn(turn);
                }
            }

            return simulation;
        }

        private static HoldemPlayer GetPlayer()
        {
            Logger.Paragraph("Add Player");

            var name = AnsiConsole.Ask<string>("Player Name: ");
            var holeCards = Prompt.PromptForCards("Hole Cards: ");

            return new HoldemPlayer(name, holeCards);
        }

        private static void PrintResults(HoldemSimulationResult results) 
            => new[]
            {
                EvaluationArtefact.Equity(results.Hands),
                EvaluationArtefact.MadeHandDistribution(results.Hands),
            }
            .ForEach(artefact => Logger.LogArtefact(artefact));
    }
}
