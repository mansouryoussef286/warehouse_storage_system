//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace warehouse_storage_system
{
    using System;
    using System.Collections.Generic;
    
    public partial class products_movement
    {
        public int product_ID { get; set; }
        public string store_to { get; set; }
        public string store_from { get; set; }
        public System.DateTime move_date { get; set; }
        public int quantity { get; set; }
        public System.DateTime production_date { get; set; }
    
        public virtual product_stores product_stores { get; set; }
        public virtual product product { get; set; }
        public virtual store store { get; set; }
        public virtual store store1 { get; set; }
    }
}
