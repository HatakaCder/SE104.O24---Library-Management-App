using QuanLyThuVien.Model;
using QuanLyThuVien.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.ViewModel
{
    public class Report1VM : INotifyPropertyChanged
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
        private Report1Service service;
        private ObservableCollection<Report1DTO> report1List;

        public ObservableCollection<Report1DTO> Report1List
        {
            get { return report1List; }
            set { report1List = value; OnPropertyChanged("Report1List"); }
        }
        public Report1VM(DateTime dt, string type)
        {
            service = new Report1Service();
            TypeReport = type;
            CurrentTime = dt;
            LoadData();
        }
        private string typeReport;

        public string TypeReport
        {
            get { return typeReport; }
            set { typeReport = value; OnPropertyChanged("TypeReport"); }
        }
        private DateTime currentTime;

        public DateTime CurrentTime
        {
            get { return currentTime; }
            set { currentTime = value; OnPropertyChanged("CurrentTime"); }
        }
        private int totalBorrowed;

        public int TotalBorrowed
        {
            get { return totalBorrowed; }
            set { totalBorrowed = value; OnPropertyChanged("TotalBorrowed"); }
        }

        private void LoadData()
        {
            Report1List = new ObservableCollection<Report1DTO>(service.GetAll(CurrentTime, TypeReport));
            TotalBorrowed = service.getTotalBorrowed(CurrentTime, TypeReport);
        }
    }
}
