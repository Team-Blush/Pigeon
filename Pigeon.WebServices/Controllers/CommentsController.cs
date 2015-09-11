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
        private const string CommentDeletedMessage = "Successfully deleted comment.";
        private const string CommentPostModelInvalidMessage = "Comment post model is null.";
        private const string CommentEditModelInvalidMessage = "Comment update model is null.";

        [HttpGet]
        [Route]
        public IHttpActionResult GetPigeonComments(int pigeonId)
        {
            var pigeon = this.Data.Pigeons.GetAll()
                .FirstOrDefault(p => p.Id == pigeonId);

            if (pigeon == null)
            {
                return this.NotFound();
            }

            var pigeonComments = this.Data.Comments.GetAll()
                .Where(c => c.PigeonId == pigeonId)
                .AsQueryable()
                .OrderBy(c => c.CreatedOn)
                .Select(CommentViewModel.Create);

            return this.Ok(pigeonComments);
        }

        [HttpPost]
        [Route]
        public IHttpActionResult AddCommentToPigeon(int pigeonId, CommentBindingModel inputComment)
        {
            if (inputComment == null)
            {
                return this.BadRequest(CommentPostModelInvalidMessage);
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.GetById(loggedUserId);

            var pigeon = this.Data.Pigeons.GetById(pigeonId);

            if (pigeon == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var commentToAdd = new Comment
            {
                Content = inputComment.Content,
                AuthorId = loggedUserId,
                Author = loggedUser,
                PigeonId = pigeon.Id,
                Pigeon = pigeon,
                CreatedOn = DateTime.Now
            };

            this.Data.Comments.Add(commentToAdd);

            pigeon.Comments.Add(commentToAdd);
            pigeon.CommentsCount++;

            this.Data.Pigeons.Update(pigeon);
            this.Data.SaveChanges();

            var commentViewModel = CommentViewModel.CreateSingle(commentToAdd);

            return this.Ok(commentViewModel);
        }

        [HttpPut]
        [Route("{commentId}")]
        public IHttpActionResult EditPigeonComment(int pigeonId, int commentId, CommentBindingModel inputComment)
        {
            if (inputComment == null)
            {
                return this.BadRequest(CommentEditModelInvalidMessage);
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(pigeonId);

            if (pigeon == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var commentToUpdate = pigeon.Comments
                .FirstOrDefault(c => c.Id == commentId);

            if (commentToUpdate == null)
            {
                return this.NotFound();
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
                return this.NotFound();
            }

            var commentToDelete = pigeon.Comments
                .FirstOrDefault(c => c.Id == commentId);

            if (commentToDelete == null)
            {
                return this.NotFound();
            }

            if (commentToDelete.AuthorId != loggedUserId)
            {
                return this.Unauthorized();
            }

            pigeon.Comments.Remove(commentToDelete);
            pigeon.CommentsCount--;

            this.Data.Comments.Delete(commentToDelete);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                message = CommentDeletedMessage
            });
        }
    }
}