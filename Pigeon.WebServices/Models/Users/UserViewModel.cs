namespace Pigeon.WebServices.Models.Users
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using PhotoUtils;
    using Pigeon.Models;
    using Pigeon.Models.Enumerations;

    public class UserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public Gender Gender { get; set; }

        public string ProfilePhotoData { get; set; }

        public string CoverPhotoData { get; set; }

        public bool IsFollowed { get; set; }

        public bool IsFollowing { get; set; }

        public static UserViewModel Create(User user, User loggedUser)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                Gender = user.Gender,
                ProfilePhotoData = PhotoUtils.CheckForProfilePhotoData(user).Base64Data,
                CoverPhotoData = PhotoUtils.CheckForCoverPhotoData(user).Base64Data,
                IsFollowed = loggedUser.Following.Any(u => u.Id == user.Id),
                IsFollowing = loggedUser.Followers.Any(u => u.Id == user.Id)
            };
        }
    }
}