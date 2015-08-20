namespace Pigeon.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlClient;

    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(5, 50)]
        public string Content { get; set; }

        [Required]
        public NotificationType Type { get; set; }
    }
}
