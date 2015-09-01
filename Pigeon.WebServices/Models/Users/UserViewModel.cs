namespace Pigeon.WebServices.Models.Users
{
    using System.Linq;
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
            var profilePhotoData = PhotoUtils.CheckForProfilePhotoData(user);
            var coverPhotoData = PhotoUtils.CheckForCoverPhotoData(user);

            return new UserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                Gender = user.Gender,
                ProfilePhotoData = profilePhotoData,
                CoverPhotoData = coverPhotoData,
                IsFollowed = user.Followers
                    .Any(u => u.Id == loggedUser.Id),
                IsFollowing = user.Following
                    .Any(u => u.Id == loggedUser.Id)
            };
        }
    }
}