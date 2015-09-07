namespace Pigeon.WebServices.Models.Users
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Pigeon.Models;

    public class UserSearchViewModel
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePhotoData { get; set; }

        public bool IsFollowed { get; set; }

        public bool IsFollowing { get; set; }

        public static Expression<Func<User, UserSearchViewModel>> Create
        {
            get
            {
                return user => new UserSearchViewModel
                {
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePhotoData = 
                    user.ProfilePhotos
                        .FirstOrDefault(photo => photo.ProfilePhotoFor == user) != null ?
                    user.ProfilePhotos
                        .FirstOrDefault(photo => photo.ProfilePhotoFor == user).Base64Data : null
                };
            }
        }
    }
}