using System.ComponentModel.DataAnnotations.Schema;

namespace Pigeon.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Comment> comments;
        private ICollection<Photo> coverPhoto;
        private ICollection<Pigeon> pigeons;
        private ICollection<Photo> profilePhoto;

        public User()
        {
            this.comments = new HashSet<Comment>();
            this.pigeons = new HashSet<Pigeon>();
            this.profilePhoto = new HashSet<Photo>();
            this.coverPhoto = new HashSet<Photo>();
        }

        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Range(0, 100)]
        public int? Age { get; set; }

        public virtual ICollection<User> Followers { get; set; } 

        public virtual ICollection<User> Following { get; set; } 

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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}