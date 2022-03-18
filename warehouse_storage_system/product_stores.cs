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
    
    public partial class product_stores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public product_stores()
        {
            this.products_movement = new HashSet<products_movement>();
            this.clientRequest_details = new HashSet<clientRequest_details>();
            this.supplierRequest_details = new HashSet<supplierRequest_details>();
        }
    
        public int product_ID { get; set; }
        public string store_name { get; set; }
        public int quantity { get; set; }
        public System.DateTime production_date { get; set; }
    
        public virtual product product { get; set; }
        public virtual store store { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<products_movement> products_movement { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<clientRequest_details> clientRequest_details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supplierRequest_details> supplierRequest_details { get; set; }
    }
}
