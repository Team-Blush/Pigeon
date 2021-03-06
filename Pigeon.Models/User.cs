namespace Pigeon.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Pigeon> pigeons;
        private ICollection<Pigeon> favouritePigeons;

        private ICollection<Comment> comments;

        private ICollection<PigeonVote> votes;

        private ICollection<User> followers;
        private ICollection<User> following;

        public User()
        {
            this.pigeons = new HashSet<Pigeon>();
            this.favouritePigeons = new HashSet<Pigeon>();
            this.comments = new HashSet<Comment>();

            this.followers = new HashSet<User>();
            this.following = new HashSet<User>();
        }

        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Range(0, 100)]
        public int? Age { get; set; }

        public virtual ICollection<Pigeon> Pigeons
        {
            get { return this.pigeons; }
            set { this.pigeons = value; }
        }

        public virtual ICollection<Pigeon> FavouritePigeons
        {
            get { return this.favouritePigeons; }
            set { this.favouritePigeons = value; }
        }

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

        public int? ProfilePhotoId { get; set; }

        [ForeignKey("ProfilePhotoId")]
        public virtual Photo ProfilePhoto { get; set; }

        public int? CoverPhotoId { get; set; }

        [ForeignKey("CoverPhotoId")]
        public virtual Photo CoverPhoto { get; set; }

        public virtual ICollection<User> Followers
        {
            get { return this.followers; }
            set { this.followers = value; }
        }

        public virtual ICollection<User> Following
        {
            get { return this.following; }
            set { this.following = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<User> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            userIdentity.AddClaim(new Claim(ClaimTypes.Name, userIdentity.Name));
            userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, this.Id));

            return userIdentity;
        }
    }
}