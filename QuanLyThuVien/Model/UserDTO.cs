using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace QuanLyThuVien.Model
{
    public class UserDTO : INotifyPropertyChanged
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
        private string taiKhoan;

        public string TaiKhoan
        {
            get { return taiKhoan; }
            set { taiKhoan = value; OnPropertyChanged("TaiKhoan"); }
        }
        private string matKhau;

        public string MatKhau
        {
            get { return matKhau; }
            set { matKhau = value; OnPropertyChanged("MatKhau"); }
        }
    }
}
