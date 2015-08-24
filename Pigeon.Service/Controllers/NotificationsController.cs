namespace Pigeon.Service.Controllers
{
    using System.Web.Http;
    using Data;

    public class NotificationsController : ApiController
    {
        private PigeonData data = new PigeonData();

        [HttpGet]
        public IHttpActionResult GetAllNotifications()
        {
            var notifications = this.data.Notifications.GetAll();
            return this.Ok(notifications);
        }
    }
}
