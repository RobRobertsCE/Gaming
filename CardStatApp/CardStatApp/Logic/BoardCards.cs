namespace CardStatApp.Logic
{
    using System.Collections.Generic;
    using CardStatApp.Models;

    public class BoardCards
    {
        public CardModel Flop0 { get; set; }
        public CardModel Flop1 { get; set; }
        public CardModel Flop2 { get; set; }
        public CardModel Turn { get; set; }
        public CardModel River { get; set; }

        public BoardCards()
        {

        }

        public BoardCards(IList<CardModel> cards)
        {
            this.Flop0 = cards[0];
            this.Flop1 = cards[1];
            this.Flop2 = cards[0];
        }
    }
}
