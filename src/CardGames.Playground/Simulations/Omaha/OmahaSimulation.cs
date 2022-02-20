using System;
using System.Collections.Generic;
using System.Linq;
using CardGames.Core.French.Cards;
using CardGames.Core.French.Dealers;
using CardGames.Core.Extensions;
using CardGames.Poker.Hands.CommunityCardHands;

namespace CardGames.Playground.Simulations.Omaha
{
    public class OmahaSimulation
    {
        private FrenchDeckDealer _dealer;
        private IList<OmahaPlayer> _players = new List<OmahaPlayer>();
        private IReadOnlyCollection<Card> _flop = new List<Card>();
        private Card _turn;
        private Card _river;

        public OmahaSimulation WithPlayer(string name, IReadOnlyCollection<Card> holeCards)
            => WithPlayer(new OmahaPlayer(name, holeCards));

        public OmahaSimulation WithPlayer(OmahaPlayer player)
        {
            _players.Add(player);
            return this;
        }

        public OmahaSimulation WithPlayers(IEnumerable<OmahaPlayer> players)
        {
            players.ForEach(player => _players.Add(player));
            return this;
        }

        public OmahaSimulation WithFlop(IReadOnlyCollection<Card> flopCards)
        {
            if (flopCards.Count != 3)
            {
                throw new ArgumentException("A flop needs to have exactly three cards.");
            }
            _flop = flopCards;
            return this;
        }

        public OmahaSimulation WithTurn(Card card)
        {
            _turn = card;
            return this;
        }

        public OmahaSimulation WithRiver(Card card)
        {
            _river = card;
            return this;
        }

        public IEnumerable<IDictionary<string, OmahaHand>> SimulateWithFullDeck(int nrOfHands)
        {
            _dealer = FrenchDeckDealer.WithFullDeck();
            return Play(nrOfHands);
        }

        public IEnumerable<IDictionary<string, OmahaHand>> SimulateWitShortDeck(int nrOfHands)
        {
            _dealer = FrenchDeckDealer.WithShortDeck();
            return Play(nrOfHands);
        }

        private IEnumerable<IDictionary<string, OmahaHand>> Play(int nrOfHands)
        {
            foreach(var _ in Enumerable.Range(1, nrOfHands))
            {
                yield return PlayHand();
            }
        }

        private IDictionary<string, OmahaHand> PlayHand()
        {
            _dealer.Shuffle();
            RemoveKnownCardsFromDeck();
            DealMissingHoleCards();
            var communityCards = DealCommunityCards();

            return _players.ToDictionary(player => player.Name, p => new OmahaHand(p.Cards.ToList(), communityCards));
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
                var missingCards = 4 - player.GivenHoleCards.Count;
                player.DealtHoleCards = _dealer.DealCards(missingCards);
            });
    }
}
