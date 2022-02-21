using MVC_Blog.Models.ItemModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_Blog.Models
{
    public class YemekBlogContext : DbContext
    {
        public YemekBlogContext() : base("YemekTarifBlogDb")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()//For Many To Many Relationship
                .HasMany(r => r.Tags)
                .WithMany(t => t.Recipes)
                .Map(m => 
                {
                    m.MapLeftKey("RecipeId");
                    m.MapRightKey("TagId");
                    m.ToTable("RecipeTags");
                });
        }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}