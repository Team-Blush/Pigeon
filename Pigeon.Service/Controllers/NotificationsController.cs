using System.Web.Http;

namespace Pigeon.Service.Controllers
{
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