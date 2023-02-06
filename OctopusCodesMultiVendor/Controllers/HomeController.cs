using IDETicaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDETicaret.Helpers;
using System.Configuration;

namespace IDETicaret.Controllers
{
    public class HomeController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();
        
        public ActionResult Index()
        {
            try
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

                var latestProducts = int.Parse(ocmde.Setting.Find(6).Value);
                ViewBag.latestProducts = ocmde.Product.Where(p => p.Status && p.Category4Id != null && p.Photo.Count > 0).OrderByDescending(p => p.Id).Take(latestProducts).ToList();
                
                //Fırsat Ürünleri
                var mostedViewed = int.Parse(ocmde.Setting.Find(7).Value);
                ViewBag.mostedViewedProducts = ocmde.Product.Where(p => p.Status && p.Category4Id != null && p.Photo.Count > 0 && p.HomeProducts.Count > 0).OrderByDescending(p => p.Views).Take(mostedViewed).ToList();
                
                var bestSellers = int.Parse(ocmde.Setting.Find(8).Value);
                var group = ocmde.OrdersDetail.Where((s)=>s.Product.Status && s.Product.Category4Id != null && s.Product.Photo.Count > 0).GroupBy(od => od.ProductId).Select(g => new { g.Key, Sum = g.Sum(od => od.Quantity) }).Take(bestSellers).OrderByDescending(g => g.Sum).ToList();
                var bestSellersProducts = new List<Product>();
                group.ForEach(g =>
                {
                    bestSellersProducts.Add(ocmde.Product.Find(g.Key));
                });
                ViewBag.bestSellersProducts = bestSellersProducts.Take(bestSellers).ToList();

                ViewBag.Slayt = ocmde.Setting.Where((x)=>x.Key.StartsWith("Slayt")).ToList();
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Home", "Index"));
            }
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult BankaHesapBilgileri()
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
            return View(ocmde.Page.Where((p)=>p.Plug == "Banka").FirstOrDefault());
        }

        public ActionResult Blog()
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
            return View(ocmde.Setting.Where((p) => p.Group == "blog").OrderByDescending((o)=>o.Id).ToList());
        }

        public ActionResult BlogDetay(int id, string name)
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
            return View(ocmde.Setting.Where((p) => p.Group == "blog" && p.Id == id).First());
        }
        public ActionResult SiteMap()
        {
            return View(ocmde.Category.Where((k)=> k.Status && k.ParentId == null).ToList());
        }
        public ActionResult SiteMap2(int Kategori1)
        {
            return View(ocmde.Category.Where((k) => k.Status && k.ParentId == Kategori1).ToList());
        }
        public ActionResult SiteMap3(int Kategori1)
        {
            return View(ocmde.Category.Where((k) => k.Status && k.ParentId == Kategori1).ToList());
        }
        public ActionResult SiteMap4(int Kategori1)
        {
            return View(ocmde.Product.Where((k) => k.Status && k.Category3Id == Kategori1).ToList());
        }
    }
}