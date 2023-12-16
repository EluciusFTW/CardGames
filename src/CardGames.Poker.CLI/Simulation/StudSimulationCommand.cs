using Spectre.Console.Cli;
using CardGames.Core.Extensions;
using Spectre.Console;
using CardGames.Poker.CLI.Evaluation;
using CardGames.Poker.CLI.Output;
using CardGames.Poker.Simulations.Stud;
using CardGames.Poker.Evaluation;

namespace CardGames.Poker.CLI.Simulation;

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

        var result = AnsiConsole
            .Status()
            .Spinner(Spinner.Known.Arrow3)
            .Start("Simulating ... ", ctx => simulation.Simulate(numberOfHands));

        AnsiConsole
            .Status()
            .Spinner(Spinner.Known.Arrow3)
            .Start("Evaluating ... ", ctx => PrintResults(result));

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
        var deadCards = Prompt.PromptForRangeOfCards("Dead cards:", 0, 52);
        simulation.WithDeadCards(deadCards);
        return simulation;
    }

    private static StudPlayer GetStudPlayer()
    {
        Logger.Paragraph("Add Player");

        var playerName = AnsiConsole.Ask<string>("Player Name: ");
        var holeCards = Prompt.PromptForRangeOfCards("Hole Cards: ", 0, 3);
        var openCards = Prompt.PromptForRangeOfCards("Board Cards: ", 0, 4);

        return new StudPlayer(playerName)
            .WithHoleCards(holeCards)
            .WithBoardCards(openCards);
    }

    private static void PrintResults(StudSimulationResult result)
        => new[]
            {
                HandsEvaluation
                    .GroupByWins(result.Hands)
                    .ToArtefact(),
                HandsEvaluation
                    .AllMadeHandDistributions(result.Hands)
                    .ToArtefact()
            }
            .ForEach(Logger.LogArtefact);
}