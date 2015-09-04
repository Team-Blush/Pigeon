namespace Pigeon.Tests.IntegrationTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebServices.Models.Users;

    [TestClass]
    public class UsersControllerTests : BaseIntegrationTest
    {
        [TestMethod]
        public void Login_Should_Return_Auth_Token_With_200Ok()
        {
            var loginResponse = this.Login(FirstUserUsername, FirstUserPassword);

            Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.IsTrue(this.httpClient.DefaultRequestHeaders.Contains("Authorization"));
        }

        [TestMethod]
        public void Login_With_Invalid_Data_Should_Return_400BadRequest()
        {
            var loginResponse = this.Login(FirstUserUsername + "wrong", FirstUserPassword);

            Assert.AreEqual(HttpStatusCode.BadRequest, loginResponse.StatusCode);
            Assert.IsFalse(this.httpClient.DefaultRequestHeaders.Contains("Authorization"));
        }

        [TestMethod]
        public void Register_Should_Return_Auth_Token_With_200Ok()
        {
            var regData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "TestUser"), 
                new KeyValuePair<string, string>("password", "TestUserPass"),
                new KeyValuePair<string, string>("confirmPassword", "TestUserPass"),
                new KeyValuePair<string, string>("email", "email@email.bg")
            });

            var regResponse = httpClient.PostAsync(Endpoints.UserRegister, regData).Result;
            var regResult = regResponse.Content.ReadAsAsync<UserSessionViewModel>().Result;

            Assert.AreEqual(HttpStatusCode.OK, regResponse.StatusCode);
            Assert.IsNotNull(regResult.Access_Token);
        }

        [TestMethod]
        public void Register_With_Invalid_Data_Should_Return_400BadRequest()
        {
            var regData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "TestUser"), 
                new KeyValuePair<string, string>("password", "TestUserPass"),
                new KeyValuePair<string, string>("confirmPassword", "TestUserPass"),
            });

            var regResponse = httpClient.PostAsync(Endpoints.UserRegister, regData).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, regResponse.StatusCode);
            Assert.IsFalse(httpClient.DefaultRequestHeaders.Contains("Authorization"));
        }

        [TestMethod]
        public void LogoutShouldReturn200Ok()
        {
            this.Login(FirstUserUsername, FirstUserPassword);

            var logoutResponse = this.httpClient.PostAsync(Endpoints.UserLogout, null).Result;

            Assert.AreEqual(HttpStatusCode.OK, logoutResponse.StatusCode);
        }

        [TestMethod]
        public void GetUserInfo_Should_Return_User_Data_And_200Ok()
        {
            this.Login(FirstUserUsername, FirstUserPassword);

            var getUserInfoResponse = this.Get(string.Format(Endpoints.UserInfo, FirstUserUsername));
            var getUserInfoResponseResult = getUserInfoResponse.Content.ReadAsAsync<UserViewModel>().Result;

            Assert.AreEqual(HttpStatusCode.OK, getUserInfoResponse.StatusCode);
            Assert.IsNotNull(getUserInfoResponseResult.Username);
            Assert.AreEqual(getUserInfoResponseResult.Username, FirstUserUsername);
            Assert.IsNotNull(getUserInfoResponseResult.Email);
            Assert.AreEqual(getUserInfoResponseResult.Email, FirstUserEmail);
        }

        [TestMethod]
        public void GetUserInfo_Should_Return_403Unauthorized_If_Not_Logged_In()
        {
            var getUserInfoResponse = this.Get(string.Format(Endpoints.UserInfo, FirstUserUsername));

            Assert.AreEqual(HttpStatusCode.Unauthorized, getUserInfoResponse.StatusCode);
        }

        [TestMethod]
        public void GetUserPreview_Should_Return_User_Preview_Data_And_200Ok()
        {
            this.Login(FirstUserUsername, FirstUserPassword);

            var getUserInfoResponse = this.Get(string.Format(Endpoints.UserPreview, FirstUserUsername));
            var getUserInfoResponseResult = getUserInfoResponse.Content.ReadAsAsync<UserPreviewViewModel>().Result;

            Assert.AreEqual(HttpStatusCode.OK, getUserInfoResponse.StatusCode);
            Assert.IsNotNull(getUserInfoResponseResult.Username);
            Assert.AreEqual(getUserInfoResponseResult.Username, FirstUserUsername);
        }

        [TestMethod]
        public void GetUserPreview_Should_Return_403Unauthorized_If_Not_Logged_In()
        {
            var getUserInfoResponse = this.Get(string.Format(Endpoints.UserPreview, FirstUserUsername));

            Assert.AreEqual(HttpStatusCode.Unauthorized, getUserInfoResponse.StatusCode);
        }
    }
}