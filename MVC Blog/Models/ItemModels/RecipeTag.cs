using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Blog.Models.ItemModels
{
    [Table("RecipeTags")]
    public class RecipeTag
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }

        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}