﻿namespace Pigeon.WebServices.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class CommentBindingModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Content { get; set; }
    }
}