using Microsoft.AspNet.Identity;
using MVC_Blog.Models;
using MVC_Blog.Models.ItemModels;
using MVC_Blog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Blog.Controllers
{
    [RoutePrefix("Recipe")]
    public class RecipeController : Controller
    {
        YemekBlogContext context = new YemekBlogContext();

        [Route("index")]
        [Route("{page:int?}")]
        public ActionResult Index(int page = 1)
        {
            return View(context.Recipes.OrderByDescending(r => r.Date).ToList());
        }

        [Route("details/{seoUrl}")]
        public ActionResult Details(string seoUrl)
        {
            Recipe recipe = context.Recipes.FirstOrDefault(r => r.SeoUrl == seoUrl);
            if (recipe == null)
                return HttpNotFound();
            return View(recipe);
        }

        [Authorize(Roles = "Admin,create")]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Tags = new SelectList(context.Tags, "Id", "Name");
            return View(new RecipeViewModel());
        }

        [Authorize(Roles = "Admin,create")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(RecipeViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Recipe.UserId = User.Identity.GetUserId();
                model.Recipe.Date = DateTime.Now;

                model.Image.SaveAs(Path.Combine(Server.MapPath("~/Content/img/recipe/"), model.Recipe.SeoUrl + Path.GetExtension(model.Image.FileName)));
                model.Recipe.Image = model.Recipe.SeoUrl + Path.GetExtension(model.Image.FileName);
                model.Recipe = context.Recipes.Add(model.Recipe);
                context.SaveChanges();

                foreach (int item in model.TagIds)
                {
                    model.Recipe.Tags.Add(context.Tags.First(t => t.Id == item));
                    context.SaveChanges();
                }

                return RedirectToAction("Index");

            }

            ViewBag.Tags = new SelectList(context.Tags, "Id", "Name");
            return View(model);
        }

        [Authorize(Roles = "Admin, update")]
        [Route("edit/{id:int}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Recipe recipe = context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                return HttpNotFound();

            if (!User.IsInRole("Admin") && recipe.UserId != User.Identity.GetUserId())
                return HttpNotFound();

            ViewBag.Tags = new SelectList(context.Tags, "Id", "Name");
            return View(new RecipeViewModel(recipe));
        }

        [Authorize(Roles = "Admin, update")]
        [Route("edit/{id:int}")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, RecipeViewModel model)
        {
            Recipe updatingRecipe = context.Recipes.FirstOrDefault(r => r.Id == id);
            if (updatingRecipe == null)
                return HttpNotFound();

            if (!User.IsInRole("Admin") && updatingRecipe.UserId != User.Identity.GetUserId())
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                updatingRecipe.Tags.ToList().ForEach(t =>
                {
                    if (!model.TagIds.Contains(t.Id))
                    {
                        updatingRecipe.Tags.Remove(t);
                        context.SaveChanges();
                    }
                });
                model.TagIds.ToList().ForEach(t =>
                {
                    if (!updatingRecipe.Tags.Select(tr => tr.Id).Contains(t))
                    {
                        updatingRecipe.Tags.Add(context.Tags.FirstOrDefault(tag => tag.Id == t));
                        context.SaveChanges();
                    }
                });

                updatingRecipe.CookTime = model.Recipe.CookTime;
                updatingRecipe.Date = model.Recipe.Date;
                updatingRecipe.Description = model.Recipe.Description;
                updatingRecipe.IngredientsContent = model.Recipe.IngredientsContent;
                updatingRecipe.MainContent = model.Recipe.MainContent;
                updatingRecipe.PrepTime = model.Recipe.PrepTime;
                updatingRecipe.RecipeName = model.Recipe.RecipeName;
                updatingRecipe.SeoUrl = model.Recipe.SeoUrl;

                if(model.Image != null)
                {
                    model.Image.SaveAs(Path.Combine(Server.MapPath("~/Content/img/recipe/"), model.Recipe.SeoUrl + Path.GetExtension(model.Image.FileName)));
                    updatingRecipe.Image = model.Recipe.SeoUrl + Path.GetExtension(model.Image.FileName);
                }
                context.SaveChanges();
            }
            ViewBag.Tags = new SelectList(context.Tags, "Id", "Name");
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult SearchMenu()
        {
            return PartialView(context.Tags.ToList());
        }
    }
}