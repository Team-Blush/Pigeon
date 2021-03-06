﻿namespace Pigeon.WebServices.Controllers
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
        private const string InvalidVoteMessage = "You can vote positively or negatively once per Pigeon.";
        private const string InvalidPigeonUpdateInputDataMessage = "Pigeon update model is null.";
        private const string InvalidPigeonPostInputDataMessage = "Pigeon model is null.";
        private const string InvalidPigeonVoteInputDataMessage = "Vote model is null.";
        private const string DuplicatingFavouritePigeonMessage = "Pigeon already favourited.";
        private const string FavouriteUnfavouritedPigeonMessage = "Cannot unfavourite a non-favourite Pigeon.";
        private const string PigeonFavouritedSuccessfullyMessage = "Successfully favourited Pigeon.";
        private const string PigeonUnavouritedSuccessfullyMessage = "Successfully unfavourited Pigeon.";
        private const string PigeonDeletedSuccessfullyMessage = "Successfully deleted pigeon.";

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

            var news = this.Data.Pigeons.GetAll()
                .Where(p => p.Author.Followers.Any(f => f.Id == loggedUserId))
                .OrderByDescending(p => p.CreatedOn)
                .Take(5)
                .Select(PigeonViewModel.Create(loggedUserId));

            return this.Ok(news);
        }

        // GET api/pigeons/favourites
        [HttpGet]
        [Route("favourites")]
        [EnableQuery]
        public IHttpActionResult GetUserFavouritePigeons()
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var favouritePigeons = this.Data.Pigeons.GetAll()
                .Where(p => p.FavouritedBy.Any(fb => fb.Id == loggedUserId))
                .OrderByDescending(p => p.CreatedOn)
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
                return this.NotFound();
            }

            return this.Ok(pigeon);
        }

        // POST api/pigeons
        [HttpPost]
        [Route]
        public IHttpActionResult AddPigeon(PigeonBindingModel inputPigeon)
        {
            if (inputPigeon == null)
            {
                return this.BadRequest(InvalidPigeonPostInputDataMessage);
            }

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
            if (voteModel == null)
            {
                return this.BadRequest(InvalidPigeonVoteInputDataMessage);
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(id);

            if (pigeon == null)
            {
                return this.NotFound();
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
                    return this.BadRequest(InvalidVoteMessage);
                }

                if ((existingVote.Value == VoteValue.Up || existingVote.Value == VoteValue.None)
                    && voteModel.Value == VoteValue.Down)
                {
                    existingVote.Value = VoteValue.Down;
                    pigeon.DownVotesCount++;
                    pigeon.UpVotesCount--;
                    this.Data.Votes.Update(existingVote);
                }

                if ((existingVote.Value == VoteValue.Down || existingVote.Value == VoteValue.None)
                    && voteModel.Value == VoteValue.Up)
                {
                    existingVote.Value = VoteValue.Up;
                    pigeon.DownVotesCount--;
                    pigeon.UpVotesCount++;
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
                if (voteModel.Value == VoteValue.Up)
                {
                    pigeon.UpVotesCount++;
                }
                else
                {
                    pigeon.DownVotesCount++;
                }
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
            if (updatedPigeon == null)
            {
                return this.BadRequest(InvalidPigeonUpdateInputDataMessage);
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var pigeonToUpdate = this.Data.Pigeons.GetById(id);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (pigeonToUpdate == null)
            {
                return this.NotFound();
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
                return this.NotFound();
            }

            if (loggedUser.FavouritePigeons.Any(p => p.Id.Equals(id)))
            {
                return this.BadRequest(DuplicatingFavouritePigeonMessage);
            }

            pigeonToFavourite.FavouritedBy.Add(loggedUser);
            loggedUser.FavouritePigeons.Add(pigeonToFavourite);

            pigeonToFavourite.FavouritedCount++;

            this.Data.Pigeons.Update(pigeonToFavourite);
            this.Data.Users.Update(loggedUser);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = PigeonFavouritedSuccessfullyMessage
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
                return this.NotFound();
            }

            if (!loggedUser.FavouritePigeons.Any(p => p.Id.Equals(id)))
            {
                return this.BadRequest(FavouriteUnfavouritedPigeonMessage);
            }

            pigeonToUnfavourite.FavouritedBy.Remove(loggedUser);
            loggedUser.FavouritePigeons.Remove(pigeonToUnfavourite);

            pigeonToUnfavourite.FavouritedCount--;

            this.Data.Pigeons.Update(pigeonToUnfavourite);
            this.Data.Users.Update(loggedUser);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = PigeonUnavouritedSuccessfullyMessage
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
                return this.NotFound();
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
                message = PigeonDeletedSuccessfullyMessage
            });
        }
    }
}