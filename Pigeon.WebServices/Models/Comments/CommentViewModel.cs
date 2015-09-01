namespace Pigeon.WebServices.Models.Comments
{
    using System;
    using System.Linq.Expressions;
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
                return p => new CommentViewModel
                {
                    Id = p.Id,
                    Content = p.Content,
                    CreatedOn = p.CreatedOn,
                    Author = new CommentAuthorViewModel
                    {
                        Id = p.AuthorId,
                        Username = p.Author.UserName,
                        FirstName = p.Author.FirstName,
                        LastName = p.Author.LastName
                    }
                };
            }
        }
    }
}