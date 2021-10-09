using System;
using System.Collections.Generic;
using System.Linq;
using CardGames.Core.French.Cards;
using CardGames.Core.French.Dealers;
using CardGames.Core.Extensions;
using CardGames.Poker.Hands.CommunityCardHands;

namespace CardGames.Playground.Simulations.Holdem
{
    public class HoldemSimulation
    {
        private FrenchDeckDealer _dealer;
        private IList<CommunityCardGamePlayer> _players = new List<CommunityCardGamePlayer>();
        private IReadOnlyCollection<Card> _flop = new List<Card>();
        private Card _turn;
        private Card _river;

        public HoldemSimulation WithPlayer(string name, IReadOnlyCollection<Card> holeCards)
        {
            if (holeCards.Count > 2)
            {
                throw new ArgumentException($"{name} has too many hole cards to play Holdem.");
            }
            _players.Add(new CommunityCardGamePlayer { Name = name, GivenHoleCards = holeCards.ToList() });
            return this;
        }

        public HoldemSimulation WithFlop(IReadOnlyCollection<Card> holeCards)
        {
            if (holeCards.Count != 3)
            {
                throw new ArgumentException("A flop needs to have exactly three cards.");
            }
            _flop = holeCards;
            return this;
        }

        public HoldemSimulation WithTurn(Card card)
        {
            _turn = card;
            return this;
        }

        public HoldemSimulation WithRiver(Card card)
        {
            _river = card;
            return this;
        }

        public HoldemSimulationResult SimulateWithFullDeck(int nrOfHands)
        {
            _dealer = FrenchDeckDealer.WithFullDeck();
            return Play(nrOfHands);
        }

        private HoldemSimulationResult Play(int nrOfHands)
        {
            var results = Enumerable
                .Range(1, nrOfHands)
                .Select(_ => PlayHand());

            return new HoldemSimulationResult(nrOfHands, results.ToList());
        }

        private IDictionary<string, HoldemHand> PlayHand()
        {
            _dealer.Shuffle();
            RemoveKnownCardsFromDeck();
            DealMissingHoleCards();
            var communityCards = DealCommunityCards();

            return _players.ToDictionary(player => player.Name, p => new HoldemHand(p.Cards.ToList(), communityCards));
        }

        private IReadOnlyCollection<Card> DealCommunityCards()
        {
            var cards = _flop.Any()
                ? _flop
                : _dealer.DealCards(3);

            return cards
                .Concat(new[]
                {
                    _turn != default
                        ? _turn
                        : _dealer.DealCard(),
                    _river != default
                        ? _river
                        : _dealer.DealCard()
                })
                .ToList();
        }

        private void RemoveKnownCardsFromDeck()
            => _players
                .SelectMany(player => player.GivenHoleCards)
                .Concat(_flop)
                .ForEach(card => _dealer.DealSpecific(card));

        private void DealMissingHoleCards()
            => _players.ForEach(player =>
                {
                    var missingCards = 2 - player.GivenHoleCards.Count;
                    player.DealtHoleCards = _dealer.DealCards(missingCards);
                });
    }
}
