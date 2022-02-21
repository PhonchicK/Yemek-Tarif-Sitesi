using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC_Blog.Identity;
using MVC_Blog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace MVC_Blog.Controllers
{
    public class UserController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        public UserController()
        {
            userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationIdentityContext()));
        }

        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginViewModel()) ;
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            if(ModelState.IsValid)
            {
                var user = userManager.Find(login.Username, login.Password);
                if(user != null)
                {
                    var identity = userManager.CreateIdentity(user, "ApplicationAuth");
                    authenticationManager.SignIn(new AuthenticationProperties() 
                    {
                        IsPersistent = login.RememberMe
                    }, identity);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(login);
        }
        #endregion
        #region Register
        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel register)
        {
            if(ModelState.IsValid)
            {
                var result = userManager.Create(new ApplicationUser()
                {
                    FullName = register.FullName,
                    UserName = register.Username,
                    Email = register.Email,
                    Description = register.Description
                }, register.Password);

                if (result.Succeeded)//If Register succeded redirect to login page.
                {
                    userManager.AddToRole(userManager.FindByName(register.Username).Id, "Beginner");
                    return RedirectToAction("Login");
                }
            }
            return View(register);
        }
        #endregion
    }
}