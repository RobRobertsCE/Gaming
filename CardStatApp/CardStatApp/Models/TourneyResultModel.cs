namespace CardStatApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TourneyResult")]
    public partial class TourneyResultModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TourneyResultKey { get; set; }

        [Required]
        [StringLength(50)]
        public string SiteName { get; set; }

        [Required]
        [StringLength(50)]
        public string TourneyNo { get; set; }

        [Required]
        [StringLength(50)]
        public string GameType { get; set; }

        [Required]
        [StringLength(50)]
        public string BuyIn { get; set; }

        [Required]
        public int BuyInTotal { get; set; }

        public int PlayerCount { get; set; }

        public int TotalPrizePool { get; set; }

        [Required]
        [StringLength(50)]
        public string StartTime { get; set; }

        public int? FinishPosition { get; set; }

        public int? Winnings { get; set; }

        public double? WinPercent { get; set; }

        public TourneyResultModel()
        {
            Winnings = 0;
            WinPercent = 0;
        }
    }
}
