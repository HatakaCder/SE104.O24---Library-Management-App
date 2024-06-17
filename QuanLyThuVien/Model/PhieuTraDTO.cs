using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class PhieuTraDTO : INotifyPropertyChanged
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
        private string maPhTra;

        public string MaPhTra
        {
            get { return maPhTra; }
            set { maPhTra = value; OnPropertyChanged("MaPhTra"); }
        }
        private string maPhMuon;

        public string MaPhMuon
        {
            get { return maPhMuon; }
            set { maPhMuon = value; OnPropertyChanged("MaPhMuon"); }
        }
        private string maDG;

        public string MaDG
        {
            get { return maDG; }
            set { maDG = value; OnPropertyChanged("MaDG"); }
        }
        private DateTime ngayTra;

        public DateTime NgayTra
        {
            get { return ngayTra; }
            set { ngayTra = value; OnPropertyChanged("NgayTra"); }
        }
        private string maSach;

        public string MaSach
        {
            get { return maSach; }
            set { maSach = value; OnPropertyChanged("MaSach"); }
        }
    }
}