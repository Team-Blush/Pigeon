namespace Pigeon.WebServices.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using Pigeon.Models;

    public class CommentViewModel
    {
        public CommentViewModel()
        {
        }

        public CommentViewModel(Comment commentDbModel)
        {
            this.Content = commentDbModel.Content;
            this.Author = new UserViewModel
            {
                Name = commentDbModel.Author.FirstName + " " + commentDbModel.Author.LastName,
                Email = commentDbModel.Author.Email
            };
        }

        public string Content { get; set; }

        public UserViewModel Author { get; set; }

        public static Expression<Func<Comment, CommentViewModel>> Create
        {
            get
            {
                return p => new CommentViewModel
                {
                    Content = p.Content,
                    Author = new UserViewModel
                    {
                        Name = p.Author.FirstName + " " + p.Author.LastName,
                        Email = p.Author.Email
                    }
                };
            }
        }
    }
}