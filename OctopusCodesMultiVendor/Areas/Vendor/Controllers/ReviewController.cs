﻿using IDETicaret.Models;
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
    public class ReviewController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                ViewBag.reviews = ocmde.Review.Where(r => r.VendorId == vendor.Id).OrderByDescending(r => r.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Review", "Index"));
            }
        }

        public ActionResult Customer(int id)
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                ViewBag.reviews = ocmde.Review.Where(r => r.VendorId == vendor.Id && r.CustomerId == id).OrderByDescending(r => r.Id).ToList();
                return View("Index");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Review", "Index"));
            }
        }

    }
}