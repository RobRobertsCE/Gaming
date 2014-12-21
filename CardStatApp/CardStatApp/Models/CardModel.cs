namespace CardStatApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Cards")]
    public class CardModel
    {
        // Qs 
        #region properties
        [Key]
        public int CardKey { get; set; }
        [Index("IX_Card", 1, IsUnique = true)]
        public CardSuit Suit { get; set; }
        [Index("IX_Card", 2, IsUnique = true)]
        public CardValue Value { get; set; }
        #endregion

        #region ctor / init
        public CardModel(CardValue value, CardSuit suit)
        {
            this.Value = value;
            this.Suit = suit;
        }
        public CardModel(string singleCardTokenString)
        {
            try
            {
                InitializeCard(singleCardTokenString.Substring(0, 1), singleCardTokenString.Substring(1, 1));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public CardModel(string value, string suit)
        {
            InitializeCard(suit.Trim(), value.Trim());
        }
        void InitializeCard(string value, string suit)
        {
            switch (suit.ToUpper())
            {
                case "C":
                    {
                        this.Suit = CardSuit.Clubs;
                        break;
                    }
                case "D":
                    {
                        this.Suit = CardSuit.Diamonds;
                        break;
                    }
                case "H":
                    {
                        this.Suit = CardSuit.Hearts;
                        break;
                    }
                case "S":
                    {
                        this.Suit = CardSuit.Spades;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(String.Format("Unrecognized Card Suit: {0}", suit.ToString()));
                    }
            }

            switch (value.ToUpper())
            {
                case "2":
                    {
                        this.Value = CardValue.Deuce;
                        break;
                    }
                case "3":
                    {
                        this.Value = CardValue.Trey;
                        break;
                    }
                case "4":
                    {
                        this.Value = CardValue.Four;
                        break;
                    }
                case "5":
                    {
                        this.Value = CardValue.Five;
                        break;
                    }
                case "6":
                    {
                        this.Value = CardValue.Six;
                        break;
                    }
                case "7":
                    {
                        this.Value = CardValue.Seven;
                        break;
                    }
                case "8":
                    {
                        this.Value = CardValue.Eight;
                        break;
                    }
                case "9":
                    {
                        this.Value = CardValue.Nine;
                        break;
                    }
                case "T":
                    {
                        this.Value = CardValue.Ten;
                        break;
                    }
                case "J":
                    {
                        this.Value = CardValue.Jack;
                        break;
                    }
                case "Q":
                    {
                        this.Value = CardValue.Queen;
                        break;
                    }
                case "K":
                    {
                        this.Value = CardValue.King;
                        break;
                    }
                case "A":
                    {
                        this.Value = CardValue.Ace;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(String.Format("Unrecognized Card Value: {0}", value.ToString()));
                    }
            }
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return String.Format("{0}{1}", CardValueAbbreviations.ToString(Value), ((CardSuitAbbreviation)Suit).ToString());
        }
        #endregion
    }
}
