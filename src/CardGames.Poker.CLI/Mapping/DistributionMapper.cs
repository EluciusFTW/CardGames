using CardGames.Poker.CLI.Artefact;
using CardGames.Poker.Evaluation;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Poker.CLI.Evaluation;

public static class DistributionMapper
{
    public static IReportArtefact ToArtefact(this IEnumerable<WinDistribution> distributions)
        => new TableArtefact(
            ["Name", "Times Won", "Win %"],
            distributions.Select(distribution => new[] { distribution.Name, $"{distribution.Wins}", $"{distribution.Percentage:P2}" }));

    public static IReportArtefact ToArtefact(this IDictionary<string, IEnumerable<TypeDistribution>> distributions)
        => new CompositeArtefact(distributions
            .Select(distribution => new TableArtefact(
                [$"{distribution.Key} made", "Times", "Frequency"],
                distribution.Value.Select(triple => new[] { $"{triple.Type}", $"{triple.Occurrences}", $"{triple.Frequency:P2}" }))));
}
