namespace CardStatApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Player")]
    public class PlayerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Name { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }
        [Required]
        public int Skill { get; set; }
        [Required]
        public PlayerCategory Category { get; set; }

        public virtual IList<SeatModel> Seats { get; set; }

        public PlayerModel()
        {
            Skill = 0;
            Category = 0;
        }
    }
}
