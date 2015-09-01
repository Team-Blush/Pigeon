namespace Pigeon.WebServices.Models.Profile
{
    public class ProfileEditBindingModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int? Age { get; set; }

        public string ProfileImageData { get; set; }

        public string CoverImageData { get; set; }
    }
}