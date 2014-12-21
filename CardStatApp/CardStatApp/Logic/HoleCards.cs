namespace CardStatApp.Logic
{
    using System.Collections.Generic;
    using CardStatApp.Models;

    public class HoleCards
    {
        public CardModel Card0 { get; set; }
        public CardModel Card1 { get; set; }

        public HoleCards(IList<CardModel> cards)
        {
            this.Card0 = cards[0];
            this.Card1 = cards[1];
        }
    }

}
