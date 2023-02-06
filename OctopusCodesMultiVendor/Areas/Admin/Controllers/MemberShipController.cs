using IDETicaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDETicaret.Security;
using IDETicaret.Helpers;

namespace IDETicaret.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class MemberShipController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {
            try
            {
                ViewBag.memberships = ocmde.MemberShip.ToList();
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "MemberShip", "Index"));
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                return View("Add", new MemberShip());
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "MemberShip", "Add"));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(MemberShip memberShip)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ocmde.MemberShip.Add(memberShip);
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("Add", memberShip);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "MemberShip", "Add"));
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                if (id == MemberShipHelper.TrialPackage)
                {
                    TempData["error"] = Resources.Customer.Can_not_delete_Trial_Package;
                    return RedirectToAction("Index");
                }
                else
                {
                    var membership = ocmde.MemberShip.SingleOrDefault(m => m.Id == id);
                    ocmde.MemberShip.Remove(membership);
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "MemberShip", "Delete"));
            }
        }

        public ActionResult Status(int id)
        {
            try
            {
                var membership = ocmde.MemberShip.SingleOrDefault(m => m.Id == id);
                membership.Status = !membership.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "MemberShip", "Status"));
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var membership = ocmde.MemberShip.SingleOrDefault(m => m.Id == id);
                return View("Edit", membership);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "MemberShip", "Edit"));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(MemberShip memberShip)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentMemberShip = ocmde.MemberShip.Find(memberShip.Id
    );
                    currentMemberShip.Name = memberShip.Name;
                    currentMemberShip.Price = memberShip.Price;
                    currentMemberShip.Status = memberShip.Status;
                    currentMemberShip.Description = memberShip.Description;
                    currentMemberShip.Month = memberShip.Month;
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("Edit", memberShip);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "MemberShip", "Edit"));
            }
        }

    }
}