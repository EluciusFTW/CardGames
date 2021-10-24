using System;

namespace CardGames.Poker.CLI.Artefact
{
    public class SimpleArtefact : IReportArtefact
    {
        public string Value { get; }
        public ArtefactLevel Level { get; }

        public SimpleArtefact(string value)
            : this(value, ArtefactLevel.Info)
        {
        }

        public SimpleArtefact(string value, ArtefactLevel level)
        {
            Value = value ?? throw new ArgumentNullException();
            Level = level;
        }
    }
}
