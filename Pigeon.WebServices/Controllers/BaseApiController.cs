namespace Pigeon.WebServices.Controllers
{
    using System.Web.Http;
    using Data;
    using Data.Contracts;
    using Microsoft.AspNet.Identity;

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

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }
    }
}