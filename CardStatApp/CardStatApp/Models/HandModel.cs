namespace CardStatApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Hand")]
    public class HandModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 HandId { get; set; }
        [Required]
        public String TableId { get; set; }
        public Int64? TourneyNo { get; set; }
        [NotMapped]
        public int HandIdx { get; set; }
        [Required]
        public int Ante { get; set; }
        [Required]
        public int SmallBlind { get; set; }
        [Required]
        public int BigBlind { get; set; }
        [Required]
        public int ButtonSeat { get; set; }
        [Required]
        public string Description { get; set; }
        [ForeignKey("FlopCard0")]
        public int FlopCard0Key { get; set; }
        public virtual CardModel FlopCard0 { get; set; }
        [ForeignKey("FlopCard1")]
        public int FlopCard1Key { get; set; }
        public virtual CardModel FlopCard1 { get; set; }
        [ForeignKey("FlopCard1")]
        public int FlopCard2Key { get; set; }   
        public virtual CardModel FlopCard2 { get; set; }
        [ForeignKey("TurnCard")]
        public int TurnCardKey { get; set; }   
        public CardModel TurnCard { get; set; }
        [ForeignKey("RiverCard")]
        public int RiverCardKey { get; set; }   
        public CardModel RiverCard { get; set; }

        public virtual IList<SeatModel> Seats { get; set; }
        public virtual IList<PlayerActionModel> Actions { get; set; }                

        [Required]
        public int Pot { get; set; }
        [Required]
        public HandRound Round { get; set; }

        public HandModel()
        {
            Seats = new List<SeatModel>();
            Actions = new List<PlayerActionModel>();
        }
    }
}
