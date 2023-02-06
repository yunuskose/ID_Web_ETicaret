using IDETicaret.Helpers;
using IDETicaret.Models;
using IDETicaret.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace IDETicaret.Controllers
{
    public class CartController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Detay()
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
            try
            {
                if (SessionPersister.account == null)
                {
                    List<Card> list = new List<Card>();
                    if (Session["cart"] != null)
                    {
                        List<Item> cart = (List<Item>)Session["cart"];
                        foreach (var item in cart)
                        {
                            Card c = new Card();
                            c.Product = item.product;
                            c.Quantity = item.quantity;
                            c.Aktarilacak = item.Aktarilacak;
                            list.Add(c);
                        }
                    }

                    return View("Detay", list);
                }
                else
                {
                    var account = SessionPersister.account;
                    Account user = ((Account)account);

                    return View("Detay", ocmde.Card.Where((x) => x.CustomerId == user.Id).ToList());
                }
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Home", "Index"));
            }
        }

        public ActionResult SepetBilgisi()
        {
            SepetBilgisi s = new SepetBilgisi();
            int totalItem = 0;
            if (SessionPersister.account != null)
            {
                if (ocmde.Card.Where((x) => x.CustomerId == ((IDETicaret.Models.Account)SessionPersister.account).Id).Count() > 0)
                {
                    List<Card> cart = ocmde.Card.Where((x) => x.CustomerId == ((IDETicaret.Models.Account)SessionPersister.account).Id).ToList();
                    s.Tutar = String.Format("{0:N2}₺", Convert.ToDecimal(cart.Sum(i => i.Product.Price * i.Quantity)));
                    totalItem = cart.Count;
                }
                else
                {
                    s.Tutar = "0.00₺";
                }
            }
            else
            {
                if (Session["cart"] != null)
                {
                    List<Item> cart = (List<Item>)Session["cart"];
                    s.Tutar = String.Format("{0:N2}₺", Convert.ToDecimal(cart.Sum(i => i.product.Price * i.quantity)));
                    totalItem = cart.Count;
                }
                else
                {
                    s.Tutar = "0.00₺";
                    s.UrunSayisi = "0";
                }
            }
            s.UrunSayisi = totalItem.ToString();
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OdemeSepetBilgisi(int OrderID)
        {
            SepetBilgisi s = new SepetBilgisi();
            int totalItem = 0;

            if (OrderID > 0)
            {
                if (SessionPersister.account != null)
                {
                    if (ocmde.OrdersDetail.Where((x) => x.OrderId == OrderID).Count() > 0)
                    {
                        List<OrdersDetail> cart = ocmde.OrdersDetail.Where((x) => x.OrderId == OrderID).ToList();
                        s.Tutar = String.Format("{0:N2}₺", Convert.ToDecimal(cart.Sum(i => i.Product.Price * i.Quantity)));
                        totalItem = cart.Count;
                    }
                    else
                    {
                        s.Tutar = "0.00₺";
                    }
                }
                else
                {
                    if (ocmde.OrdersDetail.Where((x) => x.OrderId == OrderID).Count() > 0)
                    {
                        List<OrdersDetail> cart = ocmde.OrdersDetail.Where((x) => x.OrderId == OrderID).ToList();
                        s.Tutar = String.Format("{0:N2}₺", Convert.ToDecimal(cart.Sum(i => i.Product.Price * i.Quantity)));
                        totalItem = cart.Count;
                    }
                    else
                    {
                        s.Tutar = "0.00₺";
                    }
                }
            }
            else
            {
                if (SessionPersister.account != null)
                {
                    if (ocmde.Card.Where((x) => x.CustomerId == ((IDETicaret.Models.Account)SessionPersister.account).Id).Count() > 0)
                    {
                        List<Card> cart = ocmde.Card.Where((x) => x.Aktarilacak == true && x.CustomerId == ((IDETicaret.Models.Account)SessionPersister.account).Id).ToList();
                        s.Tutar = String.Format("{0:N2}₺", Convert.ToDecimal(cart.Sum(i => i.Product.Price * i.Quantity)));
                        totalItem = cart.Count;
                    }
                    else
                    {
                        s.Tutar = "0.00₺";
                    }
                }
                else
                {
                    if (Session["cart"] != null)
                    {
                        List<Item> cart = (List<Item>)Session["cart"];
                        s.Tutar = String.Format("{0:N2}₺", Convert.ToDecimal(cart.Where((x) => x.Aktarilacak == true).Sum(i => i.product.Price * i.quantity)));
                        totalItem = cart.Where((x) => x.Aktarilacak == true).Count();
                    }
                    else
                    {
                        s.Tutar = "0.00₺";
                        s.UrunSayisi = "0";
                    }
                }
            }

            s.UrunSayisi = totalItem.ToString();
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TaksitGetir(string banka)
        {
            Session["Kullanici"] = Session["Kullanici"];
            Session["SecilenKullanici"] = Session["SecilenKullanici"];

            return Json(ocmde.Setting.Where((x) => x.Key == banka + "Taksit").FirstOrDefault().Value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SepetGuncelle(string stokkodu, int miktar)
        {
            if (SessionPersister.account == null)
            {
                List<Item> cart = (List<Item>)Session["cart"];

                cart.Where((c) => c.product.Code.Replace(" ", "IDYAZILIM") == stokkodu).First().quantity += miktar;
                Session["cart"] = cart;
            }
            else
            {
                var account = SessionPersister.account;
                Account user = ((Account)account);

                if (ocmde.Card.Where((x) => x.CustomerId == user.Id && x.Product.Code.Replace(" ", "IDYAZILIM") == stokkodu).Count() > 0)
                {
                    //Update
                    Card c = ocmde.Card.Where((x) => x.CustomerId == user.Id && x.Product.Code.Replace(" ", "IDYAZILIM") == stokkodu).FirstOrDefault();
                    c.Quantity += miktar;

                    if (c.Quantity <= 0)
                        ocmde.Card.Remove(c);

                    ocmde.SaveChanges();
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);

        }

        public ActionResult SepetStokSec(string stokkodlari)
        {
            if (SessionPersister.account == null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                foreach (var item in cart)
                {
                    item.Aktarilacak = false;
                }
                foreach (string item in stokkodlari.Split('|'))
                {
                    if (item.Trim().Length > 0)
                    {
                        cart.Where((c) => c.product.Code.Replace(" ", "IDYAZILIM") == item).First().Aktarilacak = true;
                    }
                }
                Session["cart"] = cart;
            }
            else
            {
                var account = SessionPersister.account;
                Account user = ((Account)account);
                foreach (var item in ocmde.Card.Where((x) => x.CustomerId == user.Id))
                {
                    item.Aktarilacak = false;
                }
                foreach (string item in stokkodlari.Split('|'))
                {
                    if (item.Trim().Length > 0)
                    {
                        if (ocmde.Card.Where((x) => x.CustomerId == user.Id && x.Product.Code.Replace(" ", "IDYAZILIM") == item).Count() > 0)
                        {
                            //Update
                            Card c = ocmde.Card.Where((x) => x.CustomerId == user.Id && x.Product.Code.Replace(" ", "IDYAZILIM") == item).FirstOrDefault();
                            c.Aktarilacak = true;

                            if (c.Quantity <= 0)
                                ocmde.Card.Remove(c);

                            ocmde.SaveChanges();
                        }
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);

        }

        public ActionResult ConfirmOrder()
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
            try
            {
                ViewBag.Sehirler = ocmde.Sehirler.OrderBy((o) => o.Isim).ToList();
                if (SessionPersister.account != null)
                {
                    int userID = ((IDETicaret.Models.Account)SessionPersister.account).Id;
                    ViewBag.Adresler = ocmde.Adresler.Where((x) => x.UserID == userID).ToList();
                    ViewBag.GirisYapildimi = true;
                }
                else
                {
                    ViewBag.GirisYapildimi = false;
                }
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Home", "Index"));
            }
            return View();
        }

        public ActionResult IlceleriGetir(int SehirID)
        {
            return Json(ocmde.Ilceler.Where((x) => x.SehirID == SehirID).ToList());
        }

        public ActionResult ConfirmOrder2(string OrderID)
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
            // Havale için yapılan sayfa
            ViewBag.OrderID = OrderID;
            return View();
        }

        [HttpGet]
        public ActionResult ConfirmOrder3(string AdSoyad, string Il, string Ilce, string Adres, string email,
            string CepTelefonu, string TCKimlikNo, string AdresBasligi, string BK, string FirmaAdi, string VergiDairesi,
            string VergiNumarasi)
        {



            MailGonder("Sipariş Adres Tanımlama Ekranı",
"" +
"İsim : " + AdSoyad +
"" +
"Firma : " + FirmaAdi +
"" +
"Telefon : " + CepTelefonu + @"
" + "Adres : " + Il + " " + Ilce + " " + Adres + @" 
T.C. : " + TCKimlikNo 
                       );








            ViewBag.AdSoyad = AdSoyad;
            ViewBag.Il = Il;
            ViewBag.Ilce = Ilce;
            ViewBag.Adres = Adres;
            ViewBag.CepTelefonu = CepTelefonu;
            ViewBag.TCKimlikNo = TCKimlikNo;
            ViewBag.AdresBasligi = AdresBasligi;
            ViewBag.BK = BK;
            ViewBag.FirmaAdi = FirmaAdi;
            ViewBag.VergiDairesi = VergiDairesi;
            ViewBag.VergiNumarasi = VergiNumarasi;
            ViewBag.email = email;
            List<Card> list = new List<Card>();
            if (SessionPersister.account == null)
            {
                if (Session["cart"] != null)
                {
                    List<Item> cart = (List<Item>)Session["cart"];
                    foreach (var item in cart)
                    {
                        Card c = new Card();
                        c.Product = item.product;
                        c.Quantity = item.quantity;
                        c.Aktarilacak = item.Aktarilacak;
                        list.Add(c);
                    }
                }

            }
            else
            {
                var account = SessionPersister.account;
                Account user = ((Account)account);

                list = ocmde.Card.Where((x) => x.CustomerId == user.Id).ToList();
            }

            return View(list);
        }
        
        [HttpPost]
        public ActionResult ConfirmOrder3(string AdSoyad, string Il, string Ilce, string Adres, string email,
            string CepTelefonu, string TCKimlikNo, string AdresBasligi, string BK, string FirmaAdi, string VergiDairesi,
            string VergiNumarasi, string OdemeTipi, string taksitSayisi, string kartUzerindekiIsim, string kartNumarasi,
            string ay, string yil, string cvcNumarasi, string bankaAdi, string ip)
        {





            #region Adres Kaydı
            Session["AdSoyad"] = AdSoyad;
            Session["Il"] = Il;
            Session["Ilce"] = Ilce;
            Session["Email"] = email;
            Session["CepTelefonu"] = CepTelefonu;
            Session["TCKimlikNo"] = TCKimlikNo;
            Session["AdresBasligi"] = AdresBasligi;
            Session["Adres"] = Adres;
            Session["BK"] = BK;
            Session["FirmaAdi"] = FirmaAdi;
            Session["VergiDairesi"] = VergiDairesi;
            Session["VergiNumarasi"] = VergiNumarasi;
            Adresler adres = new Adresler();
            if (SessionPersister.account != null)
            {

                int userid = ((IDETicaret.Models.Account)SessionPersister.account).Id;
                if (ocmde.Adresler.Where((a) => a.UserID == userid && a.AdresIsmi == AdresBasligi).Count() > 0)
                {
                    adres = ocmde.Adresler.Where((a) => a.UserID == userid && a.AdresIsmi == AdresBasligi).FirstOrDefault();

                    adres.Isim = AdSoyad;
                    adres.Il = Il;
                    adres.Ilce = Ilce;
                    adres.CepTelefonu = CepTelefonu;
                    adres.TC = TCKimlikNo;
                    adres.AdresIsmi = AdresBasligi;
                    adres.FaturaTuru = BK;
                    adres.FirmaAdi = FirmaAdi;
                    adres.VergiDairesi = VergiDairesi;
                    adres.VergiNumarasi = VergiNumarasi;
                    adres.Adres = Adres;
                    adres.KayitTarihi = DateTime.Now;
                    ocmde.SaveChanges();
                }
                else
                {
                    adres.Isim = AdSoyad;
                    adres.Il = Il;
                    adres.Ilce = Ilce;
                    adres.CepTelefonu = CepTelefonu;
                    adres.TC = TCKimlikNo;
                    adres.AdresIsmi = AdresBasligi;
                    adres.FaturaTuru = BK;
                    adres.FirmaAdi = FirmaAdi;
                    adres.VergiDairesi = VergiDairesi;
                    adres.VergiNumarasi = VergiNumarasi;
                    adres.Adres = Adres;
                    adres.UserID = userid;
                    adres.KayitTarihi = DateTime.Now;
                    ocmde.Adresler.Add(adres);
                    ocmde.SaveChanges();
                }
            }
            else
            {

                int userid = 0;
                if (ocmde.Adresler.Where((a) => a.UserID == userid && a.AdresIsmi == AdresBasligi).Count() > 0)
                {
                    adres = ocmde.Adresler.Where((a) => a.UserID == userid && a.AdresIsmi == AdresBasligi).FirstOrDefault();

                    adres.Isim = AdSoyad;
                    adres.Il = Il;
                    adres.Ilce = Ilce;
                    adres.CepTelefonu = CepTelefonu;
                    adres.TC = TCKimlikNo;
                    adres.AdresIsmi = AdresBasligi;
                    adres.FaturaTuru = BK;
                    adres.FirmaAdi = FirmaAdi;
                    adres.VergiDairesi = VergiDairesi;
                    adres.VergiNumarasi = VergiNumarasi;
                    adres.Adres = Adres;
                    adres.KayitTarihi = DateTime.Now;
                    ocmde.SaveChanges();
                }
                else
                {
                    adres.Isim = AdSoyad;
                    adres.Il = Il;
                    adres.Ilce = Ilce;
                    adres.CepTelefonu = CepTelefonu;
                    adres.TC = TCKimlikNo;
                    adres.AdresIsmi = AdresBasligi;
                    adres.FaturaTuru = BK;
                    adres.FirmaAdi = FirmaAdi;
                    adres.VergiDairesi = VergiDairesi;
                    adres.VergiNumarasi = VergiNumarasi;
                    adres.Adres = Adres;
                    //adres.UserID = userid;
                    adres.KayitTarihi = DateTime.Now;
                    ocmde.Adresler.Add(adres);
                    ocmde.SaveChanges();
                }
            }
            #endregion


            if (OdemeTipi == "KK")
            {

                decimal Tutar = 0;

                if (SessionPersister.account != null)
                {
                    if (ocmde.Card.Where((x) => x.CustomerId == ((IDETicaret.Models.Account)SessionPersister.account).Id).Count() > 0)
                    {
                        List<Card> cart = ocmde.Card.Where((x) => x.Aktarilacak == true && x.CustomerId == ((IDETicaret.Models.Account)SessionPersister.account).Id).ToList();
                        Tutar = Convert.ToDecimal(cart.Sum(i => i.Product.Price * i.Quantity));

                    }

                }
                else
                {
                    if (Session["cart"] != null)
                    {
                        List<Item> cart = (List<Item>)Session["cart"];
                        Tutar = Convert.ToDecimal(cart.Where((x) => x.Aktarilacak == true).Sum(i => i.product.Price * i.quantity));

                    }

                    
                }

                string OrderID = DateTime.Now.ToString("ddMMhhmmssff");
                try
                {
                    if (SessionPersister.account != null)
                    {
                        List<Card> cart = ocmde.Card.Where((x) => x.Aktarilacak == true && x.CustomerId == ((IDETicaret.Models.Account)SessionPersister.account).Id).ToList();

                        #region Geçici Sipariş Kaydı
                        Orders order_ = new Orders()
                        {

                            DateCreation = DateTime.Now,
                            Name = Resources.Vendor.New_Order_for_Vendor,
                            OrderStatusId = 1,
                            CDate = DateTime.Now,
                            Aktarildi = false,
                            VendorId = 18,
                            CustomerId = ((IDETicaret.Models.Account)SessionPersister.account).Id,
                            Isim = Convert.ToString(Session["AdSoyad"]),
                            Email = Convert.ToString(Session["Email"]),
                            CepTelefonu = Convert.ToString(Session["CepTelefonu"]),
                            Il = Convert.ToString(Session["Il"]),
                            Ilce = Convert.ToString(Session["Ilce"]),
                            Adres = Convert.ToString(Session["Adres"]),
                            TC = Convert.ToString(Session["TCKimlikNo"]),
                            FaturaTuru = Convert.ToString(Session["BK"]),
                            FirmaAdi = Convert.ToString(Session["FirmaAdi"]),
                            VergiDairesi = Convert.ToString(Session["VergiDairesi"]),
                            VergiNumarasi = Convert.ToString(Session["VergiNumarasi"]),
                            OrderID = OrderID,
                            Aktif = false
                        };

                        ocmde.Orders.Add(order_);
                        ocmde.SaveChanges();

                        cart.ForEach(i =>
                        {
                            OrdersDetail ordersDetail_ = new OrdersDetail()
                            {
                                OrderId = order_.Id,
                                Price = i.Product.Price,
                                Quantity = Convert.ToInt32(i.Quantity),
                                ProductId = i.Product.Id
                            };
                            ocmde.OrdersDetail.Add(ordersDetail_);
                            ocmde.SaveChanges();
                        });
                        #endregion

                        #region Sipariş Log
                        OrdersLog orderlog = new OrdersLog()
                        {

                            DateCreation = DateTime.Now,
                            Name = Resources.Vendor.New_Order_for_Vendor,
                            OrderStatusId = 1,
                            CDate = DateTime.Now,
                            Aktarildi = false,
                            VendorId = 18,
                            CustomerId = ((IDETicaret.Models.Account)SessionPersister.account).Id,
                            Isim = Convert.ToString(Session["AdSoyad"]),
                            Email = Convert.ToString(Session["Email"]),
                            CepTelefonu = Convert.ToString(Session["CepTelefonu"]),
                            Il = Convert.ToString(Session["Il"]),
                            Ilce = Convert.ToString(Session["Ilce"]),
                            Adres = Convert.ToString(Session["Adres"]),
                            TC = Convert.ToString(Session["TCKimlikNo"]),
                            FaturaTuru = Convert.ToString(Session["BK"]),
                            FirmaAdi = Convert.ToString(Session["FirmaAdi"]),
                            VergiDairesi = Convert.ToString(Session["VergiDairesi"]),
                            VergiNumarasi = Convert.ToString(Session["VergiNumarasi"]),
                            OrderID = (OrderID)
                        };

                        ocmde.OrdersLog.Add(orderlog);
                        ocmde.SaveChanges();

                        cart.ForEach(i =>
                        {
                            OrdersDetailLog ordersDetailLog = new OrdersDetailLog()
                            {
                                OrderId = orderlog.Id,
                                Price = i.Product.Price,
                                Quantity = Convert.ToInt32(i.Quantity),
                                ProductId = i.Product.Id,
                                CDate = DateTime.Now,
                                Kullanici = Convert.ToString(Session["AdSoyad"])
                            };
                            ocmde.OrdersDetailLog.Add(ordersDetailLog);
                            ocmde.SaveChanges();
                        });
                        #endregion

                    }
                    else
                    {
                        List<Item> cart = (List<Item>)Session["cart"];

                        #region Sipariş Geçici Kaydı

                        Orders order_ = new Orders()
                        {
                            DateCreation = DateTime.Now,
                            Name = Resources.Vendor.New_Order_for_Vendor,
                            OrderStatusId = 1,
                            CDate = DateTime.Now,
                            Aktarildi = false,
                            VendorId = 18,
                            CustomerId = 1,
                            Isim = Convert.ToString(Session["AdSoyad"]),
                            Email = Convert.ToString(Session["Email"]),
                            CepTelefonu = Convert.ToString(Session["CepTelefonu"]),
                            Il = Convert.ToString(Session["Il"]),
                            Ilce = Convert.ToString(Session["Ilce"]),
                            Adres = Convert.ToString(Session["Adres"]),
                            TC = Convert.ToString(Session["TCKimlikNo"]),
                            FaturaTuru = Convert.ToString(Session["BK"]),
                            FirmaAdi = Convert.ToString(Session["FirmaAdi"]),
                            VergiDairesi = Convert.ToString(Session["VergiDairesi"]),
                            VergiNumarasi = Convert.ToString(Session["VergiNumarasi"]),
                            OrderID = (OrderID),
                            Aktif = false
                        };

                        ocmde.Orders.Add(order_);
                        ocmde.SaveChanges();

                        cart.ForEach(i =>
                        {
                            OrdersDetail ordersDetail_ = new OrdersDetail()
                            {
                                OrderId = order_.Id,
                                Price = i.product.Price,
                                Quantity = Convert.ToInt32(i.quantity),
                                ProductId = i.product.Id
                            };
                            ocmde.OrdersDetail.Add(ordersDetail_);
                            ocmde.SaveChanges();
                        });
                        #endregion

                        #region Sipariş Log
                        OrdersLog orderlog = new OrdersLog()
                        {

                            DateCreation = DateTime.Now,
                            Name = Resources.Vendor.New_Order_for_Vendor,
                            OrderStatusId = 1,
                            CDate = DateTime.Now,
                            Aktarildi = false,
                            VendorId = 18,
                            CustomerId = 1,
                            Isim = Convert.ToString(Session["AdSoyad"]),
                            Email = Convert.ToString(Session["Email"]),
                            CepTelefonu = Convert.ToString(Session["CepTelefonu"]),
                            Il = Convert.ToString(Session["Il"]),
                            Ilce = Convert.ToString(Session["Ilce"]),
                            Adres = Convert.ToString(Session["Adres"]),
                            TC = Convert.ToString(Session["TCKimlikNo"]),
                            FaturaTuru = Convert.ToString(Session["BK"]),
                            FirmaAdi = Convert.ToString(Session["FirmaAdi"]),
                            VergiDairesi = Convert.ToString(Session["VergiDairesi"]),
                            VergiNumarasi = Convert.ToString(Session["VergiNumarasi"]),
                            OrderID = (OrderID)
                        };

                        ocmde.OrdersLog.Add(orderlog);
                        ocmde.SaveChanges();

                        cart.ForEach(i =>
                        {
                            OrdersDetailLog ordersDetailLog = new OrdersDetailLog()
                            {
                                OrderId = orderlog.Id,
                                Price = i.product.Price,
                                Quantity = Convert.ToInt32(i.quantity),
                                ProductId = i.product.Id,
                                CDate = DateTime.Now,
                                Kullanici = Convert.ToString(Session["AdSoyad"])
                            };
                            ocmde.OrdersDetailLog.Add(ordersDetailLog);
                            ocmde.SaveChanges();
                        });
                        #endregion
                    }
                }
                catch (Exception err)
                {
                    ;
                }

                PosForm posForm = new PosForm()
                {
                    OrderID = OrderID,
                    Amount = Tutar,
                    Email = "yunuskose@hotmail.com",
                    IPAdress = "192.168.2.1",
                    CcNumber = kartNumarasi.Trim(),
                    Cvc = cvcNumarasi.Trim(),
                    ExpireMonth = ay,
                    ExpireYear = yil,
                    Installment = Convert.ToInt32(taksitSayisi),
                    CcOwnerName = kartUzerindekiIsim
                };

                if (posForm.Installment == 1)
                {
                    posForm.Installment = 0;
                }

                Session["Isim"] = posForm.CcOwnerName;
                Session["GonderilenTutar"] = posForm.Amount;

                if (bankaAdi == "Garanti")
                {
                    Front.Banka = "Garanti";
                    Front.Controller.CCManager.SendPayment(this.HttpContext, posForm, IDETicaret.Banka.Entity.Banks.GARANTI);
                    /////string form1 = Front.Controller.CCManager.SendPayment2(this.HttpContext, posForm, IDETicaret.Banka.Entity.Banks.GARANTI);
                    /////BankaXml b = new BankaXml() { form = form1, CDate = DateTime.Now };
                    /////ocmde.BankaXml.Add(b);
                    /////ocmde.SaveChanges();
                    /////int id1 = b.ID;
                    /////return RedirectToAction("Odeme","Cart",new {id = id1 });
                }
                else if (bankaAdi == "Garanti")
                {
                    #region 

                    //3D
                    if (false)
                    {
                        //Front.Banka = "Garanti";
                        //Front.Controller.CCManager.SendPayment(this.HttpContext, posForm, IDWebTicariB2B.Banka.Entity.Banks.GARANTI);
                    }

                    //3D siz
                    if (true)
                    {
                        #region 


                        string sonuc = "";
                        Bankalar GarantiB = ocmde.Bankalar.Where((x) => x.Sirket == "Ozerdem" && x.Banka == "Garanti" && x.Aktif == true).FirstOrDefault();
                        if (GarantiB != null)
                        {

                            // Bank = IDWebTicariB2B.Banka.Entity.Banks.GARANTI,
                            // TerminalID = ,
                            // MerchantID = ,
                            // TerminalUserID = ,
                            // BankName = Convert.ToString(dtGaranti.Rows[0]["BankName"]),
                            // ClientID = Convert.ToString(dtGaranti.Rows[0]["ClientID"]),
                            // Name = ,
                            // Password = Convert.ToString(dtGaranti.Rows[0]["Password"]),
                            // ProvisionPassword = ,
                            // PosType = IDWebTicariB2B.Banka.Entity.PosType.GARANTI,
                            // PosURL = Convert.ToString(dtGaranti.Rows[0]["PosURL"])

                            string strMode = "PROD";
                            string strVersion = "v0.01";
                            string strTerminalID = Convert.ToString(GarantiB.TerminalID); //8 Haneli TerminalID yazılmalı.
                            string _strTerminalID = "0" + strTerminalID;
                            string strProvUserID = Convert.ToString(GarantiB.TerminalUserID);
                            string strProvisionPassword = Convert.ToString(GarantiB.ProvisionPassword); //TerminalProvUserID şifresi
                            string strUserID = Convert.ToString(GarantiB.Name);
                            string strMerchantID = Convert.ToString(GarantiB.MarchantID); //Üye İşyeri Numarası

                            string ipAddress = ip;
                            if (Request.IsLocal)
                            {
                                ipAddress = "185.22.187.144";
                            }
                            string strIPAddress = ipAddress; //Kullanıcının IP adresini alır
                            string strEmailAddress = "yunuskose@outlook.com";
                            string strOrderID = DateTime.Now.ToString("ddMMhhmmssff");
                            string strNumber = posForm.CcNumber;
                            string strExpireDate = posForm.ExpireMonth + posForm.ExpireYear.Substring(posForm.ExpireYear.Length - 2, 2);
                            string strCVV2 = posForm.Cvc;

                            string strAmount = (Tutar.ToString().Replace(".", "").Replace(",", ""));//İşlem Tutarı 1.00₺ için 100 gönderilmeli
                            string strType = "sales";

                            string strCurrencyCode = "949";
                            string strCardholderPresentCode = "0";
                            string strMotoInd = "N";
                            string strInstallmentCount = posForm.Installment.ToString();
                            if (strInstallmentCount == "1" || strInstallmentCount == "0")
                            {
                                strInstallmentCount = "";
                            }
                            string strHostAddress = "https://sanalposprov.garanti.com.tr/VPServlet";

                            string SecurityData = GetSHA1(strProvisionPassword + _strTerminalID).ToUpper();
                            string HashData = GetSHA1(strOrderID + strTerminalID + strNumber + strAmount + SecurityData).ToUpper();

                            string strXML = null;
                            strXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<GVPSRequest>" + "<Mode>" + strMode + "</Mode>" + "<Version>" + strVersion + "</Version>" + "<Terminal><ProvUserID>" + strProvUserID + "</ProvUserID><HashData>" + HashData + "</HashData><UserID>" + strUserID + "</UserID><ID>" + strTerminalID + "</ID><MerchantID>" + strMerchantID + "</MerchantID></Terminal>" + "<Customer><IPAddress>" + strIPAddress + "</IPAddress><EmailAddress>" + strEmailAddress + "</EmailAddress></Customer>" + "<Card><Number>" + strNumber + "</Number><ExpireDate>" + strExpireDate + "</ExpireDate><CVV2>" + strCVV2 + "</CVV2></Card>" + "<Order><OrderID>" + strOrderID + "</OrderID><GroupID></GroupID><AddressList><Address><Type>S</Type><Name></Name><LastName></LastName><Company></Company><Text></Text><District></District><City></City><PostalCode></PostalCode><Country></Country><PhoneNumber></PhoneNumber></Address></AddressList></Order>" + "<Transaction>" + "<Type>" + strType + "</Type><InstallmentCnt>" + strInstallmentCount + "</InstallmentCnt><Amount>" + strAmount + "</Amount><CurrencyCode>" + strCurrencyCode + "</CurrencyCode><CardholderPresentCode>" + strCardholderPresentCode + "</CardholderPresentCode><MotoInd>" + strMotoInd + "</MotoInd>" + "</Transaction>" + "</GVPSRequest>";

                            try
                            {
                                string data = "data=" + strXML;

                                WebRequest _WebRequest = WebRequest.Create(strHostAddress);
                                _WebRequest.Method = "POST";

                                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                                _WebRequest.ContentType = "application/x-www-form-urlencoded";
                                _WebRequest.ContentLength = byteArray.Length;

                                Stream dataStream = _WebRequest.GetRequestStream();
                                dataStream.Write(byteArray, 0, byteArray.Length);
                                dataStream.Close();

                                WebResponse _WebResponse = _WebRequest.GetResponse();
                                Console.WriteLine(((HttpWebResponse)_WebResponse).StatusDescription);
                                dataStream = _WebResponse.GetResponseStream();

                                StreamReader reader = new StreamReader(dataStream);
                                string responseFromServer = reader.ReadToEnd();

                                Console.WriteLine(responseFromServer);

                                //Müşteriye gösterilebilir ama Fraud riski açısından bu değerleri göstermeyelim.
                                //responseFromServer

                                //GVPSResponse XML'in değerlerini okuyoruz. İstediğiniz geri dönüş değerlerini gösterebilirsiniz.
                                string XML = responseFromServer;
                                XmlDocument xDoc = new XmlDocument();
                                xDoc.LoadXml(XML);

                                //ReasonCode
                                XmlElement xElement1 = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/ReasonCode") as XmlElement;
                                XmlElement xElement2 = xDoc.SelectSingleNode("//GVPSResponse/Transaction/RetrefNum") as XmlElement;
                                XmlElement xElement3 = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/ErrorMsg") as XmlElement;

                                //00 ReasonCode döndüğünde işlem başarılıdır. Müşteriye başarılı veya başarısız şeklinde göstermeniz tavsiye edilir. (Fraud riski)
                                if (xElement1.InnerText == "00")
                                {
                                    sonuc = "İşlem Başarılı. Sonuc : " + xElement1.InnerText + " - Ödeme başarılı bir şekilde alınmıştır.";


                                    if (SessionPersister.account == null)
                                    {

                                        List<Item> cart = ((List<Item>)Session["cart"]).Where((x) => x.Aktarilacak == true).ToList();

                                        if (cart.Count > 0)
                                        {
                                            // Create new order
                                            Orders order = new Orders()
                                            {
                                                CustomerId = 1,
                                                DateCreation = DateTime.Now,
                                                Name = Resources.Vendor.New_Order_for_Vendor,
                                                OrderStatusId = 1,
                                                Vendor = ocmde.Vendor.FirstOrDefault(),
                                                CDate = DateTime.Now,
                                                Aktarildi = false,
                                                Isim = AdSoyad,
                                                CepTelefonu = CepTelefonu,
                                                Il = Il,
                                                Ilce = Ilce,
                                                Adres = Adres,
                                                TC = TCKimlikNo,
                                                FaturaTuru = BK,
                                                FirmaAdi = FirmaAdi,
                                                VergiDairesi = VergiDairesi,
                                                VergiNumarasi = VergiNumarasi,
                                                OrderID = OrderID,
                                                Aktif = true

                                            };
                                            ocmde.Orders.Add(order);
                                            ocmde.SaveChanges();

                                            // Create order details
                                            cart.ForEach(i =>
                                            {
                                                OrdersDetail ordersDetail = new OrdersDetail()
                                                {
                                                    OrderId = order.Id,
                                                    Price = i.product.Price,
                                                    Quantity = Convert.ToInt32(i.quantity),
                                                    ProductId = i.product.Id
                                                };
                                                ocmde.OrdersDetail.Add(ordersDetail);
                                                ocmde.SaveChanges();
                                            });
                                            // Remove Cart
                                            #region 
                                            List<Item> cart2 = ((List<Item>)Session["cart"]);

                                            foreach (var item in cart)
                                            {
                                                cart2.Remove(item);
                                            }

                                            Session["cart"] = cart2;
                                        }
                                        #endregion

                                        //return View();

                                        //return View();

                                    }
                                    else
                                    {
                                        var account = SessionPersister.account;
                                        if (account is Account && !((IDETicaret.Models.Account)account).IsAdmin)
                                        {
                                            var customer = (IDETicaret.Models.Account)account;
                                            List<Card> cart = ocmde.Card.Where((x) => x.CustomerId == ((IDETicaret.Models.Account)account).Id && x.Aktarilacak == true).ToList();


                                            if (cart.Count > 0)
                                            {
                                                // Create new order
                                                Orders order = new Orders()
                                                {
                                                    CustomerId = customer.Id,
                                                    DateCreation = DateTime.Now,
                                                    Name = Resources.Vendor.New_Order_for_Vendor,
                                                    OrderStatusId = 1,
                                                    Vendor = ocmde.Vendor.FirstOrDefault(),
                                                    CDate = DateTime.Now,
                                                    Aktarildi = false,
                                                    Isim = AdSoyad,
                                                    CepTelefonu = CepTelefonu,
                                                    Il = Il,
                                                    Ilce = Ilce,
                                                    Adres = Adres,
                                                    TC = TCKimlikNo,
                                                    FaturaTuru = BK,
                                                    FirmaAdi = FirmaAdi,
                                                    VergiDairesi = VergiDairesi,
                                                    VergiNumarasi = VergiNumarasi,
                                                    OrderID = OrderID,
                                                    Aktif = true
                                                };
                                                ocmde.Orders.Add(order);
                                                ocmde.SaveChanges();
                                                // Create order details
                                                cart.ForEach(i =>
                                                {
                                                    OrdersDetail ordersDetail = new OrdersDetail()
                                                    {
                                                        OrderId = order.Id,
                                                        Price = i.Product.Price,
                                                        Quantity = Convert.ToInt32(i.Quantity),
                                                        ProductId = i.Product.Id
                                                    };
                                                    ocmde.OrdersDetail.Add(ordersDetail);
                                                    ocmde.SaveChanges();
                                                });



                                                // Remove Cart
                                                #region 
                                                ocmde.Card.RemoveRange(cart);
                                            }
                                            ocmde.SaveChanges();
                                            #endregion

                                            //return View();
                                        }
                                        //return View();
                                    }

                                    ViewBag.OrderID = OrderID;

                                    ViewBag.OdemeTipi = OdemeTipi;





                                    PosOdemeleri po = new PosOdemeleri();
                                    po.KayitYapanKullanici = "";
                                    po.Tarih = DateTime.Today;
                                    po.KayitTarihi = DateTime.Now;
                                    po.Aktarildi = false;
                                    po.KontrolEdildi = false;
                                    po.procreturncode = "";
                                    po.UserID = 0;
                                    po.Aciklama = "";
                                    po.Tip = "1";


                                    po.txnamount = Convert.ToDecimal(strAmount) / 100;
                                    po.xid = Convert.ToString("");
                                    po.hostmsg = Convert.ToString("");
                                    po.taksitAdet = Convert.ToInt32(taksitSayisi);
                                    po.bankaAd = bankaAdi;
                                    po.authcode = Convert.ToString(xElement1.InnerText);
                                    po.hostrefnum = Convert.ToString(strOrderID);
                                    po.rnd = Convert.ToString("");
                                    po.procreturncode = Convert.ToString(xElement1 == null ? "" : xElement1.InnerText);
                                    po.transid = Convert.ToString("");
                                    po.mode = Convert.ToString(strMode);
                                    po.response = Convert.ToString(xElement3.InnerText);
                                    po.successurl = Convert.ToString(strHostAddress);
                                    po.errmsg = Convert.ToString(xElement3 == null ? "" : xElement3.InnerText);
                                    po.md = Convert.ToString("");
                                    po.oid = Convert.ToString(strOrderID);
                                    po.hash = Convert.ToString(HashData);
                                    po.txntimestamp = Convert.ToString("");
                                    po.customeripaddress = Convert.ToString(strIPAddress);
                                    po.terminalid = Convert.ToString(_strTerminalID);
                                    po.MaskedPan = posForm.CcNumber.Substring(0, 6) + "***" + posForm.CcNumber.Substring(posForm.CcNumber.Length - 4, 4);
                                    po.Isim = Convert.ToString(posForm.CcOwnerName);
                                    po.secure3dhash = Convert.ToString(SecurityData);


                                    po.Tip = "5";
                                    if (SessionPersister.account == null)
                                    {
                                        po.UserID = 0;
                                        po.UserName = "Misafir Kullanıcı";
                                    }
                                    else
                                    {
                                        var account = SessionPersister.account;
                                        Account user = ((Account)account);

                                        po.UserID = user.Id;
                                        po.UserName = user.Username;
                                    }
                                    po.orderid = Convert.ToString(OrderID);
                                    po.SonucDegeri = Request.Form.AllKeys.ToString();


                                    po.Tip = "6";



                                    //po.CariKodu = ksec.CariKodu;
                                    po.Tarih = DateTime.Today;
                                    po.SonucDegeri = xDoc.InnerXml;

                                    ocmde.PosOdemeleri.Add(po);
                                    ocmde.SaveChanges();

                                    Session["GarantiSonuc"] = sonuc;
                                    ViewBag.Sonuc = sonuc;
                                    return View();

                                }
                                else
                                {
                                    sonuc = "İşlem Başarısız!!!. Bilgi : " + xElement1.InnerText + "-" + xElement3.InnerText + " <br/> ";


                                    //Session["GarantiSonuc"] = sonuc;
                                    ViewBag.Sonuc = sonuc;
                                    return View();

                                }




                            }
                            catch (Exception ex)
                            {
                                sonuc = "HATA! " + ex.Message;
                            }
                            Session["GarantiSonuc"] = sonuc;
                            ViewBag.Sonuc = sonuc;
                            return View();

                        }
                        #endregion
                    }

                    #endregion

                }
                else if (bankaAdi == "Akbank")
                {
                    Front.Banka = "Akbank";
                    Front.Controller.CCManager.SendPayment(this.HttpContext, posForm, IDETicaret.Banka.Entity.Banks.AKBANK);
                }
                else if (bankaAdi == "IsBank")
                {
                    Front.Banka = "IsBankasi";
                    Front.Controller.CCManager.SendPayment(this.HttpContext, posForm, IDETicaret.Banka.Entity.Banks.ISBANKASI);
                }
                else if (bankaAdi == "Vakif")
                {
                    /////Front.Banka = "Vakif";
                    /////Front.Controller.CCManager.SendPayment(this.HttpContext, posForm, IDWebTicariB2B.Banka.Entity.Banks.YAPIKREDI);
                    ///

                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                    Session["CVV_Numarasi"] = cvcNumarasi;
                    List<YKBPosInfo> posInfoList = new List<YKBPosInfo>();
                    Bankalar b = ocmde.Bankalar.Where((x)=>x.Sirket == "Ozerdem" && x.Banka == "Vakif" && x.Aktif == true).FirstOrDefault();
                    if (b != null)
                    {
                        posInfoList.Add(new YKBPosInfo()
                        {
                            Bank = IDETicaret.Banka.Entity.Banks.YAPIKREDI,
                            BankName = Convert.ToString(b.BankName),
                            TerminalID = Convert.ToString(b.TerminalID),
                            MerchantID = Convert.ToString(b.MarchantID),
                            ClientID = Convert.ToString(b.ClientID),
                            Name = Convert.ToString(b.Name),
                            Password = Convert.ToString(b.Password),
                            ekPassword = Convert.ToString(b.ProvisionPassword),
                            PosType = IDETicaret.Banka.Entity.PosType.YAPIKREDI,
                            PosURL = Convert.ToString(b.PosUrl),
                            Key = Convert.ToString(b.ProvisionPassword),
                            SuccessUrl = Convert.ToString(b.okURL)
                        });
                    }
                    char parcalamaKarakteri = 'x';
                    if (Tutar.ToString().Contains("."))
                    {
                        parcalamaKarakteri = '.';
                    }else if (Tutar.ToString().Contains(","))
                    {
                        parcalamaKarakteri = ',';
                    }
                    string odemeMiktari = (Tutar.ToString().Split(parcalamaKarakteri)[0]);
                    string kusurat = (Tutar.ToString().Split(parcalamaKarakteri)[1]);
                    if (kusurat == "")
                    {
                        kusurat = "00";
                    }
                    kusurat = kusurat + "0000";
                    kusurat = kusurat.Substring(0, 2);


                    string marchandId = posInfoList[0].MerchantID;
                    string parola = posInfoList[0].Password;
                    string terminalno = posInfoList[0].TerminalID;
                    string pan = kartNumarasi; // "4022780164735600";
                    string sonkullanma = yil.Substring(2, 2) + ay; // "2205";
                    string para = odemeMiktari + "." + kusurat;
                    string tl = "949";
                    string visamaster = KrediKartiniBulma(kartNumarasi);
                    string taksit = "";

                    if (taksitSayisi.Trim().Length > 0)
                        if (Convert.ToInt32(taksitSayisi) > 1)
                        {
                            taksit = "InstallmentCount=" + taksitSayisi + "&";
                        }

                    ocmde.CVVs.Add(new CVV() {
                        Key = OrderID,
                        Value = cvcNumarasi
                    });
                    ocmde.SaveChanges();
                    string data = taksit + "VerifyEnrollmentRequestId=" + OrderID + "&MerchantId=" + marchandId + "&MerchantPassword=" + parola + "&VerifyEnrollmentReques=" + OrderID + "&CVV=" + cvcNumarasi + "&Pan=" + pan + "&ExpiryDate=" + sonkullanma + "&CurrencyAmount=" + para + "&PurchaseAmount=" + para + "&Currency=" + tl + "&CurrencyCode=" + tl + "&BrandName=" + visamaster + "&SuccessUrl=" + posInfoList[0].SuccessUrl + "?Tutar=" + odemeMiktari + "&Kusurat=" + kusurat + ""; //replace <value>
                    byte[] dataStream = Encoding.UTF8.GetBytes(data);
                    HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(posInfoList[0].PosURL); //Mpi Enrollment Adresi
                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.ContentLength = dataStream.Length;
                    webRequest.KeepAlive = false;
                    string responseFromServer = "";

                    using (Stream newStream = webRequest.GetRequestStream())
                    {
                        newStream.Write(dataStream, 0, dataStream.Length);
                        newStream.Close();
                    }

                    using (WebResponse webResponse = webRequest.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            responseFromServer = reader.ReadToEnd();
                            reader.Close();
                        }

                        webResponse.Close();
                    }

                    string resultCode = "";
                    string resultDescription = "";
                    if (string.IsNullOrEmpty(responseFromServer))
                    {
                        return View();
                    }
                    else
                    {

                        var xmlResponse = new XmlDocument();
                        xmlResponse.LoadXml(responseFromServer);
                        var resultCodeNode = xmlResponse.SelectSingleNode("VposResponse/ResultCode");
                        var resultDescriptionNode = xmlResponse.SelectSingleNode("VposResponse/ResultDescription");

                        if (resultCodeNode != null)
                        {
                            resultCode = resultCodeNode.InnerText;
                        }
                        if (resultDescriptionNode != null)
                        {
                            resultDescription = resultDescriptionNode.InnerText;
                        }
                    }

                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(responseFromServer);


                    var statusNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/Status");
                    var pareqNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/PaReq");
                    var acsUrlNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/ACSUrl");
                    var termUrlNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/TermUrl");
                    var mdNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/MD");
                    var messageErrorCodeNode = xmlDocument.SelectSingleNode("IPaySecure/MessageErrorCode");

                    string statusText = "";

                    if (statusNode != null)
                    {
                        statusText = statusNode.InnerText;
                    }

                    //3d secure programına dahil
                    if (statusText == "Y")
                    {
                        string postBackForm =
                           @"<html>
                          <head>
                            <meta name=""viewport"" content=""width=device-width"" />
                            <title>MpiForm</title>
                            <script>
                              function postPage() {
                              document.forms[""frmMpiForm""].submit();
                              }
                            </script>
                          </head>
                          <body onload=""javascript:postPage();"">
                            <form action=""@ACSUrl"" method=""post"" id=""frmMpiForm"" name=""frmMpiForm"">
                              <input type=""hidden"" name=""PaReq"" value=""@PAReq"" />
                              <input type=""hidden"" name=""TermUrl"" value=""@TermUrl"" />
                              <input type=""hidden"" name=""CVV"" value=""@CVV"" />
                              <input type=""hidden"" name=""MD"" value=""@MD "" />
                              <noscript>
                                <input type=""submit"" id=""btnSubmit"" value=""Gönder"" />
                              </noscript>
                            </form>
                          </body>
                        </html>";

                        postBackForm = postBackForm.Replace("@ACSUrl", acsUrlNode.InnerText);
                        postBackForm = postBackForm.Replace("@PAReq", pareqNode.InnerText);
                        postBackForm = postBackForm.Replace("@TermUrl", termUrlNode.InnerText);
                        postBackForm = postBackForm.Replace("@CVV", cvcNumarasi);
                        postBackForm = postBackForm.Replace("@MD", mdNode.InnerText);

                        Response.ContentType = "text/html";
                        Response.Write(postBackForm);
                    }
                    else if (statusText == "E")
                    {
                        return Redirect("~/Odeme/OdemeSonucu/vakif?orderid___=" + OrderID + "&CVV=" + cvcNumarasi + "&hata=" + resultCode + "-" + resultDescription);
                    }
                }
            }
            else
            {

                int OrderID = 0;
                if (SessionPersister.account == null)
                {

                    List<Item> cart = ((List<Item>)Session["cart"]).Where((x) => x.Aktarilacak == true).ToList();

                    if (cart.Count > 0)
                    {
                        // Create new order
                        Orders order = new Orders()
                        {
                            CustomerId = 1,
                            DateCreation = DateTime.Now,
                            Name = Resources.Vendor.New_Order_for_Vendor,
                            OrderStatusId = 1,
                            Vendor = ocmde.Vendor.FirstOrDefault(),
                            CDate = DateTime.Now,
                            Aktarildi = false,
                            Isim = AdSoyad,
                            CepTelefonu = CepTelefonu,
                            Il = Il,
                            Ilce = Ilce,
                            Adres = Adres,
                            TC = TCKimlikNo,
                            FaturaTuru = BK,
                            FirmaAdi = FirmaAdi,
                            VergiDairesi = VergiDairesi,
                            VergiNumarasi = VergiNumarasi,
                            Aktif = true
                        };
                        ocmde.Orders.Add(order);
                        ocmde.SaveChanges();
                        OrderID = Convert.ToInt32(order.Id);

                        MailGonder("Havale Sipariş İşlemi Yapıldı (Misafir)",
"" +
"İsim : "+  AdSoyad+
"" +
"Firma : "+FirmaAdi+
"" +
"Telefon : "+CepTelefonu+@"
"+ "Adres : "+Il+" "+Ilce+" "+Adres+@" 
T.C. : "+TCKimlikNo
                            );
                        // Create order details
                        cart.ForEach(i =>
                        {
                            OrdersDetail ordersDetail = new OrdersDetail()
                            {
                                OrderId = order.Id,
                                Price = i.product.Price,
                                Quantity = Convert.ToInt32(i.quantity),
                                ProductId = i.product.Id
                            };
                            ocmde.OrdersDetail.Add(ordersDetail);
                            ocmde.SaveChanges();
                        });
                        // Remove Cart
                        #region 
                        List<Item> cart2 = ((List<Item>)Session["cart"]);

                        foreach (var item in cart)
                        {
                            cart2.Remove(item);
                        }

                        Session["cart"] = cart2;
                    }
                    #endregion

                    //return View();

                    //return View();

                }
                else
                {
                    var account = SessionPersister.account;
                    if (account is Account && !((IDETicaret.Models.Account)account).IsAdmin)
                    {
                        var customer = (IDETicaret.Models.Account)account;
                        List<Card> cart = ocmde.Card.Where((x) => x.CustomerId == ((IDETicaret.Models.Account)account).Id && x.Aktarilacak == true).ToList();

                        if (cart.Count > 0)
                        {
                            // Create new order
                            Orders order = new Orders()
                            {
                                CustomerId = customer.Id,
                                DateCreation = DateTime.Now,
                                Name = Resources.Vendor.New_Order_for_Vendor,
                                OrderStatusId = 1,
                                Vendor = ocmde.Vendor.FirstOrDefault(),
                                CDate = DateTime.Now,
                                Aktarildi = false,
                                Isim = AdSoyad,
                                CepTelefonu = CepTelefonu,
                                Il = Il,
                                Ilce = Ilce,
                                Adres = Adres,
                                TC = TCKimlikNo,
                                FaturaTuru = BK,
                                FirmaAdi = FirmaAdi,
                                VergiDairesi = VergiDairesi,
                                VergiNumarasi = VergiNumarasi,
                                Aktif = true
                            };
                            ocmde.Orders.Add(order);
                            ocmde.SaveChanges();


                            MailGonder("Havale Sipariş İşlemi Yapıldı (Üye) "+ customer.Username+" "+customer.FullName, @"
İsim : " +                                AdSoyad + @"
Firma : " +                                FirmaAdi + @"
Cep Telefonu : " +                                CepTelefonu + @"
Adres : " +                                Il + " " + Ilce + " " + Adres + @"
T.C. : " + TCKimlikNo + @"
" 
                                );


                            OrderID = Convert.ToInt32(order.Id);
                            // Create order details
                            cart.ForEach(i =>
                            {
                                OrdersDetail ordersDetail = new OrdersDetail()
                                {
                                    OrderId = order.Id,
                                    Price = i.Product.Price,
                                    Quantity = Convert.ToInt32(i.Quantity),
                                    ProductId = i.Product.Id
                                };
                                ocmde.OrdersDetail.Add(ordersDetail);
                                ocmde.SaveChanges();
                            });



                            // Remove Cart
                            #region 
                            ocmde.Card.RemoveRange(cart);
                        }
                        ocmde.SaveChanges();
                        #endregion

                        //return View();
                    }
                    //return View();
                }

                ViewBag.OrderID = OrderID;

                ViewBag.OdemeTipi = OdemeTipi;

                return Redirect("~/Cart/ConfirmOrder2/?OrderID=" + OrderID);
            }

            return View();
        }

        public string GetSHA1(string SHA1Data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            string HashedPassword = SHA1Data;
            byte[] hashbytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(HashedPassword);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            return GetHexaDecimal(inputbytes);
        }

        public string GetHexaDecimal(byte[] bytes)
        {
            StringBuilder s = new StringBuilder();
            int length = bytes.Length;
            for (int n = 0; n <= length - 1; n++)
            {
                s.Append(String.Format("{0,2:x}", bytes[n]).Replace(" ", "0"));
            }
            return s.ToString();
        }

        public ActionResult OdemeSonucu(string id)
        {
            if (true)
            {
                try
                {

                    string[] allkeys = Request.Form.AllKeys;
                    string logtut = "";
                    foreach (var item in allkeys)
                    {
                        if (Request.Form.GetValues(item)[0] != null)
                        {
                            logtut += "<br/>" + item + ":" + Request.Form.GetValues(item)[0];
                        }
                    }

                    var account = SessionPersister.account;
                    if (account is Account && !((IDETicaret.Models.Account)account).IsAdmin)
                    {
                        // Log Tutuyoruz
                        Loglar l = new Loglar();
                        l.Tip = "Ödeme Sonuç";
                        l.Kullanici = ((IDETicaret.Models.Account)account).Username;
                        l.Aciklama = logtut;
                        l.Tarih = DateTime.Now;
                        ocmde.Loglar.Add(l);
                    }
                    else
                    {
                        // Log Tutuyoruz
                        Loglar l = new Loglar();
                        l.Tip = "Ödeme Sonuç";
                        l.Kullanici = "Misafir Kullanıcı";
                        l.Aciklama = logtut;
                        l.Tarih = DateTime.Now;
                        ocmde.Loglar.Add(l);
                    }
                }
                catch (Exception err)
                {
                    ;
                }
            }


            PosOdemeleri po = new PosOdemeleri();
            po.KayitYapanKullanici = "";
            po.Tarih = DateTime.Today;
            po.KayitTarihi = DateTime.Now;
            po.Aktarildi = false;
            po.KontrolEdildi = false;
            po.procreturncode = "";
            po.UserID = 0;
            po.Aciklama = "";
            po.Tip = "1";
            //PaymentResult sonuc = Front.Controller.CCManager.ConfirmPayment(,Request,HttpContext.);
            try
            {
                string[] txnamount = null;
                string[] xid = null;
                string[] hostmsg = null;
                string[] taksitAdet = null;
                string[] bankaAd = null;
                string[] authcode = null;
                string[] hostrefnum = null;
                string[] rnd = null;
                string[] procreturncode = null;
                string[] transid = null;
                string[] mode = null;
                string[] response = null;
                string[] successurl = null;
                string[] errmsg = null;
                string[] md = null;
                string[] oid = null;
                string[] hash = null;
                string[] txntimestamp = null;
                string[] customeripaddress = null;
                string[] terminalid = null;
                string[] orderid = null;
                string[] MaskedPan = null;
                string[] secure3dhash = null;
                object a = Request.Form.GetValues("txnamount");
                object b = Request.Form.GetValues("amount");

                if (id == "Garanti")
                {
                    txnamount = new string[] { "0" };
                    if (Request.Form.GetValues("txnamount") != null)
                        txnamount = new string[] { Convert.ToString(Convert.ToDecimal(Request.Form.GetValues("txnamount")[0])) };
                    xid = Request.Form.GetValues("xid");
                    hostmsg = Request.Form.GetValues("hostmsg");
                    if (Request.Form.GetValues("taksitAdet") != null)
                        if (Request.Form.GetValues("taksitAdet").Length > 0)
                            if (Convert.ToString(Request.Form.GetValues("taksitAdet")[0]).Trim().Length > 0)
                                taksitAdet = new string[] { Convert.ToString(Convert.ToInt32(Request.Form.GetValues("taksitAdet")[0])) };
                    bankaAd = Request.Form.GetValues("bankaAd");
                    authcode = Request.Form.GetValues("authcode");
                    hostrefnum = Request.Form.GetValues("hostrefnum");
                    rnd = Request.Form.GetValues("rnd");
                    procreturncode = Request.Form.GetValues("procreturncode");
                    transid = Request.Form.GetValues("transid");
                    mode = Request.Form.GetValues("mode");
                    response = Request.Form.GetValues("response");
                    successurl = Request.Form.GetValues("successurl");
                    errmsg = Request.Form.GetValues("errmsg");
                    md = Request.Form.GetValues("md");
                    oid = Request.Form.GetValues("oid");
                    hash = Request.Form.GetValues("hash");
                    txntimestamp = Request.Form.GetValues("txntimestamp");
                    customeripaddress = Request.Form.GetValues("customeripaddress");
                    terminalid = Request.Form.GetValues("terminalid");
                    orderid = Request.Form.GetValues("orderid");
                    MaskedPan = Request.Form.GetValues("MaskedPan");
                    secure3dhash = Request.Form.GetValues("secure3dhash");

                    string[] _allkeys = Request.Form.AllKeys;
                    string _sonuc = "Sonuc:";
                    foreach (var item in _allkeys)
                    {
                        if (Request.Form.GetValues(item)[0] != null)
                        {
                            _sonuc += "|" + item + ":" + Request.Form.GetValues(item)[0];
                        }
                    }
                    
                }
                else if (id == "Akbank")
                {
                    po.Tip = "2";
                    txnamount = new string[] { "0" };
                    if (Request.Form.GetValues("amount") != null)
                        txnamount = new string[] { Convert.ToString(Convert.ToDecimal(Request.Form.GetValues("amount")[0])) };
                    xid = Request.Form.GetValues("xid");
                    hostmsg = Request.Form.GetValues("hostmsg");
                    if (Request.Form.GetValues("taksit") != null)
                        if (Request.Form.GetValues("taksit").Length > 0)
                            if (Convert.ToString(Request.Form.GetValues("taksit")[0]).Trim().Length > 0)
                                taksitAdet = new string[] { Convert.ToString(Convert.ToInt32(Request.Form.GetValues("taksit")[0])) };
                    bankaAd = Request.Form.GetValues("bankaAd");
                    authcode = Request.Form.GetValues("authcode");
                    hostrefnum = Request.Form.GetValues("hostrefnum");
                    rnd = Request.Form.GetValues("rnd");
                    procreturncode = Request.Form.GetValues("procreturncode");
                    transid = Request.Form.GetValues("transid");
                    mode = Request.Form.GetValues("mode");
                    response = Request.Form.GetValues("response");
                    successurl = Request.Form.GetValues("okUrl");
                    errmsg = Request.Form.GetValues("ErrMsg");
                    md = Request.Form.GetValues("md");
                    oid = Request.Form.GetValues("oid");
                    hash = Request.Form.GetValues("hash");
                    txntimestamp = Request.Form.GetValues("EXTRA.TRXDATE");
                    customeripaddress = Request.Form.GetValues("clientIp");
                    terminalid = Request.Form.GetValues("terminalid");
                    orderid = Request.Form.GetValues("ReturnOid");
                    MaskedPan = Request.Form.GetValues("MaskedPan");
                    secure3dhash = Request.Form.GetValues("HASH");
                    po.Tip = "3";


                    string[] _allkeys = Request.Form.AllKeys;
                    string _sonuc = "Sonuc:";
                    foreach (var item in _allkeys)
                    {
                        if (Request.Form.GetValues(item)[0] != null)
                        {
                            _sonuc += "|" + item + ":" + Request.Form.GetValues(item)[0];
                        }
                    }
                    po.Tip = "4";

                }
                else if (id == "Is")
                {
                    // TRANID = &PAResSyntaxOK = true & islemtipi = Auth & total1 = 1 % 2c0 & lang = tr & merchantID = 700657088936 & maskedCreditCard = 4446 + 77 * *+****+1664 & amount = 1 % 2c00 & sID = 1 & ACQBIN = 406456 & Ecom_Payment_Card_ExpDate_Year = 2020 & MaskedPan = 444677 * **1664 & clientIp = 212.156.82.206 & iReqDetail = &okUrl = http % 3a % 2f % 2flocalhost % 3a2155 % 2fOdeme % 2fOdemeSonucu % 2fIs % 3fbank % 3d6 % 26 % 26OrderID % 3dCRM - 7298114 & md = 444677 % 3a76F90B2B3F6D080412FAFB8C7DD9E4FE0E9A72C1BE4B2CFD4E72F151A99F2731 % 3a4055 % 3a % 23 % 23700657088936 & taksit = &vendorCode = &paresTxStatus = N & Ecom_Payment_Card_ExpDate_Month = 11 & storetype = 3d_pay & iReqCode = &veresEnrolledStatus = Y & mdErrorMsg = Not + authenticated & PAResVerified = true & cavv = &digest = digest & failUrl = http % 3a % 2f % 2flocalhost % 3a2155 % 2fOdeme % 2fOdemeSonucu % 2fIs & cavvAlgorithm = &price1 = 1 % 2c0 & xid = oOsKyn4eIjtGglgogxhRvMv18 % 2fI % 3d & currency = 949 & oid = CRM - 7298114 & mdStatus = 0 & dsId = 1 & eci = &version = 2.0 & clientid = 700657088936 & txstatus = N & HASH = x4qFUAlhAZXN % 2fpe % 2bjB1qQokl3x8 % 3d & rnd = Dt1RgollXJ5nYJFPv7A3 & HASHPARAMS = clientid % 3aoid % 3amdStatus % 3acavv % 3aeci % 3amd % 3arnd % 3a & HASHPARAMSVAL = 700657088936CRM - 72981140444677 % 3a76F90B2B3F6D080412FAFB8C7DD9E4FE0E9A72C1BE4B2CFD4E72F151A99F2731 % 3a4055 % 3a % 23 % 23700657088936Dt1RgollXJ5nYJFPv7A3

                    try { txnamount = new string[] { "0" }; } catch { txnamount = new string[] { "0" }; }

                    if (Request.Form.GetValues("amount") != null)
                    {
                        try { txnamount = new string[] { Convert.ToString(Convert.ToDecimal(Request.Form.GetValues("amount")[0])) }; } catch { txnamount = new string[] { "0" }; }
                    }
                    try { xid = Request.Form.GetValues("xid"); } catch { xid = new string[] { "XMLHATA" }; }
                    try { hostmsg = Request.Form.GetValues("hostmsg"); } catch { hostmsg = new string[] { "XMLHATA" }; }
                    try { taksitAdet = Request.Form.GetValues("taksit"); } catch { taksitAdet = new string[] { "0" }; }
                    try { bankaAd = Request.Form.GetValues("bankaAd"); } catch { bankaAd = new string[] { "XMLHATA" }; }
                    try { authcode = Request.Form.GetValues("authcode"); } catch { authcode = new string[] { "XMLHATA" }; }
                    try { hostrefnum = Request.Form.GetValues("hostrefnum"); } catch { hostrefnum = new string[] { "XMLHATA" }; }
                    try { rnd = Request.Form.GetValues("rnd"); } catch { rnd = new string[] { "XMLHATA" }; }
                    try { procreturncode = Request.Form.GetValues("procreturncode"); } catch { procreturncode = new string[] { "XMLHATA" }; }
                    try { transid = Request.Form.GetValues("transid"); } catch { transid = new string[] { "XMLHATA" }; }
                    try { mode = Request.Form.GetValues("mode"); } catch { mode = new string[] { "XMLHATA" }; }
                    try { response = Request.Form.GetValues("response"); } catch { response = new string[] { "XMLHATA" }; }
                    try { successurl = Request.Form.GetValues("okUrl"); } catch { successurl = new string[] { "XMLHATA" }; }
                    try { errmsg = Request.Form.GetValues("ErrMsg"); } catch { errmsg = new string[] { "XMLHATA" }; }
                    try { md = Request.Form.GetValues("md"); } catch { md = new string[] { "XMLHATA" }; }
                    try { oid = Request.Form.GetValues("oid"); } catch { oid = new string[] { "XMLHATA" }; }
                    try { hash = Request.Form.GetValues("hash"); } catch { hash = new string[] { "XMLHATA" }; }
                    try { txntimestamp = Request.Form.GetValues("EXTRA.TRXDATE"); } catch { txntimestamp = new string[] { "XMLHATA" }; }
                    try { customeripaddress = Request.Form.GetValues("clientIp"); } catch { customeripaddress = new string[] { "XMLHATA" }; }
                    try { terminalid = Request.Form.GetValues("terminalid"); } catch { terminalid = new string[] { "XMLHATA" }; }
                    try { orderid = Request.Form.GetValues("ReturnOid"); } catch { orderid = new string[] { "XMLHATA" }; }
                    try { MaskedPan = Request.Form.GetValues("MaskedPan"); } catch { MaskedPan = new string[] { "XMLHATA" }; }
                    try { secure3dhash = Request.Form.GetValues("HASH"); } catch { secure3dhash = new string[] { "XMLHATA" }; }

                    try
                    {
                        string[] _allkeys = Request.Form.AllKeys;
                        string _sonuc = "Sonuc:";
                        foreach (var item in _allkeys)
                        {

                            if (Request.Form.GetValues(item)[0] != null)
                            {
                                _sonuc += "|" + item + ":" + Request.Form.GetValues(item)[0];
                            }

                        }
                    }
                    catch
                    {
                        ;
                    }
                }
                else if (id == "Vakif" || id == "vakif")
                {


                    List<YKBPosInfo> posInfoList = new List<YKBPosInfo>();

                    Bankalar dtYP = ocmde.Bankalar.Where(__bank=> __bank.Sirket == "Ozerdem" && __bank.Banka == "Vakif" && __bank.Aktif == true ).FirstOrDefault();
                    if (dtYP != null)
                    {
                        posInfoList.Add(new YKBPosInfo()
                        {
                            Bank = IDETicaret.Banka.Entity.Banks.YAPIKREDI,
                            BankName = Convert.ToString(dtYP.BankName),
                            TerminalID = Convert.ToString(dtYP.TerminalID),
                            MerchantID = Convert.ToString(dtYP.MarchantID),
                            ClientID = Convert.ToString(dtYP.ClientID),
                            Name = Convert.ToString(dtYP.Name),
                            Password = Convert.ToString(dtYP.Password),
                            ekPassword = Convert.ToString(dtYP.ProvisionPassword),
                            PosType = IDETicaret.Banka.Entity.PosType.YAPIKREDI,
                            PosURL = Convert.ToString(dtYP.PosUrl),
                            Key = Convert.ToString(dtYP.ProvisionPassword),
                            SuccessUrl = Convert.ToString(dtYP.okURL)
                        });
                    }


                    ViewBag.Status = Request.Form["Status"];
                    ViewBag.MerchantId = Request.Form["MerchantId"];
                    ViewBag.VerifyEnrollmentRequestId = Request.Form["VerifyEnrollmentRequestId"];
                    ViewBag.Xid = Request.Form["Xid"];
                    ViewBag.PurchAmount = Request.Form["PurchAmount"];
                    ViewBag.Xid = Request.Form["Xid"];
                    ViewBag.SessionInfo = Request.Form["SessionInfo"];
                    ViewBag.PurchCurrency = Request.Form["PurchCurrency"];
                    ViewBag.Pan = Request.Form["Pan"];
                    ViewBag.ExpiryDate = Request.Form["Expiry"];
                    ViewBag.Eci = Request.Form["Eci"];
                    ViewBag.Cavv = Request.Form["Cavv"];
                    string __deger = ViewBag.VerifyEnrollmentRequestId;
                    ViewBag.CVV = Convert.ToString(ocmde.CVVs.Where(c => c.Key == __deger).OrderByDescending(o => o.ID).FirstOrDefault().Value);
                    ViewBag.InstallmentCount = Request.Form["InstallmentCount"];

                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    XmlElement rootNode = xmlDoc.CreateElement("VposRequest");
                    xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
                    xmlDoc.AppendChild(rootNode);
                    //eklemek istediğiniz diğer elementleride bu şekilde ekleyebilirsiniz.
                    XmlElement MerchantIdNode = xmlDoc.CreateElement("MerchantId");
                    XmlElement PasswordNode = xmlDoc.CreateElement("Password");
                    XmlElement TerminalNoNode = xmlDoc.CreateElement("TerminalNo");
                    XmlElement TransactionTypeNode = xmlDoc.CreateElement("TransactionType");
                    XmlElement NumberOfInstallmentsNode = null;
                    if (ViewBag.InstallmentCount != "" && ViewBag.InstallmentCount != "0" && ViewBag.InstallmentCount != "1")
                        NumberOfInstallmentsNode = xmlDoc.CreateElement("NumberOfInstallments");
                    XmlElement CvvNode = xmlDoc.CreateElement("Cvv");
                    XmlElement ECINode = xmlDoc.CreateElement("ECI");
                    XmlElement CAVVNode = xmlDoc.CreateElement("CAVV");
                    XmlElement MpiTransactionIdNode = xmlDoc.CreateElement("MpiTransactionId");
                    XmlElement OrderIdNode = xmlDoc.CreateElement("OrderId");
                    XmlElement ClientIpNode = xmlDoc.CreateElement("ClientIp");
                    XmlElement TransactionDeviceSourceNode = xmlDoc.CreateElement("TransactionDeviceSource");
                    XmlElement SurchargeAmountNode = xmlDoc.CreateElement("SurchargeAmount");
                    XmlElement CurrencyCodeNode = xmlDoc.CreateElement("CurrencyCode");
                    XmlElement CurrencyAmountNode = xmlDoc.CreateElement("CurrencyAmount");
                    XmlElement PanNode = xmlDoc.CreateElement("Pan");
                    XmlElement ExpiryNode = xmlDoc.CreateElement("Expiry");

                    

                    //yukarıda eklediğimiz node lar için değerleri ekliyoruz.
                    XmlText MerchantIdText = xmlDoc.CreateTextNode(posInfoList[0].MerchantID);
                    XmlText PasswordText = xmlDoc.CreateTextNode(posInfoList[0].Password);
                    XmlText TerminalNoText = xmlDoc.CreateTextNode(posInfoList[0].TerminalID);
                    XmlText TransactionTypeText = xmlDoc.CreateTextNode("Sale");
                    XmlText MpiTransactionIdText = xmlDoc.CreateTextNode(ViewBag.VerifyEnrollmentRequestId);
                    XmlText NumberOfInstallmentsText = null;
                    if (NumberOfInstallmentsNode != null)
                        NumberOfInstallmentsText = xmlDoc.CreateTextNode(ViewBag.InstallmentCount);
                    XmlText CvvText = xmlDoc.CreateTextNode(ViewBag.CVV);
                    XmlText ECIText = xmlDoc.CreateTextNode(ViewBag.Eci);
                    XmlText CAVVText = xmlDoc.CreateTextNode(ViewBag.Cavv);
                    XmlText OrderIdText = xmlDoc.CreateTextNode(ViewBag.Xid);
                    XmlText ClientIpText = xmlDoc.CreateTextNode("149.0.253.143");
                    XmlText TransactionDeviceSourceText = xmlDoc.CreateTextNode("0");
                    XmlText SurchargeAmountText = xmlDoc.CreateTextNode("100");
                    XmlText CurrencyCodeText = xmlDoc.CreateTextNode("949");
                    XmlText CurrencyAmountText = xmlDoc.CreateTextNode((Convert.ToDecimal(Request.Form["PurchAmount"]) / 100).ToString().Replace(",", "."));
                    XmlText PanText = xmlDoc.CreateTextNode(ViewBag.Pan);
                    XmlText ExpiryText = xmlDoc.CreateTextNode(ViewBag.ExpiryDate);
                    
                    txnamount = new string[] { (Convert.ToDecimal(Request.Form["PurchAmount"]) / 100).ToString().Replace(",",".") };
                    taksitAdet = new string[] { ViewBag.InstallmentCount };
                    orderid = new string[] { ViewBag.VerifyEnrollmentRequestId };

                    //nodeları root elementin altına ekliyoruz.
                    rootNode.AppendChild(MerchantIdNode);
                    rootNode.AppendChild(PasswordNode);
                    rootNode.AppendChild(TerminalNoNode);
                    rootNode.AppendChild(TransactionTypeNode);
                    rootNode.AppendChild(MpiTransactionIdNode);
                    if (NumberOfInstallmentsText != null)
                        rootNode.AppendChild(NumberOfInstallmentsNode);
                    rootNode.AppendChild(CvvNode);
                    rootNode.AppendChild(ECINode);
                    rootNode.AppendChild(CAVVNode);
                    rootNode.AppendChild(OrderIdNode);
                    rootNode.AppendChild(ClientIpNode);
                    rootNode.AppendChild(TransactionDeviceSourceNode);
                    rootNode.AppendChild(SurchargeAmountText);
                    rootNode.AppendChild(CurrencyCodeText);
                    rootNode.AppendChild(CurrencyAmountText);
                    rootNode.AppendChild(PanText);
                    rootNode.AppendChild(ExpiryText);

                    //nodelar için oluşturduğumuz textleri node lara ekliyoruz.
                    MerchantIdNode.AppendChild(MerchantIdText);
                    PasswordNode.AppendChild(PasswordText);
                    TerminalNoNode.AppendChild(TerminalNoText);
                    TransactionTypeNode.AppendChild(TransactionTypeText);
                    MpiTransactionIdNode.AppendChild(MpiTransactionIdText);
                    if (NumberOfInstallmentsText != null)
                        NumberOfInstallmentsNode.AppendChild(NumberOfInstallmentsText);
                    CvvNode.AppendChild(CvvText);
                    ECINode.AppendChild(ECIText);
                    CAVVNode.AppendChild(CAVVText);
                    OrderIdNode.AppendChild(OrderIdText);
                    ClientIpNode.AppendChild(ClientIpText);
                    TransactionDeviceSourceNode.AppendChild(TransactionDeviceSourceText);
                    SurchargeAmountNode.AppendChild(SurchargeAmountText);
                    CurrencyCodeNode.AppendChild(CurrencyCodeText);
                    CurrencyAmountNode.AppendChild(CurrencyAmountText);
                    PanNode.AppendChild(PanText);
                    ExpiryNode.AppendChild(ExpiryText);

                    string xmlMessage = xmlDoc.OuterXml;

                    //oluşturduğumuz xml i vposa gönderiyoruz.
                    byte[] dataStream = Encoding.UTF8.GetBytes("prmstr=" + xmlMessage);
                    HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("https://onlineodeme.vakifbank.com.tr:4443/VposService/v3/Vposreq.aspx");//Vpos adresi
                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.ContentLength = dataStream.Length;
                    webRequest.KeepAlive = false;
                    string responseFromServer = "";

                    using (Stream newStream = webRequest.GetRequestStream())
                    {
                        newStream.Write(dataStream, 0, dataStream.Length);
                        newStream.Close();
                    }

                    using (WebResponse webResponse = webRequest.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            responseFromServer = reader.ReadToEnd();
                            reader.Close();
                        }
                        webResponse.Close();
                    }

                    if (string.IsNullOrEmpty(responseFromServer))
                    {
                        return View();
                    }
                    else
                    {
                        var xmlResponse = new XmlDocument();
                        xmlResponse.LoadXml(responseFromServer);
                        var resultCodeNode = xmlResponse.SelectSingleNode("VposResponse/ResultCode");
                        var resultDescriptionNode = xmlResponse.SelectSingleNode("VposResponse/ResultDescription");
                        string resultCode = "";
                        string resultDescription = "";

                        if (resultCodeNode != null)
                        {
                            resultCode = resultCodeNode.InnerText;
                        }
                        if (resultDescriptionNode != null)
                        {
                            resultDescription = resultDescriptionNode.InnerText;
                        }

                        if (resultCode == "0000")
                        {
                            procreturncode = new string[] { "00" };
                            response = new string[] { "Approved" };
                            errmsg = new string[] { resultDescription };
                        }
                        else
                        {
                            procreturncode = new string[] { resultCode };
                            response = new string[] { "" };
                            errmsg = new string[] { resultDescription };
                        }
                        ViewBag.Sonuc = "İşlem Sonucu " + resultCode + " " + resultDescription;




                    }




                }
                

                string deger = Request.Form.ToString();

                if (id == "Garanti")
                {
                    po.txnamount = Convert.ToDecimal(txnamount == null ? "0" : txnamount[0]) / 100;
                }
                else
                {
                    po.txnamount = Convert.ToDecimal(txnamount == null ? "0" : txnamount[0]);
                }
                po.xid = Convert.ToString(xid == null ? "" : xid[0]);
                po.hostmsg = Convert.ToString(hostmsg == null ? "" : hostmsg[0]);
                po.taksitAdet = (taksitAdet == null || taksitAdet[0] == "" ? 0 : Convert.ToInt32(taksitAdet[0]));
                po.bankaAd = id;
                po.authcode = Convert.ToString(authcode == null ? "" : authcode[0]);
                po.hostrefnum = Convert.ToString(hostrefnum == null ? "" : hostrefnum[0]);
                po.rnd = Convert.ToString(rnd == null ? "" : rnd[0]);
                po.procreturncode = Convert.ToString(procreturncode == null ? "" : procreturncode[0]);
                po.transid = Convert.ToString(transid == null ? "" : transid[0]);
                po.mode = Convert.ToString(mode == null ? "" : mode[0]);
                po.response = Convert.ToString(response == null ? "" : response[0]);
                po.successurl = Convert.ToString(successurl == null ? "" : successurl[0]);
                po.errmsg = Convert.ToString(errmsg == null ? "" : errmsg[0]);
                po.md = Convert.ToString(md == null ? "" : md[0]);
                po.oid = Convert.ToString(oid == null ? "" : oid[0]);
                po.hash = Convert.ToString(hash == null ? "" : hash[0]);
                po.txntimestamp = Convert.ToString(txntimestamp == null ? "" : txntimestamp[0]);
                po.customeripaddress = Convert.ToString(customeripaddress == null ? "" : customeripaddress[0]);
                po.terminalid = Convert.ToString(terminalid == null ? "" : terminalid[0]);
                po.MaskedPan = Convert.ToString(MaskedPan == null ? "" : MaskedPan[0]);
                po.Isim = Convert.ToString(Session["Isim"] == null ? 0 : Session["Isim"]);
                po.secure3dhash = Convert.ToString(secure3dhash == null ? "" : secure3dhash[0]);

                po.Tip = "5";
                if (SessionPersister.account == null)
                {
                    po.UserID = 0;
                    po.UserName = "Misafir Kullanıcı";
                }
                else
                {
                    var account = SessionPersister.account;
                    Account user = ((Account)account);

                    po.UserID = user.Id;
                    po.UserName = user.Username;
                }

                po.orderid = Convert.ToString(orderid == null ? "" : orderid[0]);
                po.SonucDegeri = Request.Form.AllKeys.ToString();


                po.Tip = "6";

                string OrderID = (orderid == null ? "0" : orderid[0]);
                po.Tip = "6.1_0_1";

                if (response != null)
                {
                    po.Tip = "6.1_0_2";
                    if (procreturncode[0] == "00" && response[0] == "Approved")
                    {
                        po.Tip = "6.1_0_3";
                        ViewBag.Sonuc = "";
                        {
                            if (SessionPersister.account == null)
                            {
                                po.Tip = "6.1_0_4_1";
                                Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                                order.Aktif = true;
                                ocmde.SaveChanges();

                                try
                                {
                                    po.Tip = "6.1_0_4_2";
                                    List<Item> test = (List<Item>)Session["cart"];
                                    po.Tip = "6.1_" + test.Count;
                                    List<Item> cart = ((List<Item>)Session["cart"]).Where((x) => x.Aktarilacak == true).ToList();
                                    if (cart.Count > 0)
                                    {
                                        po.Tip = "6.2";

                                        // Remove Cart
                                        #region 
                                        List<Item> cart2 = ((List<Item>)Session["cart"]);

                                        foreach (var item in cart)
                                        {
                                            if (item.Aktarilacak)
                                                cart2.Remove(item);
                                        }

                                        Session["cart"] = cart2;

                                        #endregion
                                    }
                                    else
                                    {
                                        po.Tip = "6.2.2";
                                    }
                                }
                                catch(Exception ex5)
                                {

                                }
                            }
                            else
                            {
                                var account = SessionPersister.account;
                                if (account is Account && !((IDETicaret.Models.Account)account).IsAdmin)
                                {
                                    var customer = (IDETicaret.Models.Account)account;
                                    List<Card> cart = ocmde.Card.Where((x) => x.CustomerId == ((IDETicaret.Models.Account)account).Id && x.Aktarilacak == true).ToList();

                                    if (cart.Count > 0)
                                    {
                                        Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                                        order.Aktif = true;
                                        ocmde.SaveChanges();

                                        #region 
                                        ocmde.Card.RemoveRange(cart);
                                    }
                                    ocmde.SaveChanges();
                                    #endregion

                                    //return View();
                                }
                                //return View();
                            }

                            ViewBag.OrderID = OrderID;
                        }
                        {
                            Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID.ToString()).FirstOrDefault();
                            if (order != null)
                                order.OrderStatusId = 6;
                            OrdersLog ordelogr = ocmde.OrdersLog.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                            if (ordelogr != null)
                                ordelogr.OrderStatusId = 6;

                            po.GercekSiparisNo = OrderID.ToString();
                            ocmde.SaveChanges();
                        }
                        ViewBag.Sonuc = "Ödeme başarılı bir şekilde alınmıştır. <br />Sipariş Numaranız : " + po.orderid + " Onay kodunuz : " + Convert.ToString(authcode == null ? "" : authcode[0]);
                    }
                    else if (errmsg.Length > 0)
                    {
                        Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                        if (order != null)
                            order.OrderStatusId = 4;
                        ocmde.SaveChanges();
                        ViewBag.Sonuc = "ÖDEME GERÇEKLEŞMEDİ : " + errmsg[0];
                    }
                    else
                    {
                        Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                        if (order != null)
                            order.OrderStatusId = 4;
                        ocmde.SaveChanges();
                        ViewBag.Sonuc = "ÖDEME GERÇEKLEŞMEDİ : " + hostmsg[0];
                    }
                }
                else
                {
                    Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                    if (order != null)
                        order.OrderStatusId = 4;
                    ocmde.SaveChanges();
                    ViewBag.Sonuc = "ÖDEME GERÇEKLEŞMEDİ...";
                }

                MailGonder("ÖDEME SONUCU DOĞRU TAMAMLANDI", @"
Kullanıcı : " + po.UserName + @"
Banka : " + po.bankaAd + @"
Sipariş No : " + po.GercekSiparisNo + @"
OrderID: " + OrderID + @"
Tutar : " +                    po.txnamount + @"
K.K. : " + po.Isim + @"
Response : " + po.response + @"
Cevap Kodu : " +                    po.procreturncode+ @"
Code : " + po.authcode + @"
Xml : "+ Request.Form.AllKeys.ToString() +@"
"
                    );
            }
            catch (Exception err)
            {
                po.Aciklama = po.Tip + " " + err.Message;
                try
                {
                    // Hata Maili Gönderimi
                    if (false)
                    {
                        #region 


                        MailMessage ePosta = new MailMessage();
                        ePosta.From = new MailAddress(Convert.ToString("info@idyazilim.com"), "IDYAZILIM - E-Ticaret");

                        ePosta.To.Add(new MailAddress("mail.yunuskose@gmail.com", "mail.yunuskose@gmail.com"));

                        ePosta.Subject = "ÇİFTTEKER - Sipariş Hatası -" + DateTime.Today.ToString("dd-MM-yyyy");
                        //
                        ePosta.Body = @"
Sayın Yetkili; 

ciftteker.com sitesinde hata oluşmuştur, acil bakılması rica olunur.

" + po.bankaAd + @"
" + po.Tip + @"
" + po.GercekSiparisNo + @"
" + po.orderid + "-" + po.oid + @"
" + po.procreturncode + @"
" + po.authcode + @"
" + po.Isim + @"
" + po.response + @"
" + po.errmsg + @"
" + po.Aciklama + @"

İyi çalışmalar.

";

                        SmtpClient smtp = new SmtpClient();


                        smtp.Credentials = new System.Net.NetworkCredential("info@idyazilim.com", "MAİL PAROLASI");
                        smtp.Port = 587;
                        smtp.Host = "mail.idyazilim.com";
                        smtp.EnableSsl = false;

                        smtp.Send(ePosta);


                        #endregion
                    }
                }
                catch (Exception xxx)
                {
                    ;
                }
            }
            if (true)
            {
                string[] allkeys = Request.Form.AllKeys;
                string logtut = "";
                foreach (var item in allkeys)
                {
                    if (Request.Form.GetValues(item)[0] != null)
                    {
                        logtut += "<br/>" + item + ":" + Request.Form.GetValues(item)[0];
                    }
                }
                po.Aciklama += logtut;
            }



            ocmde.PosOdemeleri.Add(po);
            try
            {
                ocmde.SaveChanges();
            }
            catch (Exception err)
            {

            }



            return View("ConfirmOrder3");
        }

        public ActionResult Remove(int id)
        {
            var product = ocmde.Product.Find(id);
            var account = SessionPersister.account;
            if (SessionPersister.account == null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = Exists(id, cart);
                cart.RemoveAt(index);
                Session["cart"] = cart;
                return RedirectToAction("Detay");
            }
            else
            {
                Account user = ((Account)account);
                var vendorIds = product.VendorId;

                if (ocmde.Card.Where((x) => x.CustomerId == user.Id && x.VendorId == vendorIds && x.ProductId == product.Id).Count() > 0)
                {
                    //Update
                    Card c = ocmde.Card.Where((x) => x.CustomerId == user.Id && x.VendorId == vendorIds && x.ProductId == product.Id).FirstOrDefault();
                    ocmde.Card.Remove(c);
                    ocmde.SaveChanges();
                }
                return RedirectToAction("Detay");
            }

        }

        [HttpPost]
        public ActionResult Update(FormCollection fc)
        {
            int productId = Convert.ToInt32(fc["productId"]);
            var product = ocmde.Product.Find(productId);
            if (SessionPersister.account == null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = Exists(productId, cart);
                cart[index].quantity = Convert.ToInt32(fc["quantity"]);
                Session["cart"] = cart;
                return RedirectToAction("Detay");
            }
            else
            {
                var account = SessionPersister.account;
                Account user = ((Account)account);
                var vendorIds = product.VendorId;

                if (ocmde.Card.Where((x) => x.CustomerId == user.Id && x.VendorId == vendorIds && x.ProductId == productId).Count() > 0)
                {
                    //Update
                    Card c = ocmde.Card.Where((x) => x.CustomerId == user.Id && x.VendorId == vendorIds && x.ProductId == productId).FirstOrDefault();
                    c.ProductId = productId;
                    c.Quantity = Convert.ToInt32(fc["quantity"]);
                    ocmde.SaveChanges();
                }
                return RedirectToAction("Detay");
            }

        }

        public ActionResult Buy(int id, int quantity = 1)
        {
            try
            {

                {
                    if (SessionPersister.account == null)
                    {
                        var product = ocmde.Product.Find(id);
                        if (Session["cart"] == null)
                        {
                            List<Item> cart = new List<Item>();
                            cart.Add(new Item()
                            {
                                product = product,
                                quantity = quantity
                            });
                            Session["cart"] = cart;
                        }
                        else
                        {
                            List<Item> cart = (List<Item>)Session["cart"];
                            int index = Exists(id, cart);
                            if (index == -1)
                            {
                                cart.Add(new Item()
                                {
                                    product = product,
                                    quantity = quantity
                                });
                            }
                            else
                            {
                                cart[index].quantity++;
                            }
                            Session["cart"] = cart;
                        }
                        return RedirectToAction("Detay");

                    }
                    else
                    {
                        var product = ocmde.Product.Find(id);
                        var account = SessionPersister.account;
                        Account user = ((Account)account);
                        var vendorIds = product.VendorId;

                        if (ocmde.Card.Where((x) => x.CustomerId == user.Id && x.VendorId == vendorIds && x.ProductId == product.Id).Count() > 0)
                        {
                            //Update
                            Card c = ocmde.Card.Where((x) => x.CustomerId == user.Id && x.VendorId == vendorIds && x.ProductId == product.Id).FirstOrDefault();
                            c.ProductId = product.Id;
                            c.Quantity = quantity;
                            c.CDate = DateTime.Now;
                            ocmde.SaveChanges();
                        }
                        else
                        {
                            //İnsert
                            Card c = new Card();
                            c.CustomerId = user.Id;
                            c.VendorId = vendorIds;
                            c.ProductId = product.Id;
                            c.Quantity = quantity;
                            c.CDate = DateTime.Now;
                            ocmde.Card.Add(c);
                            ocmde.SaveChanges();
                        }
                        return RedirectToAction("Detay");
                    }
                }





            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Home", "Index"));
            }
        }

        public ActionResult BuyJquery(int id, int quantity = 1)
        {
            try
            {

                {
                    if (SessionPersister.account == null)
                    {
                        var product = ocmde.Product.Find(id);
                        if (Session["cart"] == null)
                        {
                            List<Item> cart = new List<Item>();
                            cart.Add(new Item()
                            {
                                product = product,
                                quantity = quantity
                            });
                            Session["cart"] = cart;
                        }
                        else
                        {
                            List<Item> cart = (List<Item>)Session["cart"];
                            int index = Exists(id, cart);
                            if (index == -1)
                            {
                                cart.Add(new Item()
                                {
                                    product = product,
                                    quantity = quantity
                                });
                            }
                            else
                            {
                                cart[index].quantity++;
                            }
                            Session["cart"] = cart;
                        }
                        //return RedirectToAction("Index");

                    }
                    else
                    {
                        var product = ocmde.Product.Find(id);
                        var account = SessionPersister.account;
                        Account user = ((Account)account);
                        var vendorIds = product.VendorId;

                        if (ocmde.Card.Where((x) => x.CustomerId == user.Id && x.VendorId == vendorIds && x.ProductId == product.Id).Count() > 0)
                        {
                            //Update
                            Card c = ocmde.Card.Where((x) => x.CustomerId == user.Id && x.VendorId == vendorIds && x.ProductId == product.Id).FirstOrDefault();
                            c.ProductId = product.Id;
                            c.Quantity = quantity;
                            c.CDate = DateTime.Now;
                            ocmde.SaveChanges();
                        }
                        else
                        {
                            //İnsert
                            Card c = new Card();
                            c.CustomerId = user.Id;
                            c.VendorId = vendorIds;
                            c.ProductId = product.Id;
                            c.Quantity = quantity;
                            c.CDate = DateTime.Now;
                            ocmde.Card.Add(c);
                            ocmde.SaveChanges();
                        }
                        //return RedirectToAction("Index");
                    }
                }





            }
            catch (Exception e)
            {
                //return View("Error", new HandleErrorInfo(e, "Home", "Index"));
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        private int Exists(int productId, List<Item> cart)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].product.Id == productId)
                {
                    return i;
                }
            }
            return -1;
        }

        public ActionResult Save()
        {
            try
            {
                if (SessionPersister.account == null)
                {
                    var account = SessionPersister.account;
                    if (account is Account && !((IDETicaret.Models.Account)account).IsAdmin)
                    {
                        var customer = (IDETicaret.Models.Account)account;

                        List<Item> cart = ((List<Item>)Session["cart"]).Where((x) => x.Aktarilacak == true).ToList();

                        // Create new order
                        Orders order = new Orders()
                        {
                            CustomerId = customer.Id,
                            DateCreation = DateTime.Now,
                            Name = Resources.Vendor.New_Order_for_Vendor,
                            OrderStatusId = 1,
                            Vendor = ocmde.Vendor.FirstOrDefault()
                        };
                        ocmde.Orders.Add(order);
                        ocmde.SaveChanges();

                        // Create order details
                        cart.ForEach(i =>
                        {
                            OrdersDetail ordersDetail = new OrdersDetail()
                            {
                                OrderId = order.Id,
                                Price = i.product.Price,
                                Quantity = Convert.ToInt32(i.quantity),
                                ProductId = i.product.Id
                            };
                            ocmde.OrdersDetail.Add(ordersDetail);
                            ocmde.SaveChanges();
                        });



                        // Remove Cart
                        #region 
                        List<Item> cart2 = ((List<Item>)Session["cart"]);

                        foreach (var item in cart)
                        {
                            cart2.Remove(item);
                        }

                        Session["cart"] = cart2;
                        #endregion

                        return RedirectToAction("Liste", "Orders");
                    }
                    return RedirectToAction("Liste", "Orders");
                }
                else
                {
                    var account = SessionPersister.account;
                    if (account is Account && !((IDETicaret.Models.Account)account).IsAdmin)
                    {
                        var customer = (IDETicaret.Models.Account)account;
                        List<Card> cart = ocmde.Card.Where((x) => x.CustomerId == ((IDETicaret.Models.Account)account).Id && x.Aktarilacak == true).ToList();


                        // Create new order
                        Orders order = new Orders()
                        {
                            CustomerId = customer.Id,
                            DateCreation = DateTime.Now,
                            Name = Resources.Vendor.New_Order_for_Vendor,
                            OrderStatusId = 1,
                            Vendor = ocmde.Vendor.FirstOrDefault()
                        };
                        ocmde.Orders.Add(order);
                        ocmde.SaveChanges();

                        // Create order details
                        cart.ForEach(i =>
                        {
                            OrdersDetail ordersDetail = new OrdersDetail()
                            {
                                OrderId = order.Id,
                                Price = i.Product.Price,
                                Quantity = Convert.ToInt32(i.Quantity),
                                ProductId = i.Product.Id
                            };
                            ocmde.OrdersDetail.Add(ordersDetail);
                            ocmde.SaveChanges();
                        });



                        // Remove Cart
                        #region 
                        ocmde.Card.RemoveRange(cart);
                        ocmde.SaveChanges();
                        #endregion

                        return RedirectToAction("Liste", "Orders");
                    }
                    return RedirectToAction("Liste", "Orders");
                }
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Home", "Index"));
            }
        }

        public void MailGonder(string Baslik, string Icerik)
        {
            return;
            try
            {
                // Hata Maili Gönderimi

                #region 


                MailMessage ePosta = new MailMessage();
                ePosta.From = new MailAddress(Convert.ToString("info@idyazilim.com"), "IDYAZILIM - E-Ticaret");

                ePosta.To.Add(new MailAddress("mail.yunuskose@gmail.com", "mail.yunuskose@gmail.com"));

                ePosta.Subject = "ÇİFTTEKER - "+ Baslik + " - " + DateTime.Today.ToString("dd-MM-yyyy");
                //
                ePosta.Body = @"
Sayın Yetkili; 

ciftteker.com sitesinde işlem ayrıntısı aşağıdaki gibidir.
"+(Request.Browser.IsMobileDevice == true ? "Mobile" : "Windows(Diğer)") +@"
"+ Request.UserAgent.ToLower() + @"
" + Icerik + @"

İyi çalışmalar.

";

                SmtpClient smtp = new SmtpClient();


                smtp.Credentials = new System.Net.NetworkCredential("info@idyazilim.com", "MAİL PAROLASI");
                smtp.Port = 587;
                smtp.Host = "mail.idyazilim.com";
                smtp.EnableSsl = false;

                smtp.Send(ePosta);


                #endregion

            }
            catch (Exception xxx)
            {
                ;
            }
        }

        [ValidateInput(false)]
        public ActionResult Odeme(string id)
        {
            ViewBag.Form = id;
            return View();
        }

        public ActionResult OdemePartial(int id)
        {
            return View(id);
        }

        public ActionResult Kontrol()
        {
            return View();
        }


        public ActionResult OdemeSonucuGoster(string id)
        {
            if (true)
            {
                try
                {

                    string[] allkeys = Request.Form.AllKeys;
                    string logtut = "";
                    foreach (var item in allkeys)
                    {
                        if (Request.Form.GetValues(item)[0] != null)
                        {
                            logtut += "<br/>" + item + ":" + Request.Form.GetValues(item)[0];
                        }
                    }

                    var account = SessionPersister.account;
                    if (account is Account && !((IDETicaret.Models.Account)account).IsAdmin)
                    {
                        // Log Tutuyoruz
                        Loglar l = new Loglar();
                        l.Tip = "Ödeme Sonuç";
                        l.Kullanici = ((IDETicaret.Models.Account)account).Username;
                        l.Aciklama = logtut;
                        l.Tarih = DateTime.Now;
                        ocmde.Loglar.Add(l);
                    }
                    else
                    {
                        // Log Tutuyoruz
                        Loglar l = new Loglar();
                        l.Tip = "Ödeme Sonuç";
                        l.Kullanici = "Misafir Kullanıcı";
                        l.Aciklama = logtut;
                        l.Tarih = DateTime.Now;
                        ocmde.Loglar.Add(l);
                    }
                }
                catch (Exception err)
                {
                    ;
                }
            }


            PosOdemeleri po = new PosOdemeleri();
            po.KayitYapanKullanici = "";
            po.Tarih = DateTime.Today;
            po.KayitTarihi = DateTime.Now;
            po.Aktarildi = false;
            po.KontrolEdildi = false;
            po.procreturncode = "";
            po.UserID = 0;
            po.Aciklama = "";
            po.Tip = "1";

            //PaymentResult sonuc = Front.Controller.CCManager.ConfirmPayment(,Request,HttpContext.);
            try
            {
                string[] txnamount = null;
                string[] xid = null;
                string[] hostmsg = null;
                string[] taksitAdet = null;
                string[] bankaAd = null;
                string[] authcode = null;
                string[] hostrefnum = null;
                string[] rnd = null;
                string[] procreturncode = null;
                string[] transid = null;
                string[] mode = null;
                string[] response = null;
                string[] successurl = null;
                string[] errmsg = null;
                string[] md = null;
                string[] oid = null;
                string[] hash = null;
                string[] txntimestamp = null;
                string[] customeripaddress = null;
                string[] terminalid = null;
                string[] orderid = null;
                string[] MaskedPan = null;
                string[] secure3dhash = null;
                object a = Request.Form.GetValues("txnamount");
                object b = Request.Form.GetValues("amount");

                if (id == "Garanti")
                {
                    txnamount = new string[] { "0" };
                    if (Request.Form.GetValues("txnamount") != null)
                        txnamount = new string[] { Convert.ToString(Convert.ToDecimal(Request.Form.GetValues("txnamount")[0])) };
                    xid = Request.Form.GetValues("xid");
                    hostmsg = Request.Form.GetValues("hostmsg");
                    if (Request.Form.GetValues("taksitAdet") != null)
                        if (Request.Form.GetValues("taksitAdet").Length > 0)
                            if (Convert.ToString(Request.Form.GetValues("taksitAdet")[0]).Trim().Length > 0)
                                taksitAdet = new string[] { Convert.ToString(Convert.ToInt32(Request.Form.GetValues("taksitAdet")[0])) };
                    bankaAd = Request.Form.GetValues("bankaAd");
                    authcode = Request.Form.GetValues("authcode");
                    hostrefnum = Request.Form.GetValues("hostrefnum");
                    rnd = Request.Form.GetValues("rnd");
                    procreturncode = Request.Form.GetValues("procreturncode");
                    transid = Request.Form.GetValues("transid");
                    mode = Request.Form.GetValues("mode");
                    response = Request.Form.GetValues("response");
                    successurl = Request.Form.GetValues("successurl");
                    errmsg = Request.Form.GetValues("errmsg");
                    md = Request.Form.GetValues("md");
                    oid = Request.Form.GetValues("oid");
                    hash = Request.Form.GetValues("hash");
                    txntimestamp = Request.Form.GetValues("txntimestamp");
                    customeripaddress = Request.Form.GetValues("customeripaddress");
                    terminalid = Request.Form.GetValues("terminalid");
                    orderid = Request.Form.GetValues("orderid");
                    MaskedPan = Request.Form.GetValues("MaskedPan");
                    secure3dhash = Request.Form.GetValues("secure3dhash");

                    string[] _allkeys = Request.Form.AllKeys;
                    string _sonuc = "Sonuc:";
                    foreach (var item in _allkeys)
                    {
                        if (Request.Form.GetValues(item)[0] != null)
                        {
                            _sonuc += "|" + item + ":" + Request.Form.GetValues(item)[0];
                        }
                    }

                }
                else if (id == "Akbank")
                {
                    po.Tip = "2";
                    txnamount = new string[] { "0" };
                    if (Request.Form.GetValues("amount") != null)
                        txnamount = new string[] { Convert.ToString(Convert.ToDecimal(Request.Form.GetValues("amount")[0])) };
                    xid = Request.Form.GetValues("xid");
                    hostmsg = Request.Form.GetValues("hostmsg");
                    if (Request.Form.GetValues("taksit") != null)
                        if (Request.Form.GetValues("taksit").Length > 0)
                            if (Convert.ToString(Request.Form.GetValues("taksit")[0]).Trim().Length > 0)
                                taksitAdet = new string[] { Convert.ToString(Convert.ToInt32(Request.Form.GetValues("taksit")[0])) };
                    bankaAd = Request.Form.GetValues("bankaAd");
                    authcode = Request.Form.GetValues("authcode");
                    hostrefnum = Request.Form.GetValues("hostrefnum");
                    rnd = Request.Form.GetValues("rnd");
                    procreturncode = Request.Form.GetValues("procreturncode");
                    transid = Request.Form.GetValues("transid");
                    mode = Request.Form.GetValues("mode");
                    response = Request.Form.GetValues("response");
                    successurl = Request.Form.GetValues("okUrl");
                    errmsg = Request.Form.GetValues("ErrMsg");
                    md = Request.Form.GetValues("md");
                    oid = Request.Form.GetValues("oid");
                    hash = Request.Form.GetValues("hash");
                    txntimestamp = Request.Form.GetValues("EXTRA.TRXDATE");
                    customeripaddress = Request.Form.GetValues("clientIp");
                    terminalid = Request.Form.GetValues("terminalid");
                    orderid = Request.Form.GetValues("ReturnOid");
                    MaskedPan = Request.Form.GetValues("MaskedPan");
                    secure3dhash = Request.Form.GetValues("HASH");
                    po.Tip = "3";


                    string[] _allkeys = Request.Form.AllKeys;
                    string _sonuc = "Sonuc:";
                    foreach (var item in _allkeys)
                    {
                        if (Request.Form.GetValues(item)[0] != null)
                        {
                            _sonuc += "|" + item + ":" + Request.Form.GetValues(item)[0];
                        }
                    }
                    po.Tip = "4";

                }
                else if (id == "Is")
                {
                    // TRANID = &PAResSyntaxOK = true & islemtipi = Auth & total1 = 1 % 2c0 & lang = tr & merchantID = 700657088936 & maskedCreditCard = 4446 + 77 * *+****+1664 & amount = 1 % 2c00 & sID = 1 & ACQBIN = 406456 & Ecom_Payment_Card_ExpDate_Year = 2020 & MaskedPan = 444677 * **1664 & clientIp = 212.156.82.206 & iReqDetail = &okUrl = http % 3a % 2f % 2flocalhost % 3a2155 % 2fOdeme % 2fOdemeSonucu % 2fIs % 3fbank % 3d6 % 26 % 26OrderID % 3dCRM - 7298114 & md = 444677 % 3a76F90B2B3F6D080412FAFB8C7DD9E4FE0E9A72C1BE4B2CFD4E72F151A99F2731 % 3a4055 % 3a % 23 % 23700657088936 & taksit = &vendorCode = &paresTxStatus = N & Ecom_Payment_Card_ExpDate_Month = 11 & storetype = 3d_pay & iReqCode = &veresEnrolledStatus = Y & mdErrorMsg = Not + authenticated & PAResVerified = true & cavv = &digest = digest & failUrl = http % 3a % 2f % 2flocalhost % 3a2155 % 2fOdeme % 2fOdemeSonucu % 2fIs & cavvAlgorithm = &price1 = 1 % 2c0 & xid = oOsKyn4eIjtGglgogxhRvMv18 % 2fI % 3d & currency = 949 & oid = CRM - 7298114 & mdStatus = 0 & dsId = 1 & eci = &version = 2.0 & clientid = 700657088936 & txstatus = N & HASH = x4qFUAlhAZXN % 2fpe % 2bjB1qQokl3x8 % 3d & rnd = Dt1RgollXJ5nYJFPv7A3 & HASHPARAMS = clientid % 3aoid % 3amdStatus % 3acavv % 3aeci % 3amd % 3arnd % 3a & HASHPARAMSVAL = 700657088936CRM - 72981140444677 % 3a76F90B2B3F6D080412FAFB8C7DD9E4FE0E9A72C1BE4B2CFD4E72F151A99F2731 % 3a4055 % 3a % 23 % 23700657088936Dt1RgollXJ5nYJFPv7A3

                    try { txnamount = new string[] { "0" }; } catch { txnamount = new string[] { "0" }; }

                    if (Request.Form.GetValues("amount") != null)
                    {
                        try { txnamount = new string[] { Convert.ToString(Convert.ToDecimal(Request.Form.GetValues("amount")[0])) }; } catch { txnamount = new string[] { "0" }; }
                    }
                    try { xid = Request.Form.GetValues("xid"); } catch { xid = new string[] { "XMLHATA" }; }
                    try { hostmsg = Request.Form.GetValues("hostmsg"); } catch { hostmsg = new string[] { "XMLHATA" }; }
                    try { taksitAdet = Request.Form.GetValues("taksit"); } catch { taksitAdet = new string[] { "0" }; }
                    try { bankaAd = Request.Form.GetValues("bankaAd"); } catch { bankaAd = new string[] { "XMLHATA" }; }
                    try { authcode = Request.Form.GetValues("authcode"); } catch { authcode = new string[] { "XMLHATA" }; }
                    try { hostrefnum = Request.Form.GetValues("hostrefnum"); } catch { hostrefnum = new string[] { "XMLHATA" }; }
                    try { rnd = Request.Form.GetValues("rnd"); } catch { rnd = new string[] { "XMLHATA" }; }
                    try { procreturncode = Request.Form.GetValues("procreturncode"); } catch { procreturncode = new string[] { "XMLHATA" }; }
                    try { transid = Request.Form.GetValues("transid"); } catch { transid = new string[] { "XMLHATA" }; }
                    try { mode = Request.Form.GetValues("mode"); } catch { mode = new string[] { "XMLHATA" }; }
                    try { response = Request.Form.GetValues("response"); } catch { response = new string[] { "XMLHATA" }; }
                    try { successurl = Request.Form.GetValues("okUrl"); } catch { successurl = new string[] { "XMLHATA" }; }
                    try { errmsg = Request.Form.GetValues("ErrMsg"); } catch { errmsg = new string[] { "XMLHATA" }; }
                    try { md = Request.Form.GetValues("md"); } catch { md = new string[] { "XMLHATA" }; }
                    try { oid = Request.Form.GetValues("oid"); } catch { oid = new string[] { "XMLHATA" }; }
                    try { hash = Request.Form.GetValues("hash"); } catch { hash = new string[] { "XMLHATA" }; }
                    try { txntimestamp = Request.Form.GetValues("EXTRA.TRXDATE"); } catch { txntimestamp = new string[] { "XMLHATA" }; }
                    try { customeripaddress = Request.Form.GetValues("clientIp"); } catch { customeripaddress = new string[] { "XMLHATA" }; }
                    try { terminalid = Request.Form.GetValues("terminalid"); } catch { terminalid = new string[] { "XMLHATA" }; }
                    try { orderid = Request.Form.GetValues("ReturnOid"); } catch { orderid = new string[] { "XMLHATA" }; }
                    try { MaskedPan = Request.Form.GetValues("MaskedPan"); } catch { MaskedPan = new string[] { "XMLHATA" }; }
                    try { secure3dhash = Request.Form.GetValues("HASH"); } catch { secure3dhash = new string[] { "XMLHATA" }; }

                    try
                    {
                        string[] _allkeys = Request.Form.AllKeys;
                        string _sonuc = "Sonuc:";
                        foreach (var item in _allkeys)
                        {

                            if (Request.Form.GetValues(item)[0] != null)
                            {
                                _sonuc += "|" + item + ":" + Request.Form.GetValues(item)[0];
                            }

                        }
                    }
                    catch
                    {
                        ;
                    }
                }


                string deger = Request.Form.ToString();

                if (id == "Garanti")
                {
                    po.txnamount = Convert.ToDecimal(txnamount == null ? "0" : txnamount[0]) / 100;
                }
                else
                {
                    po.txnamount = Convert.ToDecimal(txnamount == null ? "0" : txnamount[0]);
                }
                po.xid = Convert.ToString(xid == null ? "" : xid[0]);
                po.hostmsg = Convert.ToString(hostmsg == null ? "" : hostmsg[0]);
                po.taksitAdet = (taksitAdet == null || taksitAdet[0] == "" ? 0 : Convert.ToInt32(taksitAdet[0]));
                po.bankaAd = id;
                po.authcode = Convert.ToString(authcode == null ? "" : authcode[0]);
                po.hostrefnum = Convert.ToString(hostrefnum == null ? "" : hostrefnum[0]);
                po.rnd = Convert.ToString(rnd == null ? "" : rnd[0]);
                po.procreturncode = Convert.ToString(procreturncode == null ? "" : procreturncode[0]);
                po.transid = Convert.ToString(transid == null ? "" : transid[0]);
                po.mode = Convert.ToString(mode == null ? "" : mode[0]);
                po.response = Convert.ToString(response == null ? "" : response[0]);
                po.successurl = Convert.ToString(successurl == null ? "" : successurl[0]);
                po.errmsg = Convert.ToString(errmsg == null ? "" : errmsg[0]);
                po.md = Convert.ToString(md == null ? "" : md[0]);
                po.oid = Convert.ToString(oid == null ? "" : oid[0]);
                po.hash = Convert.ToString(hash == null ? "" : hash[0]);
                po.txntimestamp = Convert.ToString(txntimestamp == null ? "" : txntimestamp[0]);
                po.customeripaddress = Convert.ToString(customeripaddress == null ? "" : customeripaddress[0]);
                po.terminalid = Convert.ToString(terminalid == null ? "" : terminalid[0]);
                po.MaskedPan = Convert.ToString(MaskedPan == null ? "" : MaskedPan[0]);
                po.Isim = Convert.ToString(Session["Isim"] == null ? 0 : Session["Isim"]);
                po.secure3dhash = Convert.ToString(secure3dhash == null ? "" : secure3dhash[0]);

                po.Tip = "5";
                if (SessionPersister.account == null)
                {
                    po.UserID = 0;
                    po.UserName = "Misafir Kullanıcı";
                }
                else
                {
                    var account = SessionPersister.account;
                    Account user = ((Account)account);

                    po.UserID = user.Id;
                    po.UserName = user.Username;
                }

                po.orderid = Convert.ToString(orderid == null ? "" : orderid[0]);
                po.SonucDegeri = Request.Form.AllKeys.ToString();


                po.Tip = "6";

                string OrderID = (orderid == null ? "0" : orderid[0]);
                po.Tip = "6.1_0_1";

                if (response != null)
                {
                    po.Tip = "6.1_0_2";
                    if (procreturncode[0] == "00" && response[0] == "Approved")
                    {
                        po.Tip = "6.1_0_3";
                        ViewBag.Sonuc = "";
                        {
                            if (SessionPersister.account == null)
                            {
                                po.Tip = "6.1_0_4_1";
                                Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                                order.Aktif = true;
                                ocmde.SaveChanges();

                                po.Tip = "6.1_0_4_2";
                                List<Item> test = (List<Item>)Session["cart"];
                                po.Tip = "6.1_" + test.Count;
                                List<Item> cart = ((List<Item>)Session["cart"]).Where((x) => x.Aktarilacak == true).ToList();
                                if (cart.Count > 0)
                                {
                                    po.Tip = "6.2";

                                    // Remove Cart
                                    #region 
                                    List<Item> cart2 = ((List<Item>)Session["cart"]);

                                    foreach (var item in cart)
                                    {
                                        if (item.Aktarilacak)
                                            cart2.Remove(item);
                                    }

                                    Session["cart"] = cart2;

                                    #endregion
                                }
                                else
                                {
                                    po.Tip = "6.2.2";
                                }
                            }
                            else
                            {
                                var account = SessionPersister.account;
                                if (account is Account && !((IDETicaret.Models.Account)account).IsAdmin)
                                {
                                    var customer = (IDETicaret.Models.Account)account;
                                    List<Card> cart = ocmde.Card.Where((x) => x.CustomerId == ((IDETicaret.Models.Account)account).Id && x.Aktarilacak == true).ToList();

                                    if (cart.Count > 0)
                                    {
                                        Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                                        order.Aktif = true;
                                        ocmde.SaveChanges();

                                        #region 
                                        ocmde.Card.RemoveRange(cart);
                                    }
                                    ocmde.SaveChanges();
                                    #endregion

                                    //return View();
                                }
                                //return View();
                            }

                            ViewBag.OrderID = OrderID;
                        }
                        {
                            Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID.ToString()).FirstOrDefault();
                            if (order != null)
                                order.OrderStatusId = 6;
                            OrdersLog ordelogr = ocmde.OrdersLog.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                            if (ordelogr != null)
                                ordelogr.OrderStatusId = 6;

                            po.GercekSiparisNo = OrderID.ToString();
                            ocmde.SaveChanges();
                        }
                        ViewBag.Sonuc = "Ödeme başarılı bir şekilde alınmıştır. <br />Sipariş Numaranız : " + po.orderid + " Onay kodunuz : " + Convert.ToString(authcode == null ? "" : authcode[0]);
                    }
                    else if (errmsg.Length > 0)
                    {
                        Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                        if (order != null)
                            order.OrderStatusId = 4;
                        ocmde.SaveChanges();
                        ViewBag.Sonuc = "ÖDEME GERÇEKLEŞMEDİ : " + errmsg[0];
                    }
                    else
                    {
                        Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                        if (order != null)
                            order.OrderStatusId = 4;
                        ocmde.SaveChanges();
                        ViewBag.Sonuc = "ÖDEME GERÇEKLEŞMEDİ : " + hostmsg[0];
                    }
                }
                else
                {
                    Orders order = ocmde.Orders.Where((x) => x.OrderID == OrderID).FirstOrDefault();
                    if (order != null)
                        order.OrderStatusId = 4;
                    ocmde.SaveChanges();
                    ViewBag.Sonuc = "ÖDEME GERÇEKLEŞMEDİ...";
                }

                MailGonder("ÖDEME SONUCU DOĞRU TAMAMLANDI", @"
Kullanıcı : " + po.UserName + @"
Banka : " + po.bankaAd + @"
Sipariş No : " + po.GercekSiparisNo + @"
OrderID: " + OrderID + @"
Tutar : " + po.txnamount + @"
K.K. : " + po.Isim + @"
Response : " + po.response + @"
Cevap Kodu : " + po.procreturncode + @"
Code : " + po.authcode + @"
"
                    );
            }
            catch (Exception err)
            {
                po.Aciklama = po.Tip + " " + err.Message;
                try
                {
                    // Hata Maili Gönderimi
                    if (false)
                    {
                        #region 


                        MailMessage ePosta = new MailMessage();
                        ePosta.From = new MailAddress(Convert.ToString("info@idyazilim.com"), "IDYAZILIM - E-Ticaret");

                        ePosta.To.Add(new MailAddress("mail.yunuskose@gmail.com", "mail.yunuskose@gmail.com"));

                        ePosta.Subject = "ÇİFTTEKER - Sipariş Hatası -" + DateTime.Today.ToString("dd-MM-yyyy");
                        //
                        ePosta.Body = @"
Sayın Yetkili; 

ciftteker.com sitesinde hata oluşmuştur, acil bakılması rica olunur.

" + po.bankaAd + @"
" + po.Tip + @"
" + po.GercekSiparisNo + @"
" + po.orderid + "-" + po.oid + @"
" + po.procreturncode + @"
" + po.authcode + @"
" + po.Isim + @"
" + po.response + @"
" + po.errmsg + @"
" + po.Aciklama + @"

İyi çalışmalar.

";

                        SmtpClient smtp = new SmtpClient();


                        smtp.Credentials = new System.Net.NetworkCredential("info@idyazilim.com", "MAİL PAROLASI");
                        smtp.Port = 587;
                        smtp.Host = "mail.idyazilim.com";
                        smtp.EnableSsl = false;

                        smtp.Send(ePosta);


                        #endregion
                    }
                }
                catch (Exception xxx)
                {
                    ;
                }
            }
            if (true)
            {
                string[] allkeys = Request.Form.AllKeys;
                string logtut = "";
                foreach (var item in allkeys)
                {
                    if (Request.Form.GetValues(item)[0] != null)
                    {
                        logtut += "<br/>" + item + ":" + Request.Form.GetValues(item)[0];
                    }
                }
                po.Aciklama += logtut;
            }



            ocmde.PosOdemeleri.Add(po);
            try
            {
                ocmde.SaveChanges();
            }
            catch (Exception err)
            {

            }



            return View();
        }


        public static string KrediKartiniBulma(string KrediKartiNo)
        {
            Regex visaRegex = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
            Regex masterRegex = new Regex("^5[1-5][0-9]{14}$");
            Regex expressRegex = new Regex("^3[47][0-9]{13}$");
            Regex dinersRegex = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
            Regex discoverRegex = new Regex("^6(?:011|5[0-9]{2})[0-9]{12}$");
            Regex jcbRegex = new Regex("^(?:2131|1800|35\\d{3})\\d{11}$");

            if (visaRegex.IsMatch(KrediKartiNo))
                return "100";// "VISA";
            else if (masterRegex.IsMatch(KrediKartiNo))
                return "200";// "MASTER";
            else if (expressRegex.IsMatch(KrediKartiNo))
                return "100";//"AEXPRESS";
            else if (dinersRegex.IsMatch(KrediKartiNo))
                return "100";//"DINERS";
            else if (discoverRegex.IsMatch(KrediKartiNo))
                return "100";//"DISCOVERS";
            else if (jcbRegex.IsMatch(KrediKartiNo))
                return "100";//"JCB";
            else
                return "100";//"invalid";
        }

    }

    public class SepetBilgisi
    {
        public string UrunSayisi { get; set; }
        public string Tutar { get; set; }

    }
}