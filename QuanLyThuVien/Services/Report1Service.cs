using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Services
{
    public class Report1Service
    {
        private QLTV_BETAEntities ObjContext;
        public Report1Service()
        {
            ObjContext = new QLTV_BETAEntities();
        }

        public List<Report1DTO> GetAll(DateTime dt, string type)
        {
            List<Report1DTO> l = new List<Report1DTO>();
            if (type == "Ngày")
            {
                var rawReportData = ObjContext.PHIEUMUONs
                .Where(pm => pm.NgayMuon == dt)
                .GroupBy(pm => pm.SACH.TenTheLoai)
                .Select(g => new
                {
                    TheLoai = g.Key,
                    SoLuotMuon = g.Count(),
                    TotalBorrowings = ObjContext.PHIEUMUONs.Count(pm2 => pm2.NgayMuon == dt)
                })
                .ToList();
                var reportQuery = rawReportData
                .Select((result, index) => new Report1DTO
                {
                    STT = index + 1,
                    TenTheLoai = result.TheLoai,
                    SoLuotMuon = result.SoLuotMuon,
                    TyLe = (int)Math.Round((result.SoLuotMuon * 100.0 / result.TotalBorrowings), 2)// Assuming TiLe is an int, rounding and casting
                })
                .OrderByDescending(dto => dto.SoLuotMuon)
                .ToList();
                l = reportQuery;
            } else if (type == "Tháng")
            {
                var rawReportData = ObjContext.PHIEUMUONs
                .Where(pm => pm.NgayMuon.Value.Month == dt.Month && pm.NgayMuon.Value.Year == dt.Year)
                .GroupBy(pm => pm.SACH.TenTheLoai)
                .Select(g => new
                {
                    TheLoai = g.Key,
                    SoLuotMuon = g.Count(),
                    TotalBorrowings = ObjContext.PHIEUMUONs.Count(pm2 => pm2.NgayMuon.Value.Month == dt.Month && pm2.NgayMuon.Value.Year == dt.Year)
                })
                .ToList();
                var reportQuery = rawReportData
                .Select((result, index) => new Report1DTO
                {
                    STT = index + 1,
                    TenTheLoai = result.TheLoai,
                    SoLuotMuon = result.SoLuotMuon,
                    TyLe = (int)Math.Round((result.SoLuotMuon * 100.0 / result.TotalBorrowings), 2)// Assuming TiLe is an int, rounding and casting
                })
                .OrderByDescending(dto => dto.SoLuotMuon)
                .ToList();
                l = reportQuery;
            } else if (type == "Năm")
            {
                var rawReportData = ObjContext.PHIEUMUONs
                .Where(pm => pm.NgayMuon.Value.Year == dt.Year)
                .GroupBy(pm => pm.SACH.TenTheLoai)
                .Select(g => new
                {
                    TheLoai = g.Key,
                    SoLuotMuon = g.Count(),
                    TotalBorrowings = ObjContext.PHIEUMUONs.Count(pm2 => pm2.NgayMuon.Value.Year == dt.Year)
                })
                .ToList();
                var reportQuery = rawReportData
                .Select((result, index) => new Report1DTO
                {
                    STT = index + 1,
                    TenTheLoai = result.TheLoai,
                    SoLuotMuon = result.SoLuotMuon,
                    TyLe = (int)Math.Round((result.SoLuotMuon * 100.0 / result.TotalBorrowings), 2)// Assuming TiLe is an int, rounding and casting
                })
                .OrderByDescending(dto => dto.SoLuotMuon)
                .ToList();
                l = reportQuery;
            }
            return l;
        }
        public int getTotalBorrowed(DateTime dt, string type)
        {
            int totalBorrowings = 0;
            if (type == "Ngày")
            {
                totalBorrowings = ObjContext.PHIEUMUONs.Count(pm => pm.NgayMuon == dt);
            } else if (type == "Tháng")
            {
                totalBorrowings = ObjContext.PHIEUMUONs.Count(pm => pm.NgayMuon.Value.Month == dt.Month);
            } else if (type== "Năm")
            {
                totalBorrowings = ObjContext.PHIEUMUONs.Count(pm => pm.NgayMuon.Value.Year == dt.Year);
            }
            return totalBorrowings;
        }
    }
}
