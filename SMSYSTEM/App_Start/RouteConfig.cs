using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SMSYSTEM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");


            routes.IgnoreRoute("Content/{*pathInfo}");

            routes.IgnoreRoute("Scripts/{*pathInfo}");

            routes.IgnoreRoute("{WebPage}.aspx/{*pathInfo}");

            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");
            // routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
