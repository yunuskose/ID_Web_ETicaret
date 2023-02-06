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
    public class MessageController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                ViewBag.messages = ocmde.Message.Where(m => m.VendorId == vendor.Id).OrderByDescending(m => m.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Message", "Index"));
            }
        }

        

    }
}