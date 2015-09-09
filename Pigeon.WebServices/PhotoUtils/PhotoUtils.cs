namespace Pigeon.WebServices.PhotoUtils
{
    using Pigeon.Models;

    public static class PhotoUtils
    {
        public static string CheckForPhotoData(Photo photo)
        {
            if (photo != null)
            {
                return photo.Base64Data;
            }

            return null;
        }
    }
}