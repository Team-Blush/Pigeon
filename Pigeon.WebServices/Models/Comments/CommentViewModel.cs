namespace Pigeon.WebServices.Models.Comments
{
    using System;
    using System.Linq.Expressions;
    using PhotoUtils;
    using Pigeon.Models;

    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public CommentAuthorViewModel Author { get; set; }

        public static Expression<Func<Comment, CommentViewModel>> Create
        {
            get
            {
                return comment => new CommentViewModel
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreatedOn = comment.CreatedOn,
                    Author = new CommentAuthorViewModel
                    {
                        Id = comment.AuthorId,
                        Username = comment.Author.UserName,
                        FirstName = comment.Author.FirstName,
                        LastName = comment.Author.LastName,
                        ProfilePhotoData = PhotoUtils.CheckForProfilePhotoData(comment.Author).Base64Data
                    }
                };
            }
        }
    }
}