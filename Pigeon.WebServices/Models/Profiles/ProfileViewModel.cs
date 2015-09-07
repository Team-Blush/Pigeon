namespace Pigeon.WebServices.Models.Profiles
{
    using System.Linq;
    using Pigeon.Models;
    using Pigeon.Models.Enumerations;

    public class ProfileViewModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public Gender Gender { get; set; }

        public string ProfilePhotoData { get; set; }

        public string CoverPhotoData { get; set; }

        public static ProfileViewModel Create(User userDbModel)
        {
            var profilePhoto = userDbModel.ProfilePhotos
                .FirstOrDefault(p => p.ProfilePhotoFor == userDbModel);
            var coverPhoto = userDbModel.CoverPhotos
                .FirstOrDefault(p => p.CoverPhotoFor == userDbModel);

            return new ProfileViewModel
            {
                Username = userDbModel.UserName,
                Email = userDbModel.Email,
                FirstName = userDbModel.FirstName,
                LastName = userDbModel.LastName,
                Age = userDbModel.Age,
                Gender = userDbModel.Gender,
                ProfilePhotoData = profilePhoto != null ? profilePhoto.Base64Data : null,
                CoverPhotoData = coverPhoto != null ? coverPhoto.Base64Data : null
            };
        }
    }
}