using IDETicaret.Helpers;
using IDETicaret.Models;
using IDETicaret.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace IDETicaret.Controllers
{
    public class CartController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Index()
        {
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

                    return View("Index", list);
                }
                else
                {
                    var account = SessionPersister.account;
                    Account user = ((Account)account);

                    return View("Index", ocmde.Card.Where((x) => x.CustomerId == user.Id).ToList());
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
            try
            {
                ViewBag.Sehirler = ocmde.Sehirler.OrderBy((o) => o.Isim).ToList();
                if (SessionPersister.account != null)
                {
                    int userID = ((IDETicaret.Models.Account)SessionPersister.account).Id;
                    ViewBag.Adresler = ocmde.Adresler.Where((x) => x.UserID == userID).ToList();
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
            // Havale için yapılan sayfa
            ViewBag.OrderID = OrderID;
            return View();
        }

        [HttpGet]
        public ActionResult ConfirmOrder3(string AdSoyad, string Il, string Ilce, string Adres,
            string CepTelefonu, string TCKimlikNo, string AdresBasligi, string BK, string FirmaAdi, string VergiDairesi,
            string VergiNumarasi)
        {
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
        public ActionResult ConfirmOrder3(string AdSoyad, string Il, string Ilce, string Adres,
            string CepTelefonu, string TCKimlikNo, string AdresBasligi, string BK, string FirmaAdi, string VergiDairesi,
            string VergiNumarasi, string OdemeTipi, string taksitSayisi, string kartUzerindekiIsim, string kartNumarasi,
            string ay, string yil, string cvcNumarasi, string bankaAdi, string ip)
        {


            #region Adres Kaydı
            Session["AdSoyad"] = AdSoyad;
            Session["Il"] = Il;
            Session["Ilce"] = Ilce;
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
                /*
                int userid = ((IDETicaret.Models.Account)SessionPersister.account).Id;
                if (ocmde.Adresler.Where((a) => a.UserID == userid && a.AdresIsmi == AdresBasligi).Count() > 0)
                {
                    adres = ocmde.Adresler.Where((a) => a.UserID == 1 && a.AdresIsmi == AdresBasligi).FirstOrDefault();
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
                    ocmde.Adresler.Add(adres);
                    ocmde.SaveChanges();
                }
                */
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


                PosForm posForm = new PosForm()
                {
                    OrderID = DateTime.Now.ToString("yyMMddhhmmss"),
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
                            string strOrderID = Guid.NewGuid().ToString().Substring(0, 10).Replace("-", "");
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
                                                VergiNumarasi = VergiNumarasi

                                            };
                                            ocmde.Orders.Add(order);
                                            ocmde.SaveChanges();
                                            OrderID = order.Id;

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
                                                    VergiNumarasi = VergiNumarasi
                                                };
                                                ocmde.Orders.Add(order);
                                                ocmde.SaveChanges();
                                                OrderID = order.Id;
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

                                    //Session["GarantiSonuc"] = sonuc;
                                    ViewBag.Sonuc = sonuc;
                                    return View();

                                }
                                else
                                {
                                    sonuc = "İşlem Başarısız!!!. Bilgi : " + xElement1.InnerText + "-" + xElement3.InnerText +" <br/> "+ strXML;


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
                            VergiNumarasi = VergiNumarasi

                        };
                        ocmde.Orders.Add(order);
                        ocmde.SaveChanges();
                        OrderID = order.Id;

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
                                VergiNumarasi = VergiNumarasi
                            };
                            ocmde.Orders.Add(order);
                            ocmde.SaveChanges();
                            OrderID = order.Id;
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
                            txnamount = new string[] { Convert.ToString(Convert.ToDecimal(Request.Form.GetValues("txnamount")[0]) / Convert.ToDecimal(100)) };
                        Request.Form.GetValues("txnamount");
                        xid = Request.Form.GetValues("xid");
                        hostmsg = Request.Form.GetValues("hostmsg");
                        taksitAdet = Request.Form.GetValues("taksitAdet");
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


                    string deger = Request.Form.ToString();

                    po.txnamount = Convert.ToDecimal(txnamount == null ? "0" : txnamount[0]);
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

                    long OrderID = Convert.ToInt64(orderid == null ? "0" : orderid[0]);

                    if (response != null)
                    {
                        if (procreturncode[0] == "00" && response[0] == "Approved")
                        {



                            {
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
                                            Isim = Convert.ToString(Session["AdSoyad"]),
                                            CepTelefonu = Convert.ToString(Session["CepTelefonu"]),
                                            Il = Convert.ToString(Session["Il"]),
                                            Ilce = Convert.ToString(Session["Ilce"]),
                                            Adres = Convert.ToString(Session["Adres"]),
                                            TC = Convert.ToString(Session["TCKimlikNo"]),
                                            FaturaTuru = Convert.ToString(Session["BK"]),
                                            FirmaAdi = Convert.ToString(Session["FirmaAdi"]),
                                            VergiDairesi = Convert.ToString(Session["VergiDairesi"]),
                                            VergiNumarasi = Convert.ToString(Session["VergiNumarasi"]),
                                            OrderID = OrderID.ToString()
                                        };
                                        ocmde.Orders.Add(order);
                                        ocmde.SaveChanges();
                                        OrderID = order.Id;

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
                                                Isim = Convert.ToString(Session["AdSoyad"]),
                                                CepTelefonu = Convert.ToString(Session["CepTelefonu"]),
                                                Il = Convert.ToString(Session["Il"]),
                                                Ilce = Convert.ToString(Session["Ilce"]),
                                                Adres = Convert.ToString(Session["Adres"]),
                                                TC = Convert.ToString(Session["TCKimlikNo"]),
                                                FaturaTuru = Convert.ToString(Session["BK"]),
                                                FirmaAdi = Convert.ToString(Session["FirmaAdi"]),
                                                VergiDairesi = Convert.ToString(Session["VergiDairesi"]),
                                                VergiNumarasi = Convert.ToString(Session["VergiNumarasi"]),
                                                OrderID = OrderID.ToString()
                                            };
                                            ocmde.Orders.Add(order);
                                            ocmde.SaveChanges();
                                            OrderID = order.Id;
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
                            }
                            {
                                Orders order = ocmde.Orders.Where((x) => x.Id == OrderID).FirstOrDefault();
                                if (order != null)
                                    order.OrderStatusId = 6;
                                ocmde.SaveChanges();
                            }

                            ViewBag.Sonuc = "Ödeme başarılı bir şekilde alınmıştır. Onay kodunuz : " + Convert.ToString(authcode == null ? "" : authcode[0]);
                        }
                        else if (errmsg.Length > 0)
                        {
                            Orders order = ocmde.Orders.Where((x) => x.Id == OrderID).FirstOrDefault();
                            if (order != null)
                                order.OrderStatusId = 4;
                            ocmde.SaveChanges();
                            ViewBag.Sonuc = "ÖDEME GERÇEKLEŞMEDİ : " + errmsg[0];
                        }
                        else
                        {
                            Orders order = ocmde.Orders.Where((x) => x.Id == OrderID).FirstOrDefault();
                            if (order != null)
                                order.OrderStatusId = 4;
                            ocmde.SaveChanges();
                            ViewBag.Sonuc = "ÖDEME GERÇEKLEŞMEDİ : " + hostmsg[0];
                        }
                    }
                    else
                    {
                        Orders order = ocmde.Orders.Where((x) => x.Id == OrderID).FirstOrDefault();
                        if (order != null)
                            order.OrderStatusId = 4;
                        ocmde.SaveChanges();
                        ViewBag.Sonuc = "ÖDEME GERÇEKLEŞMEDİ...";
                    }

                }
                catch (Exception err)
                {
                    po.Aciklama = err.Message;
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
                return RedirectToAction("Index");
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
                return RedirectToAction("Index");
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
                return RedirectToAction("Index");
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
                return RedirectToAction("Index");
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
                        return RedirectToAction("Index");

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
                        return RedirectToAction("Index");
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

                        return RedirectToAction("Index", "Orders");
                    }
                    return RedirectToAction("Index", "Orders");

                    return RedirectToAction("Index", "Orders");
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

                        return RedirectToAction("Index", "Orders");
                    }
                    return RedirectToAction("Index", "Orders");
                }
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Home", "Index"));
            }
        }

    }

    public class SepetBilgisi
    {
        public string UrunSayisi { get; set; }
        public string Tutar { get; set; }

    }
}