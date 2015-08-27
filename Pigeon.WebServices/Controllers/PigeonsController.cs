namespace Pigeon.WebServices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.OData;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using Pigeon.Models;

    [Authorize]
    [RoutePrefix("api/pigeons")]
    public class PigeonsController : BaseApiController
    {
        [HttpGet]
        [Route]
        [EnableQuery]
        public IHttpActionResult GetUserPigeons()
        {
            var userId = this.User.Identity.GetUserId();

            var pigeons = this.Data.Pigeons
                .Search(p => p.Author.Id == userId)
                .Select(PigeonViewModel.Create);

            if (!pigeons.Any())
            {
                return this.Ok("No Pigeons are found.");
            }

            return this.Ok(pigeons);
        }

        [HttpPost]
        [Route]
        public IHttpActionResult AddPigeon(PigeonBindingModel inputPigeon)
        {
            var userId = this.User.Identity.GetUserId();

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid Pigeon data.");
            }

            var pigeonToAdd = new Pigeon
            {
                Title = inputPigeon.Title,
                Content = inputPigeon.Content,
                Author = this.Data.Users.Search(u => u.Id == userId).FirstOrDefault(),
                AuthorId = userId,
                CreatedOn = DateTime.Now,
                FavouritedCount = 0,
                Comments = new HashSet<Comment>(),
                Votes = new HashSet<PigeonVote>()
            };

            this.Data.Pigeons.Add(pigeonToAdd);
            this.Data.SaveChanges();

            return this.Ok(new PigeonViewModel(pigeonToAdd));
        }

        [HttpPut]
        [Route("update/{id}")]
        public IHttpActionResult UpdatePigeon(int id, PigeonBindingModel upPigeon)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons
                .Search(p => p.Id == id)
                .FirstOrDefault();

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid Pigeon data.");
            }

            if (pigeon == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (pigeon.Author.Id != userId)
            {
                return this.Unauthorized();
            }

            pigeon.Content = upPigeon.Content;

            this.Data.SaveChanges();

            return this.Ok(new PigeonViewModel(pigeon));
        }

        [HttpPut]
        [Route("favourite/{id}")]
        public IHttpActionResult AddPigeonToFavourited(int id)
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
        [Route("{id}")]
        public IHttpActionResult DeletePigeon(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons
                .Search(p => p.Id == id)
                .FirstOrDefault();

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

            return this.Ok("Successfully deleted pigeon.");
        }
    }
}