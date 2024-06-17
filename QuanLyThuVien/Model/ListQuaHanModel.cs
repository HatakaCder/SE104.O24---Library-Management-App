using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class ListQuaHanModel : INotifyPropertyChanged
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
            set { maPhMuon = value; OnPropertyChanged("MaPhMuon"); }
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
        private int tienPhat;
        public int TienPhat
        {
            get { return tienPhat; }
            set
            {
                tienPhat = value;
                OnPropertyChanged("TienPhat");
            }
        }
        private int dateQuaHan;
        public int DateQuaHan
        {
            get { return dateQuaHan; }
            set { dateQuaHan = value; OnPropertyChanged("DateQuaHan"); }
        }
    }
}