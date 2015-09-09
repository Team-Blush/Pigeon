namespace Pigeon.WebServices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.OData;
    using Microsoft.AspNet.Identity;
    using Models.Pigeons;
    using Pigeon.Models;
    using Pigeon.Models.Enumerations;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/pigeons")]
    public class PigeonsController : BaseApiController
    {
        // GET api/pigeons/{username}/all
        [HttpGet]
        [Route("{username}/all")]
        [EnableQuery]
        public IHttpActionResult GetUserPigeons(string username)
        {
            var targetUser = this.Data.Users.GetAll()
                .FirstOrDefault(u => u.UserName == username);

            var loggedUserId = this.User.Identity.GetUserId();

            if (targetUser == null)
            {
                return this.NotFound();
            }

            var pigeons = this.Data.Pigeons.GetAll()
                .Where(pigeon => pigeon.AuthorId == targetUser.Id)
                .OrderByDescending(pigeon => pigeon.CreatedOn)
                .Take(5)
                .Select(PigeonViewModel.Create(loggedUserId));

            return this.Ok(pigeons);
        }

        // GET api/pigeons/news
        [HttpGet]
        [Route("news")]
        [EnableQuery]
        public IHttpActionResult GetNewsPigeons()
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.GetById(loggedUserId);

            var newsPigeons = loggedUser.Following
                .Select(f => f.Pigeons
                    .AsQueryable()
                    .OrderByDescending(p => p.CreatedOn)
                    .Take(3)
                    .Select(PigeonViewModel.Create(loggedUserId)));

            return this.Ok(newsPigeons);
        }

        // GET api/pigeons/favourites
        [HttpGet]
        [Route("favourites")]
        [EnableQuery]
        public IHttpActionResult GetUserFavouritePigeons()
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.GetById(loggedUserId);

            var favouritePigeons = loggedUser.FavouritePigeons
                .AsQueryable()
                .OrderByDescending(p => p.CreatedOn)
                .Take(5)
                .Select(PigeonViewModel.Create(loggedUserId));

            return this.Ok(favouritePigeons);
        }

        // GET api/pigeons/{id}
        [HttpGet]
        [Route("{id}")]
        [EnableQuery]
        public IHttpActionResult GetPigeonById(int id)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var pigeon = this.Data.Pigeons.GetAll()
                .Where(p => p.Id == id)
                .Select(PigeonViewModel.Create(loggedUserId))
                .FirstOrDefault();

            if (pigeon == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            return this.Ok(pigeon);
        }

        // POST api/pigeons
        [HttpPost]
        [Route]
        public IHttpActionResult AddPigeon(PigeonBindingModel inputPigeon)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var pigeonToAdd = new Pigeon
            {
                Title = inputPigeon.Title,
                Content = inputPigeon.Content,
                Author = this.Data.Users.Search(u => u.Id == loggedUserId).FirstOrDefault(),
                AuthorId = loggedUserId,
                CreatedOn = DateTime.Now,
                Comments = new HashSet<Comment>(),
                Votes = new HashSet<PigeonVote>()
            };

            if (inputPigeon.PhotoData != null)
            {
                var photo = new Photo { Base64Data = inputPigeon.PhotoData };

                this.Data.Photos.Add(photo);
                pigeonToAdd.Photo = photo;
            }

            this.Data.Pigeons.Add(pigeonToAdd);
            this.Data.SaveChanges();

            var pigeonViewModel = PigeonViewModel.CreateSingle(pigeonToAdd);

            return this.Ok(pigeonViewModel);
        }

        // POST api/pigeons/{id}/vote
        [HttpPost]
        [Route("{id}/vote")]
        public IHttpActionResult VoteForPigeon(int id, PigeonVoteBindingModel voteModel)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(id);

            if (pigeon == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingVote = this.Data.Votes
                .Search(v => v.UserId == loggedUserId && v.PigeonId == id)
                .FirstOrDefault();

            if (existingVote != null)
            {
                if ((existingVote.Value == VoteValue.Up && voteModel.Value == VoteValue.Up) ||
                    (existingVote.Value == VoteValue.Down && voteModel.Value == VoteValue.Down))
                {
                    return this.BadRequest("You can vote positively or negatively once per Pigeon.");
                }

                if ((existingVote.Value == VoteValue.Up || existingVote.Value == VoteValue.None)
                    && voteModel.Value == VoteValue.Down)
                {
                    existingVote.Value = VoteValue.Down;
                    this.Data.Votes.Update(existingVote);
                }

                if ((existingVote.Value == VoteValue.Down || existingVote.Value == VoteValue.None)
                    && voteModel.Value == VoteValue.Up)
                {
                    existingVote.Value = VoteValue.Up;
                    this.Data.Votes.Update(existingVote);
                }
            }
            else
            {
                var vote = new PigeonVote
                {
                    UserId = loggedUserId,
                    PigeonId = pigeon.Id,
                    Value = voteModel.Value,
                    VotedOn = DateTime.Now
                };

                this.Data.Votes.Add(vote);
                pigeon.Votes.Add(vote);
            }

            this.Data.Pigeons.Update(pigeon);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = "Successfully voted for pigeon."
            });
        }

        // PUT api/pigeons/{id}/update
        [HttpPut]
        [Route("{id}/edit")]
        public IHttpActionResult EditPigeon(int id, PigeonBindingModel updatedPigeon)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var pigeonToUpdate = this.Data.Pigeons.GetById(id);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (pigeonToUpdate == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (pigeonToUpdate.Author.Id != loggedUserId)
            {
                return this.Unauthorized();
            }

            pigeonToUpdate.Content = updatedPigeon.Content;

            this.Data.Pigeons.Update(pigeonToUpdate);
            this.Data.SaveChanges();

            var pigeonViewModel = new { pigeonToUpdate.Content };

            return this.Ok(pigeonViewModel);
        }

        // PUT api/pigeons/{id}/favourite
        [HttpPut]
        [Route("{id}/favourite")]
        public IHttpActionResult FavouritePigeon(int id)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.GetById(loggedUserId);

            var pigeonToFavourite = this.Data.Pigeons.GetById(id);

            if (pigeonToFavourite == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (loggedUser.FavouritePigeons.Contains(pigeonToFavourite))
            {
                return this.BadRequest("Pigeon already favourited.");
            }

            pigeonToFavourite.FavouritedBy.Add(loggedUser);
            loggedUser.FavouritePigeons.Add(pigeonToFavourite);

            pigeonToFavourite.FavouritedCount++;

            this.Data.Pigeons.Update(pigeonToFavourite);
            this.Data.Users.Update(loggedUser);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = "Successfully favourited Pigeon."
            });
        }

        // PUT api/pigeons/{id}/unfavourite
        [HttpPut]
        [Route("{id}/unfavourite")]
        public IHttpActionResult UnFavouritePigeon(int id)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.GetById(loggedUserId);

            var pigeonToUnfavourite = this.Data.Pigeons.GetById(id);

            if (pigeonToUnfavourite == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (!loggedUser.FavouritePigeons.Contains(pigeonToUnfavourite))
            {
                return this.BadRequest("Cannot unfavourite a non-favourite Pigeon.");
            }

            pigeonToUnfavourite.FavouritedBy.Remove(loggedUser);
            loggedUser.FavouritePigeons.Remove(pigeonToUnfavourite);

            pigeonToUnfavourite.FavouritedCount--;

            this.Data.Pigeons.Update(pigeonToUnfavourite);
            this.Data.Users.Update(loggedUser);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = "Successfully unfavourited Pigeon."
            });
        }

        // DELETE api/pigeons/{id}
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeletePigeon(int id)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(id);

            if (pigeon == null)
            {
                return this.BadRequest("No such pigeon");
            }

            if (pigeon.Author.Id != loggedUserId)
            {
                return this.Unauthorized();
            }

            // Delete pigeon comments
            foreach (var comment in pigeon.Comments.ToList())
            {
                this.Data.Comments.Delete(comment);
            }

            // Delete pigeon votes
            foreach (var vote in pigeon.Votes.ToList())
            {
                this.Data.Votes.Delete(vote);
            }

            // Delete pigeon photo if it is no one's cover or profile photo
            if (pigeon.Photo != null)
            {
                this.Data.Photos.Delete(pigeon.Photo);
            }

            this.Data.Pigeons.Delete(pigeon);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = "Successfully deleted pigeon."
            });
        }
    }
}