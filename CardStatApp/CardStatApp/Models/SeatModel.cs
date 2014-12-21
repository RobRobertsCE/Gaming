namespace CardStatApp.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Seat")]
    public class SeatModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeatKey { get; set; }       
        [Required]
        public int SeatNumber { get; set; }
        [Required]
        public int PositionFromButton { get; set; }
        [Required]
        public int Chips { get; set; }
        [Required]
        public int FIP { get; set; }
        [Required]
        public int VIP { get; set; }
        [Required]
        public bool IsInHand { get; set; }
        [Required]
        public bool IsAllIn { get; set; }
        [Required]
        public bool IsBigBlind { get; set; }
        [Required]
        public bool IsSmallBlind { get; set; }
        [NotMapped]
        public bool CanAct
        {
            get
            {
                return (IsInHand & !IsAllIn);
            }
        }

        [ForeignKey("HoleCard0")]
        public int HoleCard0Key { get; set; }
        public virtual CardModel HoleCard0 { get; set; }
        [ForeignKey("HoleCard1")]
        public int HoleCard1Key { get; set; }
        public virtual CardModel HoleCard1 { get; set; }

        [ForeignKey("Player")]
        public int PlayerKey { get; set; }
        public virtual PlayerModel Player { get; set; }

        [ForeignKey("Hand")]
        public int HandKey { get; set; }
        public virtual HandModel Hand { get; set; }

    }
}
