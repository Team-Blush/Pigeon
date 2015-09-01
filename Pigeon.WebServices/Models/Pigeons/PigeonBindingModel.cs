namespace Pigeon.WebServices.Models.Pigeons
{
    using System.ComponentModel.DataAnnotations;

    public class PigeonBindingModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(150)]
        public string Content { get; set; }

        public string ImageData { get; set; }
    }
}