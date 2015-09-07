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
            if (profilePhoto != null)
            {
                return profilePhoto.Base64Data;
            }

            return null;
        }

        public static string CheckForCoverPhotoData(User userDbModel)
        {
            var coverPhoto = userDbModel.CoverPhotos
                .FirstOrDefault(p => p.CoverPhotoFor == userDbModel);
            if (coverPhoto != null)
            {
                return coverPhoto.Base64Data;
            }

            return null;
        }
    }
}