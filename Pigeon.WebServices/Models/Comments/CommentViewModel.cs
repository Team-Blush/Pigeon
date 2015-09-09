namespace Pigeon.WebServices.Models.Comments
{
    using System;
    using System.Linq.Expressions;
    using Pigeon.Models;
    using Users;

    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public AuthorViewModel Author { get; set; }

        public static Expression<Func<Comment, CommentViewModel>> Create
        {
            get
            {
                return comment => new CommentViewModel
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreatedOn = comment.CreatedOn,
                    Author = new AuthorViewModel
                    {
                        Username = comment.Author.UserName,
                        ProfilePhotoData =
                            comment.Author.ProfilePhoto != null ? comment.Author.ProfilePhoto.Base64Data : null
                    }
                };
            }
        }
    }
}