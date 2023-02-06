using IDETicaret.Models;
using IDETicaret.Models.ViewModels;
using IDETicaret.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDETicaret.Controllers
{
    [CustomAuthorize(Roles = "Customer")]
    public class OrdersController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Liste()
        {
            if (ConfigurationManager.AppSettings["ciftteker"] == "1")
            {
                if (!Request.IsLocal && !Request.IsSecureConnection)
                {
                    string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
                    Response.Redirect(redirectUrl, false);
                    HttpContext.ApplicationInstance.CompleteRequest();
                }
                if (!Request.IsLocal && !Request.Url.ToString().Contains("www.ciftteker.com"))
                {
                    string redirectUrl = Request.Url.ToString().Replace("ciftteker.com", "www.ciftteker.com"); // "https://www.ciftteker.com";
                    Response.Redirect(redirectUrl, false);
                    HttpContext.ApplicationInstance.CompleteRequest();
                }
            }
            try
            {
                var customer = (IDETicaret.Models.Account)SessionPersister.account;
                ViewBag.orders = ocmde.Orders.Where(o => o.CustomerId == customer.Id && o.OrderStatus.Name != "Sil").OrderByDescending(o => o.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Liste"));
            }
        }

        public ActionResult Detail(int id)
        {
            if (ConfigurationManager.AppSettings["ciftteker"] == "1")
            {
                if (!Request.IsLocal && !Request.IsSecureConnection)
                {
                    string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
                    Response.Redirect(redirectUrl, false);
                    HttpContext.ApplicationInstance.CompleteRequest();
                }
                if (!Request.IsLocal && !Request.Url.ToString().Contains("www.ciftteker.com"))
                {
                    string redirectUrl = Request.Url.ToString().Replace("ciftteker.com", "www.ciftteker.com"); // "https://www.ciftteker.com";
                    Response.Redirect(redirectUrl, false);
                    HttpContext.ApplicationInstance.CompleteRequest();
                }
            }
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