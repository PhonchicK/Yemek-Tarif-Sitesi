using MVC_Blog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Blog.Filters
{
    public class TagsValidate : ValidationAttribute
    {
        private readonly YemekBlogContext context;
        public TagsValidate()
        {
            context = new YemekBlogContext();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int[] tags = value as int[];

            foreach (int item in tags)
            {
                if(context.Tags.FirstOrDefault(t => t.Id == item) == null)
                {
                    return new ValidationResult("Tag error.");
                }
            }

            return ValidationResult.Success;
        }
    }
}