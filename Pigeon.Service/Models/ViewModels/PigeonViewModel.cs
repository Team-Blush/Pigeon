using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;


namespace Pigeon.Service.Models.ViewModels
{
    using Pigeon.Models;
    public class PigeonViewModel
    {
        public PigeonViewModel(Pigeon p)
        {
            this.Content = p.Content;
            this.FavouritedCount = p.FavouritedCount;
            //User = new UserViewModel()
            //{
            //    Name = p.User.FirstName + " " + p.User.LastName,
            //    Email = p.User.Email
            //},
            this.Comments = p.Comments.Select(c => new CommentViewModel()
            {
                Content = c.Content,
                Owner = new UserViewModel()
                {
                    Name = c.Author.FirstName + " " + c.Author.LastName,
                    Email = c.Author.Email
                }
            });
        }

        public PigeonViewModel()
        {

        }

        public string Content { get; set; }

        public int FavouritedCount { get; set; }

        //Additional functionality by developers preferences
        //public UserViewModel User { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Pigeon, PigeonViewModel>> Create
        {
            get
            {
                return p => new PigeonViewModel()
                {
                    Content = p.Content,
                    FavouritedCount = p.FavouritedCount,
                    //User = new UserViewModel()
                    //{
                    //    Name = p.User.FirstName + " " + p.User.LastName,
                    //    Email = p.User.Email
                    //},
                    Comments = p.Comments.Select(c => new CommentViewModel()
                    {
                        Content = c.Content,
                        Owner = new UserViewModel()
                        {
                            Name = c.Author.FirstName + " " + c.Author.LastName,
                            Email = c.Author.Email
                        }
                    })
                };
            }
        }
    }
}