namespace Pigeon.WebServices.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Data;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.Profile;
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

            var profilePhoto = PhotoUtils.CheckForProfilePhotoData(loggedUser);
            var coverPhoto = PhotoUtils.CheckForCoverPhotoData(loggedUser);

            return this.Ok(new UserViewModel
            {
                Id = loggedUser.Id,
                Username = loggedUser.UserName,
                FirstName = loggedUser.FirstName,
                LastName = loggedUser.LastName,
                Email = loggedUser.Email,
                Age = loggedUser.Age,
                Gender = loggedUser.Gender,
                ProfilePhotoData = profilePhoto.Base64Data,
                CoverPhotoData = coverPhoto.Base64Data
            });
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
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid user token.");
            }

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

            this.EditUserPhotoContent(loggedUser, profileBindingModel);

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

        private void EditUserPhotoContent(User loggedUser, ProfileEditBindingModel profileBindingModel)
        {
            var loggedUserPhotos = new[]
            {
                loggedUser.ProfilePhotos.FirstOrDefault(p => p.ProfilePhotoFor == loggedUser),
                loggedUser.CoverPhotos.FirstOrDefault(p => p.CoverPhotoFor == loggedUser)
            };

            if (loggedUserPhotos[0] != null)
            {
                loggedUserPhotos[0].ProfilePhotoFor = null;
                this.Data.Photos.Update(loggedUserPhotos[0]);
            }

            if (loggedUserPhotos[1] != null)
            {
                loggedUserPhotos[1].CoverPhotoFor = null;
                this.Data.Photos.Update(loggedUserPhotos[1]);
            }

            if (profileBindingModel.ProfilePhotoData != null)
            {
                var newProfilePhoto = new Photo
                {
                    Base64Data = profileBindingModel.ProfilePhotoData,
                    ProfilePhotoFor = loggedUser
                };
                this.Data.Photos.Add(newProfilePhoto);
                loggedUser.ProfilePhotos.Add(newProfilePhoto);
            }

            if (profileBindingModel.CoverPhotoData != null)
            {
                var newCoverPhoto = new Photo
                {
                    Base64Data = profileBindingModel.CoverPhotoData,
                    CoverPhotoFor = loggedUser
                };
                this.Data.Photos.Add(newCoverPhoto);
                loggedUser.CoverPhotos.Add(newCoverPhoto);
            }
        }
    }
}