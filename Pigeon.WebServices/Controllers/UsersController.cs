namespace Pigeon.WebServices.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.OData;
    using System.Web.Script.Serialization;
    using Data;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Testing;
    using Models.Users;
    using Pigeon.Models;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        private readonly ApplicationUserManager userManager;

        public UsersController()
        {
            this.userManager = new ApplicationUserManager(
                new UserStore<User>(new PigeonContext()));
        }

        public ApplicationUserManager UserManager
        {
            get { return this.userManager; }
        }

        private IAuthenticationManager Authentication
        {
            get { return this.Request.GetOwinContext().Authentication; }
        }

        // POST api/users/register
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> RegisterUser(RegisterUserBindingModel model)
        {
            if (this.User.Identity.GetUserId() != null)
            {
                return this.BadRequest("User is already logged in.");
            }

            if (model == null)
            {
                return this.BadRequest("Invalid user data.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var emailExists = this.Data.Users.GetAll()
                .Any(x => x.Email == model.Email);
            if (emailExists)
            {
                return this.BadRequest("Email is already taken.");
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                ProfilePhotos = new List<Photo>(),
                CoverPhotos = new List<Photo>()
            };

            var identityResult = await this.UserManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                return this.GetErrorResult(identityResult);
            }

            this.Data.SaveChanges();

            var loginResult = await this.LoginUser(new LoginUserBindingModel
            {
                Username = model.Username,
                Password = model.Password
            });

            return loginResult;
        }

        // POST api/users/login
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IHttpActionResult> LoginUser(LoginUserBindingModel model)
        {
            if (this.User.Identity.GetUserId() != null)
            {
                return this.BadRequest("User is already logged in.");
            }

            if (model == null)
            {
                return this.BadRequest("Invalid user data");
            }

            var testServer = TestServer.Create<Startup>();
            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", model.Username),
                new KeyValuePair<string, string>("password", model.Password)
            };

            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
            var tokenServiceResponse = await testServer.HttpClient.PostAsync(
                Startup.TokenEndpointPath, requestParamsFormUrlEncoded);

            if (tokenServiceResponse.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var jsSerializer = new JavaScriptSerializer();
                var responseData =
                    jsSerializer.Deserialize<Dictionary<string, string>>(responseString);
                var authToken = responseData["access_token"];
                var username = responseData["userName"];
                var owinContext = this.Request.GetOwinContext();
                var userSessionManager = new UserSessionManager(owinContext);
                userSessionManager.CreateUserSession(username, authToken);

                userSessionManager.DeleteExpiredSessions();
            }

            return this.ResponseMessage(tokenServiceResponse);
        }

        // POST api/users/logout
        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);
            var owinContext = this.Request.GetOwinContext();
            var userSessionManager = new UserSessionManager(owinContext);
            userSessionManager.InvalidateUserSession();

            return this.Ok(new
            {
                message = "Logout successful."
            });
        }

        // GET api/users/{username}
        [HttpGet]
        [Route("{username}")]
        public IHttpActionResult GetUserInfo(string username)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.GetById(loggedUserId);

            var targetUser = this.Data.Users.GetAll()
                .FirstOrDefault(u => u.UserName == username);

            if (targetUser == null)
            {
                return this.NotFound();
            }

            return this.Ok(UserViewModel.Create(targetUser, loggedUser));
        }

        // GET api/users/{username}/preview
        [HttpGet]
        [Route("{username}/preview")]
        public IHttpActionResult GetUserInfoPreview(string username)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.GetById(loggedUserId);

            var targetUser = this.Data.Users.GetAll()
                .Where(u => u.UserName == username)
                .Select(UserPreviewViewModel.Create)
                .FirstOrDefault();

            if (targetUser == null)
            {
                return this.NotFound();
            }

            targetUser.IsFollowed = loggedUser.Following.Any(u => u.Id == targetUser.Id);
            targetUser.IsFollowing = loggedUser.Followers.Any(u => u.Id == targetUser.Id);

            return this.Ok(targetUser);
        }

        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchUserByName([FromUri] string searchTerm)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.GetById(loggedUserId);

            searchTerm = searchTerm.ToLower();
            var foundUsers = this.Data.Users.GetAll()
                .Where(u => u.UserName.ToLower().Contains(searchTerm))
                .Take(5)
                .Select(UserSearchViewModel.Create)
                .ToList();

            foreach (var user in foundUsers)
            {
                user.IsFollowed = loggedUser.Following.Any(u => u.Id == user.Id);
                user.IsFollowing = loggedUser.Followers.Any(u => u.Id == user.Id);
            }

            return this.Ok(foundUsers);
        }

        // GET api/users/{username}/followers
        [HttpGet]
        [EnableQuery]
        [Route("{username}/followers")]
        public IHttpActionResult GetFollowersInfo(string username)
        {
            var targetUser = this.Data.Users.GetAll()
                .FirstOrDefault(u => u.UserName == username);

            if (targetUser == null)
            {
                return this.NotFound();
            }

            var loggedUserFollowers = targetUser.Followers.AsQueryable()
                .OrderBy(f => f.FirstName + f.LastName)
                .Take(10)
                .Select(UserFollowerPreviewViewModel.Create);

            if (!loggedUserFollowers.Any())
            {
                return this.Ok(new
                {
                    message = "User has no followers."
                });
            }

            return this.Ok(loggedUserFollowers);
        }

        // GET api/users/{username}/following
        [HttpGet]
        [EnableQuery]
        [Route("{username}/following")]
        public IHttpActionResult GetFollowingInfo(string username)
        {
            var targetUser = this.Data.Users.GetAll()
                .FirstOrDefault(u => u.UserName == username);

            if (targetUser == null)
            {
                return this.NotFound();
            }

            var loggedUserFollowing = targetUser.Following.AsQueryable()
                .OrderBy(f => f.FirstName + f.LastName)
                .Take(10)
                .Select(UserFollowerPreviewViewModel.Create);

            if (!loggedUserFollowing.Any())
            {
                return this.Ok(new
                {
                    message = "User currently follows no one."
                });
            }

            return this.Ok(loggedUserFollowing);
        }

        // GET api/users/{username}/follow
        [HttpPut]
        [Route("{username}/follow")]
        public IHttpActionResult FollowUser(string username)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUserUsername = this.User.Identity.GetUserName();

            var loggedUser = this.Data.Users.GetById(loggedUserId);
            var targetUser = this.Data.Users.GetAll()
                .FirstOrDefault(u => u.UserName == username);

            if (targetUser == null)
            {
                return this.NotFound();
            }

            if (loggedUserUsername == username)
            {
                return this.BadRequest("Cannot follow yourself.");
            }

            if (loggedUser.Following.Contains(targetUser) &&
                targetUser.Followers.Contains(loggedUser))
            {
                return this.BadRequest("Already following that user.");
            }

            loggedUser.Following.Add(targetUser);
            targetUser.Followers.Add(loggedUser);

            this.Data.Users.Update(loggedUser);
            this.Data.Users.Update(targetUser);

            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = "Successfully followed user."
            });
        }

        // GET api/users/{username}/unfollow
        [HttpPut]
        [Route("{username}/unfollow")]
        public IHttpActionResult UnfollowUser(string username)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUserUsername = this.User.Identity.GetUserName();

            var loggedUser = this.Data.Users.GetById(loggedUserId);
            var targetUser = this.Data.Users.GetAll()
                .FirstOrDefault(u => u.UserName == username);

            if (targetUser == null)
            {
                return this.NotFound();
            }

            if (loggedUserUsername == username)
            {
                return this.BadRequest("Cannot unfollow yourself.");
            }

            if (!loggedUser.Following.Contains(targetUser) &&
                !targetUser.Followers.Contains(loggedUser))
            {
                return this.BadRequest("Cannot unfollow someone you havent followed.");
            }

            loggedUser.Following.Remove(targetUser);
            targetUser.Followers.Remove(loggedUser);

            this.Data.Users.Update(loggedUser);
            this.Data.Users.Update(targetUser);

            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = "Successfully unfollowed user."
            });
        }

    }
}