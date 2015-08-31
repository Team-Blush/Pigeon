namespace Pigeon.WebServices.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Pigeon.Models;

    public class PigeonViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public int FavouritedCount { get; set; }

        //Additional functionality by developers preferences
        public PigeonAuthorViewModel Author { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Pigeon, PigeonViewModel>> Create
        {
            get
            {
                return p => new PigeonViewModel
                {
                    Title = p.Title,
                    Content = p.Content,
                    CreatedOn = p.CreatedOn,
                    FavouritedCount = p.FavouritedCount,
                    Author = new PigeonAuthorViewModel
                    {
                        Id = p.Author.Id,
                        Username = p.Author.UserName,
                        FirstName = p.Author.FirstName,
                        LastName = p.Author.LastName,
                        ProfilePhotoData = p.Author.ProfilePhotos
                            .FirstOrDefault(pp => pp.ProfilePhotoFor == p.Author).Base64Data
                    },
                    Comments = p.Comments.AsQueryable().Select(CommentViewModel.Create)
                };
            }
        }
    }
}