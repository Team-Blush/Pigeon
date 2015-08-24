namespace Pigeon.Models
{
    using System.ComponentModel.DataAnnotations;
    using Enumerations;

    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Content { get; set; }

        [Required]
        public NotificationType Type { get; set; }
    }
}