﻿namespace Pigeon.WebServices.Models.ViewModels
{
    using System.Linq;
    using Pigeon.Models;
    using Pigeon.Models.Enumerations;

    public class UserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public string ProfilePhotoData { get; set; }

        public string CoverPhotoData { get; set; }

        public bool IsFollowed { get; set; }

        public bool IsFollowing { get; set; }

        public static UserViewModel Create(User user, User loggedUser)
        {
            string profilePhotoData = null;
            string coverPhotoData = null;

            if (user.ProfilePhotos.Any())
            {
                profilePhotoData = user.ProfilePhotos
                    .FirstOrDefault(pp => pp.ProfilePhotoFor == user).Base64Data;
            }

            if (user.CoverPhotos.Any())
            {
                coverPhotoData = user.CoverPhotos
                    .FirstOrDefault(pp => pp.CoverPhotoFor == user).Base64Data;
            }

            return new UserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                ProfilePhotoData = profilePhotoData,
                CoverPhotoData = coverPhotoData,
                IsFollowed = user.Followers
                    .Any(u => u.Id == loggedUser.Id),
                IsFollowing = user.Following
                    .Any(u => u.Id == loggedUser.Id)
            };
        }
    }
}