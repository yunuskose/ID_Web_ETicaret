using IDETicaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDETicaret.Helpers;

namespace IDETicaret.Controllers
{
    public class HomeController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();
        
        public ActionResult Index()
        {
            try
            {

                if (!Request.IsLocal && !Request.IsSecureConnection)
                {
                    string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
                    Response.Redirect(redirectUrl, false);
                    HttpContext.ApplicationInstance.CompleteRequest();
                }

                var latestProducts = int.Parse(ocmde.Setting.Find(6).Value);
                ViewBag.latestProducts = ocmde.Product.Where(p => p.Status && p.Category4Id != null && p.Photo.Count > 0).OrderByDescending(p => p.Id).Take(latestProducts).ToList();
                
                //Fırsat Ürünleri
                var mostedViewed = int.Parse(ocmde.Setting.Find(7).Value);
                ViewBag.mostedViewedProducts = ocmde.Product.Where(p => p.Status && p.Category4Id != null && p.Photo.Count > 0 && p.HomeProducts.Count > 0).OrderByDescending(p => p.Views).Take(mostedViewed).ToList();
                
                var bestSellers = int.Parse(ocmde.Setting.Find(8).Value);
                var group = ocmde.OrdersDetail.GroupBy(od => od.ProductId).Select(g => new { g.Key, Sum = g.Sum(od => od.Quantity) }).OrderByDescending(g => g.Sum).ToList();
                var bestSellersProducts = new List<Product>();
                group.ForEach(g =>
                {
                    bestSellersProducts.Add(ocmde.Product.Find(g.Key));
                });
                ViewBag.bestSellersProducts = bestSellersProducts.Where(p => p.Status && p.Category4Id != null && p.Photo.Count > 0).Take(bestSellers).ToList();
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
    }
}