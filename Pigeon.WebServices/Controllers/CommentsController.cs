namespace Pigeon.WebServices.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Pigeon.Models;

    [Authorize]
    public class CommentsController : BaseApiController
    {
        [HttpPost]
        [Route("api/pigeon/{id}/comments")]
        public IHttpActionResult AddCommnetToPigeon(int id, CommentBindingModel inputComment)
        {
            var userId = this.User.Identity.GetUserId();
            var pigeon = this.Data.Pigeons.GetById(id);

            var commentToAdd = new Comment()
            {
                Content = inputComment.Content,
                // Author = this.Data.Users.Search(u => u.Id == userId).FirstOrDefault(),
                AuthorId = userId,
                // Pigeon = pigeon,
                PigeonId = pigeon.Id
            };

            this.Data.Comments.Add(commentToAdd);
            pigeon.Comments.Add(commentToAdd);

            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpPut]
        [Route("api/comments/{id}")]
        public IHttpActionResult UpdateComment(int id, CommentBindingModel inputComment)
        {
            var userId = this.User.Identity.GetUserId();
            var commentToUpdate = this.Data.Comments.GetById(id);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid comment to add.");
            }

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

            return this.Ok();
        }

        [HttpDelete]
        [Route("api/comments/{id}")]
        public IHttpActionResult DeleteComment(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var commentToDelete = this.Data.Comments
                .Search(c => c.Id == id)
                .FirstOrDefault();

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
