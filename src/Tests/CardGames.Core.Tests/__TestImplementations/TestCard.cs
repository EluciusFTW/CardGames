namespace CardGames.Core.Tests.__TestImplementations
{
    internal class TestCard
    {
        public TestCard(int propertyValue)
        {
            SomeDistinguishingProperty = propertyValue;
        }

        public int SomeDistinguishingProperty { get; }
    }
}
