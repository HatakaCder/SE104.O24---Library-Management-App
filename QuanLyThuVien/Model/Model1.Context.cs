﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyThuVien.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QLTV_BETAEntities : DbContext
    {
        public QLTV_BETAEntities()
            : base("name=QLTV_BETAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ACCOUNT> ACCOUNT { get; set; }
        public virtual DbSet<DOCGIA> DOCGIA { get; set; }
        public virtual DbSet<PHIEUMUON> PHIEUMUON { get; set; }
        public virtual DbSet<PHIEUTHU> PHIEUTHU { get; set; }
        public virtual DbSet<PHIEUTRA> PHIEUTRA { get; set; }
        public virtual DbSet<SACH> SACH { get; set; }
        public virtual DbSet<THELOAI> THELOAI { get; set; }
        public virtual DbSet<THUTHU> THUTHU { get; set; }
    }
}
