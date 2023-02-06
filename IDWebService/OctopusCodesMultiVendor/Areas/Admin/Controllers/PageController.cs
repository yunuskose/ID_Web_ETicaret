using IDETicaret.Areas.Admin.Models;
using IDETicaret.Models;
using IDETicaret.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDETicaret.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class PageController : Controller
    {
        public bool SessionKontrol()
        {
            bool kontrol = false;
            if (SessionPersister.account != null)
            {
                var account = SessionPersister.account;
                Account user = ((Account)account);
                if (user.IsAdmin)
                {
                    return true;
                }
            }
            return kontrol;
        }

        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {
            try
            {
                ViewBag.pages = ocmde.Page.ToList();
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Page", "Index"));
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var page = ocmde.Page.SingleOrDefault(p => p.Id == id);
                return View("Edit", page);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Page", "Edit"));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Page page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentPage = ocmde.Page.Find(page.Id
    );
                    currentPage.Plug = page.Plug;
                    currentPage.Title = page.Title;
                    currentPage.Status = page.Status;
                    currentPage.Detail = page.Detail;
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("Edit", page);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Page", "Edit"));
            }
        }

        public ActionResult Status(int id)
        {
            try
            {
                var page = ocmde.Page.SingleOrDefault(p => p.Id == id);
                page.Status = !page.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Page", "Status"));
            }
        }



        public ActionResult FirsatUrunleri()
        {
            try
            {
                if (!SessionKontrol())
                    return Redirect("~/");

                return View(ocmde.HomeProducts.ToList());
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Page", "FirsatUrunleri"));
            }
        }

        public ActionResult UrunAra(string keyword)
        {
            if (!SessionKontrol())
                return Redirect("~/");

            IList<Product> model = ocmde.f_StokAra(keyword, "0", "0", "0", "0", 1).ToList();
            List<IDCombobox> list = new List<IDCombobox>();
            foreach (Product item in model)
            {
                list.Add(new IDCombobox() { ID = item.Id, Deger = item.Name });
            }
            JsonResult jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public ActionResult FirsatUrunleriEkle(string stok)
        {
            try
            {
                if (!SessionKontrol())
                    return Redirect("~/");

                HomeProducts p = new HomeProducts();
                p.ProdutID = Convert.ToInt32(stok);
                p.Type = "FirsatUrunleri";
                ocmde.HomeProducts.Add(p);
                ocmde.SaveChanges();

                return Redirect("~/Admin/Page/FirsatUrunleri");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Page", "FirsatUrunleriEkle"));
            }
        }

        public ActionResult FirsatUrunleriSil(int id)
        {
            try
            {
                if (!SessionKontrol())
                    return Redirect("~/");

                ocmde.HomeProducts.Remove(ocmde.HomeProducts.Where((p) => p.ID == id).FirstOrDefault());
                ocmde.SaveChanges();

                return Redirect("~/Admin/Page/FirsatUrunleri");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Page", "FirsatUrunleriSil"));
            }
        }


    }
}