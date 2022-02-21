using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Blog.Models.ItemModels
{
    public class Recipe
    {
        public Recipe()
        {
            Tags = new List<Tag>();
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        public string SeoUrl { get; set; }
        [Required]
        public string RecipeName { get; set; }
        [Required]
        [AllowHtml]
        public string Description { get; set; }
        [Required]
        [AllowHtml]
        public string IngredientsContent { get; set; }
        [Required]
        [AllowHtml]
        public string MainContent { get; set; }
        public string Image { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public int PrepTime { get; set; }
        [Required]
        public int CookTime { get; set; }

        public virtual List<Tag> Tags { get; set; }
    }
}