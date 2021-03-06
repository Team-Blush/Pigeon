﻿namespace Pigeon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Pigeon
    {
        private ICollection<Comment> comments;
        private ICollection<PigeonVote> votes;
        private ICollection<User> favouritedBy;

        public Pigeon()
        {
            this.comments = new HashSet<Comment>();
            this.votes = new HashSet<PigeonVote>();
            this.favouritedBy = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(150)]
        public string Content { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public int? PhotoId { get; set; }

        public virtual Photo Photo { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int FavouritedCount { get; set; }

        public virtual ICollection<User> FavouritedBy
        {
            get { return this.favouritedBy; }
            set { this.favouritedBy = value; }
        }

        public int CommentsCount { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public int UpVotesCount { get; set; }

        public int DownVotesCount { get; set; }

        public virtual ICollection<PigeonVote> Votes
        {
            get { return this.votes; }
            set { this.votes = value; }
        }
    }
}