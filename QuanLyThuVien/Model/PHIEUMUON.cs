//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class PHIEUMUON
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHIEUMUON()
        {
            this.PHIEUTRA = new HashSet<PHIEUTRA>();
        }
    
        public string MaPhMuon { get; set; }
        public string MaDG { get; set; }
        public string MaSach { get; set; }
        public Nullable<System.DateTime> NgayMuon { get; set; }
        public Nullable<System.DateTime> NgayPhTra { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual DOCGIA DOCGIA { get; set; }
        public virtual SACH SACH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUTRA> PHIEUTRA { get; set; }
    }
}
