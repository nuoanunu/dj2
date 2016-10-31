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
    
    public partial class tb_warranty_activities
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_warranty_activities()
        {
            this.logs = new HashSet<log>();
            this.warrantyActivityFees = new HashSet<warrantyActivityFee>();
            this.warrantyActivityFixingFees = new HashSet<warrantyActivityFixingFee>();
        }
    
        public int id { get; set; }
        public System.DateTime startDate { get; set; }
        public string employee { get; set; }
        public Nullable<int> warrantyID { get; set; }
        public string Description { get; set; }
        public int status { get; set; }
        public Nullable<System.DateTime> realeaseDATE { get; set; }
        public string itemID { get; set; }
        public Nullable<System.DateTime> finishFixingDate { get; set; }
        public string empFixer { get; set; }
        public string CodeBaoHanh { get; set; }
        public string TenKhach { get; set; }
        public string SDT { get; set; }
        public Nullable<int> productDetailID { get; set; }
        public string Note { get; set; }
        public Nullable<int> type { get; set; }
    
        public virtual ActivityType ActivityType { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<log> logs { get; set; }
        public virtual tb_warranty tb_warranty { get; set; }
        public virtual tb_warrnaty_status tb_warrnaty_status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<warrantyActivityFee> warrantyActivityFees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<warrantyActivityFixingFee> warrantyActivityFixingFees { get; set; }
    }
}
