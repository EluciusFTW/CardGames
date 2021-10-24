using System;
using Spectre.Console;
using System.Collections.Generic;
using CardGames.Core.Extensions;
using CardGames.Poker.CLI.Artefact;
using Spectre.Console.Rendering;
using System.Linq;

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

        public void LogApplicationStart()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            var title = "Poker-CLI";
            var contents = new[]
            {
                "Stay up to date with newest development and features by",
                "- following me on Twitter (@EluciusFTW)",
                "- visiting the GitHub page (https://github.com/EluciusFTW/CardGames)"
            };
            LogTitle(title, contents);
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

        internal void LogArtefacts(IEnumerable<IReportArtefact> artefacts)
            => artefacts.ForEach(LogArtefact);

        internal void LogArtefact(IReportArtefact artefact)
        {
            switch (artefact)
            {
                case CompositeArtefact composite:
                    composite.Artefacts.ForEach(artefact => LogArtefact(artefact));
                    break;
                case TableArtefact table:
                    AnsiConsole.Write(ToTable(table));
                    break;
                case ValueCollectionArtefact list:
                    // AnsiConsole.Render(ToBarChart(list));
                    break;
                case SimpleArtefact item:
                    LogSimple(item);
                    break;
                case GroupArtefact: break;
                case CollectionArtefact item:
                    Lined(item.Title, ParagraphColor);
                    item.Items.ForEach(item => Log(item));
                    break;
                default: throw new NotSupportedException();
            }
        }

        private void LogSimple(SimpleArtefact item)
        {
            switch (item.Level)
            {
                case ArtefactLevel.Heading:
                    {
                        Paragraph(item.Value);
                        break;
                    }
                case ArtefactLevel.Info:
                    {
                        Log(item.Value);
                        break;
                    }
                case ArtefactLevel.Warning:
                    {
                        Log(item.Value, WarningStyle);
                        break;
                    }
                case ArtefactLevel.Error:
                    {
                        Log(item.Value, ErrorStyle);
                        break;
                    }
                default: throw new NotImplementedException();
            }
        }

        //private IRenderable ToBarChart(ValueCollectionArtefact list)
        //{
        //    var bar = new BarChart()
        //        .Width(80)
        //        .Label($"[{MessageStyle}]{list.Title}[/]");
        //    list.Values
        //        .ForEach((line, index) => bar.AddItem(line.Key, line.Value, BarChartColors[index % BarChartColors.Length]));
        //    return bar;
        //}

        private static IRenderable ToTable(TableArtefact table)
        {
            var output = new Table();

            table.Headers.ForEach(header => output.AddColumn(header));
            output.Columns[0].Width(25);
            output.Columns[1].Width(12);

            table.Rows.ForEach(row => output.AddRow(row.Select(cell => new Markup(cell, null))));
            return output;
        }
    }
}
