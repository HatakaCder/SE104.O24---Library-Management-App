using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTVDemo.Models
{
    public class LIBRARIAN : INotifyPropertyChanged
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
        private string maTT;

        public string MaTT
        {
            get { return maTT; }
            set { maTT = value; OnPropertyChanged("MaTT"); }
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
    }
}
