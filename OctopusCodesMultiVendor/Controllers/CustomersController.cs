using IDETicaret.Helpers;
using IDETicaret.Models;
using IDETicaret.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public ActionResult forgot_password()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Account account)
        {
            try
            {
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
                if (account.Password == null && account.Password.Length == 0)
                {
                    ModelState.AddModelError("Password", Resources.Vendor.Password_validate_message);
                }

                if (ModelState.IsValid)
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
                }
                else
                {
                    return View("Register", account);
                }
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Customers", "Register"));
            }
        }

        [HttpPost]
        public ActionResult forgot_password(Account account)
        {
            try
            {
                if (account.Email.Length > 0)
                {
                    if (!ExistsUser(account.Email))
                    {
                        ModelState.AddModelError("Email", "Kullanıcı Bulunamadı");
                        ViewBag.error = Resources.Customer.Invalid_Account;
                    }
                    else
                    {
                        string token = Guid.NewGuid().ToString();
                        Account result = ocmde.Account.Where(b => b.Email.Equals(account.Email)).FirstOrDefault();
                        if (result != null)
                        {
                            result.Token = token;
                            ocmde.SaveChanges();

                            MailAddress gonderen = new MailAddress("info@ciftteker.com", "cifteker.com  Şifre Yenileme");
                            MailMessage _email = new MailMessage();
                            _email.From = gonderen;
                            _email.To.Add(account.Email);
                            _email.Subject = "Şifre Sıfırlama - Şifre Yenileme";
                            _email.Body = " Şifrenizi değiştirmek için " + "<a href='https://www.ciftteker.com/customers/password_change/?token=" + token + "'> bu linki</a> kullanabilirsiniz.";
                            _email.IsBodyHtml = true;
                            _email.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-9");
                            SmtpClient gonder = new SmtpClient("mail.ciftteker.com", 587);
                            gonder.EnableSsl = false;
                            gonder.Credentials = new NetworkCredential("info@ciftteker.com", "TekerCift12");
                            gonder.Send(_email);
                            account.Email = "";
                            ViewBag.success = "Şifre sıfırlama linki mail adresinize gönderildi";
                            return View(account);
                        }
                    }
                }
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Giris", "Login");
                }
                else
                {
                    return View("forgot_password", null);
                }
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Customers", "Register"));
            }
        }

        [HttpPost]
        public ActionResult password_change(FormCollection fc)
        {
            try
            {
                string password1 = fc["password1"];
                string password2 = fc["password2"];
                string token = fc["token"];
                if (token.Length > 0)
                {
                    if (password1 != password2)
                    {
                        ViewBag.error = "Şifreler uyuşmuyor.";
                        return View();
                    }

                    if (!ExistsUserToken(token))
                    {
                        ModelState.AddModelError("Şifre", "Bağlantı Geçersiz.");
                    }
                    else
                    {
                        Account result = ocmde.Account.Where(b => b.Token == token).FirstOrDefault();
                        if (result != null)
                        {
                            result.Password = BCrypt.Net.BCrypt.HashPassword(password2);
                            result.Token = null;
                            ocmde.SaveChanges();
                            ViewBag.success = "Parolanız Değiştirildi.";
                        }
                    }
                }
                else
                {
                    ViewBag.error = "Bağlantı Geçersiz.";
                }
                return View();
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Customers", "password_change"));
            }
        }

        public ActionResult password_change(string token)
        {
            if (!ExistsUserToken(token))
            {
                ViewBag.token = null;
                ViewBag.error = "Bağlantı Geçersiz.";
            }
            else
            {
                ViewBag.token = token;
            }
            return View();
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

        private bool ExistsUser(string email)
        {
            return ocmde.Account.Count(a => a.Email.Equals(email) && !a.IsAdmin) > 0;
        }

        private bool ExistsUserToken(string token)
        {
            return ocmde.Account.Count(a => a.Token.Equals(token)) > 0;
        }
    }
}