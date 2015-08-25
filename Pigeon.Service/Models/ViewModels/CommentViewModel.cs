using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pigeon.Models;

namespace Pigeon.Service.Models.ViewModels
{
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