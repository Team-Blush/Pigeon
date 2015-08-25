using System.Web.Http;
using Pigeon.Data;
using Pigeon.Data.Contracts;

namespace Pigeon.Service.Controllers
{
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