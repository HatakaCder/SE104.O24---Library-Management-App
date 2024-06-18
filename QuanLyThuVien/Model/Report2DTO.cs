using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class Report2DTO : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        private int stt;

        public int STT
        {
            get { return stt; }
            set { stt = value; OnPropertyChanged("STT"); }
        }

        private string tenSach;

        public string TenSach
        {
            get { return tenSach; }
            set { tenSach = value; OnPropertyChanged("TenSach"); }
        }
        private DateTime ngayMuon;

        public DateTime NgayMuon
        {
            get { return ngayMuon; }
            set { ngayMuon = value; OnPropertyChanged("NgayMuon"); }
        }
        private int soNgayTraTre;

        public int SoNgayTraTre
        {
            get { return soNgayTraTre; }
            set { soNgayTraTre = value; OnPropertyChanged("SoNgayTraTre"); }
        }


    }
}
