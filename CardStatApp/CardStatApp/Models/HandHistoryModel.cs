namespace CardStatApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HandHistory")]
    public class HandHistoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 HandHistoryKey { get; set; }
        [Required]
        public string HandNo { get; set; }        
        public string TourneyNo { get; set; }
        [Required]
        public int PlayerSeat { get; set; }
        [Required]
        public int ButtonSeat { get; set; }
        [Required]
        public bool IsBigBlind { get; set; }
        [Required]
        public bool IsSmallBlind { get; set; }
        [Required]
        public bool IsAnte { get; set; }
        [Required]
        public bool IsButton { get; set; }
        [Required]
        public int Ante { get; set; }
        [Required]
        public int Blind { get; set; }
        [Required]
        public int StartChips { get; set; }
        [Required]
        public int EndChips { get; set; }
        [Required]
        public int VIP { get; set; }
        [Required]
        public int Won { get; set; }
        [Required]
        public int Net { get; set; }
        [Required]
        public int TotalPot { get; set; }
        [Required, MaxLength(2)]
        public string HoleCard1 { get; set; }
        [Required, MaxLength(2)]
        public string HoleCard2 { get; set; }
        public string Board { get; set; }
    }
}
