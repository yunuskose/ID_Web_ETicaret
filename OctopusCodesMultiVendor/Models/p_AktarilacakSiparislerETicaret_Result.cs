//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IDETicaret.Models
{
    using System;
    
    public partial class p_AktarilacakSiparislerETicaret_Result
    {
        public int ID { get; set; }
        public System.DateTime Tarih { get; set; }
        public string BelgeNo { get; set; }
        public string StokKodu { get; set; }
        public string YedekStokKodu { get; set; }
        public string CariKodu { get; set; }
        public string CariAdi { get; set; }
        public string CariAdres { get; set; }
        public string CariEMail { get; set; }
        public string CariTelefon { get; set; }
        public string CariIl { get; set; }
        public string CariIlce { get; set; }
        public string VergiDairesi { get; set; }
        public string VergiNumarasi { get; set; }
        public string TC { get; set; }
        public int Miktar { get; set; }
        public int Kdv { get; set; }
        public string Birimi { get; set; }
        public decimal Fiyat { get; set; }
        public Nullable<decimal> Tutar { get; set; }
        public string Notlar { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public string KayitYapanKullanici { get; set; }
        public Nullable<bool> Aktarildi { get; set; }
        public string PlasiyerKodu { get; set; }
        public string Hata { get; set; }
    }
}