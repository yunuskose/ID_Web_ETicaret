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
    public class CategoryController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                ViewBag.categories = ocmde.Category.Where(c => c.VendorId == vendor.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Category", "Index"));
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                CategoryViewModel categoryViewModel = new CategoryViewModel()
                {
                    category = new Category()
                    {
                        Status = true
                    },
                    Parent = ocmde.Category.Where(c => c.ParentId == null).Select(c => new SelectListItem()
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList()
                };
                return View("Add", categoryViewModel);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Category", "Add"));
            }
        }

        [HttpPost]
        public ActionResult Add(CategoryViewModel categoryViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                    if (categoryViewModel.category.ParentId == -1)
                    {
                        categoryViewModel.category.ParentId = null;
                    }
                    categoryViewModel.category.VendorId = vendor.Id;
                    ocmde.Category.Add(categoryViewModel.category);
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("Add", categoryViewModel);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Category", "Add"));
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var category = ocmde.Category.SingleOrDefault(c => c.Id == id);
                ocmde.Category.Remove(category);
                ocmde.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Category", "Delete"));
            }
        }

        public ActionResult Status(int id)
        {
            try
            {
                var category = ocmde.Category.SingleOrDefault(c => c.Id == id);
                category.Status = !category.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Category", "Status"));
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                CategoryViewModel categoryViewModel = new CategoryViewModel()
                {
                    category = ocmde.Category.SingleOrDefault(c => c.Id == id),
                    Parent = ocmde.Category.Where(c => c.ParentId == null).Select(c => new SelectListItem()
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList()
                };
                return View("Edit", categoryViewModel);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Category", "Edit"));
            }
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel categoryViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentCategory = ocmde.Category.SingleOrDefault(c => c.Id == categoryViewModel.category.Id);
                    if (categoryViewModel.category.ParentId == -1)
                    {
                        currentCategory.ParentId = null;
                    }
                    else
                    {
                        currentCategory.ParentId = categoryViewModel.category.ParentId;
                    }
                    currentCategory.Name = categoryViewModel.category.Name;
                    currentCategory.Status = categoryViewModel.category.Status;
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                categoryViewModel.Parent = ocmde.Category.Where(c => c.ParentId == null).Select(c => new SelectListItem()
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();
                return View("Edit", categoryViewModel);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Category", "Edit"));
            }
        }

    }
}