using IDETicaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDETicaret.Controllers
{
    public class XmlController : Controller
    {
        private ETicaretEntities db = new ETicaretEntities();

        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        public ActionResult Kategori1()
        {

            string KullaniciIP = GetIp();

            Loglar l = new Loglar();
            l.Tip = "Xml";
            l.Aciklama = "Ana Kategoriler";
            l.Tarih = DateTime.Now;
            l.Kullanici = "IP:" + KullaniciIP;
            db.Loglar.Add(l);
            db.SaveChanges();

            string xml = @"<?xml version='1.0' encoding='UTF-8'?><kategoriler>";
            List<Category> kategoriler = db.Category.Where((x) => x.ParentId == null && x.Product.Where((p) => p.Status).Count() > 0).ToList();
            foreach (Category item in kategoriler)
            {
                xml += @"
<kategori>
<ID>" + item.Id + @"</ID>
<KategoriAdi>" + item.Name + @"</KategoriAdi>
<KategoriAdi>" + item.Status + @"</KategoriAdi>
</kategori>";

            }


            xml += @"</kategoriler>";

            //ViewBag.Xml = xml;
            return this.Content(xml, "text/xml"); // View();
        }


        public ActionResult Kategori2(int id)
        {

            string KullaniciIP = GetIp();

            Loglar l = new Loglar();
            l.Tip = "Xml";
            l.Aciklama = "Kategori ID:" + id;
            l.Tarih = DateTime.Now;
            l.Kullanici = "IP:" + KullaniciIP;
            db.Loglar.Add(l);
            db.SaveChanges();

            string xml = @"<?xml version='1.0' encoding='UTF-8'?><kategoriler>";
            List<Category> kategoriler = db.Category.Where((x) => x.ParentId == id && x.Product1.Where((p) => p.Status).Count() > 0).ToList();
            foreach (Category item in kategoriler)
            {
                xml += @"
<kategori>
<ID>" + item.Id + @"</ID>
<KategoriAdi>" + item.Name + @"</KategoriAdi>
<KategoriAdi>" + item.Status + @"</KategoriAdi>
</kategori>";

            }


            xml += @"</kategoriler>";

            //ViewBag.Xml = xml;
            return this.Content(xml, "text/xml"); // View();
        }


        public ActionResult Kategori3(int id)
        {

            string KullaniciIP = GetIp();

            Loglar l = new Loglar();
            l.Tip = "Xml";
            l.Aciklama = "Kategori ID:" + id;
            l.Tarih = DateTime.Now;
            l.Kullanici = "IP:" + KullaniciIP;
            db.Loglar.Add(l);
            db.SaveChanges();

            string xml = @"<?xml version='1.0' encoding='UTF-8'?><kategoriler>";
            List<Category> kategoriler = db.Category.Where((x) => x.ParentId == id && x.Product2.Where((p) => p.Status).Count() > 0).ToList();
            foreach (Category item in kategoriler)
            {
                xml += @"
<kategori>
<ID>" + item.Id + @"</ID>
<KategoriAdi>" + item.Name + @"</KategoriAdi>
<KategoriAdi>" + item.Status + @"</KategoriAdi>
</kategori>";

            }


            xml += @"</kategoriler>";

            //ViewBag.Xml = xml;
            return this.Content(xml, "text/xml"); // View();
        }


        public ActionResult Kategori4(int id)
        {

            string KullaniciIP = GetIp();

            Loglar l = new Loglar();
            l.Tip = "Xml";
            l.Aciklama = "Kategori ID:" + id;
            l.Tarih = DateTime.Now;
            l.Kullanici = "IP:" + KullaniciIP;
            db.Loglar.Add(l);
            db.SaveChanges();

            string xml = @"<?xml version='1.0' encoding='UTF-8'?><kategoriler>";
            List<Category> kategoriler = db.Category.Where((x) => x.ParentId == id && x.Product3.Where((p) => p.Status).Count() > 0).ToList();
            foreach (Category item in kategoriler)
            {
                xml += @"
<kategori>
<ID>" + item.Id + @"</ID>
<KategoriAdi>" + item.Name + @"</KategoriAdi>
<KategoriAdi>" + item.Status + @"</KategoriAdi>
</kategori>";

            }


            xml += @"</kategoriler>";

            //ViewBag.Xml = xml;
            return this.Content(xml, "text/xml"); // View();
        }

        public ActionResult Urunler(int id)
        {

            string KullaniciIP = GetIp();

            int IPKontrol = db.KullanicilarXmls.Where((x) => x.IP == KullaniciIP).Count();

            string xml = @"<?xml version='1.0' encoding='UTF-8'?><urunler>";
            if (IPKontrol > 0)
            {
                Loglar l = new Loglar();
                l.Tip = "Xml";
                l.Aciklama = "Ürün ID:" + id;
                l.Tarih = DateTime.Now;
                l.Kullanici = "IP:" + KullaniciIP;
                db.Loglar.Add(l);
                db.SaveChanges();

                List<Product> urunler = db.Product.Where((x) =>
                    x.Status &&
                    (x.CategoryId == id ||
                    x.Category2Id == id ||
                    x.Category3Id == id ||
                    x.Category4Id == id) &&
                    x.CategoryId != null &&
                    x.Category2Id != null &&
                    x.Category3Id != null &&
                    x.Category4Id != null
                ).ToList();
                foreach (Product item in urunler)
                {
                    xml += @"
<urun>
<ID>" + item.Id + @"</ID>
<Kod>" + item.Code + @"</Kod>
<Isim>" + item.Name + @"</Isim>
<KategoriID1>" + item.CategoryId + @"</KategoriID1>
<KategoriAdi1>" + item.Category.Name + @"</KategoriAdi1>
<KategoriID2>" + item.Category2Id + @"</KategoriID2>
<KategoriAdi2>" + item.Category1.Name + @"</KategoriAdi2>
<KategoriID3>" + item.Category3Id + @"</KategoriID3>
<KategoriAdi3>" + item.Category2.Name + @"</KategoriAdi3>
<KategoriID4>" + item.Category4Id + @"</KategoriID4>
<KategoriAdi4>" + item.Category3.Name + @"</KategoriAdi4>
<Birim>" + item.OlcuBirimi + @"</Birim>
<Fiyat>" + item.Price + @"</Fiyat>
<Keywords>" + item.Keywords + @"</Keywords>
<Marka>" + item.OzelKod1 + @"</Marka>
<Resim>https://ozerdemmotosiklet.com/Resimler/" + (item.Photo.Where((x) => x.Main && x.Status).Count() > 0 ? item.Photo.Where((x) => x.Main && x.Status).FirstOrDefault().Name : "ResimYok.png") + @"</Resim>
</urun>";

                }
            }
            else
            {

                Loglar l = new Loglar();
                l.Tip = "Xml";
                l.Aciklama = "Ürün ID:" + id;
                l.Tarih = DateTime.Now;
                l.Kullanici = "IP:" + KullaniciIP+", Yetkisiz Giriş";
                db.Loglar.Add(l);
                db.SaveChanges();
            }


            xml += @"</urunler>";
            //ViewBag.Xml = xml;
            return this.Content(xml, "text/xml"); // View();
        }
    }
}
