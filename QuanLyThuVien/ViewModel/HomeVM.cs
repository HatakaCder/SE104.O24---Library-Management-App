using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.ViewModel
{
    public class HomeVM 
    {
        private List<DOCGIA> _readers;
        public List<DOCGIA> DOCGIAs
        {
            get { return _readers; }
            set { _readers = value; }
        }
        public HomeVM() {
            LoadData(); 
        }
        public void LoadData()
        {
           QLTV_BETAEntities db = new QLTV_BETAEntities();
           DOCGIAs =  db.DOCGIA.ToList();
        }
    }
}
