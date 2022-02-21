using Microsoft.AspNet.Identity;
using MVC_Blog.Models;
using MVC_Blog.Models.ItemModels;
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

        [Authorize(Roles = "Admin,Chef")]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Tags = new SelectList(context.Tags, "Id", "Name");
            return View(new Recipe());
        }

        [Authorize(Roles = "Admin,Chef")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Recipe recipe, int[] tags, HttpPostedFileBase image)
        {
            if (image == null)
            {
                ModelState.AddModelError("image", "Image can't be empty.");
            }
            else
            {
                if (image.ContentLength > 5000000)
                {
                    ModelState.AddModelError("image", "Image size must be lower than 5mb.");
                }
            }

            ModelState.Remove("Tags");
            if (tags == null)
            {
                ModelState.AddModelError("tags", "Please select tag.");
            }

            if (ModelState.IsValid)
            {
                recipe.UserId = User.Identity.GetUserId();
                recipe.Date = DateTime.Now;

                image.SaveAs(Path.Combine(Server.MapPath("~/Content/img/recipe/"), recipe.SeoUrl + Path.GetExtension(image.FileName)));
                recipe.Image = recipe.SeoUrl + Path.GetExtension(image.FileName);
                recipe = context.Recipes.Add(recipe);
                context.SaveChanges();

                foreach (int item in tags)
                {
                    recipe.Tags.Add(context.Tags.First(t => t.Id == item));
                    context.SaveChanges();
                }

                return RedirectToAction("Index");

            }

            ViewBag.Tags = new SelectList(context.Tags, "Id", "Name");
            return View(recipe);
        }
    }
}