using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pigeon.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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


        public virtual ICollection<Photo> ProfilePhoto
        {
            get { return this.profilePhoto;}
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