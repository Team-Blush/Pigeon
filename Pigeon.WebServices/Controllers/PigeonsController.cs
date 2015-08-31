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
    using Pigeon.Models.Enumerations;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/pigeons")]
    public class PigeonsController : BaseApiController
    {
        [HttpGet]
        [Route("own")]
        [EnableQuery]
        public IHttpActionResult GetUserOwnPigeons()
        {
            var userId = this.User.Identity.GetUserId();

            var pigeons = this.Data.Pigeons
                .Search(p => p.Author.Id == userId)
                .OrderByDescending(p => p.CreatedOn)
                .Take(10)
                .Select(PigeonViewModel.Create);

            if (!pigeons.Any())
            {
                return this.Ok("No Pigeons are found.");
            }

            return this.Ok(pigeons);
        }

        [HttpGet]
        [Route("news")]
        [EnableQuery]
        public IHttpActionResult GetUserNewsPigeons()
        {
            var userId = this.User.Identity.GetUserId();

            var user = this.Data.Users.GetAll().First(u => u.Id == userId);

            if (!user.Followers.Any())
            {
                return this.Ok("Follow someone to see their Pigeons.");
            }

            var newsPigeons = this.Data.Pigeons.GetAll()
                .Where(p => user.Following.Select(uf => uf.Id).Contains(p.AuthorId))
                .OrderByDescending(p => p.CreatedOn)
                .Take(10)
                .Select(PigeonViewModel.Create);

            if (!newsPigeons.Any())
            {
                return this.Ok("No Pigeons found.");
            }

            return this.Ok(newsPigeons);
        }

        [HttpGet]
        [Route("{id}")]
        [EnableQuery]
        public IHttpActionResult GetPigeonById(int id)
        {
            var pigeon = this.Data.Pigeons.GetAll()
                .Where(p => p.Id == id)
                .Select(PigeonViewModel.Create)
                .FirstOrDefault();

            if (pigeon == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            return this.Ok(pigeon);
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
                var photo = new Photo {Base64Data = inputPigeon.ImageData};

                this.Data.Photos.Add(photo);
                pigeonToAdd.Photo = photo;
            }

            this.Data.Pigeons.Add(pigeonToAdd);
            this.Data.SaveChanges();

            var pigeonViewModel = this.Data.Pigeons.GetAll()
                .Where(p => p.Id == pigeonToAdd.Id)
                .Select(PigeonViewModel.Create)
                .FirstOrDefault();

            return this.Ok(pigeonViewModel);
        }

        [HttpPut]
        [Route("update/{id}")]
        public IHttpActionResult UpdatePigeon(int id, PigeonBindingModel updatedPigeon)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeonToUpdate = this.Data.Pigeons.GetById(id);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid Pigeon data.");
            }

            if (pigeonToUpdate == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (pigeonToUpdate.Author.Id != userId)
            {
                return this.Unauthorized();
            }

            pigeonToUpdate.Content = updatedPigeon.Content;

            this.Data.Pigeons.Update(pigeonToUpdate);
            this.Data.SaveChanges();

            var pigeonViewModel = this.Data.Pigeons.GetAll()
                .Where(p => p.Id == pigeonToUpdate.Id)
                .Select(PigeonViewModel.Create)
                .FirstOrDefault();

            return this.Ok(pigeonViewModel);
        }

        [HttpPut]
        [Route("favourite/{id}")]
        public IHttpActionResult AddPigeonToFavourited(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeonToUpdate = this.Data.Pigeons.GetById(id);

            if (pigeonToUpdate == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (pigeonToUpdate.Author.Id == userId)
            {
                return this.BadRequest("You cannot favourite your own Pigeon.");
            }

            pigeonToUpdate.FavouritedCount++;

            this.Data.Pigeons.Update(pigeonToUpdate);
            this.Data.SaveChanges();

            var pigeonViewModel = this.Data.Pigeons.GetAll()
                .Where(p => p.Id == pigeonToUpdate.Id)
                .Select(PigeonViewModel.Create)
                .FirstOrDefault();

            return this.Ok(pigeonViewModel);
        }

        [HttpPost]
        [Route("vote/{id}")]
        public IHttpActionResult VoteForPigeon(int id, PigeonVoteBindingModel voteModel)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(id);

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

            return this.Ok("Successfully deleted pigeon.");
        }
    }
}