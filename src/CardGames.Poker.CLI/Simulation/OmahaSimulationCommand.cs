using CardGames.Core.Extensions;
using CardGames.Poker.CLI.Evaluation;
using CardGames.Poker.CLI.Output;
using CardGames.Poker.Hands.CommunityCardHands;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Generic;
using System.Linq;
using CardGames.Poker.Simulations.Omaha;
using CardGames.Core.French.Cards;

namespace CardGames.Poker.CLI.Simulation;

internal class OmahaSimulationCommand : Command<SimulationSettings>
{
    private static readonly SpectreLogger Logger = new();

    private int _nrOfHands;
    private IList<OmahaPlayer> _players = new List<OmahaPlayer>();
    private IReadOnlyCollection<Card> _flop = new List<Card>();
    private Card _turn = default;

    public override int Execute(CommandContext context, SimulationSettings settings)
    {
        Logger.LogApplicationStart();
        do
        {
            CollectData(settings);
            var simulation = CreateSimulation();
            var results = RunSimulation(simulation);
            EvaluateResults(results);
        } while (AnsiConsole.Confirm("Do you want to run another simulation?"));

        return 0;
    }

    private OmahaSimulation CreateSimulation()
    {
        var simulation = new OmahaSimulation()
            .WithPlayers(_players);

        if (_flop.Any())
        {
            simulation.WithFlop(_flop);
        }

        if (_turn != default)
        {
            simulation.WithTurn(_turn);
        }
        return simulation;
    }

    private static void EvaluateResults(List<IDictionary<string, OmahaHand>> results)
        => AnsiConsole
            .Status()
            .Spinner(Spinner.Known.Arrow3)
            .Start("Evaluating ... ", _ => PrintResults(results));

    private List<IDictionary<string, OmahaHand>> RunSimulation(OmahaSimulation simulation)
         => AnsiConsole
            .Status()
            .Spinner(Spinner.Known.Arrow3)
            .Start("Simulating ... ", _ => simulation.SimulateWithFullDeck(_nrOfHands).ToList());

    private void CollectData(SimulationSettings settings)
    {
        do
        {
            _players.Add(GetPlayer());
        }
        while (AnsiConsole.Confirm("Do you want to add another player?"));

        Logger.Paragraph("Add Details");
        var flop = Prompt.PromptForCards("Flop: ", 3, false);
        if (flop.Any())
        {
            _flop = flop;
            _turn = Prompt.PromptForCard("Turn: ");
        }
     
        _nrOfHands = settings.NumberOfHands == default
            ? AnsiConsole.Ask<int>("How many hands?")
            : settings.NumberOfHands;
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
