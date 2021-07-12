namespace EluciusFTW.CardGames.Core.Tests.TestImplementations
{
    internal readonly struct TestCard
    {
        public TestCard(int propertyValue)
        {
            SomeDistinguishingProperty = propertyValue;
        }

        public int SomeDistinguishingProperty { get; }
    }
}
