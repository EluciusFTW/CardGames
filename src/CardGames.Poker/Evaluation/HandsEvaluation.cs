using CardGames.Poker.Hands;
using CardGames.Poker.Hands.HandTypes;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Poker.Evaluation;

public static class HandsEvaluation
{
    public static IEnumerable<string> GetPlayers<THand>(IReadOnlyCollection<IDictionary<string, THand>> hands)
        where THand : HandBase
        => hands.First().Keys;

    public static IEnumerable<WinDistribution> GroupByWins<THand>(
        IReadOnlyCollection<IDictionary<string, THand>> hands)
        where THand : HandBase
        => hands
            .Select(round => round.MaxBy(hand => hand.Value.Strength))
            .GroupBy(hand => hand.Key)
            .Select(handsOfPlayer => new WinDistribution(handsOfPlayer.Key, handsOfPlayer.Count(), hands.Count))
            .OrderBy(triple => triple.Wins);

    public static IDictionary<string, IEnumerable<TypeDistribution>> AllMadeHandDistributions<THand>(
        IReadOnlyCollection<IDictionary<string, THand>> hands)
        where THand : HandBase
        => GetPlayers(hands).ToDictionary(player => player, player => MadeHandDistributionOf(hands, player));

    public static IEnumerable<TypeDistribution> MadeHandDistributionOf<THand>(
        IReadOnlyCollection<IDictionary<string, THand>> hands,
        string name)
        where THand : HandBase
        => hands
            .Select(round => round[name].Type)
            .GroupBy(type => type)
            .Select(grp => new TypeDistribution(grp.Key, grp.Count(), hands.Count))
            .OrderByDescending(grp => grp.Occurrences);
}

public sealed record TypeDistribution(HandType Type, int Occurrences, int Total)
{
    public decimal Frequency => (decimal)Occurrences / Total;
}

public sealed record WinDistribution(string Name, int Wins, int Total)
{
    public decimal Percentage => (decimal)Wins / Total;
}