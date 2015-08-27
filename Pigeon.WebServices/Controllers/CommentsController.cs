namespace Pigeon.WebServices.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using Pigeon.Models;

    [Authorize]
    [RoutePrefix("api/pigeons/{pigeonId}/comments")]
    public class CommentsController : BaseApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult GetPigeonComments(int pigeonId)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons
                .Search(p => p.Author.Id == userId)
                .FirstOrDefault();

            if (pigeon == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            var pigeonComments = pigeon.Comments.AsQueryable().Select(CommentViewModel.Create);

            if (!pigeonComments.Any())
            {
                return this.Ok("No comments for the Pigeon.");
            }

            return this.Ok(pigeonComments);
        }

        [HttpPost]
        [Route]
        public IHttpActionResult AddCommentToPigeon(int pigeonId, CommentBindingModel inputComment)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons
                .Search(p => p.Id == pigeonId)
                .FirstOrDefault();

            if (pigeon == null)
            {
                return this.BadRequest("No such Pigeon.");
            }

            var commentToAdd = new Comment
            {
                Content = inputComment.Content,
                AuthorId = userId,
                PigeonId = pigeon.Id
            };

            this.Data.Comments.Add(commentToAdd);
            pigeon.Comments.Add(commentToAdd);

            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpPut]
        [Route("{commentId}")]
        public IHttpActionResult UpdatePigeonComment(int pigeonId, int commentId, CommentBindingModel inputComment)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons
                .Search(p => p.Id == pigeonId)
                .FirstOrDefault();

            if (pigeon == null)
            {
                return this.BadRequest("No such pigeon.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid comment to add.");
            }

            var commentToUpdate = pigeon.Comments
                .FirstOrDefault(c => c.Id == commentId);

            if (commentToUpdate == null)
            {
                return this.BadRequest("No such comment.");
            }

            if (commentToUpdate.AuthorId != userId)
            {
                return this.Unauthorized();
            }

            commentToUpdate.Content = inputComment.Content;

            this.Data.SaveChanges();

            return this.Ok("Comment updated successfully.");
        }

        [HttpDelete]
        [Route("{commentId}")]
        public IHttpActionResult DeletePigeonComment(int pigeonId, int commentId)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons
                .Search(p => p.Id == pigeonId)
                .FirstOrDefault();

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

            if (commentToDelete.AuthorId != userId)
            {
                return this.Unauthorized();
            }

            this.Data.Comments.Delete(commentToDelete);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}