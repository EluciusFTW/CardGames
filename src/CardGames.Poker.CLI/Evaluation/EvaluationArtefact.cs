using CardGames.Poker.CLI.Artefact;
using CardGames.Poker.Hands;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Poker.CLI.Evaluation;

public static class EvaluationArtefact
{
    public static IReportArtefact Equity<THand>(IReadOnlyCollection<IDictionary<string, THand>> hands)
        where THand : HandBase
        => new TableArtefact(
            [ "Name", "Times Won", "Win %" ],
            HandsEvaluation
                .GroupByWins(hands)
                .Select(triple => new[] { triple.Name, $"{triple.Wins}", $"{triple.Percentage:P2}" }));

    public static IReportArtefact MadeHandDistribution<THand>(IReadOnlyCollection<IDictionary<string, THand>> hands)
        where THand : HandBase
        => new CompositeArtefact(
            HandsEvaluation
                .AllMadeHandDistributions(hands)
                .Select(playerDistribution => new TableArtefact(
                    [ $"{ playerDistribution.Key} made", "Times", "Frequency" ],
                    playerDistribution.Value.Select(triple => new[] { $"{triple.Type}", $"{triple.Occurrences}", $"{triple.Frequency:P2}" }))));
}
