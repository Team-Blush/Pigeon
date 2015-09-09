namespace Pigeon.WebServices.Models.Profiles
{
    using System;
    using System.Linq.Expressions;
    using Pigeon.Models;
    using Users;

    public class ProfileViewModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public string ProfilePhotoData { get; set; }

        public string CoverPhotoData { get; set; }

        public static Expression<Func<User, UserViewModel>> Create
        {
            get
            {
                return user => new UserViewModel
                {
                    Username = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    ProfilePhotoData = user.ProfilePhoto != null ? user.ProfilePhoto.Base64Data : null,
                    CoverPhotoData = user.CoverPhoto != null ? user.CoverPhoto.Base64Data : null
                };
            }
        }
    }
}