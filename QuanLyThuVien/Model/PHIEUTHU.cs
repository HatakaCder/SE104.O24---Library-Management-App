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
    
    public partial class PHIEUTHU
    {
        public int ID { get; set; }
        public string MaPhTra { get; set; }
        public Nullable<short> SoNgayQHan { get; set; }
        public Nullable<int> SoTienThu { get; set; }
    
        public virtual PHIEUTRA PHIEUTRA { get; set; }
    }
}
