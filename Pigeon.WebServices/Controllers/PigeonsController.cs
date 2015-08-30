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

            var pigeonPhotoData = inputPigeon.ImageData;
            if (pigeonPhotoData != null)
            {
                var photo = new Photo { Base64Data = inputPigeon.ImageData };

                this.Data.Photos.Add(photo);
                pigeonToAdd.Photo = photo;
            }

            this.Data.Pigeons.Add(pigeonToAdd);
            this.Data.SaveChanges();

            return this.Ok(new PigeonViewModel(pigeonToAdd));
        }

        [HttpPut]
        [Route("update/{id}")]
        public IHttpActionResult UpdatePigeon(int id, PigeonBindingModel updatedPigeon)
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

            pigeon.Content = updatedPigeon.Content;

            this.Data.Pigeons.Update(pigeon);
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

            this.Data.Pigeons.Update(pigeon);
            this.Data.SaveChanges();

            return this.Ok(new PigeonViewModel(pigeon));
        }

        [HttpPost]
        [Route("vote/{id}")]
        public IHttpActionResult VoteForPigeon(int id, PigeonVoteBindingModel voteModel)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons
                .Search(p => p.Id == id)
                .FirstOrDefault();

            if (pigeon == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid vote value.");
            }

            var existingVote = this.Data.Votes
                .Search(v => v.UserId == userId && v.PigeonId == id)
                .FirstOrDefault();

            if (existingVote != null)
            {
                if ((existingVote.Value && voteModel.Value == VoteValue.Up) ||
                    (!existingVote.Value && voteModel.Value == VoteValue.Down))
                {
                    return this.BadRequest("You can vote positively once per pigeon.");
                }

                if (existingVote.Value && voteModel.Value == VoteValue.Down)
                {
                    existingVote.Value = false;
                    this.Data.Votes.Update(existingVote);
                }

                if (!existingVote.Value && voteModel.Value == VoteValue.Up)
                {
                    existingVote.Value = true;
                    this.Data.Votes.Update(existingVote);
                }
            }
            else
            {
                var vote = new PigeonVote
                {
                    UserId = userId,
                    PigeonId = pigeon.Id,
                    Pigeon = pigeon,
                    Value = voteModel.Value == VoteValue.Up
                };

                pigeon.Votes.Add(vote);
                this.Data.Votes.Add(vote);
            }

            this.Data.Pigeons.Update(pigeon);
            this.Data.SaveChanges();

            return this.Ok("Successfully voted for pigeon.");
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