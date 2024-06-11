using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class READER : INotifyPropertyChanged
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
        private string maDG;

        public string MaDG
        {
            get { return maDG; }
            set { maDG = value; OnPropertyChanged("MaDG"); }
        }

        private string hoTen;

        public string HoTen
        {
            get { return hoTen; }
            set { hoTen = value; OnPropertyChanged("HoTen"); }
        }
        private string loaiDG;

        public string LoaiDG
        {
            get { return loaiDG; }
            set { loaiDG = value; OnPropertyChanged("LoaiDG"); }
        }

        private string gioiTinh;

        public string GioiTinh
        {
            get { return gioiTinh; }
            set { gioiTinh = value; OnPropertyChanged("GioiTinh"); }
        }
        private DateTime ngaySinh;

        public DateTime NgaySinh
        {
            get { return ngaySinh; }
            set { ngaySinh = value; OnPropertyChanged("NgaySinh"); }
        }
        private string diaChi;

        public string DiaChi
        {
            get { return diaChi; }
            set { diaChi = value; OnPropertyChanged("DiaChi"); }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged("Email"); }
        }
        private string soDT;

        public string SoDT
        {
            get { return soDT; }
            set { soDT = value; OnPropertyChanged("SoDT"); }
        }

        private DateTime ngaylapThe;

        public DateTime NgayLapThe
        {
            get { return ngaylapThe; }
            set { ngaylapThe = value; OnPropertyChanged("NgayLapThe"); }
        }
        private string soCCCD;

        public string SoCCCD
        {
            get { return soCCCD; }
            set { soCCCD = value; OnPropertyChanged("SoCCCD"); }
        }
        private string anhDaiDien;

        public string AnhDaiDien
        {
            get { return anhDaiDien; }
            set { anhDaiDien = value; OnPropertyChanged("AnhDaiDien"); }
        }

    }
}
