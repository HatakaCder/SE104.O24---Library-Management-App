using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class Report1DTO : INotifyPropertyChanged
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

        private string tenTheLoai;

        public string TenTheLoai
        {
            get { return tenTheLoai; }
            set { tenTheLoai = value; OnPropertyChanged("TenTheLoai"); }
        }
        private int soLuotMuon;

        public int SoLuotMuon
        {
            get { return soLuotMuon; }
            set { soLuotMuon = value; OnPropertyChanged("SoLuotMuon"); }
        }
        private int tyLe;

        public int TyLe
        {
            get { return tyLe; }
            set { tyLe = value; OnPropertyChanged("TyLe"); }
        }


    }
}
