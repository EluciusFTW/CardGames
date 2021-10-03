using BenchmarkDotNet.Attributes;
using CardGames.Core.French.Cards.Extensions;
using CardGames.Core.French.Dealers;
using CardGames.Playground.Simulations.Dummy;
using CardGames.Playground.Simulations.Holdem;

namespace CardGames.Playground.Runner
{
    public class HeadsUpSimulations
    {
        [Benchmark]
        public HeadsUpResult PlayerOneNeedsHigherCardThanPlayerTwoButGetsMoreOfThem() 
            => HeadsUpDummySimulation
                .Create(FrenchDeckDealer.WithFullDeck())
                .Simulate(10000, 3, 1, (c1, c2) => c1.HighestValue() > c2.HighestValue());

        [Benchmark]
        public HeadsUpResult PlayerOneNeedsHigherCardThanPlayerTwoButGetsMoreOfThemWithShortDeck()
            => HeadsUpDummySimulation
                .Create(FrenchDeckDealer.WithShortDeck())
                .Simulate(10000, 3, 1, (c1, c2) => c1.HighestValue() > c2.HighestValue());

    }

    public class HoldemSimulations
    {
        [Benchmark]
        public SimulationResult PlayHoldemWithoutDedicatedHandsContainer(int nrOfHands)
            => new HoldemSimulation()
                .WithPlayer("Stefan", "Js Jd".ToCards())
                .WithPlayer("Matthias", "8s 6d".ToCards())
                .WithPlayer("Guy", "Ad Kd".ToCards())
                .SimulateWithFullDeck(nrOfHands);

        [Benchmark]
        public SimulationResultEncapsulated PlayHoldemWithDedicatedHandsContainer(int nrOfHands)
            => new HoldemSimulationWithHands()
                .WithPlayer("Stefan", "Js Jd".ToCards())
                .WithPlayer("Matthias", "8s 6d".ToCards())
                .WithPlayer("Guy", "Ad Kd".ToCards())
                .SimulateWithFullDeck(nrOfHands);

    }
}
