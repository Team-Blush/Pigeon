namespace Pigeon.Service.Controllers
{
    using System.Web.Http;

    public class NotificationsController : BaseApiController
    {
        [HttpGet]
        public IHttpActionResult GetAllNotifications()
        {
            var notifications = this.Data.Notifications.GetAll();
            return this.Ok(notifications);
        }
    }
}