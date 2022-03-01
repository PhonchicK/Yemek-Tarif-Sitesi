using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(MVC_Blog.App_Start.Startup))]

namespace MVC_Blog.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions() 
            {
                AuthenticationType = "ApplicationAuth",
                LoginPath = new PathString("/User/Login"),
                LogoutPath = new PathString("/User/Logout"),
                ExpireTimeSpan = TimeSpan.FromHours(2.0)
            });
        }
    }
}
