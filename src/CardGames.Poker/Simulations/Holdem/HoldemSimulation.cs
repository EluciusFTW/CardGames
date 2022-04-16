using System;
using System.Collections.Generic;
using System.Linq;
using CardGames.Core.Extensions;
using CardGames.Core.French.Cards;
using CardGames.Core.French.Dealers;
using CardGames.Poker.Hands.CommunityCardHands;

namespace CardGames.Poker.Simulations.Holdem
{
    public class HoldemSimulation
    {
        private FrenchDeckDealer _dealer;
        private IList<HoldemPlayer> _players = new List<HoldemPlayer>();
        private IReadOnlyCollection<Card> _flop = new List<Card>();
        private Card _turn;
        private Card _river;

        public HoldemSimulation WithPlayer(string name, IReadOnlyCollection<Card> holeCards)
            => WithPlayer(new HoldemPlayer(name, holeCards));

        public HoldemSimulation WithPlayer(HoldemPlayer player)
        {
            _players.Add(player);
            return this;
        }

        public HoldemSimulation WithFlop(IReadOnlyCollection<Card> flopCards)
        {
            if (flopCards.Count != 3)
            {
                throw new ArgumentException("A flop needs to have exactly three cards.");
            }
            _flop = flopCards;
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

        public HoldemSimulationResult SimulateWitShortDeck(int nrOfHands)
        {
            _dealer = FrenchDeckDealer.WithShortDeck();
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
                    _turn is not null
                        ? _turn
                        : _dealer.DealCard(),
                    _river is not null
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
