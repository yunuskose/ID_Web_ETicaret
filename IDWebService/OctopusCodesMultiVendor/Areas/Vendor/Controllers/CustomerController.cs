using IDETicaret.Models;
using IDETicaret.Models.ViewModels;
using IDETicaret.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDETicaret.Areas.Vendor.Controllers
{
    [CustomAuthorize(Roles = "Vendor")]
    public class CustomerController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                var customerIds = ocmde.Vendor.Find(vendor.Id).Orders.Select(o => o.CustomerId).Distinct().ToList();
                var customers = new List<Account>();
                customerIds.ForEach(id => {
                    customers.Add(ocmde.Account.Find(id));
                });
                ViewBag.customers = customers;
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Customer", "Index"));
            }
        }

        

    }
}