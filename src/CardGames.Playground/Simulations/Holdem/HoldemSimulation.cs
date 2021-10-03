using System;
using System.Collections.Generic;
using System.Linq;
using CardGames.Core.French.Cards;
using CardGames.Core.French.Dealers;
using CardGames.Core.Extensions;
using CardGames.Poker.Hands;
using CardGames.Poker.Hands.CommunityCardHands;

namespace CardGames.Playground.Simulations.Holdem
{
    public class HoldemSimulation
    {
        private FrenchDeckDealer _dealer;
        private IList<Player> _players = new List<Player>();
        private IReadOnlyCollection<Card> _flop = new List<Card>();
        private Card _turn;
        private Card _river;

        public HoldemSimulation WithPlayer(string name, IReadOnlyCollection<Card> holeCards)
        {
            if (holeCards.Count > 2)
            {
                throw new ArgumentException($"{name} has too many hole cards to play Holdem.");
            }
            _players.Add(new Player { Name = name, GivenHoleCards = holeCards.ToList() });
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

        public SimulationResult SimulateWithFullDeck(int nrOfHands)
        {
            _dealer = FrenchDeckDealer.WithFullDeck();
            return Play(nrOfHands);
        }

        private SimulationResult Play(int nrOfHands)
        {
            var results = Enumerable
                .Range(1, nrOfHands)
                .Select(_ => PlayHand());

            return new SimulationResult(nrOfHands, results.ToList());
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


        private class Player
        {
            public string Name { get; init; }
            public IReadOnlyCollection<Card> GivenHoleCards { get; init; }
            public IReadOnlyCollection<Card> DealtHoleCards { get; set; }
            public IEnumerable<Card> Cards => GivenHoleCards.Concat(DealtHoleCards);
        }
    }

    public class HoldemSimulationWithHands
    {
        private FrenchDeckDealer _dealer;
        private IList<Player> _players = new List<Player>();
        private IReadOnlyCollection<Card> _flop = new List<Card>();
        private Card _turn;
        private Card _river;

        public HoldemSimulationWithHands WithPlayer(string name, IReadOnlyCollection<Card> holeCards)
        {
            if (holeCards.Count > 2)
            {
                throw new ArgumentException($"{name} has too many hole cards to play Holdem.");
            }
            _players.Add(new Player { Name = name, GivenHoleCards = holeCards.ToList() });
            return this;
        }

        public HoldemSimulationWithHands WithFlop(IReadOnlyCollection<Card> holeCards)
        {
            if (holeCards.Count != 3)
            {
                throw new ArgumentException("A flop needs to have exactly three cards.");
            }
            _flop = holeCards;
            return this;
        }

        public HoldemSimulationWithHands WithTurn(Card card)
        {
            _turn = card;
            return this;
        }

        public HoldemSimulationWithHands WithRiver(Card card)
        {
            _river = card;
            return this;
        }

        public SimulationResultEncapsulated SimulateWithFullDeck(int nrOfHands)
        {
            _dealer = FrenchDeckDealer.WithFullDeck();
            return Play(nrOfHands);
        }

        private SimulationResultEncapsulated Play(int nrOfHands)
        {
            var results = Enumerable
                .Range(1, nrOfHands)
                .Select(_ => new Hands(PlayHand()))
                .ToList();

            return new SimulationResultEncapsulated(nrOfHands, results);
        }

        private IDictionary<string, HandBase> PlayHand()
        {
            _dealer.Shuffle();
            RemoveKnownCardsFromDeck();
            DealMissingHoleCards();
            var communityCards = DealCommunityCards();

            return _players.ToDictionary(player => player.Name, p => new HoldemHand(p.Cards.ToList(), communityCards) as HandBase);
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


        private class Player
        {
            public string Name { get; init; }
            public IReadOnlyCollection<Card> GivenHoleCards { get; init; }
            public IReadOnlyCollection<Card> DealtHoleCards { get; set; }
            public IEnumerable<Card> Cards => GivenHoleCards.Concat(DealtHoleCards);
        }
    }

    public class Hands
    {
        public IDictionary<string, HandBase> HandsByPlayer { get; }

        public Hands(IDictionary<string, HandBase> hands)
        {
            HandsByPlayer = hands
                .OrderByDescending(hand => hand.Value.Strength)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public IReadOnlyCollection<string> TiedPlayers
            => HandsByPlayer
                .GroupBy(pair => pair.Value.Strength)
                .First()
                .Select(pair => pair.Key)
                .ToList();

        public bool Tie() => WinningHands().Count() > 1;

        public IEnumerable<string> GetPlayers() => HandsByPlayer.Keys;

        public HandBase HandOfPlayer(string Name) => HandsByPlayer[Name];

        public string GetWinner()
            => string.Join(", ", WinningHands().Select(hand => hand.Key));

        public IEnumerable<KeyValuePair<string, HandBase>> WinningHands()
            => HandsByPlayer
                .GroupBy(pair => pair.Value.Strength)
                .OrderBy(grp => grp.First().Value.Strength)
                .First();

        public KeyValuePair<string, HandBase> WinningHand()
            => HandsByPlayer
                .OrderByDescending(hand => hand.Value.Strength)
                .First();
    }
}
