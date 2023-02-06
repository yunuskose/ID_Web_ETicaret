using IDETicaret.Models;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace IDETicaret
{
    public class MvcApplication : System.Web.HttpApplication
    {

        

        protected void Application_Start()
        {


            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            ETicaretEntities db = new ETicaretEntities();

            Application["Categories"] = db.Category.Where(c => 
                c.Status &&
                (c.Product.Where((p) => p.Status == true).Count() > 0 ||
                c.Product1.Where((p1) => p1.Status == true).Count() > 0 ||
                c.Product2.Where((p2) => p2.Status == true).Count() > 0 ||
                c.Product3.Where((p3) => p3.Status == true).Count() > 0)
            ).OrderBy((o)=>o.Name).ToList();
            
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }
        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var language = "en";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
        }
    }
}
