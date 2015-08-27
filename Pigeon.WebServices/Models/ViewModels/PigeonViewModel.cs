namespace Pigeon.WebServices.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Pigeon.Models;

    public class PigeonViewModel
    {
        public PigeonViewModel(Pigeon pigeonDbModel)
        {
            this.Title = pigeonDbModel.Title;
            this.Content = pigeonDbModel.Content;
            this.FavouritedCount = pigeonDbModel.FavouritedCount;
            this.Author = new UserViewModel
            {
                Name = pigeonDbModel.Author.FirstName + " " + pigeonDbModel.Author.LastName,
                Email = pigeonDbModel.Author.Email
            };
            this.Comments = pigeonDbModel.Comments.Select(c => new CommentViewModel
            {
                Content = c.Content,
                Author = new UserViewModel
                {
                    Name = c.Author.FirstName + " " + c.Author.LastName,
                    Email = c.Author.Email
                }
            });
        }

        public PigeonViewModel()
        {
        }

        public string Title { get; set; }

        public string Content { get; set; }

        public int FavouritedCount { get; set; }

        //Additional functionality by developers preferences
        public UserViewModel Author { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Pigeon, PigeonViewModel>> Create
        {
            get
            {
                return p => new PigeonViewModel
                {
                    Title = p.Title,
                    Content = p.Content,
                    FavouritedCount = p.FavouritedCount,
                    Author = new UserViewModel
                    {
                        Name = p.Author.FirstName + " " + p.Author.LastName,
                        Email = p.Author.Email
                    },
                    Comments = p.Comments.Select(c => new CommentViewModel
                    {
                        Content = c.Content,
                        Author = new UserViewModel
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