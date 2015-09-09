namespace Pigeon.WebServices.Models.Pigeons
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using PhotoUtils;
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

        public int CommentsCount { get; set; }

        public static Expression<Func<Pigeon, PigeonViewModel>> CreateExpr(User currentUser)
        {
            return pigeon => new PigeonViewModel
            {
                Id = pigeon.Id,
                Title = pigeon.Title,
                Content = pigeon.Content,
                PhotoData = pigeon.Photo != null ? pigeon.Photo.Base64Data : null,
                CreatedOn = pigeon.CreatedOn,
                Voted =
                    currentUser.Votes.Any(v => v.UserId == currentUser.Id)
                        ? currentUser.Votes.FirstOrDefault(v => v.UserId == currentUser.Id).Value
                        : VoteValue.None,
                UpVotesCount = pigeon.Votes.Count(c => c.Value == VoteValue.Up),
                DownVotesCount = pigeon.Votes.Count(c => c.Value == VoteValue.Down),
                FavouritedCount = pigeon.FavouritedCount,
                Favourited = currentUser.FavouritePigeons.Any(p => p.Id == pigeon.Id),
                Author = new AuthorViewModel
                {
                    Username = pigeon.Author.UserName,
                    ProfilePhotoData = pigeon.Author.ProfilePhoto != null ? pigeon.Author.ProfilePhoto.Base64Data : null
                },
                CommentsCount = pigeon.Comments.Count
            };
        }

        public static PigeonViewModel CreateSingle(Pigeon pigeonDbModel)
        {
            return new PigeonViewModel
            {
                Id = pigeonDbModel.Id,
                Title = pigeonDbModel.Title,
                Content = pigeonDbModel.Content,
                PhotoData = PhotoUtils.CheckForPhotoData(pigeonDbModel.Photo),
                CreatedOn = pigeonDbModel.CreatedOn,
                Author = new AuthorViewModel
                {
                    Username = pigeonDbModel.Author.UserName,
                    ProfilePhotoData =
                        pigeonDbModel.Author.ProfilePhoto != null ? pigeonDbModel.Author.ProfilePhoto.Base64Data : null
                }
            };
        }
    }
}