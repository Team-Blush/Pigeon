namespace Pigeon.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0, 20)]
        public string UserName { get; set; }

        [Range(0, 20)]
        public string FirstName { get; set; }

        [Required]
        [Range(0, 20)]
        public string LastName { get; set; }

        [Required]
        [Range(5, 50)]
        public string Email { get; set; }

        [Range(0, 100)]
        public int? Age { get; set; }

        [Range(4, 20)]
        public string PhoneNumber { get; set; }

        [Column("ProfilePhoto")]
        public int ProfilePhotoId { get; set; }

        //[ForeignKey("ProfilePhotoId")]
        //public virtual Photo ProfilePhoto { get; set; }

        [Column("CoverPhoto")]
        public int CoverPhotoId { get; set; }

        //[ForeignKey("CoverPhotoId")]
        //public virtual Photo CoverPhoto { get; set; }
    }
}