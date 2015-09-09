namespace Pigeon.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Title { get; set; }

        public string Url { get; set; }

        [Required]
        public string Base64Data { get; set; }
    }
}