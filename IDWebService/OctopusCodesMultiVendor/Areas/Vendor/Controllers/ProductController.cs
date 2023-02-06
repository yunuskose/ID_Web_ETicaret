using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDETicaret.Security;
using IDETicaret.Models.ViewModels;
using IDETicaret.Models;
using System.IO;

namespace IDETicaret.Areas.Vendor.Controllers
{
    [CustomAuthorize(Roles = "Vendor")]
    public class ProductController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                ViewBag.products = ocmde.Product.Where(c => c.VendorId == vendor.Id).OrderByDescending(o => o.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Index"));
            }
        }

        public ActionResult Category(int id)
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                ViewBag.products = ocmde.Product.Where(c => c.VendorId == vendor.Id && c.CategoryId == id).OrderByDescending(o => o.Id).ToList();
                return View("Index");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Index"));
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;

                var categories = ocmde.Category.Where(c => c.VendorId == vendor.Id || (c.VendorId == null && c.ParentId != null)).Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Group = c.Category2.Name
                }).ToList();

                var productViewModel = new ProductViewModel()
                {
                    product = new Product(),
                    CategoriesMultiLevel = new SelectList(categories, "Id", "Name", "Group", 1)
                };
                return View("Add", productViewModel);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Add"));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(ProductViewModel productViewModel)
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                if (ModelState.IsValid)
                {
                    productViewModel.product.VendorId = vendor.Id;
                    productViewModel.product.Views = 0;
                    ocmde.Product.Add(productViewModel.product);
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                var categories = ocmde.Category.Where(c => c.VendorId == vendor.Id).Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Group = c.Category2.Name
                }).ToList();
                productViewModel.CategoriesMultiLevel = new SelectList(categories, "Id", "Name", "Group", 1);
                return View("Add", productViewModel);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Add"));
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                var categories = ocmde.Category.Where(c => c.VendorId == vendor.Id).Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Group = c.Category2.Name
                }).ToList();

                var productViewModel = new ProductViewModel()
                {
                    product = ocmde.Product.Find(id),
                    CategoriesMultiLevel = new SelectList(categories, "Id", "Name", "Group", 1)
                };
                return View("Edit", productViewModel);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Edit"));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ProductViewModel productViewModel)
        {
            try
            {
                var vendor = (IDETicaret.Models.Vendor)SessionPersister.account;
                if (ModelState.IsValid)
                {
                    var currentProduct = ocmde.Product.Find(productViewModel.product.Id);
                    currentProduct.Name = productViewModel.product.Name;
                    currentProduct.Price = productViewModel.product.Price;
                    currentProduct.Quantity = productViewModel.product.Quantity;
                    currentProduct.Description = productViewModel.product.Description;
                    currentProduct.Status = productViewModel.product.Status;
                    currentProduct.CategoryId = productViewModel.product.CategoryId; 
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                var categories = ocmde.Category.Where(c => c.VendorId == vendor.Id).Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Group = c.Category2.Name
                }).ToList();
                productViewModel.CategoriesMultiLevel = new SelectList(categories, "Id", "Name", "Group", 1);
                return View("Edit", productViewModel);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Edit"));
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var product = ocmde.Product.SingleOrDefault(p => p.Id == id);
                ocmde.Product.Remove(product);
                ocmde.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Delete"));
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

        public ActionResult StatusPhoto(int id)
        {
            try
            {
                var photo = ocmde.Photo.SingleOrDefault(p => p.Id == id);
                photo.Status = !photo.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Photos", "Product", new { id = photo.ProductId });
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "StatusPhoto"));
            }
        }

        public ActionResult Photos(int id)
        {
            try
            {
                ViewBag.product = ocmde.Product.Find(id);
                return View("Photos");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Photos"));
            }
        }

        [HttpGet]
        public ActionResult AddPhoto(int id)
        {
            try
            {
                var photo = new Photo()
                {
                    ProductId = id
                };
                ViewBag.product = ocmde.Product.Find(id);
                return View("AddPhoto", photo);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "AddPhoto"));
            }
        }

        [HttpPost]
        public ActionResult AddPhoto(Photo photo, HttpPostedFileBase name)
        {
            try
            {
                if (name != null && name.ContentLength > 0 && !name.ContentType.Contains("image"))
                {
                    ViewBag.errorPhoto = Resources.Vendor.Photo_Invalid;
                    return View("Profile", photo);
                }
                if (ModelState.IsValid)
                {
                    if (name != null && name.ContentLength > 0 && name.ContentType.Contains("image"))
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(name.FileName);
                        name.SaveAs(Path.Combine(Server.MapPath("~/Content/User/Images"), fileName));
                        photo.Name = fileName;
                    }
                    photo.Main = false;
                    ocmde.Photo.Add(photo);
                    ocmde.SaveChanges();
                    return RedirectToAction("Photos", "Product", new { id = photo.ProductId });
                }
                return View("AddPhoto", photo);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "AddPhoto"));
            }
        }

        public ActionResult DeletePhoto(int id)
        {
            try
            {
                var photo = ocmde.Photo.Find(id);
                var productId = photo.ProductId;
                ocmde.Photo.Remove(photo);
                ocmde.SaveChanges();
                return RedirectToAction("Photos", "Product", new { id = productId });
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "DeletePhoto"));
            }
        }

        [HttpGet]
        public ActionResult EditPhoto(int id)
        {
            try
            {
                var photo = ocmde.Photo.Find(id);
                ViewBag.product = photo.Product;
                return View("EditPhoto", photo);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "EditPhoto"));
            }
        }

        [HttpPost]
        public ActionResult EditPhoto(Photo photo, HttpPostedFileBase name)
        {
            try
            {
                if (name != null && name.ContentLength > 0 && !name.ContentType.Contains("image"))
                {
                    ViewBag.errorPhoto = Resources.Vendor.Photo_Invalid;
                    return View("Profile", photo);
                }
                if (ModelState.IsValid)
                {
                    var currentPhoto = ocmde.Photo.Find(photo.Id);
                    if (name != null && name.ContentLength > 0 && name.ContentType.Contains("image"))
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(name.FileName);
                        name.SaveAs(Path.Combine(Server.MapPath("~/Content/User/Images"), fileName));
                        currentPhoto.Name = fileName;
                    }
                    currentPhoto.Main = photo.Main;
                    currentPhoto.Status = photo.Status;
                    ocmde.SaveChanges();
                    return RedirectToAction("Photos", "Product", new { id = photo.ProductId });
                }
                return View("EditPhoto", photo);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "EditPhoto"));
            }
        }

        public ActionResult MainPhoto(int id)
        {
            try
            {
                var product = ocmde.Photo.Find(id).Product;
                product.Photo.ToList().ForEach(p => {
                    var photo = ocmde.Photo.Find(p.Id);
                    photo.Main = false;
                    ocmde.SaveChanges();
                });
                var mainPhoto = ocmde.Photo.Find(id);
                mainPhoto.Main = true;
                ocmde.SaveChanges();
                return RedirectToAction("Photos", "Product", new { id = product.Id });
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "MainPhoto"));
            }
        }

    }
}