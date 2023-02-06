﻿using IDETicaret.Models;
using IDETicaret.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDETicaret.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Group(string id)
        {
            try
            {
                ViewBag.settings = ocmde.Setting.Where(g => g.Group.Equals(id)).ToList();
                return View("Index");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Setting", "Group"));
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var setting = ocmde.Setting.SingleOrDefault(p => p.Id == id);
                return View("Edit", setting);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Setting", "Edit"));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Setting setting, HttpPostedFileBase logo)
        {
            try
            {
                if (logo != null && logo.ContentLength > 0 && !logo.ContentType.Contains("image"))
                {
                    ViewBag.errorPhoto = Resources.Vendor.Photo_Invalid;
                    return View("Edit", ocmde.Setting.Find(setting.Id));
                }

                if (ModelState.IsValid)
                {   
                    var currentSetting = ocmde.Setting.Find(setting.Id);
                    if (logo != null && logo.FileName.Length != 0)
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(logo.FileName);
                        logo.SaveAs(Path.Combine(Server.MapPath("~/Content/User/Images"), fileName));
                        currentSetting.Value = fileName;
                    }
                    else
                    {
                        currentSetting.Value = setting.Value;
                    }
                    ocmde.SaveChanges();
                    return RedirectToAction("Group", "Setting", new { id = currentSetting.Group });
                }
                return View("Edit", setting);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Setting", "Edit"));
            }
        }

        

    }
}