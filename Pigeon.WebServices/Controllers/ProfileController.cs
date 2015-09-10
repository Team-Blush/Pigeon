namespace Pigeon.WebServices.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Data;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.Profiles;
    using Models.Users;
    using Pigeon.Models;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/profile")]
    public class ProfileController : BaseApiController
    {
        private readonly ApplicationUserManager userManager;
        private const string EmailAlreadyTakenMessage = "Email is already taken.";
        private const string ProfileEditedSuccessfullyMessage = "Profile edited successfully.";
        private const string PasswordChangedSuccessfullyMessage = "Password successfully changed.";

        public ProfileController()
        {
            this.userManager = new ApplicationUserManager(
                new UserStore<User>(new PigeonContext()));
        }

        public ApplicationUserManager UserManager
        {
            get { return this.userManager; }
        }

        // GET api/profile
        [HttpGet]
        [Route]
        public IHttpActionResult GetProfileInfo()
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUserView = this.Data.Users.GetAll()
                .Where(u => u.Id == loggedUserId)
                .Select(ProfileViewModel.Create)
                .FirstOrDefault();

            return this.Ok(loggedUserView);
        }

        // PUT api/profile/edit
        [HttpPut]
        [Route("edit")]
        public IHttpActionResult EditProfileInfo(ProfileEditBindingModel profileBindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.GetById(loggedUserId);

            var emailHolder = this.Data.Users.GetAll()
                .FirstOrDefault(u => u.Email == profileBindingModel.Email);
            if (emailHolder != null && emailHolder.Id != loggedUserId)
            {
                return this.BadRequest(EmailAlreadyTakenMessage);
            }

            loggedUser.FirstName = profileBindingModel.FirstName;
            loggedUser.LastName = profileBindingModel.LastName;
            loggedUser.Email = profileBindingModel.Email;
            loggedUser.Age = profileBindingModel.Age;

            this.UpdateUserProfilePhoto(profileBindingModel.ProfilePhotoData, loggedUser);
            this.UpdateUserCoverPhoto(profileBindingModel.CoverPhotoData, loggedUser);

            this.Data.Users.Update(loggedUser);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = ProfileEditedSuccessfullyMessage
            });
        }

        // PUT api/profile/changePassword
        [HttpPut]
        [Route("changePassword")]
        public async Task<IHttpActionResult> ChangeProfilePassword(ChangePasswordBindingModel passChangeBindingModel)
        {
            var userPassword = this.User.Identity.GetUserId();

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this.UserManager.ChangePasswordAsync(
                userPassword,
                passChangeBindingModel.OldPassword,
                passChangeBindingModel.NewPassword);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok(new
            {
                message = PasswordChangedSuccessfullyMessage
            });
        }

        private void UpdateUserCoverPhoto(string coverPhotoData, User loggedUser)
        {
            if (coverPhotoData != null)
            {
                var coverPhoto = new Photo { Base64Data = coverPhotoData };

                this.Data.Photos.Add(coverPhoto);
                loggedUser.CoverPhoto = coverPhoto;
            }
        }

        private void UpdateUserProfilePhoto(string profilePhotoData, User loggedUser)
        {
            if (profilePhotoData != null)
            {
                var profilePhoto = new Photo { Base64Data = profilePhotoData };

                this.Data.Photos.Add(profilePhoto);
                loggedUser.ProfilePhoto = profilePhoto;
            }
        }
    }
}