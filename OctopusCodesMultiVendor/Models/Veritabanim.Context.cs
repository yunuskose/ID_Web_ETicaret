﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ETicaretEntities : DbContext
    {
        public ETicaretEntities()
            : base("name=ETicaretEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Adresler> Adresler { get; set; }
        public virtual DbSet<AspNet_SqlCacheTablesForChangeNotification> AspNet_SqlCacheTablesForChangeNotification { get; set; }
        public virtual DbSet<Bankalar> Bankalar { get; set; }
        public virtual DbSet<Card> Card { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<HomeProducts> HomeProducts { get; set; }
        public virtual DbSet<Ilceler> Ilceler { get; set; }
        public virtual DbSet<MemberShip> MemberShip { get; set; }
        public virtual DbSet<MemberShipVendor> MemberShipVendor { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<OrdersDetail> OrdersDetail { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Page> Page { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<PosOdemeleri> PosOdemeleri { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<Sehirler> Sehirler { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<YanlisKelimeler> YanlisKelimeler { get; set; }
        public virtual DbSet<Loglar> Loglar { get; set; }
        public virtual DbSet<OrdersDetailLog> OrdersDetailLog { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrdersLog> OrdersLog { get; set; }
        public virtual DbSet<BankaXml> BankaXml { get; set; }
        public virtual DbSet<CVV> CVVs { get; set; }
        public virtual DbSet<ProductView> ProductViews { get; set; }
    
        [DbFunction("ETicaretEntities", "StringArray")]
        public virtual IQueryable<StringArray_Result> StringArray(string paramaterList, string delimiter)
        {
            var paramaterListParameter = paramaterList != null ?
                new ObjectParameter("ParamaterList", paramaterList) :
                new ObjectParameter("ParamaterList", typeof(string));
    
            var delimiterParameter = delimiter != null ?
                new ObjectParameter("Delimiter", delimiter) :
                new ObjectParameter("Delimiter", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<StringArray_Result>("[ETicaretEntities].[StringArray](@ParamaterList, @Delimiter)", paramaterListParameter, delimiterParameter);
        }
    
        public virtual ObjectResult<AspNet_SqlCachePollingStoredProcedure_Result> AspNet_SqlCachePollingStoredProcedure()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AspNet_SqlCachePollingStoredProcedure_Result>("AspNet_SqlCachePollingStoredProcedure");
        }
    
        public virtual ObjectResult<string> AspNet_SqlCacheQueryRegisteredTablesStoredProcedure()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("AspNet_SqlCacheQueryRegisteredTablesStoredProcedure");
        }
    
        public virtual int AspNet_SqlCacheRegisterTableStoredProcedure(string tableName)
        {
            var tableNameParameter = tableName != null ?
                new ObjectParameter("tableName", tableName) :
                new ObjectParameter("tableName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AspNet_SqlCacheRegisterTableStoredProcedure", tableNameParameter);
        }
    
        public virtual int AspNet_SqlCacheUnRegisterTableStoredProcedure(string tableName)
        {
            var tableNameParameter = tableName != null ?
                new ObjectParameter("tableName", tableName) :
                new ObjectParameter("tableName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AspNet_SqlCacheUnRegisterTableStoredProcedure", tableNameParameter);
        }
    
        public virtual int AspNet_SqlCacheUpdateChangeIdStoredProcedure(string tableName)
        {
            var tableNameParameter = tableName != null ?
                new ObjectParameter("tableName", tableName) :
                new ObjectParameter("tableName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AspNet_SqlCacheUpdateChangeIdStoredProcedure", tableNameParameter);
        }
    
        public virtual ObjectResult<p_AktarilacakSiparislerETicaret_Result> p_AktarilacakSiparislerETicaret()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<p_AktarilacakSiparislerETicaret_Result>("p_AktarilacakSiparislerETicaret");
        }
    
        public virtual int p_AktarimStokKaydet(Nullable<int> baglantiID, string stokKodu, string stokAdi, string barkod, string barkod2, string barkod3, string kategori1, string kategori2, string kategori3, string kategori4, Nullable<decimal> bakiye, Nullable<decimal> kdv, string birimi, Nullable<bool> aktiflik, Nullable<decimal> fiyat1, string bilgi, string gercekStokKodu, string ozelKod1, string aciklama)
        {
            var baglantiIDParameter = baglantiID.HasValue ?
                new ObjectParameter("BaglantiID", baglantiID) :
                new ObjectParameter("BaglantiID", typeof(int));
    
            var stokKoduParameter = stokKodu != null ?
                new ObjectParameter("StokKodu", stokKodu) :
                new ObjectParameter("StokKodu", typeof(string));
    
            var stokAdiParameter = stokAdi != null ?
                new ObjectParameter("StokAdi", stokAdi) :
                new ObjectParameter("StokAdi", typeof(string));
    
            var barkodParameter = barkod != null ?
                new ObjectParameter("Barkod", barkod) :
                new ObjectParameter("Barkod", typeof(string));
    
            var barkod2Parameter = barkod2 != null ?
                new ObjectParameter("Barkod2", barkod2) :
                new ObjectParameter("Barkod2", typeof(string));
    
            var barkod3Parameter = barkod3 != null ?
                new ObjectParameter("Barkod3", barkod3) :
                new ObjectParameter("Barkod3", typeof(string));
    
            var kategori1Parameter = kategori1 != null ?
                new ObjectParameter("Kategori1", kategori1) :
                new ObjectParameter("Kategori1", typeof(string));
    
            var kategori2Parameter = kategori2 != null ?
                new ObjectParameter("Kategori2", kategori2) :
                new ObjectParameter("Kategori2", typeof(string));
    
            var kategori3Parameter = kategori3 != null ?
                new ObjectParameter("Kategori3", kategori3) :
                new ObjectParameter("Kategori3", typeof(string));
    
            var kategori4Parameter = kategori4 != null ?
                new ObjectParameter("Kategori4", kategori4) :
                new ObjectParameter("Kategori4", typeof(string));
    
            var bakiyeParameter = bakiye.HasValue ?
                new ObjectParameter("Bakiye", bakiye) :
                new ObjectParameter("Bakiye", typeof(decimal));
    
            var kdvParameter = kdv.HasValue ?
                new ObjectParameter("Kdv", kdv) :
                new ObjectParameter("Kdv", typeof(decimal));
    
            var birimiParameter = birimi != null ?
                new ObjectParameter("Birimi", birimi) :
                new ObjectParameter("Birimi", typeof(string));
    
            var aktiflikParameter = aktiflik.HasValue ?
                new ObjectParameter("Aktiflik", aktiflik) :
                new ObjectParameter("Aktiflik", typeof(bool));
    
            var fiyat1Parameter = fiyat1.HasValue ?
                new ObjectParameter("Fiyat1", fiyat1) :
                new ObjectParameter("Fiyat1", typeof(decimal));
    
            var bilgiParameter = bilgi != null ?
                new ObjectParameter("Bilgi", bilgi) :
                new ObjectParameter("Bilgi", typeof(string));
    
            var gercekStokKoduParameter = gercekStokKodu != null ?
                new ObjectParameter("GercekStokKodu", gercekStokKodu) :
                new ObjectParameter("GercekStokKodu", typeof(string));
    
            var ozelKod1Parameter = ozelKod1 != null ?
                new ObjectParameter("OzelKod1", ozelKod1) :
                new ObjectParameter("OzelKod1", typeof(string));
    
            var aciklamaParameter = aciklama != null ?
                new ObjectParameter("Aciklama", aciklama) :
                new ObjectParameter("Aciklama", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("p_AktarimStokKaydet", baglantiIDParameter, stokKoduParameter, stokAdiParameter, barkodParameter, barkod2Parameter, barkod3Parameter, kategori1Parameter, kategori2Parameter, kategori3Parameter, kategori4Parameter, bakiyeParameter, kdvParameter, birimiParameter, aktiflikParameter, fiyat1Parameter, bilgiParameter, gercekStokKoduParameter, ozelKod1Parameter, aciklamaParameter);
        }
    
        public virtual int p_BilgileriGuncelle()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("p_BilgileriGuncelle");
        }
    
        public virtual ObjectResult<p_StokAra_Result> p_StokAra(string ara, string kategori1, string kategori2, string kategori3, string kategori4, Nullable<int> sayfa)
        {
            var araParameter = ara != null ?
                new ObjectParameter("Ara", ara) :
                new ObjectParameter("Ara", typeof(string));
    
            var kategori1Parameter = kategori1 != null ?
                new ObjectParameter("Kategori1", kategori1) :
                new ObjectParameter("Kategori1", typeof(string));
    
            var kategori2Parameter = kategori2 != null ?
                new ObjectParameter("Kategori2", kategori2) :
                new ObjectParameter("Kategori2", typeof(string));
    
            var kategori3Parameter = kategori3 != null ?
                new ObjectParameter("Kategori3", kategori3) :
                new ObjectParameter("Kategori3", typeof(string));
    
            var kategori4Parameter = kategori4 != null ?
                new ObjectParameter("Kategori4", kategori4) :
                new ObjectParameter("Kategori4", typeof(string));
    
            var sayfaParameter = sayfa.HasValue ?
                new ObjectParameter("Sayfa", sayfa) :
                new ObjectParameter("Sayfa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<p_StokAra_Result>("p_StokAra", araParameter, kategori1Parameter, kategori2Parameter, kategori3Parameter, kategori4Parameter, sayfaParameter);
        }
    
        public virtual int p_StokSil(string stokKodu)
        {
            var stokKoduParameter = stokKodu != null ?
                new ObjectParameter("StokKodu", stokKodu) :
                new ObjectParameter("StokKodu", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("p_StokSil", stokKoduParameter);
        }
    
        public virtual ObjectResult<Product> f_StokAra(string ara, string kategori1, string kategori2, string kategori3, string kategori4, Nullable<int> sayfa)
        {
            var araParameter = ara != null ?
                new ObjectParameter("Ara", ara) :
                new ObjectParameter("Ara", typeof(string));
    
            var kategori1Parameter = kategori1 != null ?
                new ObjectParameter("Kategori1", kategori1) :
                new ObjectParameter("Kategori1", typeof(string));
    
            var kategori2Parameter = kategori2 != null ?
                new ObjectParameter("Kategori2", kategori2) :
                new ObjectParameter("Kategori2", typeof(string));
    
            var kategori3Parameter = kategori3 != null ?
                new ObjectParameter("Kategori3", kategori3) :
                new ObjectParameter("Kategori3", typeof(string));
    
            var kategori4Parameter = kategori4 != null ?
                new ObjectParameter("Kategori4", kategori4) :
                new ObjectParameter("Kategori4", typeof(string));
    
            var sayfaParameter = sayfa.HasValue ?
                new ObjectParameter("Sayfa", sayfa) :
                new ObjectParameter("Sayfa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Product>("f_StokAra", araParameter, kategori1Parameter, kategori2Parameter, kategori3Parameter, kategori4Parameter, sayfaParameter);
        }
    
        public virtual ObjectResult<Product> f_StokAra(string ara, string kategori1, string kategori2, string kategori3, string kategori4, Nullable<int> sayfa, MergeOption mergeOption)
        {
            var araParameter = ara != null ?
                new ObjectParameter("Ara", ara) :
                new ObjectParameter("Ara", typeof(string));
    
            var kategori1Parameter = kategori1 != null ?
                new ObjectParameter("Kategori1", kategori1) :
                new ObjectParameter("Kategori1", typeof(string));
    
            var kategori2Parameter = kategori2 != null ?
                new ObjectParameter("Kategori2", kategori2) :
                new ObjectParameter("Kategori2", typeof(string));
    
            var kategori3Parameter = kategori3 != null ?
                new ObjectParameter("Kategori3", kategori3) :
                new ObjectParameter("Kategori3", typeof(string));
    
            var kategori4Parameter = kategori4 != null ?
                new ObjectParameter("Kategori4", kategori4) :
                new ObjectParameter("Kategori4", typeof(string));
    
            var sayfaParameter = sayfa.HasValue ?
                new ObjectParameter("Sayfa", sayfa) :
                new ObjectParameter("Sayfa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Product>("f_StokAra", mergeOption, araParameter, kategori1Parameter, kategori2Parameter, kategori3Parameter, kategori4Parameter, sayfaParameter);
        }
    }
}