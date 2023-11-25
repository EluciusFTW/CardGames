using System.Collections.Generic;
using System.Linq;
using CardGames.Core.French.Dealers;
using CardGames.Core.Extensions;
using CardGames.Core.French.Cards;
using CardGames.Poker.Hands.StudHands;

namespace CardGames.Poker.Simulations.Stud;

public class SevenCardStudSimulation
{
    private FrenchDeckDealer _dealer;
    private IList<StudPlayer> _players = new List<StudPlayer>();
    private IReadOnlyCollection<Card> _deadCards = new List<Card>();

    public SevenCardStudSimulation WithPlayer(StudPlayer player)
    {
        _players.Add(player);
        return this;
    }

    public SevenCardStudSimulation WithDeadCards(IReadOnlyCollection<Card> cards)
    {
        _deadCards = cards;
        return this;
    }

    public StudSimulationResult Simulate(int nrOfHands)
    {
        _dealer = FrenchDeckDealer.WithFullDeck();
        return Play(nrOfHands);
    }

    private StudSimulationResult Play(int nrOfHands)
    {
        var results = Enumerable
            .Range(1, nrOfHands)
            .Select(_ => PlayHand());

        return new StudSimulationResult(nrOfHands, results.ToList());
    }

    private IDictionary<string, SevenCardStudHand> PlayHand()
    {
        _dealer.Shuffle();
        RemovePlayerCardsFromDeck();
        RemoveDeadCardsFromDeck();
        DealMissingHoleCards();
        DealMissingBoardCards();

        return _players.ToDictionary(
            player => player.Name,
            player => new SevenCardStudHand(player.HoleCards.Take(2).ToList(), player.BoardCards.ToList(), player.HoleCards.Last()));
    }

    private void RemoveDeadCardsFromDeck()
        => _deadCards.ForEach(card => _dealer.DealSpecific(card));

    private void RemovePlayerCardsFromDeck()
        => _players
            .SelectMany(player => player.Cards)
            .ForEach(card => _dealer.DealSpecific(card));

    private void DealMissingHoleCards()
        => _players.ForEach(player =>
            {
                var missingCards = 3 - player.GivenHoleCards.Count;
                player.DealtHoleCards = _dealer.DealCards(missingCards);
            });

    private void DealMissingBoardCards()
        => _players.ForEach(player =>
            {
                var missingCards = 4 - player.GivenBoardCards.Count;
                player.DealtBoardCards = _dealer.DealCards(missingCards);
            });
}
