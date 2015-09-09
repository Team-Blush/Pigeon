namespace Pigeon.WebServices.Models.Users
{
    using System.Linq;
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

        public static UserViewModel Create(User targetUser, User loggedUser)
        {
            return new UserViewModel
            {
                Username = targetUser.UserName,
                Email = targetUser.Email,
                FirstName = targetUser.FirstName,
                LastName = targetUser.LastName,
                Age = targetUser.Age,
                ProfilePhotoData = targetUser.ProfilePhoto != null ? targetUser.ProfilePhoto.Base64Data : null,
                CoverPhotoData = targetUser.CoverPhoto != null ? targetUser.CoverPhoto.Base64Data : null,
                IsFollower = loggedUser.Followers.Any(f => f.Id.Equals(targetUser.Id)),
                IsFollowed = loggedUser.Following.Any(f => f.Id.Equals(targetUser.Id))
            };
        }
    }
}