namespace CardStatApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Action")]
    public class ActionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ActionKey { get; set; }
        [Required]
        public string Action { get; set; }
    }
}
