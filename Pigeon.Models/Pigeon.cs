namespace Pigeon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Pigeon
    {
        private ICollection<Comment> comments;
        private ICollection<PigeonVote> votes;

        public Pigeon()
        {
            this.comments = new HashSet<Comment>();
            this.votes = new HashSet<PigeonVote>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(150)]
        public string Content { get; set; }

        // [Required]
        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public int? PhotoId { get; set; }

        public virtual Photo Photo { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int FavouritedCount { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<PigeonVote> Votes
        {
            get { return this.votes; }
            set { this.votes = value; }
        }
    }
}