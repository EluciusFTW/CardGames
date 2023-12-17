using CardGames.Poker.CLI.Simulation;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(configuration => 
{
    configuration.SetApplicationName("poker-cli");
    configuration.AddBranch<SimulationSettings>("sim", sim =>
    {
        sim
            .AddCommand<StudSimulationCommand>("stud-hi")
            .WithAlias("7cs-hi")
            .WithAlias("stud")
            .WithDescription("Runs a 7-card Stud hi simulation.");

        sim
            .AddCommand<HoldemSimulationCommand>("holdem")
            .WithAlias("nlh")
            .WithAlias("lhe")
            .WithDescription("Runs a Holdem simulation.");

        sim
            .AddCommand<OmahaSimulationCommand>("omaha")
            .WithAlias("plo")
            .WithDescription("Runs an Omaha simulation.");
    });
}); 

app.Run(args);
