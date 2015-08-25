namespace Pigeon.WebServices.Controllers
{
    using System.Web.Http;
    using Data;
    using Data.Contracts;

    public abstract class BaseApiController : ApiController
    {
        protected BaseApiController()
            : this(new PigeonData())
        {
        }

        protected BaseApiController(IPigeonData data)
        {
            this.Data = data;
        }

        protected IPigeonData Data { get; private set; }
    }
}