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
    
    public partial class item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public item()
        {
            this.tb_warranty = new HashSet<tb_warranty>();
        }
    
        public int id { get; set; }
        public string productID { get; set; }
        public int productDetailID { get; set; }
        public int inventoryID { get; set; }
        public Nullable<int> customerID { get; set; }
        public System.DateTime DateOfSold { get; set; }
        public int orderID { get; set; }
        public Nullable<int> CustomerType { get; set; }
        public Nullable<bool> warrantyAvailable { get; set; }
        public string tempname { get; set; }
    
        public virtual CustomerType CustomerType1 { get; set; }
        public virtual order order { get; set; }
        public virtual tb_customer tb_customer { get; set; }
        public virtual tb_inventory_name tb_inventory_name { get; set; }
        public virtual tb_product_detail tb_product_detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_warranty> tb_warranty { get; set; }
    }
}
