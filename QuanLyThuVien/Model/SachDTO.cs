using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class SachDTO
    {
        public string id { get; set; }
        public string tensach { get; set; } = string.Empty;
        public string tentacgia { get; set; } = string.Empty;
        public DateTime ngaytra { get; set; } = DateTime.Now;
        public DateTime ngaymuon { get; set; } = DateTime.Now;
        public string quahan { get; set; } = string.Empty;

    }
}