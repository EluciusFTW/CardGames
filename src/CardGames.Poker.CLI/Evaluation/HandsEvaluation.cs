using CardGames.Poker.CLI.Artefact;
using CardGames.Poker.Hands;
using CardGames.Poker.Hands.HandTypes;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Poker.CLI.Evaluation;

public static class HandsEvaluation
{
    public static IEnumerable<string> GetPlayers<THand>(IReadOnlyCollection<IDictionary<string, THand>> hands)
        where THand : HandBase
        => hands.First().Keys;

    public static IEnumerable<(string Name, int Wins, decimal WinPercentage)> GroupByWins<THand>(
        IReadOnlyCollection<IDictionary<string, THand>> hands)
        where THand : HandBase
        => hands
            .Select(playerHands => playerHands.OrderByDescending(hand => hand.Value.Strength).First())
            .GroupBy(playerHands => playerHands.Key)
            .Select(grp => (grp.Key, grp.Count(), (decimal)grp.Count() / hands.Count))
            .OrderBy(res => res.Item2);

    public static IDictionary<string, IEnumerable<(HandType Type, int Occurences, decimal Frequency)>> AllMadeHandDistributions<THand>(
        IReadOnlyCollection<IDictionary<string, THand>> hands)
        where THand : HandBase
        => GetPlayers(hands)
            .ToDictionary(player => player, player => MadeHandDistributionOf(hands, player));

    public static IEnumerable<(HandType Type, int Occurences, decimal Frequency)> MadeHandDistributionOf<THand>(
        IReadOnlyCollection<IDictionary<string, THand>> hands,
        string name)
        where THand : HandBase
        => hands
            .Select(playerHands => playerHands[name].Type)
            .GroupBy(type => type)
            .Select(grp => (grp.Key, grp.Count(), (decimal)grp.Count() / hands.Count))
            .OrderByDescending(grp => grp.Item2);
}

public static class EvaluationArtefact
{
    public static IReportArtefact Equity<THand>(IReadOnlyCollection<IDictionary<string, THand>> hands)
        where THand : HandBase
        => new TableArtefact(
            new[] { "Name", "Times Won", "Win %" },
            HandsEvaluation
                .GroupByWins(hands)
                .Select(triple => new[] { triple.Name, $"{triple.Wins}", $"{triple.WinPercentage:P2}" }));

    public static IReportArtefact MadeHandDistribution<THand>(IReadOnlyCollection<IDictionary<string, THand>> hands)
        where THand : HandBase
        => new CompositeArtefact(
            HandsEvaluation
                .AllMadeHandDistributions(hands)
                .Select(playerDistribution => new TableArtefact(
                    new[] { $"{ playerDistribution.Key} made", "Times", "Frequency" },
                    playerDistribution.Value.Select(triple => new[] { $"{triple.Type}", $"{triple.Occurences}", $"{triple.Frequency:P2}" }))));
}
