using System.Collections.Generic;

namespace CardGames.Poker.CLI.Artefact;

public class CompositeArtefact : IReportArtefact
{
    public CompositeArtefact(IEnumerable<IReportArtefact> artefacts)
    {
        Artefacts = artefacts;
    }

    public IEnumerable<IReportArtefact> Artefacts { get; }
}
