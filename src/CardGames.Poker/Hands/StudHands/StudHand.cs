using CardGames.Core.French.Cards;
using CardGames.Poker.Hands.Strength;
using CardGames.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Poker.Hands.StudHands;

public class StudHand : HandBase
{
    protected override HandTypeStrengthRanking Ranking => HandTypeStrengthRanking.Classic;

    public IReadOnlyCollection<Card> HoleCards { get; }
    public IReadOnlyCollection<Card> OpenCards { get; }
    public IReadOnlyCollection<Card> DownCards { get; }

    public StudHand(
        IReadOnlyCollection<Card> holeCards,
        IReadOnlyCollection<Card> openCards,
        IReadOnlyCollection<Card> downCards)
        : base(holeCards.Concat(openCards).Concat(downCards).ToList())
    {
        HoleCards = holeCards;
        OpenCards = openCards;
        DownCards = downCards;
    }

    protected override IEnumerable<IReadOnlyCollection<Card>> PossibleHands()
        => Cards
            .SubsetsOfSize(5)
            .Select(cards => cards.ToList());
}
