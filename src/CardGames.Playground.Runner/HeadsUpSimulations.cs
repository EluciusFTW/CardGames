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

    [MemoryDiagnoser]
    public class HoldemSimulations
    {
        [Benchmark]
        public HoldemSimulationResult PlayHoldem100()
            => new HoldemSimulation()
                .WithPlayer("Stefan", "Js Jd".ToCards())
                .WithPlayer("Matthias", "8s 6d".ToCards())
                .WithPlayer("Guy", "Ad Kd".ToCards())
                .SimulateWithFullDeck(100);

        [Benchmark]
        public HoldemSimulationResult PlayHoldem50()
            => new HoldemSimulation()
                .WithPlayer("Stefan", "Js Jd".ToCards())
                .WithPlayer("Matthias", "8s 6d".ToCards())
                .WithPlayer("Guy", "Ad Kd".ToCards())
                .SimulateWithFullDeck(50);

        [Benchmark]
        public HoldemSimulationResult PlayHoldem500()
            => new HoldemSimulation()
                .WithPlayer("Stefan", "Js Jd".ToCards())
                .WithPlayer("Matthias", "8s 6d".ToCards())
                .WithPlayer("Guy", "Ad Kd".ToCards())
                .SimulateWithFullDeck(500);

        //[Benchmark]
        //public SimulationResultEncapsulated PlayHoldemWithDedicatedHandsContainer()
        //    => new HoldemSimulationWithHands()
        //        .WithPlayer("Stefan", "Js Jd".ToCards())
        //        .WithPlayer("Matthias", "8s 6d".ToCards())
        //        .WithPlayer("Guy", "Ad Kd".ToCards())
        //        .SimulateWithFullDeck(100);

    }
}
