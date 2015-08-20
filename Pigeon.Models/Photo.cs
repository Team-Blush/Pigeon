namespace Pigeon.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string Base64Data { get; set; }

        public virtual User ProfilePhotoFor { get; set; }

        public virtual User CoverPhotoFor { get; set; }
    }
}