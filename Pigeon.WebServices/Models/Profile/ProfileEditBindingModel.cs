namespace Pigeon.WebServices.Models.Profile
{
    public class ProfileEditBindingModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int? Age { get; set; }

        public string ProfilePhotoData { get; set; }

        public string CoverPhotoData { get; set; }
    }
}