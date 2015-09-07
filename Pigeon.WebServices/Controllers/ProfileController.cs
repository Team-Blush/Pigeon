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
    using PhotoUtils;
    using Pigeon.Models;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/profile")]
    public class ProfileController : BaseApiController
    {
        private readonly ApplicationUserManager userManager;

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
            var loggedUser = this.Data.Users.GetById(loggedUserId);

            var profileViewModel = new UserViewModel
            {
                Username = loggedUser.UserName,
                FirstName = loggedUser.FirstName,
                LastName = loggedUser.LastName,
                Email = loggedUser.Email,
                Age = loggedUser.Age,
                Gender = loggedUser.Gender,
                ProfilePhotoData = PhotoUtils.CheckForProfilePhotoData(loggedUser),
                CoverPhotoData = PhotoUtils.CheckForCoverPhotoData(loggedUser)
            };

            return this.Ok(profileViewModel);
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
                return this.BadRequest("Email is already taken.");
            }

            loggedUser.FirstName = profileBindingModel.FirstName;
            loggedUser.LastName = profileBindingModel.LastName;
            loggedUser.Email = profileBindingModel.Email;
            loggedUser.Age = profileBindingModel.Age ?? null;

            this.EditUserProfilePhoto(loggedUser, profileBindingModel.ProfilePhotoData);
            this.EditUserCoverPhoto(loggedUser, profileBindingModel.CoverPhotoData);

            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = "Profile edited successfully."
            });
        }

        // PUT api/profile/changePassword
        [HttpPut]
        [Route("changePassword")]
        public async Task<IHttpActionResult> ChangeProfilePassword(ChangePasswordBindingModel passChangeBindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this.UserManager.ChangePasswordAsync(
                this.User.Identity.GetUserId(),
                passChangeBindingModel.OldPassword,
                passChangeBindingModel.NewPassword);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok(new
            {
                message = "Password successfully changed."
            });
        }

        private void EditUserProfilePhoto(User loggedUser, string profilePhotoData)
        {
            var userProfilePhoto = loggedUser.ProfilePhotos
                .FirstOrDefault(p => p.ProfilePhotoFor == loggedUser);

            if (userProfilePhoto != null)
            {
                userProfilePhoto.ProfilePhotoFor = null;
                this.Data.Photos.Update(userProfilePhoto);
            }

            if (profilePhotoData != null)
            {
                var newProfilePhoto = new Photo
                {
                    Base64Data = profilePhotoData,
                    ProfilePhotoFor = loggedUser
                };
                this.Data.Photos.Add(newProfilePhoto);
                loggedUser.ProfilePhotos.Add(newProfilePhoto);
            }
        }

        private void EditUserCoverPhoto(User loggedUser, string coverPhotoData)
        {
            var userCoverPhoto = loggedUser.CoverPhotos
                .FirstOrDefault(p => p.CoverPhotoFor == loggedUser);

            if (userCoverPhoto != null)
            {
                userCoverPhoto.CoverPhotoFor = null;
                this.Data.Photos.Update(userCoverPhoto);
            }

            if (coverPhotoData != null)
            {
                var newCoverPhoto = new Photo
                {
                    Base64Data = coverPhotoData,
                    CoverPhotoFor = loggedUser
                };
                this.Data.Photos.Add(newCoverPhoto);
                loggedUser.CoverPhotos.Add(newCoverPhoto);
            }
        }
    }
}