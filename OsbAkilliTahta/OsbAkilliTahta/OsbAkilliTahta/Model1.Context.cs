﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OsbAkilliTahta
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OsbAkilliTahtaEntities : DbContext
    {
        public OsbAkilliTahtaEntities()
            : base("name=OsbAkilliTahtaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TBL_OGRETMENLER> TBL_OGRETMENLER { get; set; }
        public virtual DbSet<TBL_HABERLER> TBL_HABERLER { get; set; }
        public virtual DbSet<TBL_SOZLER> TBL_SOZLER { get; set; }
    }
}