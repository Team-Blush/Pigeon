namespace Pigeon.WebServices.Models.Users
{
    using System;
    using System.Linq.Expressions;
    using PhotoUtils;
    using Pigeon.Models;

    public class UserFollowerPreviewViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePhotoData { get; set; }

        public static Expression<Func<User, UserFollowerPreviewViewModel>> GetModel
        {
            get
            {
                return user => new UserFollowerPreviewViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePhotoData = PhotoUtils.CheckForProfilePhotoData(user).Base64Data
                };
            }
        }
    }
}