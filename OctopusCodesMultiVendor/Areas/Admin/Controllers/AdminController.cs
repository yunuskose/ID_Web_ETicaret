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
    public class AdminController : Controller
    {
        private ETicaretEntities db = new ETicaretEntities();
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
        // GET: Admin/Admin
        public ActionResult Index()
        {
            if (!SessionKontrol())
                return Redirect("~/");

            return View();
        }


    }
}