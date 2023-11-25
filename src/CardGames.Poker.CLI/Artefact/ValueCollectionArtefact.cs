using System.Collections.Generic;
using System.Linq;

namespace CardGames.Poker.CLI.Artefact;

public class ValueCollectionArtefact : IReportArtefact
{
    public string Title { get; }
    public IDictionary<string, int> Values { get; }

    public ValueCollectionArtefact(string title, IEnumerable<KeyValuePair<string, int>> values, bool orderByValues = false)
    {
        var orderedValues = orderByValues
            ? values.OrderBy(kvp => kvp.Value)
            : values;

        Values = orderedValues.ToDictionary(x => x.Key, x => x.Value);
        Title = title;
    }
}
