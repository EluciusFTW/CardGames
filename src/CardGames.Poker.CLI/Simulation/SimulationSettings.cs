using Spectre.Console.Cli;
using System.ComponentModel;

namespace CardGames.Poker.CLI.Simulation
{
    internal class SimulationSettings : CommandSettings
    {
        [Description("Number of hands to run in the simulation")]
        [CommandOption("-n|--numberOfHands")]
        public int NumberOfHands { get; set; }
    }
}