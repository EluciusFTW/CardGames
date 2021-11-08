using CardGames.Playground.Simulations.Omaha;
using CardGames.Core.Extensions;
using CardGames.Poker.CLI.Evaluation;
using CardGames.Poker.CLI.Output;
using CardGames.Poker.Hands.CommunityCardHands;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Poker.CLI.Simulation
{
    internal class OmahaSimulationCommand : Command<SimulationSettings>
    {
        private static readonly SpectreLogger Logger = new();

        public override int Execute(CommandContext context, SimulationSettings settings)
        {
            Logger.LogApplicationStart();

            var simulation = ConfigureSimulation();
            var numberOfHands = settings.NumberOfHands == default
                ? AnsiConsole.Ask<int>("How many hands?")
                : settings.NumberOfHands;

            var results = AnsiConsole.Status()
                .Spinner(Spinner.Known.Arrow3)
                .Start("Simulating ... ", ctx => simulation.SimulateWithFullDeck(numberOfHands).ToList());

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Arrow3)
                .Start("Evaluating ... ", ctx => PrintResults(results));

            return 0;
        }

        private static OmahaSimulation ConfigureSimulation()
        {
            var simulation = new OmahaSimulation();
            do
            {
                simulation.WithPlayer(GetPlayer());
            }
            while (AnsiConsole.Confirm("Do you want to add another player?"));

            Logger.Paragraph("Add Details");
            var flop = Prompt.PromptForCards("Flop: ", 3, false);
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

        private static OmahaPlayer GetPlayer()
        {
            Logger.Paragraph("Add Player");

            var name = AnsiConsole.Ask<string>("Player Name: ");
            var holeCards = Prompt.PromptForRangeOfCards("Hole Cards: ", 0, 4);

            return new OmahaPlayer(name, holeCards);
        }

        private static void PrintResults(IReadOnlyCollection<IDictionary<string, OmahaHand>> result)
            => new[]
                {
                    EvaluationArtefact.Equity(result),
                    EvaluationArtefact.MadeHandDistribution(result),
                }
                .ForEach(artefact => Logger.LogArtefact(artefact));
    }
}

