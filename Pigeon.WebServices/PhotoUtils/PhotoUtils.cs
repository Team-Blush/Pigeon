namespace Pigeon.WebServices.PhotoUtils
{
    using System.Linq;
    using Pigeon.Models;

    public static class PhotoUtils
    {
        public static string CheckForProfilePhotoData(User userDbModel)
        {
            var profilePhoto = userDbModel.ProfilePhotos
                .FirstOrDefault(p => p.ProfilePhotoFor == userDbModel);
            return CheckForPhotoData(profilePhoto);
        }

        public static string CheckForCoverPhotoData(User userDbModel)
        {
            var coverPhoto = userDbModel.CoverPhotos
                .FirstOrDefault(p => p.CoverPhotoFor == userDbModel);
            return CheckForPhotoData(coverPhoto);
        }

        public static string CheckForPhotoData(Photo photo)
        {
            return photo == null ? null : photo.Base64Data;
        }
    }
}