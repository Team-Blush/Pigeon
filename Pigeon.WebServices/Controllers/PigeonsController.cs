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
            var user = this.Data.Users.GetAll()
                .FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return this.NotFound();
            }

            var pigeons = this.Data.Pigeons.GetAll()
                .Where(pigeon => pigeon.AuthorId == user.Id)
                .OrderByDescending(pigeon => pigeon.CreatedOn)
                .Take(5)
                .Select(PigeonViewModel.Create);

            if (!pigeons.Any())
            {
                return this.Ok(new
                {
                    message = "No Pigeons are found."
                });
            }

            return this.Ok(pigeons);
        }

        // GET api/pigeons/news
        [HttpGet]
        [Route("news")]
        [EnableQuery]
        public IHttpActionResult GetNewsPigeons()
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userId);
            if (!user.Followers.Any())
            {
                return this.Ok(new
                {
                    message = "Follow someone to see their Pigeons."
                });
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

        // GET api/pigeons/favourites
        [HttpGet]
        [Route("favourites")]
        [EnableQuery]
        public IHttpActionResult GetUserFavouritePigeons()
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userId);

            var favouritePigeons = user.FavouritePigeons
                .AsQueryable()
                .Select(PigeonViewModel.Create)
                .ToList();

            if (!favouritePigeons.Any())
            {
                return this.Ok(new
                {
                    message = "You have no favourite pigeons at this time."
                });
            }

            return this.Ok(favouritePigeons);
        }

        // GET api/pigeons/{id}
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

        // POST api/pigeons
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

            var pigeonViewModel = this.Data.Pigeons.GetAll()
                .Where(p => p.Id == pigeonToAdd.Id)
                .Select(PigeonViewModel.Create)
                .FirstOrDefault();

            return this.Ok(pigeonViewModel);
        }

        // POST api/pigeons/{id}/vote
        [HttpPost]
        [Route("{id}/vote")]
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
                    Value = voteModel.Value == VoteValue.Up,
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

        // PUT api/pigeons/{id}/favourite
        [HttpPut]
        [Route("{id}/favourite")]
        public IHttpActionResult FavouritePigeon(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userId);

            var pigeonToFavourite = this.Data.Pigeons.GetById(id);

            if (pigeonToFavourite == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (pigeonToFavourite.Author.Id == userId)
            {
                return this.BadRequest("You cannot favourite your own Pigeon.");
            }

            if (user.FavouritePigeons.Contains(pigeonToFavourite))
            {
                return this.BadRequest("Pigeon already favourited.");
            }

            pigeonToFavourite.FavouritedBy.Add(user);
            user.FavouritePigeons.Add(pigeonToFavourite);

            pigeonToFavourite.FavouritedCount++;

            this.Data.Pigeons.Update(pigeonToFavourite);
            this.Data.Users.Update(user);
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
            var userId = this.User.Identity.GetUserId();
            var user = this.Data.Users.GetById(userId);
            var pigeonToUnfavourite = this.Data.Pigeons.GetById(id);

            if (pigeonToUnfavourite == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (pigeonToUnfavourite.Author.Id == userId)
            {
                return this.BadRequest("You cannot unfavourite your own Pigeon.");
            }

            if (!user.FavouritePigeons.Contains(pigeonToUnfavourite))
            {
                return this.BadRequest("Cannot unfavourite a non-favourite Pigeon.");
            }

            pigeonToUnfavourite.FavouritedBy.Remove(user);
            user.FavouritePigeons.Remove(pigeonToUnfavourite);

            pigeonToUnfavourite.FavouritedCount--;

            this.Data.Pigeons.Update(pigeonToUnfavourite);
            this.Data.Users.Update(user);
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
                if (pigeon.Photo.CoverPhotoFor == null &&
                    pigeon.Photo.ProfilePhotoFor == null)
                {
                    this.Data.Photos.Delete(pigeon.Photo);
                }
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