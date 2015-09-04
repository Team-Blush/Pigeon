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

        public static Expression<Func<User, UserFollowerPreviewViewModel>> Create
        {
            get
            {
                return follower => new UserFollowerPreviewViewModel
                {
                    Id = follower.Id,
                    Username = follower.UserName,
                    FirstName = follower.FirstName,
                    LastName = follower.LastName,
                    ProfilePhotoData = PhotoUtils.CheckForProfilePhotoData(follower).Base64Data
                };
            }
        }
    }
}