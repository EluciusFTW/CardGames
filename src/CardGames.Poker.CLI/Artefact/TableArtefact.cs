using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Poker.CLI.Artefact
{
    public class TableArtefact : IReportArtefact
    {
        public TableArtefact(IReadOnlyCollection<string> headers, IEnumerable<IReadOnlyCollection<string>> rows)
        {
            Headers = headers ?? throw new ArgumentNullException(nameof(headers));
            Rows = rows ?? throw new ArgumentNullException(nameof(rows));

            if (rows.Any(row => row.Count != headers.Count))
            {
                throw new ArgumentException("There needs to be equally many items in the header collection as in each row.");
            }
        }

        public IReadOnlyCollection<string> Headers { get; }
        public IEnumerable<IReadOnlyCollection<string>> Rows { get; }
    }
}
