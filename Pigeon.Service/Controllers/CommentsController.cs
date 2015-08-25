using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Pigeon.Models;
using Pigeon.Service.Models.BindingModels;
using Pigeon.Service.Models.ViewModels;

namespace Pigeon.Service.Controllers
{
    //[Authorize] //When the account system work all of the pigeons should be accessible from registert users
    public class CommentsController : BaseApiController
    {
        [HttpPost]
        [Route("api/Comments/pigeon/{id}/comment")]
        public IHttpActionResult AddCommnetToPigeon(int id, CommentBindingModel inputComment)
        {
            //var userId = this.User.Identity.GetUserId();
            string userId = null; // for test purposes, must be corrected in the Comment Model
            var pigeon = this.Data.Pigeons.GetById(id);

            var commentToAdd = new Comment()
            {
                Content = inputComment.Content,
                Author = this.Data.Users.Search(u => u.Id == userId).FirstOrDefault(),
                AuthorId = userId,
                Pigeon = pigeon,
                PigeonId = pigeon.Id
            };

            this.Data.Comments.Add(commentToAdd);
            pigeon.Comments.Add(commentToAdd);

            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpPut]
        [Route("api/Comments/updatecomment/{id}")]
        public IHttpActionResult UpdateComment(int id, CommentBindingModel inputComment)
        {
            var userId = this.User.Identity.GetUserId();
            var commentToUpdate = this.Data.Comments.GetById(id);

            //When the authorization work
            //if (commentToUpdate.Author.Id != userId)
            //{
            //    return this.Unauthorized();
            //}

            commentToUpdate.Content = inputComment.Content;

            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpDelete]
        [Route("api/Comments/deletecomment/{id}")]
        public IHttpActionResult DeleteComment(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var commentToDelete = this.Data.Comments.GetById(id);

            //When the authorization work
            //if (commentToUpdate.Author.Id != userId)
            //{
            //    return this.Unauthorized();
            //}

            this.Data.Comments.Delete(commentToDelete);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
