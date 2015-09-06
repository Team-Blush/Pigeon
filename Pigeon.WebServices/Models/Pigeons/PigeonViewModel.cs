namespace Pigeon.WebServices.Models.Pigeons
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Comments;
    using Pigeon.Models;
    using PhotoUtils;

    public class PigeonViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Photo Photo { get; set; }

        public DateTime CreatedOn { get; set; }

        public int FavouritedCount { get; set; }

        public bool VotedFor { get; set; }

        public bool Favourited { get; set; }

        public PigeonAuthorViewModel Author { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Pigeon, PigeonViewModel>> Create
        {
            get
            {
                return pigeon => new PigeonViewModel
                {
                    Id = pigeon.Id,
                    Title = pigeon.Title,
                    Content = pigeon.Content,
                    Photo = pigeon.Photo,
                    CreatedOn = pigeon.CreatedOn,
                    FavouritedCount = pigeon.FavouritedCount,
                    Author = new PigeonAuthorViewModel
                    {
                        Id = pigeon.Author.Id,
                        Username = pigeon.Author.UserName,
                        FirstName = pigeon.Author.FirstName,
                        LastName = pigeon.Author.LastName,
                        ProfilePhotoData = PhotoUtils.CheckForProfilePhotoData(pigeon.Author).Base64Data
                    },
                    Comments = pigeon.Comments
                        .AsQueryable()
                        .OrderByDescending(c => c.CreatedOn)
                        .Take(3)
                        .Select(CommentViewModel.Create)
                };
            }
        }
    }
}