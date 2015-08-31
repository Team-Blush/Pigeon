namespace Pigeon.WebServices.Models.ViewModels
{
    using System.Linq;
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

        public static UserViewModel Create(User user, User loggedUser)
        {
            string profilePhotoData = null;

            if (user.ProfilePhotos.Any())
            {
                profilePhotoData = user.ProfilePhotos
                    .FirstOrDefault(pp => pp.ProfilePhotoFor == user).Base64Data;
            }

            return new UserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                ProfilePhotoData = profilePhotoData,
                IsFollowed = user.Followers
                    .Any(u => u.Id == loggedUser.Id),
                IsFollowing = user.Following
                    .Any(u => u.Id == loggedUser.Id)
            };
        }
    }
}