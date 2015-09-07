namespace Pigeon.WebServices.Models.Pigeons
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Comments;
    using Pigeon.Models;
    using Pigeon.Models.Enumerations;
    using Users;

    public class PigeonViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PhotoData { get; set; }

        public DateTime CreatedOn { get; set; }

        public VoteValue Voted { get; set; }

        public int UpVotesCount { get; set; }

        public int DownVotesCount { get; set; }

        public bool Favourited { get; set; }

        public int FavouritedCount { get; set; }

        public AuthorViewModel Author { get; set; }

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
                    PhotoData = pigeon.Photo != null ? pigeon.Photo.Base64Data : null,
                    CreatedOn = pigeon.CreatedOn,
                    UpVotesCount = pigeon.Votes.Count(c => c.Value == VoteValue.Up),
                    DownVotesCount = pigeon.Votes.Count(c => c.Value == VoteValue.Down),
                    FavouritedCount = pigeon.FavouritedCount,
                    Author = new AuthorViewModel
                    {
                        Username = pigeon.Author.UserName,
                        ProfilePhotoData =
                            pigeon.Author.ProfilePhotos
                                .FirstOrDefault(photo => photo.ProfilePhotoFor == pigeon.Author) != null ?
                            pigeon.Author.ProfilePhotos
                                .FirstOrDefault(photo => photo.ProfilePhotoFor == pigeon.Author).Base64Data : null
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