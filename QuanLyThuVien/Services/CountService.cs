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
    public class CountService
    {
        QLTV_BETAEntities Objcontext;
        public CountService()
        {
            Objcontext = new QLTV_BETAEntities();
        }
        public int getCount()
        {
            int n = 0;
            n = Objcontext.DOCGIA.Count();
            return n;
        }

        public int getCountBook()
        {
            int n = 0;
            n = Objcontext.SACH.Count();
            return n;
        }

        public int getOutDateBook()
        {

            DateTime ngayHomNay = DateTime.Today;
            int count = 0;

            try
            {
                // Lấy số lượng sách mượn quá hạn
                count = Objcontext.PHIEUMUON
                                         .Where(pm => pm.NgayPhTra < ngayHomNay && pm.IsDeleted == false)
                                         .Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Đã xảy ra lỗi: " + ex.Message);
            }

            return count;

        }
        public int getCountBookBorrowed()
        {
            int count = 0;
            try
            {
                var query = from phieumuon in Objcontext.PHIEUMUON
                            join phieutra in Objcontext.PHIEUTRA
                                on phieumuon.MaPhMuon equals phieutra.MaPhMuon into gj
                            from subPhieuTra in gj.DefaultIfEmpty()
                            where subPhieuTra == null
                            select new
                            {
                                phieumuon.MaPhMuon
                            };
                count = query.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return count;
        }

        public int getOverdueBooksCount()
        {
            DateTime ngayHomNay = DateTime.Today;
            int count = 0;

            try
            {
                // Lấy số lượng sách mượn quá hạn chưa được trả
                count = Objcontext.PHIEUMUON
                                 .Where(pm => pm.NgayPhTra < ngayHomNay && pm.PHIEUTRA.Any(pt => pt.NgayTra < ngayHomNay))
                                 .Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Đã xảy ra lỗi: " + ex.Message);
            }

            return count;
        }
    }
}