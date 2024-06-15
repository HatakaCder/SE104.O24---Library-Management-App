using QuanLyThuVien.Model;
using QuanLyThuVien.Services;
using QuanLyThuVien.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.View;

namespace QuanLyThuVien.ViewModel
{
    public class ReportVM : INotifyPropertyChanged
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
        private List<string> columnsName;

        public List<string> ColumnsName
        {
            get { return columnsName; }
            set { columnsName = value; OnPropertyChanged("ColumnsName"); }
        }
        private ReportService reportService;

        #region DisplayOperation
        private ObservableCollection<object> items;

        public ObservableCollection<object> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(nameof(Items)); }
        }
        private RelayCommand switchToReport1Command;

        public RelayCommand SwitchToReport1Command
        {
            get { return switchToReport1Command; }
        }
        private RelayCommand switchToReport2Command;

        public RelayCommand SwitchToReport2Command
        {
            get { return switchToReport2Command; }
        }
        public ObservableCollection<Report_1> rp_1;
        public ObservableCollection<Report_2> rp_2;
        private void LoadData_1()
        {
            rp_1
        }
        public void SwitchToReport1()
        {
            Items= new ObservableCollection<object>(rp_1);
        }
        public void SwitchToReport2()   
        {
            Items = new ObservableCollection<object>(rp_2);
        }
        #endregion
        #region AddOperation
    }
}
