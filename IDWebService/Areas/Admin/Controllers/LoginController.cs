using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDETicaret.Models;
using IDETicaret.Security;
using IDETicaret.Helpers;

namespace IDETicaret.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {

            ViewBag.sitename = ocmde.Setting.Find(4).Value;
            return View();
        }

        [HttpPost]
        public ActionResult Process(FormCollection fc)
        {
            try
            {
                string username = fc["username"];
                string password = fc["password"];
                var account = login(username, password);
                if (account == null)
                {
                    ViewBag.error = Resources.Customer.Invalid_Account;
                    return View("Index");
                }
                else
                {
                    SessionPersister.account = account;

                    return RedirectToAction("Customer", "Account");
                }
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Login", "Process"));
            }
        }

        public ActionResult SignOut()
        {
            try
            {
                SessionPersister.account = null;
                return Redirect("~/Admin/Login");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Login", "SignOut"));
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Profile()
        {
            try
            {
                var account = (Account)SessionPersister.account;
                return View("Profile", account);
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Login", "Profile"));
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Profile(Account account)
        {
            try
            {
                var loginedAccount = (Account)SessionPersister.account;
                
                var currentAccount = ocmde.Account.SingleOrDefault(a => a.Id == account.Id);
                
                if (account.Username != null && account.Username.Length > 0 && loginedAccount.Username != account.Username)
                {
                    if (Exists(account.Username))
                    {
                        ModelState.AddModelError("Username", Resources.Customer.Username_exists);
                    }
                }

                if (account.Password != null && account.Password.Length != 0 && !PasswordHelper.IsValidPassword(account.Password))
                {
                    ModelState.AddModelError("Password", Resources.Vendor.Password_validate_message);
                }

                if (ModelState.IsValid)
                {
                    if (account.Password != null && account.Password.Length != 0)
                    {
                        currentAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                    }
                    currentAccount.Email = account.Email;
                    currentAccount.FullName = account.FullName;
                    currentAccount.Phone = account.Phone;
                    currentAccount.Username = account.Username;
                    ocmde.SaveChanges();
                    SessionPersister.account = ocmde.Account.Find(account.Id);
                    TempData["msg"] = Resources.Customer.Update_Success;
                    return RedirectToAction("Profile", "Login");
                }
                else
                {
                    return View("Profile", account);
                }
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Login", "Profile"));
            }
        }

        private bool Exists(string username)
        {
            return ocmde.Account.Count(a => a.Username.Equals(username)) > 0;
        }

        private Account login(string username, string password)
        {
            try
            {
                var account = ocmde.Account.SingleOrDefault(a => a.Username.Equals(username) && a.IsAdmin);
                if (account != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, account.Password))
                    {
                        return account;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

    }
}