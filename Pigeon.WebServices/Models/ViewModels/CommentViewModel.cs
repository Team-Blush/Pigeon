namespace Pigeon.WebServices.Models.ViewModels
{
    using Pigeon.Models;

    public class CommentViewModel
    {
        public CommentViewModel()
        {
            
        }

        public CommentViewModel(Comment c)
        {
            this.Content = c.Content;
            this.Owner = new UserViewModel()
            {
                Name = c.Author.FirstName + " " + c.Author.LastName,
                Email = c.Author.Email
            };
        }

        public string Content { get; set; }

        public UserViewModel Owner { get; set; }
    }
}