using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Paranovels.Mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;
            routes.AppendTrailingSlash = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            // for seo 
            routes.MapRoute(
                name: "SEO",
                url: "{controller}/{action}/{seo}/{id}",
                defaults: new {}
            );

            // default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            // for /m
            routes.MapRoute(
                name: "M",
                url: "m",
                defaults: new {}
                );
            // for /mobile
            routes.MapRoute(
                name: "Mobile",
                url: "mobile",
                defaults: new {}
                );
        }
    }
}
