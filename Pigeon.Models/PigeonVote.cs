﻿namespace Pigeon.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Enumerations;

    public class PigeonVote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public VoteValue Value { get; set; }

        [Required]
        public DateTime VotedOn { get; set; }

        [Required]
        public int PigeonId { get; set; }

        public virtual Pigeon Pigeon { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}