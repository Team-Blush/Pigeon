namespace Pigeon.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        private ICollection<Comment> comments;
        private ICollection<Pigeon> pigeons;
        private ICollection<Photo> coverPhoto;
        private ICollection<Photo> profilePhoto;

        public User()
        {
            this.comments = new HashSet<Comment>();
            this.pigeons = new HashSet<Pigeon>();
            this.profilePhoto = new HashSet<Photo>();
            this.coverPhoto = new HashSet<Photo>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string UserName { get; set; }

        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Email { get; set; }

        [Range(0, 100)]
        public int? Age { get; set; }

        [MinLength(4)]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Photo> ProfilePhoto
        {
            get { return this.profilePhoto; }
            set { this.profilePhoto = value; }
        }

        public virtual ICollection<Photo> CoverPhoto
        {
            get { return this.coverPhoto; }
            set { this.coverPhoto = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<Pigeon> Pigeons
        {
            get { return this.pigeons; }
            set { this.pigeons = value; }
        }


    }
}