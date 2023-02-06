using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDETicaret.Security;
using IDETicaret.Models.ViewModels;
using IDETicaret.Models;

namespace IDETicaret.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public ActionResult Liste(string type= "Beklemede")
        {
            try
            {
                DateTime tarih = DateTime.Today.AddMonths(-3);
                ViewBag.Beklemede = ocmde.Orders.Where((o) => o.Aktif == true && o.OrderStatusId == 1).Count();
                ViewBag.OdemeBekliyor = ocmde.Orders.Where((o) => o.Aktif == true && o.OrderStatusId == 4).Count();
                ViewBag.Hazirlaniyor = ocmde.Orders.Where((o) => o.Aktif == true && o.OrderStatusId == 6).Count();
                ViewBag.Gonderildi = ocmde.Orders.Where((o) => o.Aktif == true && o.OrderStatusId == 8).Count();
                ViewBag.TeslimEdildi = ocmde.Orders.Where((o) => o.Aktif == true && o.OrderStatusId == 9).Count();
                ViewBag.IptalEdildi = ocmde.Orders.Where((o) => o.Aktif == true && o.OrderStatusId == 10).Count();

                if (type == "Teslim Edildi")
                {
                    return View(ocmde.Orders.Where((o) => o.Aktif == true && o.OrderStatus.Name == type).OrderByDescending(o => o.CDate).Take(100).ToList());
                }
                else
                {
                    return View(ocmde.Orders.Where((o) => o.Aktif == true && o.OrderStatus.Name == type).OrderByDescending(o => o.CDate).ToList());
                }
                
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }


        public ActionResult OrdersLog()
        {
            try
            {
                return View(ocmde.OrdersLog.OrderByDescending(o => o.CDate).ToList());
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }


        public ActionResult PosOdemeleri()
        {
            try
            {
                DateTime tarih = DateTime.Today.AddMonths(-2);
                return View(ocmde.PosOdemeleri.Where((x)=>x.KayitTarihi >= tarih).OrderByDescending(o => o.KayitTarihi).ToList());
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }

        public ActionResult Cards()
        {
            try
            {
                return View(ocmde.Card.OrderByDescending(o => o.Id).ToList());
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }

        public ActionResult Detail(int id)
        {
            try
            {
                return View(ocmde.Orders.Where(o => o.Id == id).FirstOrDefault());
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }

        public ActionResult DetailLog(int id)
        {
            try
            {

                return View(ocmde.OrdersLog.Where(o => o.Id == id).FirstOrDefault());
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "OrdersLog", "Index"));
            }
        }


        public ActionResult OrderStatuChange(int SipID, int DurumID)
        {
            try
            {
                ocmde.Orders.Where((x) => x.Id == SipID).FirstOrDefault().OrderStatusId = DurumID;
                ocmde.SaveChanges();

                return RedirectToAction("Liste", "Orders", null);
                //return RedirectToAction("Detail", "Orders", new { id = SipID });
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }

        public ActionResult OrderKargoBilgisi(int SipID, string Kargo, string KargoNo)
        {
            try
            {
                ocmde.Orders.Where((x) => x.Id == SipID).FirstOrDefault().Kargo = Kargo;
                ocmde.Orders.Where((x) => x.Id == SipID).FirstOrDefault().KargoNo = KargoNo;
                ocmde.SaveChanges();

                return RedirectToAction("Liste", "Orders", null);
                //return RedirectToAction("Detail", "Orders", new { id = SipID });
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }
        public ActionResult OrderAciklamaKaydet(int SipID, string Aciklama1)
        {
            try
            {
                ocmde.Orders.Where((x) => x.Id == SipID).FirstOrDefault().Aciklama1 = Aciklama1;
                ocmde.SaveChanges();

                return RedirectToAction("Liste", "Orders", null);
                //return RedirectToAction("Detail", "Orders", new { id = SipID });
            }
            catch (Exception e)
            {
                return View("Error", new HandleErrorInfo(e, "Orders", "Index"));
            }
        }


        public ActionResult GercekSiparisOlustur(int SipID)
        {

            OrdersLog sipUst = ocmde.OrdersLog.Where((x)=>x.Id == SipID).FirstOrDefault();
            List<OrdersDetailLog> sipKalemler = ocmde.OrdersDetailLog.Where((x)=>x.OrderId == sipUst.Id).ToList();

            Orders order = new Orders();
            order.CustomerId = sipUst.CustomerId;
            order.Adres = sipUst.Adres;
            order.Aktarildi = sipUst.Aktarildi;
            order.CepTelefonu = sipUst.CepTelefonu;
            order.DateCreation = DateTime.Now;
            order.CDate = DateTime.Now;
            order.Ertele = sipUst.Ertele;
            order.FaturaTuru = sipUst.FaturaTuru;
            order.FirmaAdi = sipUst.FirmaAdi;
            order.Il = sipUst.Il;
            order.Ilce = sipUst.Ilce;
            order.Isim = sipUst.Isim;
            order.Name = sipUst.Name;
            order.OrderID = Convert.ToString(sipUst.OrderID);
            order.OrderStatusId = sipUst.OrderStatusId;
            order.PaymentId = sipUst.PaymentId;
            order.TC = sipUst.TC;
            order.VendorId = sipUst.VendorId;
            order.VergiDairesi = sipUst.VergiDairesi;
            order.VergiNumarasi = sipUst.VergiNumarasi;
            order.Aktif = true;
            ocmde.Orders.Add(order);
            ocmde.SaveChanges();

            foreach (OrdersDetailLog item in sipKalemler)
            {
                OrdersDetail kalem = new OrdersDetail();
                kalem.OrderId = order.Id;
                kalem.Price = item.Price;
                kalem.ProductId = item.ProductId;
                kalem.Quantity = item.Quantity;
                ocmde.OrdersDetail.Add(kalem);
                ocmde.SaveChanges();
            }


            return RedirectToAction("Detail", "Orders", new { id = order.Id });
        }


        public ActionResult DurumuDegistir(string idler, string Tip)
        {
            foreach (string item in idler.Split('|'))
            {
                if (item != "|" && item != "")
                {
                    int id = Convert.ToInt32(item);
                    int statusid_ = 0;
                    int statusid = ocmde.Orders.Where((x) => x.Id == id).FirstOrDefault().OrderStatusId;
                    switch (statusid)
                    {
                        case 1: statusid_ = 4; break;
                        case 4: statusid_ = 6; break;
                        case 6: statusid_ = 8; break;
                        case 8: statusid_ = 9; break;
                        case 9: statusid_ = 10; break;
                        case 10: statusid_ = 11; break;                        
                        default:
                            break;
                    }

                    int _tip = Convert.ToInt32(Tip);
                    ocmde.Orders.Where((x) => x.Id == id).FirstOrDefault().OrderStatusId = (_tip);
                }
            }
            ocmde.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}