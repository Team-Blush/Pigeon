namespace Pigeon.WebServices.PhotoUtils
{
    using System.Linq;
    using Pigeon.Models;

    public static class PhotoUtils
    {
        public static string CheckForCoverPhotoData(User userDbModel)
        {
            if (userDbModel.CoverPhotos.Any())
            {
                return userDbModel.CoverPhotos
                    .FirstOrDefault(pp => pp.CoverPhotoFor == userDbModel).Base64Data;
            }

            return null;
        }

        public static string CheckForProfilePhotoData(User userDbModel)
        {
            if (userDbModel.ProfilePhotos.Any())
            {
                return userDbModel.ProfilePhotos
                    .FirstOrDefault(pp => pp.ProfilePhotoFor == userDbModel).Base64Data;
            }

            return null;
        }
    }
}