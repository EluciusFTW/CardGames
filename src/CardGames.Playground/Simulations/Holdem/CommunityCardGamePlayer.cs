using System.Collections.Generic;
using System.Linq;
using CardGames.Core.French.Cards;

namespace CardGames.Playground.Simulations.Holdem
{

    public class CommunityCardGamePlayer
    {
        public string Name { get; init; }
        public IReadOnlyCollection<Card> GivenHoleCards { get; init; }
        public IReadOnlyCollection<Card> DealtHoleCards { get; set; }
        public IEnumerable<Card> Cards => GivenHoleCards.Concat(DealtHoleCards);
    }
}       
