﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class warehouseDBEntities : DbContext
    {
        public warehouseDBEntities()
            : base("name=warehouseDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<client_requests> client_requests { get; set; }
        public virtual DbSet<clientRequest_details> clientRequest_details { get; set; }
        public virtual DbSet<client> clients { get; set; }
        public virtual DbSet<product_stores> product_stores { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<products_units> products_units { get; set; }
        public virtual DbSet<store> stores { get; set; }
        public virtual DbSet<supplier_requests> supplier_requests { get; set; }
        public virtual DbSet<supplierRequest_details> supplierRequest_details { get; set; }
        public virtual DbSet<supplier> suppliers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<products_movement> products_movement { get; set; }
    }
}
