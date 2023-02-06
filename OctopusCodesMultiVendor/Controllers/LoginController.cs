using IDETicaret.Helpers;
using IDETicaret.Models;
using IDETicaret.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDETicaret.Controllers
{
    public class LoginController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Giris()
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

                    List<Item> sepetKontrol = (List<Item>)Session["cart"];
                    if (sepetKontrol == null)
                    {
                        sepetKontrol = new List<Item>();
                    }
                    foreach (Item item in sepetKontrol)
                    {
                        var product = ocmde.Product.Find(item.product.Id);

                        Account user = ((Account)account);
                        var vendorIds = product.VendorId;

                        if (ocmde.Card.Where((x) => x.CustomerId == user.Id && x.VendorId == vendorIds && x.ProductId == product.Id).Count() > 0)
                        {
                            //Update
                            Card c = ocmde.Card.Where((x) => x.CustomerId == user.Id && x.VendorId == vendorIds && x.ProductId == product.Id).FirstOrDefault();
                            c.ProductId = product.Id;
                            c.Quantity = item.quantity;
                        }
                        else
                        {
                            //İnsert
                            Card c = new Card();
                            c.CustomerId = user.Id;
                            c.VendorId = vendorIds;
                            c.ProductId = product.Id;
                            c.Quantity = item.quantity;
                            ocmde.Card.Add(c);
                        }
                    }
                    ocmde.SaveChanges();
                    //Session["cart"] = null;
                    return Redirect("~/");
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
                return RedirectToAction("Giris", "Login");
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Login", "SignOut"));
            }
        }

        [CustomAuthorize(Roles = "Customer")]
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

        [CustomAuthorize(Roles = "Customer")]
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
                        ModelState.AddModelError("username", "Kullanıcı adı başka kullanıcı tarafından kullanılıyor.");
                    }
                }
                if (account.Email != null && account.Email.Length > 0 && loginedAccount.Username != account.Username)
                {
                    if (ExistsEmail(account.Email))
                    {
                        ModelState.AddModelError("Email", "Email başka kullanıcı tarafından kullanılıyor.");
                    }
                }

                if (account.Password != null && account.Password.Length != 0 && 1== 2) // && !PasswordHelper.IsValidPassword(account.Password))
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

        private bool ExistsEmail(string username)
        {
            return ocmde.Account.Count(a => a.Email.Equals(username)) > 0;
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
    }
}