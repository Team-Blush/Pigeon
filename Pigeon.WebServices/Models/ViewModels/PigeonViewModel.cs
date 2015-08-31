namespace Pigeon.WebServices.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Pigeon.Models;

    public class PigeonViewModel
    {
        public PigeonViewModel()
        {
        }

        public PigeonViewModel(Pigeon pigeonDbModel)
        {
            this.Title = pigeonDbModel.Title;
            this.Content = pigeonDbModel.Content;
            this.CreatedOn = pigeonDbModel.CreatedOn;
            this.FavouritedCount = pigeonDbModel.FavouritedCount;
            this.Author = new UserViewModel(pigeonDbModel.Author);
            this.Comments = pigeonDbModel.Comments.AsQueryable().Select(CommentViewModel.Create);
        }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public int FavouritedCount { get; set; }

        //Additional functionality by developers preferences
        public UserViewModel Author { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Pigeon, PigeonViewModel>> Create
        {
            get { return p => new PigeonViewModel(p); }
        }
    }
}