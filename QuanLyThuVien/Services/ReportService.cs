using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Services
{
    public class ReportService
    {
        QLTV_BETAEntities ObjContext;
        public ReportService()
        {
            ObjContext = new QLTV_BETAEntities();
        }
        public List<Report_1> GetAll()
        {
            List<Report_1>ObjReportList1 = new List<Report_1>();
            try
            {
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ObjReportList1;
        }
    }
}
