namespace Pigeon.WebServices.Models.Comments
{
    using System;
    using System.Linq;
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
                        comment.Author.ProfilePhotos
                            .FirstOrDefault(photo => photo.ProfilePhotoFor == comment.Author) != null ?
                        comment.Author.ProfilePhotos
                            .FirstOrDefault(photo => photo.ProfilePhotoFor == comment.Author).Base64Data : null
                    }
                };
            }
        }
    }
}