namespace Pigeon.WebServices.Models.Users
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Pigeon.Models;

    public class UserViewModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public string ProfilePhotoData { get; set; }

        public string CoverPhotoData { get; set; }

        public bool IsFollower { get; set; }

        public bool IsFollowed { get; set; }

        public static Expression<Func<User, UserViewModel>> Create(User loggedUser)
        {
            return targetUser => new UserViewModel
            {
                Username = targetUser.UserName,
                Email = targetUser.Email,
                FirstName = targetUser.FirstName,
                LastName = targetUser.LastName,
                Age = targetUser.Age,
                ProfilePhotoData = targetUser.ProfilePhoto != null ? targetUser.ProfilePhoto.Base64Data : null,
                CoverPhotoData = targetUser.CoverPhoto != null ? targetUser.CoverPhoto.Base64Data : null,
                IsFollower = targetUser.Following.Any(f => f.Id.Equals(loggedUser.Id)),
                IsFollowed = targetUser.Followers.Any(f => f.Id.Equals(loggedUser.Id))
            };
        }
    }
}