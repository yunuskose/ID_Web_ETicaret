using IDETicaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDETicaret.Controllers
{
    public class PageController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        [HttpGet]
        public ActionResult Detail(string id)
        {
            try
            {
                ViewBag.page = ocmde.Page.SingleOrDefault(p => p.Plug.Equals(id));
                return View("Index");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Page", "Detail"));
            }
        }

        
    }
}