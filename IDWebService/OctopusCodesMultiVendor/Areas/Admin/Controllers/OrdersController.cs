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
    public class OrdersController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {
            try
            {
                return View(ocmde.Orders.OrderByDescending(o => o.CDate).ToList());
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }

        public ActionResult Cards()
        {
            try
            {
                return View(ocmde.Card.OrderByDescending(o => o.Id).ToList());
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
                return View(ocmde.Orders.Where(o => o.Id == id).FirstOrDefault());
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }

        public ActionResult OrderStatuChange(int SipID, int DurumID)
        {
            try
            {
                ocmde.Orders.Where((x) => x.Id == SipID).FirstOrDefault().OrderStatusId = DurumID;
                ocmde.SaveChanges();

                return RedirectToAction("Detail", "Orders", new { id = SipID });                
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }

    }
}