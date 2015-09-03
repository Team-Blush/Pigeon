namespace Pigeon.WebServices.PhotoUtils
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Pigeon.Models;

    public static class PhotoUtils
    {
        private static readonly string DefaultUserImagesLocation =
            HostingEnvironment.MapPath("~/Resources/");

        private static readonly string DefaultProfilePhotoName = "Default-Profile.png";
        private static readonly string DefaultCoverPhotoName = "Default-Cover.jpg";
        private static readonly string PhotoPrefix = "data:image/png;base64,";

        public static Photo CheckForCoverPhotoData(User userDbModel)
        {
            var hasCoverPhoto = userDbModel.CoverPhotos
                .Any(p => p.CoverPhotoFor.UserName.Equals(userDbModel.UserName));
            if (hasCoverPhoto)
            {
                return userDbModel.CoverPhotos
                    .FirstOrDefault(pp => pp.CoverPhotoFor == userDbModel);
            }

            return GetDefaultCoverPhoto(userDbModel);
        }

        public static Photo CheckForProfilePhotoData(User userDbModel)
        {
            var hasProfilePhoto = userDbModel.ProfilePhotos
                .Any(p => p.ProfilePhotoFor.UserName.Equals(userDbModel.UserName));
            if (hasProfilePhoto)
            {
                return userDbModel.ProfilePhotos
                    .FirstOrDefault(pp => pp.ProfilePhotoFor == userDbModel);
            }

            return GetDefaultProfilePhoto(userDbModel);
        }

        private static Photo GetDefaultCoverPhoto(User userDbModel)
        {
            var bytes = File.ReadAllBytes(DefaultUserImagesLocation + DefaultCoverPhotoName);
            var photoData = PhotoPrefix + Convert.ToBase64String(bytes);

            var coverPhoto = new Photo
            {
                Title = userDbModel.UserName,
                Url = DefaultCoverPhotoName,
                Base64Data = photoData
            };

            return coverPhoto;
        }

        private static Photo GetDefaultProfilePhoto(User userDbModel)
        {
            var bytes = File.ReadAllBytes(DefaultUserImagesLocation + DefaultProfilePhotoName);
            var photoData = PhotoPrefix + Convert.ToBase64String(bytes);

            var profilePhoto = new Photo
            {
                Title = userDbModel.UserName,
                Url = DefaultProfilePhotoName,
                Base64Data = photoData
            };

            return profilePhoto;
        }
    }
}