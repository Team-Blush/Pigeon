namespace Pigeon.WebServices.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.OData;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using Pigeon.Models;

    [Authorize]
    public class PigeonsController : BaseApiController
    {
        [HttpGet]
        [Route("api/pigeons")]
        [EnableQuery]
        public IHttpActionResult ReturnAllPigeonsForUser()
        {
            var userId = this.User.Identity.GetUserId();

            var pigeons = this.Data.Pigeons.Search(p => p.Author.Id == userId)
                .Select(PigeonViewModel.Create);

            if (!pigeons.Any())
            {
                return this.Ok("No Pigeons are found.");
            }

            return this.Ok(pigeons);
        }

        [HttpPost]
        [Route("api/pigeons")]
        public IHttpActionResult AddPigeon(PigeonBindigModel inputPigeon)
        {
            var userId = this.User.Identity.GetUserId();

            var pigeonToAdd = new Pigeon()
            {
                Author = this.Data.Users.Search(u => u.Id == userId).FirstOrDefault(),
                Title = inputPigeon.Title,
                FavouritedCount = 0,
                Comments = new HashSet<Comment>()
            };

            this.Data.Pigeons.Add(pigeonToAdd);
            this.Data.SaveChanges();

            return this.Ok(new PigeonViewModel(pigeonToAdd));
        }

        [HttpPut]
        [Route("api/pigeons/update/{id}")]
        public IHttpActionResult UpdatePigeon(int id, PigeonBindigModel upPigeon)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(id);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid Pigeon to add.");
            }

            if (pigeon.Author.Id != userId)
            {
                return this.Unauthorized();
            }

            pigeon.Title = upPigeon.Title;

            this.Data.SaveChanges();

            return this.Ok(new PigeonViewModel(pigeon));
        }

        [HttpPut]
        [Route("api/pigeons/favourite/{id}")]
        public IHttpActionResult FauvoritedPigeonsAdd(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons
                .Search(p => p.Id == id)
                .FirstOrDefault();

            if (pigeon == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (pigeon.Author.Id == userId)
            {
                return this.BadRequest("You cannot favourite your own Pigeon.");
            }

            pigeon.FavouritedCount++;

            this.Data.SaveChanges();

            return this.Ok(new PigeonViewModel(pigeon));
        }

        [HttpDelete]
        [Route("api/pigeons/{id}")]
        public IHttpActionResult DeletePigeon(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(id);

            if (pigeon == null)
            {
                return this.BadRequest("No such pigeon");
            }

            if (pigeon.Author.Id != userId)
            {
                return this.Unauthorized();
            }

            this.Data.Pigeons.Delete(pigeon);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
