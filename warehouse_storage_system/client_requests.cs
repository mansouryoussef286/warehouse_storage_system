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
    
    public partial class client_requests
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public client_requests()
        {
            this.clientRequest_details = new HashSet<clientRequest_details>();
        }
    
        public int outRequest_ID { get; set; }
        public int client_ID { get; set; }
        public System.DateTime date { get; set; }
    
        public virtual client client { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<clientRequest_details> clientRequest_details { get; set; }
    }
}
