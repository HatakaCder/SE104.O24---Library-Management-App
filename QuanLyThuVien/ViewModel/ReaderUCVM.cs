using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.Commands;

namespace QuanLyThuVien.ViewModel
{
    public class ReaderUCVM: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        DataOperation ObjDataOperation;
        private ObservableCollection<READER> readerList;
        private READER CurrentReader;
        //private RelayCommand saveCommand;
        //private RelayCommand deleteCommand;
        //private RelayCommand searchCommand;
        //private RelayCommand updateCommand;

        public ObservableCollection<READER> ReaderList
        {
            get { return readerList; }
            set { readerList = value; OnPropertyChanged("ReaderList"); }
        }


        public ReaderUCVM()
        {
            ObjDataOperation = new DataOperation();
            LoadData();
            CurrentReader = new READER();
            // saveCommand = new RelayCommand(Save);
        }

        private void LoadData()
        {
            ReaderList = new ObservableCollection<READER>(ObjDataOperation.getAllReader());
        }

        //public void save()
        //{
        //    try
        //    {
        //        //var IsSaved = ObjDataOperation.Add()
        //    }
        //    catch { }
        //}
    }
}
