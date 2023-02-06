using IDETicaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDETicaret.Helpers;
using System.Configuration;

namespace IDETicaret.Controllers
{
    public class HomeController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();
        
        public ActionResult Index()
        {
            return Redirect("~/Admin/Login/Index");
        }

    }
}