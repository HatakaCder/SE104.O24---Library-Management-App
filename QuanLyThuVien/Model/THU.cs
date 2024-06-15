using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class THU : INotifyPropertyChanged
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
        private string iD;
        public string ID
        {
            get { return iD; }
            set { iD = value; OnPropertyChanged("ID"); }
        }
        private string maPhTra;
        public string MaPhTra
        {
            get { return maPhTra; }
            set { maPhTra = value; OnPropertyChanged("MaPhTra"); }
        }

        private int soNgayQuahan;
        public int SoNgayQuahan
        {
            get { return soNgayQuahan; }
            set { soNgayQuahan = value; OnPropertyChanged("SoNgayQuahan"); }
        }
        private int soTienThu;
        public int SoTienThu
        {
            get { return soTienThu; }
            set { soTienThu = value; OnPropertyChanged("SoTienThu"); }
        }
    }
}
