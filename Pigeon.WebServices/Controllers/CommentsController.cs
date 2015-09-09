namespace Pigeon.WebServices.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models.Comments;
    using Pigeon.Models;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/pigeons/{pigeonId}/comments")]
    public class CommentsController : BaseApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult GetPigeonComments(int pigeonId)
        {
            var pigeonComments = this.Data.Pigeons.GetAll()
                .Where(p => p.Id == pigeonId)
                .Select(p => p.Comments
                    .AsQueryable()
                    .OrderBy(c => c.CreatedOn)
                    .Select(CommentViewModel.Create))
                .FirstOrDefault();

            if (pigeonComments == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            return this.Ok(pigeonComments);
        }

        [HttpPost]
        [Route]
        public IHttpActionResult AddCommentToPigeon(int pigeonId, CommentBindingModel inputComment)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.GetById(loggedUserId);

            var pigeon = this.Data.Pigeons.GetById(pigeonId);

            if (pigeon == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var commentToAdd = new Comment
            {
                Content = inputComment.Content,
                AuthorId = loggedUserId,
                PigeonId = pigeon.Id,
                CreatedOn = DateTime.Now
            };

            this.Data.Comments.Add(commentToAdd);
            pigeon.Comments.Add(commentToAdd);

            this.Data.SaveChanges();

            var commentViewModel = CommentViewModel.CreateSingle(commentToAdd);

            return this.Ok(commentViewModel);
        }

        [HttpPut]
        [Route("{commentId}")]
        public IHttpActionResult EditPigeonComment(int pigeonId, int commentId, CommentBindingModel inputComment)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(pigeonId);

            if (pigeon == null)
            {
                return this.BadRequest("No such pigeon.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var commentToUpdate = pigeon.Comments
                .FirstOrDefault(c => c.Id == commentId);

            if (commentToUpdate == null)
            {
                return this.BadRequest("No such comment.");
            }

            if (commentToUpdate.AuthorId != loggedUserId)
            {
                return this.Unauthorized();
            }

            commentToUpdate.Content = inputComment.Content;

            this.Data.SaveChanges();

            return this.Ok(new
            {
                commentToUpdate.Id,
                commentToUpdate.Content
            });
        }

        [HttpDelete]
        [Route("{commentId}")]
        public IHttpActionResult DeletePigeonComment(int pigeonId, int commentId)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(pigeonId);

            if (pigeon == null)
            {
                return this.BadRequest("No such pigeon.");
            }

            var commentToDelete = pigeon.Comments
                .FirstOrDefault(c => c.Id == commentId);

            if (commentToDelete == null)
            {
                return this.BadRequest("No such comment.");
            }

            if (commentToDelete.AuthorId != loggedUserId)
            {
                return this.Unauthorized();
            }

            this.Data.Comments.Delete(commentToDelete);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = "Successfully deleted comment."
            });
        }
    }
}