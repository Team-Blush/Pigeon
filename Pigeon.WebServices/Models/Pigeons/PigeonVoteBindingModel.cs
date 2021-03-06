namespace Pigeon.WebServices.Models.Pigeons
{
    using System.ComponentModel.DataAnnotations;
    using Pigeon.Models.Enumerations;

    public class PigeonVoteBindingModel
    {
        [Required]
        [Range(-1, 1)]
        public VoteValue Value { get; set; }
    }
}