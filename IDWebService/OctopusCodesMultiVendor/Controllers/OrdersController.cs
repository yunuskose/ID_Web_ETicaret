using IDETicaret.Models;
using IDETicaret.Models.ViewModels;
using IDETicaret.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDETicaret.Controllers
{
    [CustomAuthorize(Roles = "Customer")]
    public class OrdersController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {
            try
            {
                var customer = (IDETicaret.Models.Account)SessionPersister.account;
                ViewBag.orders = ocmde.Orders.Where(o => o.CustomerId == customer.Id).OrderByDescending(o => o.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }

        public ActionResult Detail(int id)
        {
            try
            {
                ViewBag.order = ocmde.Orders.Find(id);
                ViewBag.OrderID = ocmde.Orders.Find(id).OrderID;
                return View("Detail");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Detail"));
            }
        }

    }
}