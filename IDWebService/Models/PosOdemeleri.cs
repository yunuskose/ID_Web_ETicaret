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
    using System.Collections.Generic;
    
    public partial class PosOdemeleri
    {
        public int ID { get; set; }
        public string Tip { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<decimal> GonderilenTutar { get; set; }
        public Nullable<decimal> txnamount { get; set; }
        public string xid { get; set; }
        public string hostmsg { get; set; }
        public Nullable<int> taksitAdet { get; set; }
        public string bankaAd { get; set; }
        public string authcode { get; set; }
        public string hostrefnum { get; set; }
        public string rnd { get; set; }
        public string procreturncode { get; set; }
        public string transid { get; set; }
        public string mode { get; set; }
        public string response { get; set; }
        public string successurl { get; set; }
        public string errmsg { get; set; }
        public string md { get; set; }
        public string oid { get; set; }
        public string hash { get; set; }
        public string txntimestamp { get; set; }
        public string customeripaddress { get; set; }
        public string terminalid { get; set; }
        public string orderid { get; set; }
        public string MaskedPan { get; set; }
        public string Isim { get; set; }
        public string secure3dhash { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string KayitYapanKullanici { get; set; }
        public Nullable<System.DateTime> KayitTarihi { get; set; }
        public Nullable<System.DateTime> DuzenlemeTarihi { get; set; }
        public Nullable<bool> KontrolEdildi { get; set; }
        public string Aciklama { get; set; }
        public Nullable<bool> Aktarildi { get; set; }
        public string SonucDegeri { get; set; }
        public string GercekSiparisNo { get; set; }
    }
}
