namespace Pigeon.Tests
{
    public static class Endpoints
    {
        public const string UserRegister = "/api/users/register";
        public const string UserLogin = "/api/users/login";
        public const string UserLogout = "/api/users/logout";
        public const string UserPreview = "/api/users/{0}/preview";
        public const string UserInfo = "/api/users/{0}";
        public const string FolloweUser = "/api/users/{0}/follow";
        public const string UnfollowUser = "/api/users/{0}/unfollow";

        public const string GetOwnProfileInfo = "/api/profile";
        public const string EditProfileInfo = "/api/profile/edit";
        public const string ChangePassword = "/api/profile/changepassword";

        public const string GetPigeonById = "/api/pigeons/{0}";
        public const string GetLoggedUserOwnPigeons = "/api/pigeons/own";
        public const string GetLoggedUserNewsPigeons = "/api/pigeons/news";
        public const string GetLoggedUserFavouritePigeons = "/api/pigeons/news";
        public const string AddPigeon = "/api/pigeons";
        public const string EditPigeonById = "/api/pigeons/{0}/edit";
        public const string FavouritePigeonById = "/api/pigeons/{0}/favourite";
        public const string UnfavouritePigeonById = "/api/pigeons/{0}/unfavourite";
        public const string DeletePigeonById = "/api/pigeons/{0}/delete";
        public const string VoteForPigeonById = "/api/pigeons/{0}/vote";

        public const string GetCommentsByPigeonId = "/api/pigeons/{0}/comments";
        public const string AddCommentToPigeon = "/api/pigeons/{0}/comments";
        public const string EditPigeonComment = "/api/pigeons/{0}/comments/{1}";
        public const string DeletePigeonComment = "/api/pigeons/{0}/comments/{1}";
    }
}
