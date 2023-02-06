using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDETicaret.Security;
using IDETicaret.Models.ViewModels;
using IDETicaret.Models;


namespace IDETicaret.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {
            try
            {
                ViewBag.products = ocmde.Product.OrderByDescending(o => o.Id).ToList().Take(500);
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Index"));
            }
        }

        public ActionResult Status(int id)
        {
            try
            {
                var product = ocmde.Product.SingleOrDefault(p => p.Id == id);
                product.Status = !product.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Status"));
            }
        }

    }
}