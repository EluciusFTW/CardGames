using CardGames.Core.French.Cards;
using CardGames.Core.French.Cards.Extensions;
using Spectre.Console;
using System;
using System.Collections.Generic;

namespace CardGames.Poker.CLI.Logging
{
    internal static class Prompt
    {
        internal static IReadOnlyCollection<Card> PromptForCards(string message)
        {
            var cardsPrompt = new TextPrompt<string>(message)
                .Validate(input =>
                {
                    if (string.IsNullOrEmpty(input))
                    {
                        return ValidationResult.Success();
                    }
                    try
                    {
                        input.ToCards();
                        return ValidationResult.Success();
                    }
                    catch
                    {
                        return ValidationResult.Error("Can't parse those cards. Please enter cards in the format like '3d 5h Jc'");
                    }
                })
                .AllowEmpty();

            var input = AnsiConsole.Prompt(cardsPrompt);
            return !string.IsNullOrEmpty(input)
                ? input.ToCards()
                : Array.Empty<Card>();
        }

        internal static Card PromptForCard(string message)
        {
            var cardPrompt = new TextPrompt<string>(message)
                .Validate(input =>
                {
                    if (string.IsNullOrEmpty(input))
                    {
                        return ValidationResult.Success();
                    }
                    try
                    {
                        input.ToCard();
                        return ValidationResult.Success();
                    }
                    catch
                    {
                        return ValidationResult.Error("Can't parse the card. Please enter the card in the format like 'Jc'");
                    }
                })
                .AllowEmpty();

            var input = AnsiConsole.Prompt(cardPrompt);

            return !string.IsNullOrEmpty(input)
                ? input.ToCard()
                : default;
        }
    }
}
