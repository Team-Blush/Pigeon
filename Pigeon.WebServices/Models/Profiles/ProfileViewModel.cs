namespace Pigeon.WebServices.Models.Profiles
{
    using PhotoUtils;
    using Pigeon.Models;
    using Pigeon.Models.Enumerations;

    public class ProfileViewModel
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

        public static ProfileViewModel Create(User userDbModel)
        {
            var profilePhoto = PhotoUtils.CheckForProfilePhotoData(userDbModel);
            var coverPhoto = PhotoUtils.CheckForCoverPhotoData(userDbModel);

            return new ProfileViewModel
            {
                Id = userDbModel.Id,
                Username = userDbModel.UserName,
                Email = userDbModel.Email,
                FirstName = userDbModel.FirstName,
                LastName = userDbModel.LastName,
                Age = userDbModel.Age,
                Gender = userDbModel.Gender,
                ProfilePhotoData = profilePhoto.Base64Data,
                CoverPhotoData = coverPhoto.Base64Data
            };
        }
    }
}