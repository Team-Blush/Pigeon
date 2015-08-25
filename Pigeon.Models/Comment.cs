namespace Pigeon.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(100)]
        public string Content { get; set; }

        //[Required] For test purposes
        public string AuthorId { get; set; } //Its GUI and can't be int. Should be commented if it's necessary to have AuthorID

        public virtual User Author { get; set; }

        [Required]
        public int PigeonId { get; set; }

        public virtual Pigeon Pigeon { get; set; }
    }
}