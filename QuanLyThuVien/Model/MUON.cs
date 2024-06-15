using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class MUON : INotifyPropertyChanged
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
        private string maPhMuon;
        public string MaPhMuon
        {
            get { return maPhMuon; }
            set { maPhMuon = value; OnPropertyChanged("maPhMuon"); }
        }
        private string maDG;
        public string MaDG
        {
            get { return maDG; }
            set { maDG = value; OnPropertyChanged("MaDG"); }
        }
        private string maSach;
        public string MaSach
        {
            get { return maSach; }
            set { maSach = value; OnPropertyChanged("MaSach"); }
        }
        private DateTime ngayMuon;
        public DateTime NgayMuon
        {
            get { return ngayMuon; }
            set { ngayMuon = value; OnPropertyChanged("NgayMuon"); }
        }
        private DateTime ngayPhTra;
        public DateTime NgayPhTra
        {
            get { return ngayPhTra; }
            set { ngayPhTra = value; OnPropertyChanged("NgayPhTra"); }
        }
    }
}
