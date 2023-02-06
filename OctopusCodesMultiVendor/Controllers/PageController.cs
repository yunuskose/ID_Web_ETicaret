using IDETicaret.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDETicaret.Controllers
{
    public class PageController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        [HttpGet]
        public ActionResult Detail(string id)
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
                ViewBag.page = ocmde.Page.SingleOrDefault(p => p.Plug.Equals(id));
                return View("Index");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Page", "Detail"));
            }
        }
        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
        public ActionResult Xml(int id)
        {


            Loglar l = new Loglar();
            l.Tip = "Xml";
            l.Aciklama = id.ToString();
            l.Tarih = DateTime.Now;
            l.Kullanici = GetIp();
            ocmde.Loglar.Add(l);
            ocmde.SaveChanges();

            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9'>
";
            List<Product> urunler = ocmde.Product.Where((x) => x.CategoryId == id).ToList();
            foreach (Product item in urunler)
            {
                xml += @"
<url>
<loc>https://www.ciftteker.com/"+item.url+@"</loc>
<lastmod>"+DateTime.Today.ToString("yyyy-MM-dd")+@"</lastmod>
<priority>0</priority>
<unite>"+item.OlcuBirimi+ @"</unite>
<category1>"+item.Category.Name+@"</category1>
<category2>"+item.Category1.Name+@"</category2>
<category3>"+item.Category2.Name+@"</category3>
<code>"+item.Code+@"</code>
<title>"+item.Name+@"</title>
<price>"+item.Price+ @"</price>
<description>" + item.Description + @"</description>
<status>" + (item.Status == true ? 1 : 0) + @"</status>
<image>"+(System.Configuration.ConfigurationManager.AppSettings["resimurlsite"]) + (item.Photo.Count > 0 ? item.Photo.Where((y)=>y.Status && y.Main).FirstOrDefault().Name : "ResimYok.jpg") +@"</image>
</url>";

            }


            xml += @"</urlset>";

            ViewBag.Xml = xml;
            return View();
        }


    }
}