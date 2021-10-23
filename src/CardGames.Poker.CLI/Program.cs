using Spectre.Console.Cli;

namespace CardGames.Poker.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(configuration => 
            {
                configuration
                    .AddCommand<StudSimulation>("stud-hi")
                    .WithAlias("7cs-hi")
                    .WithAlias("stud")
                    .WithDescription("Runs a 7-card Stud hi simulation.");

                configuration
                    .AddCommand<StudSimulation>("holdem")
                    .WithAlias("nlh")
                    .WithAlias("lhe")
                    .WithDescription("Runs a Holdem simulation.");
            }); 
            app.Run(args);
        }
    }
}
