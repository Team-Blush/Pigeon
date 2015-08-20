namespace Pigeon.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(10, 100)]
        public string Content { get; set; }

        [Required]
        public int AuthorId { get; set; }

        //public virtual User Author { get; set; }

        [Required]
        public int PigeonId { get; set; }

        public virtual Pigeon Pigeon { get; set; }
    }
}
