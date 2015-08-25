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

    //[Authorize] //When the account system work all of the pigeons should be accessible from registert users
    public class PigeonsController : BaseApiController
    {
        [HttpGet]
        [Route("api/Pigeon/allpigeons")]
        [EnableQuery] // Odata enable for easy pagenation
        public IHttpActionResult ReturnAllPigeonsForUser()
        {
            //Logic when the Account system work
            //var userId = this.User.Identity.GetUserId();

            //var pigeons = this.Data.Pigeons.Search(p => p.User.Id == userId)
            //    .Select(PigeonViewModel.Create);

            //if (!pigeons.Any())
            //{
            //    return this.Ok("No Pigeons are found.");
            //}

            var pigeons = this.Data.Pigeons.GetAll().Select(PigeonViewModel.Create);

            return this.Ok(pigeons);
        }

        [HttpPost]
        [Route("api/Pigeon/addpigeon")]
        public IHttpActionResult AddPigeon(PigeonBindigModel inputPigeon)
        {
            var userId = this.User.Identity.GetUserId();

            var pigeonToAdd = new Pigeon()
            {
                User = this.Data.Users.Search(u => u.Id == userId).FirstOrDefault(),
                Content = inputPigeon.Content,
                FavouritedCount = 0,
                Comments = new HashSet<Comment>()
            };

            this.Data.Pigeons.Add(pigeonToAdd);
            this.Data.SaveChanges();

            return this.Ok(new PigeonViewModel(pigeonToAdd));
        }

        [HttpPut]
        [Route("api/Pigeon/updatepigeon/{id}")]
        public IHttpActionResult UpdatePigeon(int id, PigeonBindigModel upPigeon)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(id);

            //When the authorization work
            //if (pigeon.User.Id != userId)
            //{
            //    return this.Unauthorized();
            //}

            pigeon.Content = upPigeon.Content;

            this.Data.SaveChanges();

            return this.Ok(new PigeonViewModel(pigeon));
        }

        [HttpPut]
        [Route("api/Pigeon/fauvorited/{id}")]
        public IHttpActionResult FauvoritedPigeonAdd(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(id);

            //When the authorization work
            //if (pigeon.User.Id != userId)
            //{
            //    return this.Unauthorized();
            //}

            pigeon.FavouritedCount++;

            this.Data.SaveChanges();

            return this.Ok(new PigeonViewModel(pigeon));
        }

        [HttpDelete]
        [Route("api/Pigeon/deletepigeon/{id}")]
        public IHttpActionResult DeletePigeon(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(id);

            //When the authorization work
            //if (pigeon.User.Id != userId)
            //{
            //    return this.Unauthorized();
            //}
            
            this.Data.Pigeons.Delete(pigeon);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
