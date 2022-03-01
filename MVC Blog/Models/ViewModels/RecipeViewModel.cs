using MVC_Blog.Models.ItemModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVC_Blog.Filters;

namespace MVC_Blog.Models.ViewModels
{
    public class RecipeViewModel
    {
        public RecipeViewModel()
        {
            this.Recipe = new Recipe();
        }
        public RecipeViewModel(Recipe recipe)
        {
            this.Recipe = recipe;
            this.SelectedIds = recipe.Tags.Select(t => t.Id).ToArray();
        }
        public int[] SelectedIds { get; set; }
        public Recipe Recipe { get; set; }

        [FileValidate("png,jpg,jpeg", 5000000)]
        public HttpPostedFileBase Image { get; set; }
    }
}