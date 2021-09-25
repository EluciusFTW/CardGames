using BenchmarkDotNet.Attributes;
using CardGames.Core.French.Cards.Extensions;
using CardGames.Core.French.Dealers;
using CardGames.Playground.Simulations.Dummy;

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
}
