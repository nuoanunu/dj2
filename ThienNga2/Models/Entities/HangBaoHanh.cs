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
    
    public partial class HangBaoHanh
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public int wactID { get; set; }
    
        public virtual tb_warranty_activities tb_warranty_activities { get; set; }
    }
}
