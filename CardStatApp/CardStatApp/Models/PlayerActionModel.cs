namespace CardStatApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PlayerAction")]
    public class PlayerActionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerActionKey { get; set; }       
        [Required]
        public int Amount { get; set; }

        [ForeignKey("Action")]
        public GameAction ActionKey { get; set; }
        public virtual ActionModel Action { get; set; }

        [ForeignKey("Seat")]
        public int SeatKey { get; set; }
        public virtual SeatModel Seat { get; set; }
    }
}
