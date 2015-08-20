namespace Pigeon.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Pigeon
    {
        private ICollection<Comment> comments;
        private ICollection<User> repigeonedBy;
        private ICollection<User> favouritedBy;

        public Pigeon()
        {
            this.comments = new HashSet<Comment>();
            this.repigeonedBy = new HashSet<User>();
            this.favouritedBy = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Range(10, 150)]
        public string Content { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        //public virtual ICollection<User> RepigeonedBy
        //{
        //    get { return this.repigeonedBy; }
        //    set { this.repigeonedBy = value; }
        //}
        //public virtual ICollection<User> FavouritedBy
        //{
        //    get { return this.favouritedBy; }
        //    set { this.favouritedBy = value; }
        //}

        public int FavouritedCount { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }
    }
}
