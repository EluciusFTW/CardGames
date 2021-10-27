using CardGames.Playground.Simulations.Stud;
using Spectre.Console.Cli;
using CardGames.Core.Extensions;
using Spectre.Console;
using CardGames.Poker.CLI.Evaluation;
using CardGames.Poker.CLI.Output;

namespace CardGames.Poker.CLI.Simulation
{
    internal class StudSimulationCommand : Command<SimulationSettings>
    {
        private static readonly SpectreLogger Logger = new();

        public override int Execute(CommandContext context, SimulationSettings settings)
        {
            Logger.LogApplicationStart();
            var simulation = ConfigureSimulation();

            var numberOfHands = settings.NumberOfHands == default
                ? AnsiConsole.Ask<int>("How many hands?")
                : settings.NumberOfHands;

            PrintResults(simulation.Simulate(numberOfHands));

            return 0;
        }

        private static SevenCardStudSimulation ConfigureSimulation()
        {
            var simulation = new SevenCardStudSimulation();
            do
            {
                simulation.WithPlayer(GetStudPlayer());
            }
            while (AnsiConsole.Confirm("Do you want to add another player?"));

            Logger.Paragraph("Other configuration");
            var deadCards = Prompt.PromptForCards("Dead cards:");
            simulation.WithDeadCards(deadCards);
            return simulation;
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

        private static void PrintResults(StudSimulationResult results)
            => new[]
            {
                EvaluationArtefact.Equity(results.Hands),
                EvaluationArtefact.MadeHandDistribution(results.Hands),
            }
            .ForEach(artefact => Logger.LogArtefact(artefact));
    }
}