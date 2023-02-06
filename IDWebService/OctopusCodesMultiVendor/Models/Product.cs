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
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.OrdersDetail = new HashSet<OrdersDetail>();
            this.Photo = new HashSet<Photo>();
            this.Card = new HashSet<Card>();
            this.HomeProducts = new HashSet<HomeProducts>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }
        public Nullable<int> Category2Id { get; set; }
        public Nullable<int> Category3Id { get; set; }
        public int VendorId { get; set; }
        public int Views { get; set; }
        public string Keywords { get; set; }
        public Nullable<System.DateTime> CDate { get; set; }
        public string GercekStokKodu { get; set; }
        public Nullable<int> Category4Id { get; set; }
        public Nullable<int> UrunSayisi { get; set; }
        public string OlcuBirimi { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Category Category1 { get; set; }
        public virtual Category Category2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdersDetail> OrdersDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Photo> Photo { get; set; }
        public virtual Vendor Vendor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Card> Card { get; set; }
        public virtual Category Category3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HomeProducts> HomeProducts { get; set; }
    }
}
