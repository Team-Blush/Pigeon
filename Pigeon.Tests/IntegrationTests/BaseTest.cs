namespace Pigeon.Tests.IntegrationTests
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using Data;
    using Data.Contracts;
    using EntityFramework.Extensions;
    using Microsoft.Owin.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Owin;
    using WebServices;
    using WebServices.Models.Users;

    [TestClass]
    public class BaseIntegrationTest
    {
        protected const string FirstUserUsername = "username1";
        protected const string FirstUserPassword = "password1";
        protected const string FirstUserEmail = "mail1@ma.il";

        protected const string SecondUserUsername = "username2";
        protected const string SecondUserPassword = "password2";
        protected const string SecondUserEmail = "mail2@ma.il";

        protected HttpClient httpClient;
        protected TestServer httpTestServer;

        public BaseIntegrationTest()
            : this(new PigeonData())
        {
        }

        public BaseIntegrationTest(IPigeonData data)
        {
            this.Data = data;
        }

        public IPigeonData Data { get; set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Database.Delete("SocialNetwork");
        }

        [TestInitialize]
        public void TestInit()
        {
            // Start OWIN testing HTTP server with Web API support
            this.httpTestServer = TestServer.Create(appBuilder =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);

                var startup = new Startup();
                startup.ConfigureAuth(appBuilder);

                appBuilder.UseWebApi(config);
            });

            this.httpClient = this.httpTestServer.HttpClient;
            this.SeedTestUsers();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (this.httpTestServer != null)
            {
                this.httpTestServer.Dispose();
            }

            var context = new PigeonContext();
            context.Users.Delete();
            context.UserSessions.Delete();
        }

        protected HttpResponseMessage Get(string endpoint)
        {
            var getResponse = httpClient.GetAsync(endpoint);

            return getResponse.Result;
        }

        protected HttpResponseMessage Post(string endpoint, HttpContent data)
        {
            var postResponse = httpClient.PostAsync(endpoint, data);

            return postResponse.Result;
        }

        protected HttpResponseMessage Put(string endpoint, HttpContent data)
        {
            var putResponse = httpClient.PutAsync(endpoint, data);

            return putResponse.Result;
        }

        protected HttpResponseMessage Delete(string endpoint)
        {
            var deleteResponse = httpClient.DeleteAsync(endpoint);

            return deleteResponse.Result;
        }

        protected HttpResponseMessage Login(string username, string password)
        {
            var loginData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            var loginResponse = httpClient.PostAsync(Endpoints.UserLogin, loginData).Result;
            var responseData = loginResponse.Content.ReadAsAsync<UserSessionViewModel>().Result;

            if (loginResponse.StatusCode == HttpStatusCode.OK)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    responseData.Access_Token);
            }

            return loginResponse;
        }

        protected void ReloadContext()
        {
            this.Data = new PigeonData();
        }

        private void SeedTestUsers()
        {
            var firstUserData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", FirstUserUsername),
                new KeyValuePair<string, string>("password", FirstUserPassword),
                new KeyValuePair<string, string>("confirmPassword", FirstUserPassword),
                new KeyValuePair<string, string>("email", FirstUserEmail)
            });

            var secondUserData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", SecondUserUsername),
                new KeyValuePair<string, string>("password", SecondUserPassword),
                new KeyValuePair<string, string>("confirmPassword", SecondUserPassword),
                new KeyValuePair<string, string>("email", SecondUserEmail)
            });

            var firstReg = this.httpClient.PostAsync(Endpoints.UserRegister, firstUserData).Result;
            var secondReg = this.httpClient.PostAsync(Endpoints.UserRegister, secondUserData).Result;
        }
    }
}