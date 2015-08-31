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
            this.CreatedOn = commentDbModel.CreatedOn;
            this.Author = new UserViewModel(commentDbModel.Author);
        }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public UserViewModel Author { get; set; }

        public static Expression<Func<Comment, CommentViewModel>> Create
        {
            get { return p => new CommentViewModel(p); }
        }
    }
}