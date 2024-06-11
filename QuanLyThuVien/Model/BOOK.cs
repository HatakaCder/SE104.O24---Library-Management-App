﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class BOOK : INotifyPropertyChanged
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

        private string maSach;
        public string MaSach
        {
            get { return maSach; }
            set { maSach = value; OnPropertyChanged("MaSach"); }
        }
        private string tenSach;
        public string TenSach
        {
            get { return tenSach; }
            set { tenSach = value; OnPropertyChanged("TenSach"); }
        }
        private string theLoai;
        public string TheLoai
        {
            get { return theLoai; }
            set { theLoai = value; OnPropertyChanged("TheLoai"); }
        }
        private string tacGia;
        public string TacGia
        {
            get { return tacGia; }
            set { tacGia = value; OnPropertyChanged("TacGia"); }
        }
        private string namXB;
        public string NamXB
        {
            get { return namXB; }
            set { namXB = value; OnPropertyChanged("NamXB"); }
        }
        private string nhaXB;
        public string NhaXB
        {
            get { return nhaXB; }
            set { nhaXB = value; OnPropertyChanged("NhaXB"); }
        }
        private string ngayNhap;
        public string NgayNhap
        {
            get { return ngayNhap; }
            set { ngayNhap = value; OnPropertyChanged("NgayNhap"); }
        }
        private string triGia;
        public string TriGia
        {
            get { return triGia; }
            set { triGia = value; OnPropertyChanged("TriGia"); }
        }
        private string tinhTrang;
        public string TinhTrang
        {
            get { return tinhTrang; }
            set { tinhTrang = value; OnPropertyChanged("TinhTrang"); }
        }

    }
}