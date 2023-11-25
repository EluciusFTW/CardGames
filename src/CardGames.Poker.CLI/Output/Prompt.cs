using System;
using System.Collections.Generic;
using CardGames.Core.French.Cards;
using CardGames.Core.French.Cards.Extensions;
using Spectre.Console;

namespace CardGames.Poker.CLI.Output;

internal static class Prompt
{
    internal static IReadOnlyCollection<Card> PromptForRangeOfCards(string message, int expectedMin, int expectedMax)
    {
        var cardsPrompt = new TextPrompt<string>(message)
            .Validate(input =>
            {
                if (string.IsNullOrEmpty(input) && expectedMin == default)
                {
                    return ValidationResult.Success();
                }
                try
                {
                    var cards = input.ToCards();
                    return expectedMin != default && cards.Count < expectedMin
                        ? ValidationResult.Error($"Sorry, but you have to entered {cards.Count} but you need to enter at least {expectedMin}.")
                        : expectedMax != default && cards.Count > expectedMax
                            ? ValidationResult.Error($"Sorry, but you have to entered {cards.Count} but you need to enter at most {expectedMax}.")
                            : ValidationResult.Success();
                }
                catch
                {
                    return ValidationResult.Error("Can't parse your input as cards. Please enter cards in the format like '3d 5h Jc'");
                }
            })
            .AllowEmpty();

        var validatedInput = AnsiConsole.Prompt(cardsPrompt);
        return !string.IsNullOrEmpty(validatedInput)
            ? validatedInput.ToCards()
            : Array.Empty<Card>();
    }

    internal static IReadOnlyCollection<Card> PromptForCards(string message, int expected, bool required)
    {
        var cardsPrompt = new TextPrompt<string>(message)
            .Validate(input =>
            {
                if (string.IsNullOrEmpty(input) && !required)
                {
                    return ValidationResult.Success();
                }
                try
                {
                    var cards = input.ToCards();
                    return expected != default && cards.Count != expected
                        ? ValidationResult.Error($"You have to entered {cards.Count} but you need to enter {expected}.")
                        : ValidationResult.Success();
                }
                catch
                {
                    return ValidationResult.Error("Can't parse your input as cards. Please enter cards in the format like '3d 5h Jc'");
                }
            })
            .AllowEmpty();

        var validatedInput = AnsiConsole.Prompt(cardsPrompt);
        return !string.IsNullOrEmpty(validatedInput)
            ? validatedInput.ToCards()
            : Array.Empty<Card>();
    }

    internal static Card PromptForCard(string message, bool required = false)
    {
        var cardPrompt = new TextPrompt<string>(message)
            .Validate(input =>
            {
                if (string.IsNullOrEmpty(input) && !required)
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
