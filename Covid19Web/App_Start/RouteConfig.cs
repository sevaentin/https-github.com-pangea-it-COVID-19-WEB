using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Covid19Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "GetImages",
               url: "home/GetImages/{id}",
               defaults: new { controller = "Home", action = "GetImages" }
            );

            routes.MapRoute(
               name: "GetStatus",
               url: "home/GetStatus/{id}",
               defaults: new { controller = "Home", action = "GetStatus" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
