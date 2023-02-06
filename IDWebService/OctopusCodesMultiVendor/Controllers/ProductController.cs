using IDETicaret.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDETicaret.Helpers;
using System.Data.Entity.Core.Objects;

namespace IDETicaret.Controllers
{
    public class ProductController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        [HttpGet]
        public ActionResult AdvancedSearch()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(int page = 1, int pageSize = 20)
        {
            try
            {
                pageSize = int.Parse(ocmde.Setting.Find(9).Value);
                string keyword = Request.QueryString.Get("keyword");
                int kategori1 = Convert.ToInt32(Request.QueryString.Get("kategori1"));
                int kategori2 = Convert.ToInt32(Request.QueryString.Get("kategori2"));
                int kategori3 = Convert.ToInt32(Request.QueryString.Get("kategori3"));
                int kategori4 = Convert.ToInt32(Request.QueryString.Get("kategori4"));

                List<Product> model = ocmde.f_StokAra(keyword, kategori1.ToString(), kategori2.ToString(), kategori3.ToString(), kategori4.ToString(), page).ToList();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_AramaSonucu", model);
                }
                else
                {
                    //PagedList<Product> model = new PagedList<Product>(
                    //    ocmde.Product
                    //         .Where(p => (p.Name.Contains(keyword) || (p.Category.Id == kategori1 || p.Category1.Id == kategori2 || p.Category2.Id == kategori3)) && p.Status)
                    //         .OrderBy((o) => o.Id), page, pageSize
                    //);
                }

                ViewBag.keyword = keyword;
                ViewBag.kategori1 = kategori1;
                ViewBag.kategori2 = kategori2;
                ViewBag.kategori3 = kategori3;
                ViewBag.kategori4 = kategori4;
                return View("Search", model);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Search"));
            }
        }

        
        [HttpGet]
        public ActionResult Search2(int page = 1, int pageSize = 20, string keyword = "", int kategori1 = 0, int kategori2 = 0, int kategori3 = 0, int kategori4 = 0)
        {
            try
            {
                //pageSize = int.Parse(ocmde.Setting.Find(9).Value);

                keyword = keyword.Trim();
                if (Request.IsAjaxRequest())
                {
                    List<Product> model = ocmde.f_StokAra(keyword, kategori1.ToString(), kategori2.ToString(), kategori3.ToString(), kategori4.ToString(), page).ToList();

                    return PartialView("_AramaSonucu", model);
                }
                else
                {
                    keyword = Request.QueryString.Get("keyword");
                    kategori1 = Convert.ToInt32(Request.QueryString.Get("kategori1"));
                    kategori2 = Convert.ToInt32(Request.QueryString.Get("kategori2"));
                    kategori3 = Convert.ToInt32(Request.QueryString.Get("kategori3"));
                    kategori4 = Convert.ToInt32(Request.QueryString.Get("kategori4"));

                    List<Product> model = ocmde.f_StokAra(keyword, kategori1.ToString(), kategori2.ToString(), kategori3.ToString(), kategori4.ToString(), page).ToList();

                    ViewBag.keyword = keyword;
                    ViewBag.kategori1 = kategori1;
                    ViewBag.kategori2 = kategori2;
                    ViewBag.kategori3 = kategori3;
                    ViewBag.kategori4 = kategori4;
                    return View("Search2", model);
                }

            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Search2"));
            }
        }

        public ActionResult Category(int id, int page = 1, int pageSize = 9)
        {
            try
            {
                pageSize = int.Parse(ocmde.Setting.Find(9).Value);
                List<Product> listProducts = ocmde.Product.Where(p => p.CategoryId == id && p.Status).ToList();
                var products = new List<Product>();
                listProducts.ForEach(p =>
                {
                    if (VendorHelper.checkExpires(p.VendorId))
                    {
                        products.Add(p);
                    }
                });
                PagedList<Product> model = new PagedList<Product>(products, page, pageSize);
                ViewBag.category = ocmde.Category.Find(id);
                TempData["categorySelected"] = ocmde.Category.Find(id).Category2.Id;
                return View("Category", model);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Category"));
            }
        }

        public ActionResult Expires()
        {
            return View("Expires");
        }

        [Route("Product/Detail/{id}/{seo_text}")]
        public ActionResult Detail(int id, string seo_text)
        {
            try
            {
                var product = ocmde.Product.Find(id);
                /////if (!VendorHelper.checkExpires(product.VendorId))
                /////{
                /////    return RedirectToAction("Expires", "Product");
                /////}
                /////else
                {
                    product.Views = product.Views + 1;
                    ocmde.SaveChanges();
                    ViewBag.product = product;
                    ViewBag.relatedProducts = ocmde.Product.Where(p => p.Id != id && p.CategoryId == product.CategoryId && p.VendorId == product.VendorId).Take(6).ToList();
                    return View("Detail");
                }
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Product", "Detail"));
            }
        }
    }
}