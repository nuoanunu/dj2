//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThienNga2.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_product_detail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_product_detail()
        {
            this.items = new HashSet<item>();
            this.RequestMuons = new HashSet<RequestMuon>();
            this.tb_warranty_activities = new HashSet<tb_warranty_activities>();
        }
    
        public int id { get; set; }
        public string producFactoryID { get; set; }
        public string productStoreID { get; set; }
        public string productName { get; set; }
        public double price { get; set; }
        public Nullable<int> cateID { get; set; }
        public string description { get; set; }
        public Nullable<int> minThresHold { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<item> items { get; set; }
        public virtual tb_cate tb_cate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestMuon> RequestMuons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_warranty_activities> tb_warranty_activities { get; set; }
    }
}
