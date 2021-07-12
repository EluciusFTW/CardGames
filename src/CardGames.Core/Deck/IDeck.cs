namespace EluciusFTW.CardGames.Core.Cards.Deck
{
    public interface IDeck<CardKind> 
        where CardKind : struct
    {
        int NumberOfCardsLeft();
        CardKind GetFromRemaining(int index);
        CardKind GetSpecific(CardKind specificCard);
        void Reset();
    };
}
