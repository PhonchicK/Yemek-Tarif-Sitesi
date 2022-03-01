using MVC_Blog.Models;
using MVC_Blog.Models.ItemModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC_Blog.Identity;

namespace MVC_Blog.Controllers
{
    public class HomeController : Controller
    {
        YemekBlogContext context = new YemekBlogContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}