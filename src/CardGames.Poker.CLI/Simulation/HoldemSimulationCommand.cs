﻿using CardGames.Core.Extensions;
using CardGames.Poker.CLI.Evaluation;
using CardGames.Poker.CLI.Output;
using CardGames.Poker.Evaluation;
using CardGames.Poker.Simulations.Holdem;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CardGames.Poker.CLI.Simulation;

internal class HoldemSimulationCommand : Command<SimulationSettings>
{
    private static readonly SpectreLogger Logger = new();

    public override int Execute(CommandContext context, SimulationSettings settings)
    {
        Logger.LogApplicationStart();

        var simulation = CreateSimulation();
        var numberOfHands = settings.NumberOfHands == default
            ? AnsiConsole.Ask<int>("How many hands?")
            : settings.NumberOfHands;

        var result = AnsiConsole
            .Status()
            .Spinner(Spinner.Known.Arrow3)
            .Start("Simulating ... ", ctx => simulation.SimulateWithFullDeck(numberOfHands));

        AnsiConsole
            .Status()
            .Spinner(Spinner.Known.Arrow3)
            .Start("Evaluating ... ", ctx => PrintResults(result));

        return 0;
    }

    private static HoldemSimulation CreateSimulation()
    {
        var simulation = new HoldemSimulation();
        do
        {
            simulation.WithPlayer(GetPlayer());
        }
        while (AnsiConsole.Confirm("Do you want to add another player?"));

        Logger.Paragraph("Add Details");
        var flop = Prompt.PromptForCards("Flop: ", 3, false);
        if (flop.Count > 0)
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
        var holeCards = Prompt.PromptForRangeOfCards("Hole Cards: ", 0, 2);

        return new HoldemPlayer(name, holeCards);
    }

    private static void PrintResults(HoldemSimulationResult result)
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
