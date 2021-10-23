using System;
using Spectre.Console;
using System.Collections.Generic;
using CardGames.Core.Extensions;

namespace CardGames.Poker.CLI.Logging
{
    internal class SpectreLogger
    {
        private const string MessageStyle = "cyan3";
        private const string WarningStyle = "darkorange3_1";
        private const string ErrorStyle = "red";
        private const string ParagraphColor = "cyan";
        private const string HeadlineColor = "magenta";

        private Color[] BarChartColors = new[]
        {
            new Color(215,155,215),
            new Color(215,155,195),
            new Color(215,155,175),
            new Color(215,155,155),
            new Color(215,155,135)
        };
        
        public SpectreLogger()
        {
            // Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        private void Lined(string line, string style, Justify justify = Justify.Left)
        {
            var rule = new Rule($"[{ParagraphColor}]{line}[/]")
            {
                Alignment = justify
            };
            AnsiConsole.Write(rule);
        }

        public void Log(string message) => Log(message, MessageStyle);
        public void Log(string message, string styles) => AnsiConsole.MarkupLine($"[{styles}]{message}[/]");

        public void Paragraph(string line)
        {
            NewLine();
            Lined(line, ParagraphColor);
        }

        public void Headline(string line)
        {
            NewLine();
            Lined(line, HeadlineColor);
        }

        private static void NewLine() => AnsiConsole.WriteLine(string.Empty);

        
        internal void LogTitle(string title, IEnumerable<string> lines)
        {
            AnsiConsole.Write(new Rule());
            AnsiConsole.Write(new Rule());
            AnsiConsole.Write(new Rule());
            AnsiConsole.Write(
                new FigletText(title)
                    .LeftAligned()
                    .Color(Color.Yellow));
            AnsiConsole.Write(new Rule());
            AnsiConsole.Write(new Rule());
            lines.ForEach(line => Log(line, "yellow"));
            AnsiConsole.Write(new Rule());
            AnsiConsole.Write(new Rule());
        }
    }
}
