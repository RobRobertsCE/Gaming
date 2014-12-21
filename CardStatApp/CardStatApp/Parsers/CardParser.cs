namespace CardStatApp.Parsers
{
    using System;
    using System.Collections.Generic;
    using CardStatApp.Models;

    public static class CardParser
    {
        public static IList<CardModel> ParseCards(string value, string suit)
        {
            IList<CardModel> cards = new List<CardModel>();
            cards.Add(new CardModel(value,suit));
            return cards;
        }
        public static IList<CardModel> ParseCards(string cardTokenString)
        {
            IList<CardModel> cards = new List<CardModel>();
            string cardTokenBuffer = cardTokenString.Replace('[', ' ').Replace(']', ' ').Replace("  ","").Trim();
            string[] cardTokens = cardTokenBuffer.Split(' ');
            foreach (string cardToken in cardTokens)
            {
                cards.Add(new CardModel(cardToken.Trim()));
            }
            return cards;
        }
        public static void ParseFlopCards(ref HandModel hand, string cardTokenString)
        {
            IList<CardModel> cards = ParseCards(cardTokenString);
            hand.FlopCard0 = cards[0];
            hand.FlopCard1 = cards[1];
            hand.FlopCard2 = cards[2];
        }
        public static void ParseTurnCard(ref HandModel hand, string cardTokenString)
        {
            IList<CardModel> cards = ParseCards(cardTokenString);
            hand.TurnCard = cards[3];           
        }
        public static void ParseRiverCard(ref HandModel hand, string cardTokenString)
        {
            IList<CardModel> cards = ParseCards(cardTokenString);
            hand.RiverCard = cards[4];
        }
    }
}
