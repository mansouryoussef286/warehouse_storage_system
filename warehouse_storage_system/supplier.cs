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
    
    public partial class supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public supplier()
        {
            this.supplier_requests = new HashSet<supplier_requests>();
        }
    
        public int supplier_ID { get; set; }
        public string supplier_name { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string website { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supplier_requests> supplier_requests { get; set; }
    }
}
