namespace Pigeon.WebServices.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using UserSessionUtils;

    [SessionAuthorize]
    public class NotificationsController : BaseApiController
    {
        [HttpGet]
        public IHttpActionResult GetAllNotifications()
        {
            var notifications = this.Data.Notifications.GetAll();
            return this.Ok(notifications.Count());
        }
    }
}