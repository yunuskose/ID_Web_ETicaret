using IDETicaret.Helpers;
using IDETicaret.Models;
using IDETicaret.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDETicaret.Controllers
{
    public class CustomersController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        [HttpGet]
        public ActionResult Register()
        {
            try
            {
                return View("Register", new Account());
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Customers", "Register"));
            }
        }

        [HttpPost]
        public ActionResult Register(Account account)
        {
            try {
                if (account.Username != null && account.Username.Length > 0)
                {
                    if (Exists(account.Username))
                    {
                        ModelState.AddModelError("username", "Sistemde böyle bir kullanıcı zaten var.");
                    }
                }

                if (account.Email != null && account.Email.Length > 0)
                {
                    if (ExistsEmail(account.Email))
                    {
                        ModelState.AddModelError("Email", "Bu email başka bir kullanıcı tarafından kullanılıyor.");
                    }
                }
                if (account.Password == null && account.Password.Length == 0) // && !PasswordHelper.IsValidPassword(account.Password))
                {
                    ModelState.AddModelError("Password", Resources.Vendor.Password_validate_message);
                }

                if  (ModelState.IsValid)
                {
                    account.IsAdmin = false;
                    account.Status = true;

                    string kullaniciadi = account.Username;
                    string parola = account.Password;

                    account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                    ocmde.Account.Add(account);
                    ocmde.SaveChanges();

                    var account2 = login(kullaniciadi, parola);
                    if (account2 == null)
                    {
                        ViewBag.error = Resources.Customer.Invalid_Account;
                        return Redirect("~/Login");
                    }
                    else
                    {
                        SessionPersister.account = account2;
                        return Redirect("~/");
                    }
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    return View("Register",account);
                }
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Customers", "Register"));
            }
        }


        private Account login(string username, string password)
        {
            try
            {
                var account = ocmde.Account.SingleOrDefault(a => a.Username.Equals(username) && !a.IsAdmin);
                if (account != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, account.Password) && account.Status)
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

        private bool Exists(string username)
        {
            return ocmde.Account.Count(a => a.Username.Equals(username) && !a.IsAdmin) > 0;
        }
        private bool ExistsEmail(string username)
        {
            return ocmde.Account.Count(a => a.Email.Equals(username) && !a.IsAdmin) > 0;
        }
    }
}