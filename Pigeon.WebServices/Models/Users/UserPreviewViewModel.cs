namespace Pigeon.WebServices.Models.Users
{
    using System.Linq;
    using PhotoUtils;
    using Pigeon.Models;
    using Pigeon.Models.Enumerations;

    public class UserPreviewViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public string ProfilePhotoData { get; set; }

        public bool IsFollowed { get; set; }

        public bool IsFollowing { get; set; }

        public static UserPreviewViewModel Create(User user, User loggedUser)
        {
            var profilePhoto = PhotoUtils.CheckForProfilePhotoData(user);

            return new UserPreviewViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                ProfilePhotoData = profilePhoto.Base64Data,
                IsFollowed = user.Followers
                    .Any(u => u.Id == loggedUser.Id),
                IsFollowing = user.Following
                    .Any(u => u.Id == loggedUser.Id)
            };
        }
    }
}