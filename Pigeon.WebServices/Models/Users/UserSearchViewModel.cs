﻿namespace Pigeon.WebServices.Models.Users
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

        public bool IsFollower { get; set; }

        public bool IsFollowed { get; set; }

        public static Expression<Func<User, UserSearchViewModel>> Create(string loggedUserId)
        {
            return targetUser => new UserSearchViewModel
            {
                Username = targetUser.UserName,
                FirstName = targetUser.FirstName,
                LastName = targetUser.LastName,
                ProfilePhotoData = targetUser.ProfilePhoto != null ?
                    targetUser.ProfilePhoto.Base64Data : null,
                IsFollower = targetUser.Following.Any(u => u.Id.Equals(loggedUserId)),
                IsFollowed = targetUser.Followers.Any(u => u.Id.Equals(loggedUserId))
            };
        }
    }
}