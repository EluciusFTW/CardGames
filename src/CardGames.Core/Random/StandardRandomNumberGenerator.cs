namespace CardGames.Core.Random;

public class StandardRandomNumberGenerator : IRandomNumberGenerator
{
    private System.Random _random = new System.Random();

    public int Next(int upperBound)
        => _random.Next(upperBound);
}
