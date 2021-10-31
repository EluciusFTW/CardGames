using System;
using System.Collections.Generic;
using System.Linq;
using CardGames.Core.Extensions;
using CardGames.Poker.CLI.Artefact;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace CardGames.Poker.CLI.Output
{
    internal class SpectreLogger
    {
        private const string MessageStyle = "cyan3";
        private const string WarningStyle = "darkorange3_1";
        private const string ErrorStyle = "red";
        private const string ParagraphColor = "cyan";
        private const string HeadlineColor = "magenta";

        private Color[] _barChartColors = new[]
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

        public void Log(string message) 
            => Log(message, MessageStyle);
        
        public void Log(string message, string style) 
            => AnsiConsole.MarkupLine($"[{style}]{message}[/]");

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
        public void LogArtefacts(IEnumerable<IReportArtefact> artefacts)
            => artefacts.ForEach(LogArtefact);

        public void LogArtefact(IReportArtefact artefact)
        {
            switch (artefact)
            {
                case CompositeArtefact composite:
                    composite.Artefacts.ForEach(LogArtefact);
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
                    item.Items.ForEach(Log);
                    break;
                default: throw new NotSupportedException();
            }
        }

        private void Lined(string line, string style, Justify justify = Justify.Left)
        {
            var rule = new Rule($"[{style}]{line}[/]")
            {
                Alignment = justify
            };
            AnsiConsole.Write(rule);
        }

        private static void NewLine() => AnsiConsole.WriteLine(string.Empty);
        
        private void LogTitle(string title, IEnumerable<string> lines)
        {
            AnsiConsole.Write(
                new FigletText(title)
                    .LeftAligned()
                    .Color(Color.Green));
            AnsiConsole.Write(new Rule());
            lines.ForEach(line => Log(line, "green"));
            AnsiConsole.Write(new Rule());
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

        private IRenderable ToBarChart(ValueCollectionArtefact list)
        {
            var bar = new BarChart()
                .Width(80)
                .Label($"[{MessageStyle}]{list.Title}[/]");
            list.Values
                .ForEach((line, index) => bar.AddItem(line.Key, line.Value, _barChartColors[index % _barChartColors.Length]));
            return bar;
        }

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
