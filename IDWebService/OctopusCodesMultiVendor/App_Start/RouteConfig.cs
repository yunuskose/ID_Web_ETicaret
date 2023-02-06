using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IDETicaret
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "IDETicaret.Controllers" }
            );
            routes.MapRoute(
                name: "Default2",
                url: "{controller}/{action}/{id}/{seo_text}",
                defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional, seo_text = UrlParameter.Optional },
                namespaces: new[] { "IDETicaret.Controllers" }
            );
            routes.MapRoute(
                name: "Default3",
                url: "{controller}/{action}/{id}/{seo_text}/{kategori1}/{kategori2}/{kategori3}/{kategori4}",
                defaults: new { controller = "Product", action = "Detail",
                    id = UrlParameter.Optional,
                    seo_text = UrlParameter.Optional,
                    kategori1 = UrlParameter.Optional,
                    kategori2 = UrlParameter.Optional,
                    kategori3 = UrlParameter.Optional,
                    kategori4 = UrlParameter.Optional
                },
                namespaces: new[] { "IDETicaret.Controllers" }
            );
        }
    }
}
