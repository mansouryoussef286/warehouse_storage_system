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
    
    public partial class supplier_requests
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public supplier_requests()
        {
            this.supplierRequest_details = new HashSet<supplierRequest_details>();
        }
    
        public int inRequest_ID { get; set; }
        public int supplier_ID { get; set; }
        public System.DateTime date { get; set; }
    
        public virtual supplier supplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supplierRequest_details> supplierRequest_details { get; set; }
    }
}
