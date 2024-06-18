using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Services
{
    public class Report2Service
    {
        private QLTV_BETAEntities ObjContext;
        public Report2Service()
        {
            ObjContext = new QLTV_BETAEntities();
        }

        public List<Report2DTO> GetAll(DateTime dt, string type)
        {
            List<Report2DTO> l = new List<Report2DTO>();
            if (type == "Ngày")
            {
                var rawData = ObjContext.PHIEUMUONs
            .Where(pm => (pm.IsDeleted == false || pm.IsDeleted == null) &&
                         ObjContext.PHIEUTRAs.Any(pt => pt.MaPhMuon == pm.MaPhMuon &&
                                                         (pt.IsDeleted == false || pt.IsDeleted == null) &&
                                                         pt.NgayTra == dt &&
                                                         pt.NgayTra > pt.PHIEUMUON.NgayPhTra))
            .Join(ObjContext.PHIEUTRAs.Where(pt => (pt.IsDeleted == false || pt.IsDeleted == null) && pt.NgayTra == dt),
                  pm => pm.MaPhMuon,
                  pt => pt.MaPhMuon,
                  (pm, pt) => new { pm, pt })
            .Join(ObjContext.SACHes.Where(s => s.IsDeleted == false || s.IsDeleted == null),
                  combined => combined.pm.MaSach,
                  s => s.MaSach,
                  (combined, s) => new { combined.pm, combined.pt, s })
            .ToList(); // Fetch data into memory

                var result = rawData
                    .Select((x, index) => new Report2DTO
                    {
                        STT = index + 1,
                        TenSach = x.s.TenSach,
                        NgayMuon = x.pm.NgayMuon.Value,
                        SoNgayTraTre = (x.pt.NgayTra - x.pm.NgayPhTra).Value.Days
                    })
                    .OrderBy(x => x.NgayMuon)
                    .ToList();
                l = result;
            }
            else if (type == "Tháng")
            {
                var rawData = ObjContext.PHIEUMUONs
            .Where(pm => (pm.IsDeleted == false || pm.IsDeleted == null) &&
                         ObjContext.PHIEUTRAs.Any(pt => pt.MaPhMuon == pm.MaPhMuon &&
                                                         (pt.IsDeleted == false || pt.IsDeleted == null) &&
                                                         pt.NgayTra.Value.Month == dt.Month &&
                                                         pt.NgayTra > pt.PHIEUMUON.NgayPhTra))
            .Join(ObjContext.PHIEUTRAs.Where(pt => (pt.IsDeleted == false || pt.IsDeleted == null) && pt.NgayTra.Value.Month == dt.Month),
                  pm => pm.MaPhMuon,
                  pt => pt.MaPhMuon,
                  (pm, pt) => new { pm, pt })
            .Join(ObjContext.SACHes.Where(s => s.IsDeleted == false || s.IsDeleted == null),
                  combined => combined.pm.MaSach,
                  s => s.MaSach,
                  (combined, s) => new { combined.pm, combined.pt, s })
            .ToList();

                var result = rawData
                    .Select((x, index) => new Report2DTO
                    {
                        STT = index + 1,
                        TenSach = x.s.TenSach,
                        NgayMuon = x.pm.NgayMuon.Value,
                        SoNgayTraTre = (x.pt.NgayTra - x.pm.NgayPhTra).Value.Days
                    })
                    .OrderBy(x => x.NgayMuon)
                    .ToList();
                l = result;
            }
            else if (type == "Năm")
            {
                var rawData = ObjContext.PHIEUMUONs
            .Where(pm => (pm.IsDeleted == false || pm.IsDeleted == null) &&
                         ObjContext.PHIEUTRAs.Any(pt => pt.MaPhMuon == pm.MaPhMuon &&
                                                         (pt.IsDeleted == false || pt.IsDeleted == null) &&
                                                         pt.NgayTra.Value.Year == dt.Year &&
                                                         pt.NgayTra > pt.PHIEUMUON.NgayPhTra))
            .Join(ObjContext.PHIEUTRAs.Where(pt => (pt.IsDeleted == false || pt.IsDeleted == null) && pt.NgayTra.Value.Year == dt.Year),
                  pm => pm.MaPhMuon,
                  pt => pt.MaPhMuon,
                  (pm, pt) => new { pm, pt })
            .Join(ObjContext.SACHes.Where(s => s.IsDeleted == false || s.IsDeleted == null),
                  combined => combined.pm.MaSach,
                  s => s.MaSach,
                  (combined, s) => new { combined.pm, combined.pt, s })
            .ToList(); // Fetch data into memory

                var result = rawData
                    .Select((x, index) => new Report2DTO
                    {
                        STT = index + 1,
                        TenSach = x.s.TenSach,
                        NgayMuon = x.pm.NgayMuon.Value,
                        SoNgayTraTre = (x.pt.NgayTra - x.pm.NgayPhTra).Value.Days
                    })
                    .OrderBy(x => x.NgayMuon)
                    .ToList();
                l = result;
            }

            return l;
        }
    }
}
