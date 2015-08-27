namespace Pigeon.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PigeonVote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool Value { get; set; }

        [Required]
        public int PigeonId { get; set; }

        public virtual Pigeon Pigeon { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}