using System;
namespace CardStatApp
{
    public enum CardValue
    {
        Ace = 1,
        Deuce,
        Trey,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }
    public static class CardValueAbbreviations
    {
        public static string Ace = "A";
        public static string Deuce = "2";
        public static string Trey = "3";
        public static string Four = "4";
        public static string Five = "5";
        public static string Six = "6";
        public static string Seven = "7";
        public static string Eight = "8";
        public static string Nine = "9";
        public static string Ten = "T";
        public static string Jack = "J";
        public static string Queen = "Q";
        public static string King = "K";

        public static string ToString(CardValue value)
        {
            switch (value)
            {
                case CardValue.Deuce:
                    {
                        return Deuce;
                    }
                case CardValue.Trey:
                    {
                        return Trey;
                    }
                case CardValue.Four:
                    {
                        return Four;
                    }
                case CardValue.Five:
                    {
                        return Five;
                    }
                case CardValue.Six:
                    {
                        return Six;
                    }
                case CardValue.Seven:
                    {
                        return Seven;
                    }
                case CardValue.Eight:
                    {
                        return Eight;
                    }
                case CardValue.Nine:
                    {
                        return Nine;
                    }
                case CardValue.Ten:
                    {
                        return Ten;
                    }
                case CardValue.Jack:
                    {
                        return Jack;
                    }
                case CardValue.Queen:
                    {
                        return Queen;
                    }
                case CardValue.King:
                    {
                        return King;
                    }
                case CardValue.Ace:
                    {
                        return Ace;
                    }
                default:
                    {
                        throw new ArgumentException(String.Format("Unrecognized Card Value: {0}", value.ToString()));
                    }
            }
        }
    }
}