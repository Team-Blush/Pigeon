namespace Pigeon.WebServices.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class PigeonVoteBindingModel
    {
        [Required]
        [Range(0, 1)]
        public VoteValue Value { get; set; }
    }
}