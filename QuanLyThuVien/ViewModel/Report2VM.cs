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
    public class Report2VM : INotifyPropertyChanged
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
        private Report2Service service;
        private ObservableCollection<Report2DTO> report2List;

        public ObservableCollection<Report2DTO> Report2List
        {
            get { return report2List; }
            set { report2List = value; OnPropertyChanged("Report2List"); }
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
        private void LoadData()
        {
            Report2List = new ObservableCollection<Report2DTO>(service.GetAll(CurrentTime, TypeReport));
        }
        public Report2VM(DateTime dt, string type)
        {
            service = new Report2Service();
            TypeReport = type;
            CurrentTime = dt;
            LoadData();
        }
    }
}
