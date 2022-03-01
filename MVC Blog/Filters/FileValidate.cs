using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace MVC_Blog.Filters
{
    public class FileValidate : ValidationAttribute
    {
        private readonly List<string> _types;
        public int FileSize { get; set; }

        public FileValidate(string types, int fileSize)
        {
            _types = types.Split(',').Select(t => "." + t).ToList();
            FileSize = fileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;
            if (file != null)
            {
                if (!_types.Contains(Path.GetExtension(file.FileName)))
                {
                    return new ValidationResult("File must be an image.");
                }

                if (file.ContentLength > FileSize)
                {
                    return new ValidationResult("File size must be lower than " + FileSize.ToString() + " bytes");
                }
            }
            return ValidationResult.Success;
        }
    }
}