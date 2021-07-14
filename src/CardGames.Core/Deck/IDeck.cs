namespace CardGames.Core.Deck
{
    public interface IDeck<TCardKind> 
        where TCardKind : struct
    {
        int NumberOfCardsLeft();
        TCardKind GetFromRemaining(int index);
        TCardKind GetSpecific(TCardKind specificCard);
        void Reset();
    };
}
